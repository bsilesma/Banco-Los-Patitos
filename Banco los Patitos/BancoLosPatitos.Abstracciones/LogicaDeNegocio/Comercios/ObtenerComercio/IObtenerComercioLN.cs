using BancoLosPatitos.Abstracciones.ModelosParaUI;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Comercios.ObtenerComercio
{
    public interface IObtenerComercioLN
    {
        ComercioDto BuscarPorId(int idComercio);
    }
}
