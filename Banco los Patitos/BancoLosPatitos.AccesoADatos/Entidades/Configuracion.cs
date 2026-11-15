using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoLosPatitos.AccesoADatos.Entidades
{
    [Table("Configuraciones")]
    public class Configuracion
    {
        [Key]
        public int IdConfiguracion { get; set; }

        public int IdComercio { get; set; }

        public int TipoConfiguracion { get; set; }

        public int Comision { get; set; }

        public DateTime FechaDeRegistro { get; set; }

        public DateTime? FechaDeModificacion { get; set; }

        public bool Estado { get; set; }
    }
}
