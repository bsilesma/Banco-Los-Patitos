using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Configuraciones.RegistrarConfiguracion;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using BancoLosPatitos.AccesoADatos.Entidades;
using BancoLosPatitos.LogicaDeNegocio.Bitacora.RegistrarEvento;
using System;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Configuraciones.RegistrarConfiguracion
{
    public class RegistrarConfiguracionLN : IRegistrarConfiguracionLN
    {
        public void Registrar(ConfiguracionDto configuracion)
        {
            ValidadorDeConfiguracion.ValidarRegistro(configuracion);

            using (var contexto = new BancoLosPatitosContext())
            using (var transaccion = contexto.Database.BeginTransaction())
            {
                try
                {
                    var comercio = contexto.Comercios
                        .FirstOrDefault(x => x.IdComercio == configuracion.IdComercio);

                    if (comercio == null)
                    {
                        throw new ReglaDeNegocioException("El comercio seleccionado no existe.");
                    }

                    if (!comercio.Estado)
                    {
                        throw new ReglaDeNegocioException("No se puede registrar una configuración para un comercio inactivo.");
                    }

                    bool yaTieneConfiguracion = contexto.Configuraciones
                        .Any(x => x.IdComercio == configuracion.IdComercio);

                    if (yaTieneConfiguracion)
                    {
                        throw new ReglaDeNegocioException("El comercio " + comercio.Nombre + " ya tiene una configuración registrada.");
                    }

                    var nueva = new Configuracion
                    {
                        IdComercio = configuracion.IdComercio,
                        TipoConfiguracion = configuracion.TipoConfiguracion,
                        Comision = configuracion.Comision,
                        FechaDeRegistro = DateTime.Now,
                        FechaDeModificacion = null,
                        Estado = true
                    };

                    contexto.Configuraciones.Add(nueva);
                    contexto.SaveChanges();

                    IRegistrarEventoLN bitacora = new RegistrarEventoLN(contexto);
                    bitacora.Registrar(new EventoBitacoraDto
                    {
                        TablaDeEvento = TablasDeEvento.Configuraciones,
                        TipoDeEvento = TiposDeEvento.Registrar,
                        DescripcionDeEvento = "Se registró la configuración del comercio " + comercio.Nombre + ".",
                        DatosAnteriores = nueva
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
    }
}
