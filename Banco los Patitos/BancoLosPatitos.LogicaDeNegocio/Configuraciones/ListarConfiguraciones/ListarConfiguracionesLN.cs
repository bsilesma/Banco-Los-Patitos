using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Configuraciones.ListarConfiguraciones;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using System.Collections.Generic;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Configuraciones.ListarConfiguraciones
{
    public class ListarConfiguracionesLN : IListarConfiguracionesLN
    {
        public List<ConfiguracionDto> Obtener()
        {
            using (var contexto = new BancoLosPatitosContext())
            {
                // Se une con Comercios para mostrar el nombre en lugar del id.
                return contexto.Configuraciones
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
                    .OrderBy(x => x.IdConfiguracion)
                    .ToList();
            }
        }
    }
}
