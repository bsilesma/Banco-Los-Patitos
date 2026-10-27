using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Sinpes.RegistrarSinpe;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using BancoLosPatitos.AccesoADatos.Entidades;
using BancoLosPatitos.LogicaDeNegocio.Bitacora.RegistrarEvento;
using System;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Sinpes.RegistrarSinpe
{
    public class RegistrarSinpeLN : IRegistrarSinpeLN
    {
        public int Registrar(SinpeDto sinpe)
        {
            ValidadorDeSinpe.ValidarRegistro(sinpe);

            string telefonoDestinatario = sinpe.TelefonoDestinatario.Trim();

            using (var contexto = new BancoLosPatitosContext())
            using (var transaccion = contexto.Database.BeginTransaction())
            {
                try
                {
                    // El índice de la base garantiza que solo una caja activa tenga cada teléfono.
                    var caja = contexto.Cajas
                        .FirstOrDefault(x => x.TelefonoSINPE == telefonoDestinatario && x.Estado);

                    if (caja == null)
                    {
                        bool existeCaja = contexto.Cajas
                            .Any(x => x.TelefonoSINPE == telefonoDestinatario);

                        if (!existeCaja)
                        {
                            throw new ReglaDeNegocioException("El teléfono " + telefonoDestinatario + " no está registrado en ninguna caja.");
                        }

                        throw new ReglaDeNegocioException("La caja con el teléfono " + telefonoDestinatario + " está inactiva y no puede recibir pagos.");
                    }

                    var comercio = contexto.Comercios
                        .First(x => x.IdComercio == caja.IdComercio);

                    if (!comercio.Estado)
                    {
                        throw new ReglaDeNegocioException("El comercio de la caja con el teléfono " + telefonoDestinatario + " está inactivo y no puede recibir pagos.");
                    }

                    var nuevo = new Sinpe
                    {
                        TelefonoOrigen = sinpe.TelefonoOrigen.Trim(),
                        NombreOrigen = sinpe.NombreOrigen.Trim(),
                        TelefonoDestinatario = telefonoDestinatario,
                        // El destinatario es el comercio dueño de la caja; el usuario no lo escribe.
                        NombreDestinatario = comercio.Nombre.Trim(),
                        Monto = sinpe.Monto.Value,
                        FechaDeRegistro = DateTime.Now,
                        Descripcion = string.IsNullOrWhiteSpace(sinpe.Descripcion) ? null : sinpe.Descripcion.Trim(),
                        Estado = false
                    };

                    contexto.Sinpes.Add(nuevo);
                    contexto.SaveChanges();

                    IRegistrarEventoLN bitacora = new RegistrarEventoLN(contexto);
                    bitacora.Registrar(new EventoBitacoraDto
                    {
                        TablaDeEvento = TablasDeEvento.Sinpe,
                        TipoDeEvento = TiposDeEvento.Registrar,
                        DescripcionDeEvento = "Se registró un SINPE de " + nuevo.Monto.ToString("N2") + " hacia el teléfono " + nuevo.TelefonoDestinatario + ".",
                        DatosAnteriores = nuevo
                    });

                    transaccion.Commit();

                    return caja.IdCaja;
                }
                catch
                {
                    transaccion.Rollback();
                    throw;
                }
            }
        }
    }
}
