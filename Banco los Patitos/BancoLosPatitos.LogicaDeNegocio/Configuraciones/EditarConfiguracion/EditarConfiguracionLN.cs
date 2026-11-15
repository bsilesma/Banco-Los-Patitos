using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Configuraciones.EditarConfiguracion;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using BancoLosPatitos.AccesoADatos.Entidades;
using BancoLosPatitos.LogicaDeNegocio.Bitacora.RegistrarEvento;
using System;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Configuraciones.EditarConfiguracion
{
    public class EditarConfiguracionLN : IEditarConfiguracionLN
    {
        public void Editar(ConfiguracionDto configuracion)
        {
            ValidadorDeConfiguracion.ValidarEdicion(configuracion);

            using (var contexto = new BancoLosPatitosContext())
            using (var transaccion = contexto.Database.BeginTransaction())
            {
                try
                {
                    var existente = contexto.Configuraciones
                        .FirstOrDefault(x => x.IdConfiguracion == configuracion.IdConfiguracion);

                    if (existente == null)
                    {
                        throw new ReglaDeNegocioException("La configuración que intenta editar no existe.");
                    }

                    Configuracion anterior = Copiar(existente);

                    // El comercio no se puede cambiar; solo tipo, comisión y estado.
                    existente.TipoConfiguracion = configuracion.TipoConfiguracion;
                    existente.Comision = configuracion.Comision;
                    existente.Estado = configuracion.Estado;
                    existente.FechaDeModificacion = DateTime.Now;

                    contexto.SaveChanges();

                    IRegistrarEventoLN bitacora = new RegistrarEventoLN(contexto);
                    bitacora.Registrar(new EventoBitacoraDto
                    {
                        TablaDeEvento = TablasDeEvento.Configuraciones,
                        TipoDeEvento = TiposDeEvento.Editar,
                        DescripcionDeEvento = "Se editó la configuración " + existente.IdConfiguracion + " del comercio " + existente.IdComercio + ".",
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
        private static Configuracion Copiar(Configuracion configuracion)
        {
            return new Configuracion
            {
                IdConfiguracion = configuracion.IdConfiguracion,
                IdComercio = configuracion.IdComercio,
                TipoConfiguracion = configuracion.TipoConfiguracion,
                Comision = configuracion.Comision,
                FechaDeRegistro = configuracion.FechaDeRegistro,
                FechaDeModificacion = configuracion.FechaDeModificacion,
                Estado = configuracion.Estado
            };
        }
    }
}
