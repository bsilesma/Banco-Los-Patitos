using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoLosPatitos.AccesoADatos.Entidades
{
    [Table("Sinpe")]
    public class Sinpe
    {
        [Key]
        public int IdSinpe { get; set; }

        [Required]
        [MaxLength(10)]
        public string TelefonoOrigen { get; set; }

        [Required]
        [MaxLength(200)]
        public string NombreOrigen { get; set; }

        [Required]
        [MaxLength(10)]
        public string TelefonoDestinatario { get; set; }

        [Required]
        [MaxLength(200)]
        public string NombreDestinatario { get; set; }

        public decimal Monto { get; set; }

        public DateTime FechaDeRegistro { get; set; }

        [MaxLength(50)]
        public string Descripcion { get; set; }

        public bool Estado { get; set; }
    }
}
