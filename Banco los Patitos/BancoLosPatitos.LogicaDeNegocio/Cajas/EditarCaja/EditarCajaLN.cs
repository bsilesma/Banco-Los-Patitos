using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Cajas.EditarCaja;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using BancoLosPatitos.AccesoADatos.Entidades;
using BancoLosPatitos.LogicaDeNegocio.Bitacora.RegistrarEvento;
using System;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Cajas.EditarCaja
{
    public class EditarCajaLN : IEditarCajaLN
    {
        public void Editar(CajaDto caja)
        {
            ValidadorDeCaja.ValidarEdicion(caja);

            string nombre = caja.Nombre.Trim();
            string telefono = caja.TelefonoSINPE.Trim();

            using (var contexto = new BancoLosPatitosContext())
            using (var transaccion = contexto.Database.BeginTransaction())
            {
                try
                {
                    var existente = contexto.Cajas
                        .FirstOrDefault(x => x.IdCaja == caja.IdCaja);

                    if (existente == null)
                    {
                        throw new ReglaDeNegocioException("La caja que intenta editar no existe.");
                    }

                    // El comercio sale de la base de datos: una caja no cambia de comercio al editarse.
                    bool nombreRepetido = contexto.Cajas
                        .Any(x => x.IdComercio == existente.IdComercio
                            && x.Nombre == nombre
                            && x.IdCaja != existente.IdCaja);

                    if (nombreRepetido)
                    {
                        throw new ReglaDeNegocioException("El comercio ya tiene una caja con el nombre " + nombre + ".");
                    }

                    // Los SINPE se buscan por teléfono: si cambiara, la caja perdería los pagos que ya recibió.
                    if (telefono != existente.TelefonoSINPE)
                    {
                        bool tieneSinpe = contexto.Sinpes
                            .Any(x => x.TelefonoDestinatario == existente.TelefonoSINPE);

                        if (tieneSinpe)
                        {
                            throw new ReglaDeNegocioException("No se puede cambiar el teléfono SINPE de la caja " + existente.Nombre + " porque ya recibió pagos.");
                        }
                    }

                    // El teléfono identifica a la caja que recibe los SINPE, aunque esté inactiva.
                    bool telefonoEnUso = contexto.Cajas
                        .Any(x => x.TelefonoSINPE == telefono && x.IdCaja != existente.IdCaja);

                    if (telefonoEnUso)
                    {
                        throw new ReglaDeNegocioException("Ya existe una caja con el teléfono SINPE " + telefono + ".");
                    }

                    Caja anterior = Copiar(existente);

                    existente.Nombre = nombre;
                    existente.Descripcion = caja.Descripcion.Trim();
                    existente.TelefonoSINPE = telefono;
                    existente.Estado = caja.Estado;
                    existente.FechaDeModificacion = DateTime.Now;

                    contexto.SaveChanges();

                    IRegistrarEventoLN bitacora = new RegistrarEventoLN(contexto);
                    bitacora.Registrar(new EventoBitacoraDto
                    {
                        TablaDeEvento = TablasDeEvento.Cajas,
                        TipoDeEvento = TiposDeEvento.Editar,
                        DescripcionDeEvento = "Se editó la caja " + existente.Nombre + ".",
                        DatosAnteriores = anterior,
                        DatosPosteriores = existente
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

        // Copia los valores antes de modificarlos, porque la entidad original cambia al editar.
        private static Caja Copiar(Caja caja)
        {
            return new Caja
            {
                IdCaja = caja.IdCaja,
                IdComercio = caja.IdComercio,
                Nombre = caja.Nombre,
                Descripcion = caja.Descripcion,
                TelefonoSINPE = caja.TelefonoSINPE,
                FechaDeRegistro = caja.FechaDeRegistro,
                FechaDeModificacion = caja.FechaDeModificacion,
                Estado = caja.Estado
            };
        }
    }
}
