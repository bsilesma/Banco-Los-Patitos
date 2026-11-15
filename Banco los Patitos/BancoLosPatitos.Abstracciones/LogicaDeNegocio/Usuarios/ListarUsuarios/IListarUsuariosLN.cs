using BancoLosPatitos.Abstracciones.ModelosParaUI;
using System.Collections.Generic;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Usuarios.ListarUsuarios
{
    public interface IListarUsuariosLN
    {
        List<UsuarioDto> ObtenerPorComercio(int idComercio);
    }
}
