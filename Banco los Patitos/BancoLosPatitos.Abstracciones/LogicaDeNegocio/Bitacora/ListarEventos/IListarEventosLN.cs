using BancoLosPatitos.Abstracciones.ModelosParaUI;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.ListarEventos
{
    public interface IListarEventosLN
    {
        PaginaDeEventosDto ObtenerPagina(int pagina);
    }
}
