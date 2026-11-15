using BancoLosPatitos.Abstracciones.Excepciones;
using System.ComponentModel.DataAnnotations;

namespace BancoLosPatitos.LogicaDeNegocio
{
    internal static class ValidacionesComunes
    {
        private const int LargoDelTelefono = 8;

        public static void ValidarTexto(string valor, string campo, int largoMaximo)
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

        public static void ValidarRango(int valor, int minimo, int maximo, string mensaje)
        {
            if (valor < minimo || valor > maximo)
            {
                throw new ReglaDeNegocioException(mensaje);
            }
        }

        public static void ValidarTelefono(string telefono, string campo)
        {
            string valor = telefono == null ? string.Empty : telefono.Trim();

            if (valor.Length != LargoDelTelefono || !SonSoloDigitos(valor))
            {
                throw new ReglaDeNegocioException("El campo " + campo + " debe contener exactamente " + LargoDelTelefono + " dígitos.");
            }
        }

        public static void ValidarCorreo(string correo)
        {
            if (!new EmailAddressAttribute().IsValid(correo.Trim()))
            {
                throw new ReglaDeNegocioException("El correo electrónico no tiene un formato válido.");
            }
        }

        public static void ValidarSoloDigitos(string valor, string campo, int largoMinimo, int largoMaximo)
        {
            string texto = valor == null ? string.Empty : valor.Trim();

            if (texto.Length < largoMinimo || texto.Length > largoMaximo || !SonSoloDigitos(texto))
            {
                throw new ReglaDeNegocioException("El campo " + campo + " debe contener solo dígitos, entre " + largoMinimo + " y " + largoMaximo + ".");
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
    }
}
