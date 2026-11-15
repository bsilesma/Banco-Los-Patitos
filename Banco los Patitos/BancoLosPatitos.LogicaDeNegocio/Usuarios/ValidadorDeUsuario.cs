using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.ModelosParaUI;

namespace BancoLosPatitos.LogicaDeNegocio.Usuarios
{
    internal static class ValidadorDeUsuario
    {
        private const int LargoMinimoDeIdentificacion = 9;
        private const int LargoMaximoDeIdentificacion = 10;

        public static void ValidarRegistro(UsuarioDto usuario)
        {
            ValidarQueExista(usuario);

            if (usuario.IdComercio <= 0)
            {
                throw new ReglaDeNegocioException("No se indicó el comercio del usuario.");
            }

            ValidarDatosEditables(usuario);
        }

        public static void ValidarEdicion(UsuarioDto usuario)
        {
            ValidarQueExista(usuario);
            ValidarDatosEditables(usuario);
        }

        private static void ValidarDatosEditables(UsuarioDto usuario)
        {
            ValidacionesComunes.ValidarTexto(usuario.Nombres, "Nombres", 100);
            ValidacionesComunes.ValidarTexto(usuario.PrimerApellido, "Primer apellido", 100);
            ValidacionesComunes.ValidarTexto(usuario.SegundoApellido, "Segundo apellido", 100);
            ValidacionesComunes.ValidarSoloDigitos(usuario.Identificacion, "Identificación", LargoMinimoDeIdentificacion, LargoMaximoDeIdentificacion);
            ValidacionesComunes.ValidarTexto(usuario.CorreoElectronico, "Correo electrónico", 200);
            ValidacionesComunes.ValidarCorreo(usuario.CorreoElectronico);
        }

        private static void ValidarQueExista(UsuarioDto usuario)
        {
            if (usuario == null)
            {
                throw new ReglaDeNegocioException("No se recibieron los datos del usuario.");
            }
        }
    }
}
