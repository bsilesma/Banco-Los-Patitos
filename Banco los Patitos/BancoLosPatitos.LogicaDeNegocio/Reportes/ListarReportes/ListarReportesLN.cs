using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Reportes.ListarReportes;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using System.Collections.Generic;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Reportes.ListarReportes
{
    public class ListarReportesLN : IListarReportesLN
    {
        public List<ReporteMensualDto> Obtener()
        {
            using (var contexto = new BancoLosPatitosContext())
            {
                // Se une con Comercios para mostrar el nombre; los más recientes primero.
                return contexto.ReportesMensuales
                    .Join(contexto.Comercios,
                        reporte => reporte.IdComercio,
                        comercio => comercio.IdComercio,
                        (reporte, comercio) => new ReporteMensualDto
                        {
                            IdReporte = reporte.IdReporte,
                            IdComercio = reporte.IdComercio,
                            NombreDelComercio = comercio.Nombre,
                            CantidadDeCajas = reporte.CantidadDeCajas,
                            MontoTotalRecaudado = reporte.MontoTotalRecaudado,
                            CantidadDeSINPES = reporte.CantidadDeSINPES,
                            MontoTotalComision = reporte.MontoTotalComision,
                            FechaDelReporte = reporte.FechaDelReporte
                        })
                    .OrderByDescending(x => x.FechaDelReporte)
                    .ThenBy(x => x.NombreDelComercio)
                    .ToList();
            }
        }
    }
}
