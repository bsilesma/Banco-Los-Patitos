using BancoLosPatitos.Abstracciones.ModelosParaUI;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Usuarios.RegistrarUsuario
{
    public interface IRegistrarUsuarioLN
    {
        void Registrar(UsuarioDto usuario);
    }
}
