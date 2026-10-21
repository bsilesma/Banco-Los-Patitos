using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.ObtenerEvento;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Bitacora.ObtenerEvento
{
    public class ObtenerEventoLN : IObtenerEventoLN
    {
        public EventoRegistradoDto BuscarPorId(int idEvento)
        {
            using (var contexto = new BancoLosPatitosContext())
            {
                return contexto.BitacoraEventos
                    .Where(x => x.IdEvento == idEvento)
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
                    .FirstOrDefault();
            }
        }
    }
}
