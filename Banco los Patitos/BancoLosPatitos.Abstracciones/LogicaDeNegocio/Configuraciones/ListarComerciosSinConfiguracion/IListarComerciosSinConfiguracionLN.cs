using BancoLosPatitos.Abstracciones.ModelosParaUI;
using System.Collections.Generic;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Configuraciones.ListarComerciosSinConfiguracion
{
    public interface IListarComerciosSinConfiguracionLN
    {
        List<ComercioDto> Obtener();
    }
}
