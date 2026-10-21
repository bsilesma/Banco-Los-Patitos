using BancoLosPatitos.Abstracciones.ModelosParaUI;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.ObtenerEvento
{
    public interface IObtenerEventoLN
    {
        EventoRegistradoDto BuscarPorId(int idEvento);
    }
}
