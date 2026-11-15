using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Configuraciones.ListarComerciosSinConfiguracion;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using System.Collections.Generic;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Configuraciones.ListarComerciosSinConfiguracion
{
    public class ListarComerciosSinConfiguracionLN : IListarComerciosSinConfiguracionLN
    {
        // Solo comercios activos que todavía no tienen configuración, para el formulario de registro.
        public List<ComercioDto> Obtener()
        {
            using (var contexto = new BancoLosPatitosContext())
            {
                return contexto.Comercios
                    .Where(x => x.Estado
                        && !contexto.Configuraciones.Any(c => c.IdComercio == x.IdComercio))
                    .OrderBy(x => x.Nombre)
                    .Select(x => new ComercioDto
                    {
                        IdComercio = x.IdComercio,
                        Nombre = x.Nombre
                    })
                    .ToList();
            }
        }
    }
}
