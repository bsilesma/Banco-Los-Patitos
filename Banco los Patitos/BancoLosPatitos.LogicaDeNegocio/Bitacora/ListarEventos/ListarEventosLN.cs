using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.ListarEventos;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Bitacora.ListarEventos
{
    public class ListarEventosLN : IListarEventosLN
    {
        private const int EventosPorPagina = 10;

        public PaginaDeEventosDto ObtenerPagina(int pagina)
        {
            using (var contexto = new BancoLosPatitosContext())
            {
                int totalDeEventos = contexto.BitacoraEventos.Count();
                int totalDePaginas = Math.Max(1, (totalDeEventos + EventosPorPagina - 1) / EventosPorPagina);

                // Una página fuera de rango se ajusta a la primera o a la última.
                int paginaActual = Math.Min(Math.Max(pagina, 1), totalDePaginas);

                List<EventoRegistradoDto> eventos = contexto.BitacoraEventos
                    .OrderByDescending(x => x.FechaDeEvento)
                    .ThenByDescending(x => x.IdEvento)
                    .Skip((paginaActual - 1) * EventosPorPagina)
                    .Take(EventosPorPagina)
                    .Select(x => new EventoRegistradoDto
                    {
                        IdEvento = x.IdEvento,
                        TablaDeEvento = x.TablaDeEvento,
                        TipoDeEvento = x.TipoDeEvento,
                        FechaDeEvento = x.FechaDeEvento,
                        DescripcionDeEvento = x.DescripcionDeEvento,
                        StackTrace = x.StackTrace,
                        DatosAnteriores = x.DatosAnteriores,
                        DatosPosteriores = x.DatosPosteriores
                    })
                    .ToList();

                return new PaginaDeEventosDto
                {
                    Eventos = eventos,
                    PaginaActual = paginaActual,
                    TotalDePaginas = totalDePaginas
                };
            }
        }
    }
}
