using System;

namespace BancoLosPatitos.Abstracciones.ModelosParaUI
{
    public class ConfiguracionDto
    {
        public int IdConfiguracion { get; set; }
        public int IdComercio { get; set; }
        public string NombreDelComercio { get; set; }
        public int TipoConfiguracion { get; set; }
        public int Comision { get; set; }
        public DateTime FechaDeRegistro { get; set; }
        public DateTime? FechaDeModificacion { get; set; }
        public bool Estado { get; set; }

        public string TipoConfiguracionTexto
        {
            get
            {
                switch (TipoConfiguracion)
                {
                    case 1:
                        return "Plataforma";
                    case 2:
                        return "Externa";
                    case 3:
                        return "Ambas";
                    default:
                        return "Sin clasificar";
                }
            }
        }

        public string EstadoTexto
        {
            get
            {
                return Estado ? "Activo" : "Inactivo";
            }
        }
    }
}
