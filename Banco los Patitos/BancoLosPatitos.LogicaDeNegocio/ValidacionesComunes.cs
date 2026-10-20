using BancoLosPatitos.Abstracciones.Excepciones;

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

        public static void ValidarTelefono(string telefono, string campo)
        {
            string valor = telefono == null ? string.Empty : telefono.Trim();

            if (valor.Length != LargoDelTelefono || !SonSoloDigitos(valor))
            {
                throw new ReglaDeNegocioException("El campo " + campo + " debe contener exactamente " + LargoDelTelefono + " dígitos.");
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
