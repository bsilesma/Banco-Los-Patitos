using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoLosPatitos.AccesoADatos.Entidades
{
    [Table("Comercios")]
    public class Comercio
    {
        [Key]
        public int IdComercio { get; set; }

        [Required]
        [MaxLength(30)]
        public string Identificacion { get; set; }

        public int TipoIdentificacion { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; }

        public int TipoDeComercio { get; set; }

        [Required]
        [MaxLength(20)]
        public string Telefono { get; set; }

        [Required]
        [MaxLength(200)]
        public string CorreoElectronico { get; set; }

        [Required]
        [MaxLength(500)]
        public string Direccion { get; set; }

        public DateTime FechaDeRegistro { get; set; }

        public DateTime? FechaDeModificacion { get; set; }

        public bool Estado { get; set; }
    }
}
