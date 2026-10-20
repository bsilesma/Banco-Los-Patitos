using BancoLosPatitos.Abstracciones.ModelosParaUI;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Cajas.ObtenerCaja
{
    public interface IObtenerCajaLN
    {
        CajaDto BuscarPorId(int idCaja);
    }
}
