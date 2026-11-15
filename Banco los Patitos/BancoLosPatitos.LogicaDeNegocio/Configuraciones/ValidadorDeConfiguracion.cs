using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.ModelosParaUI;

namespace BancoLosPatitos.LogicaDeNegocio.Configuraciones
{
    internal static class ValidadorDeConfiguracion
    {
        private const int ComisionMinima = 1;
        private const int ComisionMaxima = 100;

        public static void ValidarRegistro(ConfiguracionDto configuracion)
        {
            ValidarQueExista(configuracion);

            if (configuracion.IdComercio <= 0)
            {
                throw new ReglaDeNegocioException("Debe seleccionar un comercio.");
            }

            ValidarDatosEditables(configuracion);
        }

        public static void ValidarEdicion(ConfiguracionDto configuracion)
        {
            ValidarQueExista(configuracion);
            ValidarDatosEditables(configuracion);
        }

        private static void ValidarDatosEditables(ConfiguracionDto configuracion)
        {
            ValidacionesComunes.ValidarRango(configuracion.TipoConfiguracion, 1, 3, "Debe seleccionar un tipo de configuración válido.");
            ValidacionesComunes.ValidarRango(configuracion.Comision, ComisionMinima, ComisionMaxima,
                "La comisión debe ser un número entero entre " + ComisionMinima + " y " + ComisionMaxima + ".");
        }

        private static void ValidarQueExista(ConfiguracionDto configuracion)
        {
            if (configuracion == null)
            {
                throw new ReglaDeNegocioException("No se recibieron los datos de la configuración.");
            }
        }
    }
}
