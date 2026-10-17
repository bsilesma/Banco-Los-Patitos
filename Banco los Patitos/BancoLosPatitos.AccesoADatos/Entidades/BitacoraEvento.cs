using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoLosPatitos.AccesoADatos.Entidades
{
    [Table("BITACORA_EVENTOS")]
    public class BitacoraEvento
    {
        [Key]
        public int IdEvento { get; set; }

        [Required]
        [MaxLength(20)]
        public string TablaDeEvento { get; set; }

        [Required]
        [MaxLength(20)]
        public string TipoDeEvento { get; set; }

        public DateTime FechaDeEvento { get; set; }

        [Required]
        public string DescripcionDeEvento { get; set; }

        [Required]
        public string StackTrace { get; set; }

        public string DatosAnteriores { get; set; }

        public string DatosPosteriores { get; set; }
    }
}
