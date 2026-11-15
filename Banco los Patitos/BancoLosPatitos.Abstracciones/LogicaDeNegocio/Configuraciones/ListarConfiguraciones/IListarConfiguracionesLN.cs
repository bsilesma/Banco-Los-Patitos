using BancoLosPatitos.Abstracciones.ModelosParaUI;
using System.Collections.Generic;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Configuraciones.ListarConfiguraciones
{
    public interface IListarConfiguracionesLN
    {
        List<ConfiguracionDto> Obtener();
    }
}
