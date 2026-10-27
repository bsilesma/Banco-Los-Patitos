using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using System;

namespace BancoLosPatitos.LogicaDeNegocio.Sinpes
{
    internal static class ValidadorDeSinpe
    {
        private const int DecimalesDelMonto = 2;
        private const int LargoMaximoDeDescripcion = 50;

        // Límite de la columna Monto DECIMAL(18,2): 16 dígitos enteros.
        private const decimal MontoMaximo = 9999999999999999.99m;

        public static void ValidarRegistro(SinpeDto sinpe)
        {
            ValidarQueExista(sinpe);
            ValidacionesComunes.ValidarTelefono(sinpe.TelefonoOrigen, "Teléfono origen");
            ValidacionesComunes.ValidarTexto(sinpe.NombreOrigen, "Nombre origen", 200);
            ValidacionesComunes.ValidarTelefono(sinpe.TelefonoDestinatario, "Teléfono destinatario");
            ValidarMonto(sinpe.Monto);
            ValidarDescripcion(sinpe.Descripcion);
        }

        private static void ValidarQueExista(SinpeDto sinpe)
        {
            if (sinpe == null)
            {
                throw new ReglaDeNegocioException("No se recibieron los datos del SINPE.");
            }
        }

        private static void ValidarMonto(decimal? monto)
        {
            if (!monto.HasValue)
            {
                throw new ReglaDeNegocioException("El campo Monto es obligatorio y debe ser un número válido, por ejemplo 4500,50.");
            }

            if (monto.Value <= 0)
            {
                throw new ReglaDeNegocioException("El monto debe ser mayor a 0.");
            }

            if (monto.Value > MontoMaximo)
            {
                throw new ReglaDeNegocioException("El monto supera el máximo permitido.");
            }

            if (monto.Value != Math.Round(monto.Value, DecimalesDelMonto))
            {
                throw new ReglaDeNegocioException("El monto admite un máximo de " + DecimalesDelMonto + " decimales.");
            }
        }

        // La descripción es opcional, pero no puede pasar de 50 caracteres.
        private static void ValidarDescripcion(string descripcion)
        {
            if (!string.IsNullOrWhiteSpace(descripcion) && descripcion.Trim().Length > LargoMaximoDeDescripcion)
            {
                throw new ReglaDeNegocioException("El campo Descripción admite un máximo de " + LargoMaximoDeDescripcion + " caracteres.");
            }
        }
    }
}
