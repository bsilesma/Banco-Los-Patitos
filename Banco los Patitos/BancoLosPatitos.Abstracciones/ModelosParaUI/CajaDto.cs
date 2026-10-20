using System;

namespace BancoLosPatitos.Abstracciones.ModelosParaUI
{
    public class CajaDto
    {
        private const int LargoDeDescripcionCorta = 10;

        public int IdCaja { get; set; }
        public int IdComercio { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string TelefonoSINPE { get; set; }
        public DateTime FechaDeRegistro { get; set; }
        public DateTime? FechaDeModificacion { get; set; }
        public bool Estado { get; set; }

        // La lista de cajas solo muestra los primeros 10 caracteres de la descripción.
        public string DescripcionCorta
        {
            get
            {
                if (string.IsNullOrEmpty(Descripcion) || Descripcion.Length <= LargoDeDescripcionCorta)
                {
                    return Descripcion;
                }

                return Descripcion.Substring(0, LargoDeDescripcionCorta) + "…";
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
