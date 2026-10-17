using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using System.ComponentModel.DataAnnotations;

namespace BancoLosPatitos.LogicaDeNegocio.Comercios
{
    internal static class ValidadorDeComercio
    {
        private const int LargoDelTelefono = 8;

        public static void ValidarRegistro(ComercioDto comercio)
        {
            ValidarQueExista(comercio);
            ValidarTexto(comercio.Identificacion, "Identificación", 30);
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
            ValidarTexto(comercio.Nombre, "Nombre", 200);
            ValidarRango(comercio.TipoDeComercio, 1, 4, "Debe seleccionar un tipo de comercio válido.");
            ValidarTelefono(comercio.Telefono);
            ValidarTexto(comercio.CorreoElectronico, "Correo electrónico", 200);
            ValidarCorreo(comercio.CorreoElectronico);
            ValidarTexto(comercio.Direccion, "Dirección", 500);
        }

        private static void ValidarQueExista(ComercioDto comercio)
        {
            if (comercio == null)
            {
                throw new ReglaDeNegocioException("No se recibieron los datos del comercio.");
            }
        }

        private static void ValidarTexto(string valor, string campo, int largoMaximo)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new ReglaDeNegocioException("El campo " + campo + " es obligatorio.");
            }

            if (valor.Trim().Length > largoMaximo)
            {
                throw new ReglaDeNegocioException("El campo " + campo + " admite un máximo de " + largoMaximo + " caracteres.");
            }
        }

        private static void ValidarRango(int valor, int minimo, int maximo, string mensaje)
        {
            if (valor < minimo || valor > maximo)
            {
                throw new ReglaDeNegocioException(mensaje);
            }
        }

        private static void ValidarTelefono(string telefono)
        {
            string valor = telefono == null ? string.Empty : telefono.Trim();

            if (valor.Length != LargoDelTelefono || !SonSoloDigitos(valor))
            {
                throw new ReglaDeNegocioException("El teléfono debe contener exactamente " + LargoDelTelefono + " dígitos.");
            }
        }

        // Compara contra '0'-'9' porque char.IsDigit también acepta dígitos de otros alfabetos.
        private static bool SonSoloDigitos(string valor)
        {
            foreach (char caracter in valor)
            {
                if (caracter < '0' || caracter > '9')
                {
                    return false;
                }
            }

            return true;
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
