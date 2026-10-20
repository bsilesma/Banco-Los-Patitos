using BancoLosPatitos.Abstracciones.ModelosParaUI;
using System.Collections.Generic;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Cajas.ListarCajas
{
    public interface IListarCajasLN
    {
        List<CajaDto> ObtenerPorComercio(int idComercio);
    }
}
