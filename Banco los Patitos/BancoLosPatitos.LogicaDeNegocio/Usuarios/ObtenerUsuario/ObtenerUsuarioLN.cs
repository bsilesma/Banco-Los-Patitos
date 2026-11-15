using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuario;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Usuarios.ObtenerUsuario
{
    public class ObtenerUsuarioLN : IObtenerUsuarioLN
    {
        public UsuarioDto BuscarPorId(int idUsuario)
        {
            using (var contexto = new BancoLosPatitosContext())
            {
                return contexto.Usuarios
                    .Where(x => x.IdUsuario == idUsuario)
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
                    .FirstOrDefault();
            }
        }
    }
}
