namespace BancoLosPatitos.Abstracciones.ModelosParaUI
{
    public class EventoBitacoraDto
    {
        public string TablaDeEvento { get; set; }
        public string TipoDeEvento { get; set; }
        public string DescripcionDeEvento { get; set; }
        public string StackTrace { get; set; }
        public object DatosAnteriores { get; set; }
        public object DatosPosteriores { get; set; }
    }
}
