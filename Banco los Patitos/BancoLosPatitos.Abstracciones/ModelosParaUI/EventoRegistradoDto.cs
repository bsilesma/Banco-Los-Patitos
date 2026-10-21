using System;

namespace BancoLosPatitos.Abstracciones.ModelosParaUI
{
    public class EventoRegistradoDto
    {
        private const int LargoDeTextoCorto = 40;

        public int IdEvento { get; set; }
        public string TablaDeEvento { get; set; }
        public string TipoDeEvento { get; set; }
        public DateTime FechaDeEvento { get; set; }
        public string DescripcionDeEvento { get; set; }
        public string StackTrace { get; set; }
        public string DatosAnteriores { get; set; }
        public string DatosPosteriores { get; set; }

        public string DescripcionCorta
        {
            get { return Recortar(DescripcionDeEvento); }
        }

        public string StackTraceCorto
        {
            get { return Recortar(StackTrace); }
        }

        public string DatosAnterioresCortos
        {
            get { return Recortar(DatosAnteriores); }
        }

        public string DatosPosterioresCortos
        {
            get { return Recortar(DatosPosteriores); }
        }

        // La lista muestra solo los primeros 40 caracteres; el detalle muestra el texto completo.
        private static string Recortar(string texto)
        {
            if (string.IsNullOrEmpty(texto) || texto.Length <= LargoDeTextoCorto)
            {
                return texto;
            }

            return texto.Substring(0, LargoDeTextoCorto) + "…";
        }
    }
}
