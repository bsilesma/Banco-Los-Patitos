using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.ModelosParaUI;

namespace BancoLosPatitos.LogicaDeNegocio.Cajas
{
    internal static class ValidadorDeCaja
    {
        public static void ValidarRegistro(CajaDto caja)
        {
            ValidarQueExista(caja);
            ValidarDatosEditables(caja);
        }

        public static void ValidarEdicion(CajaDto caja)
        {
            ValidarQueExista(caja);
            ValidarDatosEditables(caja);
        }

        private static void ValidarDatosEditables(CajaDto caja)
        {
            ValidacionesComunes.ValidarTexto(caja.Nombre, "Nombre", 100);
            ValidacionesComunes.ValidarTexto(caja.Descripcion, "Descripción", 150);
            ValidacionesComunes.ValidarTelefono(caja.TelefonoSINPE, "Teléfono SINPE");
        }

        private static void ValidarQueExista(CajaDto caja)
        {
            if (caja == null)
            {
                throw new ReglaDeNegocioException("No se recibieron los datos de la caja.");
            }
        }
    }
}
