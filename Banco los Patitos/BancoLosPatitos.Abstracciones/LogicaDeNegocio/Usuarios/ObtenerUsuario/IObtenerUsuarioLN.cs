using BancoLosPatitos.Abstracciones.ModelosParaUI;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuario
{
    public interface IObtenerUsuarioLN
    {
        UsuarioDto BuscarPorId(int idUsuario);
    }
}
