using System;

namespace BancoLosPatitos.Abstracciones.ModelosParaUI
{
    public class ReporteMensualDto
    {
        public int IdReporte { get; set; }
        public int IdComercio { get; set; }
        public string NombreDelComercio { get; set; }
        public int CantidadDeCajas { get; set; }
        public decimal MontoTotalRecaudado { get; set; }
        public int CantidadDeSINPES { get; set; }
        public decimal MontoTotalComision { get; set; }
        public DateTime FechaDelReporte { get; set; }

        public string MontoTotalRecaudadoTexto
        {
            get { return MontoTotalRecaudado.ToString("N2"); }
        }

        public string MontoTotalComisionTexto
        {
            get { return MontoTotalComision.ToString("N2"); }
        }
    }
}
