using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Configuraciones.ObtenerConfiguracion;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Configuraciones.ObtenerConfiguracion
{
    public class ObtenerConfiguracionLN : IObtenerConfiguracionLN
    {
        public ConfiguracionDto BuscarPorId(int idConfiguracion)
        {
            using (var contexto = new BancoLosPatitosContext())
            {
                return contexto.Configuraciones
                    .Where(x => x.IdConfiguracion == idConfiguracion)
                    .Join(contexto.Comercios,
                        configuracion => configuracion.IdComercio,
                        comercio => comercio.IdComercio,
                        (configuracion, comercio) => new ConfiguracionDto
                        {
                            IdConfiguracion = configuracion.IdConfiguracion,
                            IdComercio = configuracion.IdComercio,
                            NombreDelComercio = comercio.Nombre,
                            TipoConfiguracion = configuracion.TipoConfiguracion,
                            Comision = configuracion.Comision,
                            FechaDeRegistro = configuracion.FechaDeRegistro,
                            FechaDeModificacion = configuracion.FechaDeModificacion,
                            Estado = configuracion.Estado
                        })
                    .FirstOrDefault();
            }
        }
    }
}
