using System;

namespace BancoLosPatitos.Abstracciones.ModelosParaUI
{
    public class ComercioDto
    {
        public int IdComercio { get; set; }
        public string Identificacion { get; set; }
        public int TipoIdentificacion { get; set; }
        public string Nombre { get; set; }
        public int TipoDeComercio { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaDeRegistro { get; set; }
        public DateTime? FechaDeModificacion { get; set; }
        public bool Estado { get; set; }

        public string TipoIdentificacionTexto
        {
            get
            {
                switch (TipoIdentificacion)
                {
                    case 1:
                        return "Física";
                    case 2:
                        return "Jurídica";
                    default:
                        return "Sin clasificar";
                }
            }
        }

        public string TipoDeComercioTexto
        {
            get
            {
                switch (TipoDeComercio)
                {
                    case 1:
                        return "Restaurantes";
                    case 2:
                        return "Supermercados";
                    case 3:
                        return "Ferreterías";
                    case 4:
                        return "Otros";
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
