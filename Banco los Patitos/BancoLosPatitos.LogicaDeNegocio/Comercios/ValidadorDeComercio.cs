using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.ModelosParaUI;

namespace BancoLosPatitos.LogicaDeNegocio.Comercios
{
    internal static class ValidadorDeComercio
    {
        public static void ValidarRegistro(ComercioDto comercio)
        {
            ValidarQueExista(comercio);
            ValidacionesComunes.ValidarTexto(comercio.Identificacion, "Identificación", 30);
            ValidacionesComunes.ValidarRango(comercio.TipoIdentificacion, 1, 2, "Debe seleccionar un tipo de identificación válido.");
            ValidarDatosEditables(comercio);
        }

        public static void ValidarEdicion(ComercioDto comercio)
        {
            ValidarQueExista(comercio);
            ValidarDatosEditables(comercio);
        }

        private static void ValidarDatosEditables(ComercioDto comercio)
        {
            ValidacionesComunes.ValidarTexto(comercio.Nombre, "Nombre", 200);
            ValidacionesComunes.ValidarRango(comercio.TipoDeComercio, 1, 4, "Debe seleccionar un tipo de comercio válido.");
            ValidacionesComunes.ValidarTelefono(comercio.Telefono, "Teléfono");
            ValidacionesComunes.ValidarTexto(comercio.CorreoElectronico, "Correo electrónico", 200);
            ValidacionesComunes.ValidarCorreo(comercio.CorreoElectronico);
            ValidacionesComunes.ValidarTexto(comercio.Direccion, "Dirección", 500);
        }

        private static void ValidarQueExista(ComercioDto comercio)
        {
            if (comercio == null)
            {
                throw new ReglaDeNegocioException("No se recibieron los datos del comercio.");
            }
        }
    }
}
