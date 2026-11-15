using BancoLosPatitos.Abstracciones.ModelosParaUI;
using System.Collections.Generic;

namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Reportes.ListarReportes
{
    public interface IListarReportesLN
    {
        List<ReporteMensualDto> Obtener();
    }
}
