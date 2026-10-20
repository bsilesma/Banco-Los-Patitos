using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using System.ComponentModel.DataAnnotations;

namespace BancoLosPatitos.LogicaDeNegocio.Comercios
{
    internal static class ValidadorDeComercio
    {
        public static void ValidarRegistro(ComercioDto comercio)
        {
            ValidarQueExista(comercio);
            ValidacionesComunes.ValidarTexto(comercio.Identificacion, "Identificación", 30);
            ValidarRango(comercio.TipoIdentificacion, 1, 2, "Debe seleccionar un tipo de identificación válido.");
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
            ValidarRango(comercio.TipoDeComercio, 1, 4, "Debe seleccionar un tipo de comercio válido.");
            ValidacionesComunes.ValidarTelefono(comercio.Telefono, "Teléfono");
            ValidacionesComunes.ValidarTexto(comercio.CorreoElectronico, "Correo electrónico", 200);
            ValidarCorreo(comercio.CorreoElectronico);
            ValidacionesComunes.ValidarTexto(comercio.Direccion, "Dirección", 500);
        }

        private static void ValidarQueExista(ComercioDto comercio)
        {
            if (comercio == null)
            {
                throw new ReglaDeNegocioException("No se recibieron los datos del comercio.");
            }
        }

        private static void ValidarRango(int valor, int minimo, int maximo, string mensaje)
        {
            if (valor < minimo || valor > maximo)
            {
                throw new ReglaDeNegocioException(mensaje);
            }
        }

        private static void ValidarCorreo(string correo)
        {
            if (!new EmailAddressAttribute().IsValid(correo.Trim()))
            {
                throw new ReglaDeNegocioException("El correo electrónico no tiene un formato válido.");
            }
        }
    }
}
