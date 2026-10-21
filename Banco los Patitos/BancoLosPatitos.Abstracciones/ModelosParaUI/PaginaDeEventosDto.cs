using System.Collections.Generic;

namespace BancoLosPatitos.Abstracciones.ModelosParaUI
{
    public class PaginaDeEventosDto
    {
        public List<EventoRegistradoDto> Eventos { get; set; }
        public int PaginaActual { get; set; }
        public int TotalDePaginas { get; set; }

        public bool TieneAnterior
        {
            get { return PaginaActual > 1; }
        }

        public bool TieneSiguiente
        {
            get { return PaginaActual < TotalDePaginas; }
        }
    }
}
