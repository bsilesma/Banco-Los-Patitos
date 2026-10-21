using System;

namespace BancoLosPatitos.Abstracciones.ModelosParaUI
{
    public class SinpeDto
    {
        public int IdSinpe { get; set; }
        public string TelefonoOrigen { get; set; }
        public string NombreOrigen { get; set; }
        public string TelefonoDestinatario { get; set; }
        public string NombreDestinatario { get; set; }

        // Nullable para que el formulario inicie vacío y un monto mal escrito llegue sin valor.
        public decimal? Monto { get; set; }

        public DateTime FechaDeRegistro { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }

        public string MontoTexto
        {
            get
            {
                return Monto.HasValue ? Monto.Value.ToString("N2") : string.Empty;
            }
        }

        public string EstadoTexto
        {
            get
            {
                return Estado ? "Sincronizado" : "No sincronizado";
            }
        }
    }
}
