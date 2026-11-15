using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Usuarios.ListarUsuarios;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using System.Collections.Generic;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Usuarios.ListarUsuarios
{
    public class ListarUsuariosLN : IListarUsuariosLN
    {
        public List<UsuarioDto> ObtenerPorComercio(int idComercio)
        {
            using (var contexto = new BancoLosPatitosContext())
            {
                return contexto.Usuarios
                    .Where(x => x.IdComercio == idComercio)
                    .OrderBy(x => x.IdUsuario)
                    .Select(x => new UsuarioDto
                    {
                        IdUsuario = x.IdUsuario,
                        IdComercio = x.IdComercio,
                        IdNetUser = x.IdNetUser,
                        Nombres = x.Nombres,
                        PrimerApellido = x.PrimerApellido,
                        SegundoApellido = x.SegundoApellido,
                        Identificacion = x.Identificacion,
                        CorreoElectronico = x.CorreoElectronico,
                        FechaDeRegistro = x.FechaDeRegistro,
                        FechaDeModificacion = x.FechaDeModificacion,
                        Estado = x.Estado
                    })
                    .ToList();
            }
        }
    }
}
