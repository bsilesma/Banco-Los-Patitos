using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoLosPatitos.AccesoADatos.Entidades
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        public int IdComercio { get; set; }

        public Guid? IdNetUser { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombres { get; set; }

        [Required]
        [MaxLength(100)]
        public string PrimerApellido { get; set; }

        [Required]
        [MaxLength(100)]
        public string SegundoApellido { get; set; }

        [Required]
        [MaxLength(10)]
        public string Identificacion { get; set; }

        [Required]
        [MaxLength(200)]
        public string CorreoElectronico { get; set; }

        public DateTime FechaDeRegistro { get; set; }

        public DateTime? FechaDeModificacion { get; set; }

        public bool Estado { get; set; }
    }
}
