using BancoLosPatitos.Abstracciones.ModelosParaUI;
using System.Collections.Generic;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Comercios.ListarComercios
{
    public interface IListarComerciosLN
    {
        List<ComercioDto> Obtener();
    }
}
