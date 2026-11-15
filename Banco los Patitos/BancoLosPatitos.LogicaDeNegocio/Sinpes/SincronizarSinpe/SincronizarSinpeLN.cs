using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Sinpes.SincronizarSinpe;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using BancoLosPatitos.AccesoADatos.Entidades;
using BancoLosPatitos.LogicaDeNegocio.Bitacora.RegistrarEvento;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Sinpes.SincronizarSinpe
{
    public class SincronizarSinpeLN : ISincronizarSinpeLN
    {
        private const int TipoPlataforma = 1;
        private const int TipoAmbas = 3;

        public void Sincronizar(int idSinpe)
        {
            using (var contexto = new BancoLosPatitosContext())
            using (var transaccion = contexto.Database.BeginTransaction())
            {
                try
                {
                    var sinpe = contexto.Sinpes
                        .FirstOrDefault(x => x.IdSinpe == idSinpe);

                    if (sinpe == null)
                    {
                        throw new ReglaDeNegocioException("El SINPE que intenta sincronizar no existe.");
                    }

                    if (sinpe.Estado)
                    {
                        throw new ReglaDeNegocioException("El SINPE ya está sincronizado.");
                    }

                    // El teléfono SINPE es único entre todas las cajas, así que identifica a una sola.
                    var caja = contexto.Cajas
                        .FirstOrDefault(x => x.TelefonoSINPE == sinpe.TelefonoDestinatario);

                    if (caja == null)
                    {
                        throw new ReglaDeNegocioException("No existe una caja con el teléfono " + sinpe.TelefonoDestinatario + ".");
                    }

                    if (!caja.Estado)
                    {
                        throw new ReglaDeNegocioException("La caja " + caja.Nombre + " está inactiva y no puede sincronizar SINPE.");
                    }

                    var comercio = contexto.Comercios
                        .First(x => x.IdComercio == caja.IdComercio);

                    if (!comercio.Estado)
                    {
                        throw new ReglaDeNegocioException("El comercio " + comercio.Nombre + " está inactivo y no puede sincronizar SINPE.");
                    }

                    ValidarConfiguracion(contexto, comercio);

                    Sinpe anterior = Copiar(sinpe);

                    sinpe.Estado = true;

                    contexto.SaveChanges();

                    IRegistrarEventoLN bitacora = new RegistrarEventoLN(contexto);
                    bitacora.Registrar(new EventoBitacoraDto
                    {
                        TablaDeEvento = TablasDeEvento.Sinpe,
                        TipoDeEvento = TiposDeEvento.Editar,
                        DescripcionDeEvento = "Se sincronizó el SINPE " + sinpe.IdSinpe + " de la caja " + caja.Nombre + ".",
                        DatosAnteriores = anterior,
                        DatosPosteriores = sinpe
                    });

                    transaccion.Commit();
                }
                catch
                {
                    transaccion.Rollback();
                    throw;
                }
            }
        }

        // La sincronización desde la aplicación web solo aplica a configuraciones Plataforma o Ambas.
        private static void ValidarConfiguracion(BancoLosPatitosContext contexto, Comercio comercio)
        {
            var configuracion = contexto.Configuraciones
                .FirstOrDefault(x => x.IdComercio == comercio.IdComercio);

            if (configuracion == null)
            {
                throw new ReglaDeNegocioException("El comercio " + comercio.Nombre + " no tiene una configuración registrada y no puede sincronizar SINPE.");
            }

            if (!configuracion.Estado)
            {
                throw new ReglaDeNegocioException("La configuración del comercio " + comercio.Nombre + " está inactiva y no puede sincronizar SINPE.");
            }

            if (configuracion.TipoConfiguracion != TipoPlataforma && configuracion.TipoConfiguracion != TipoAmbas)
            {
                throw new ReglaDeNegocioException("La configuración del comercio " + comercio.Nombre + " es Externa; sus SINPE se sincronizan por medio del API.");
            }
        }

        // Copia los valores antes de modificarlos, porque la entidad original cambia al sincronizar.
        private static Sinpe Copiar(Sinpe sinpe)
        {
            return new Sinpe
            {
                IdSinpe = sinpe.IdSinpe,
                TelefonoOrigen = sinpe.TelefonoOrigen,
                NombreOrigen = sinpe.NombreOrigen,
                TelefonoDestinatario = sinpe.TelefonoDestinatario,
                NombreDestinatario = sinpe.NombreDestinatario,
                Monto = sinpe.Monto,
                FechaDeRegistro = sinpe.FechaDeRegistro,
                Descripcion = sinpe.Descripcion,
                Estado = sinpe.Estado
            };
        }
    }
}
