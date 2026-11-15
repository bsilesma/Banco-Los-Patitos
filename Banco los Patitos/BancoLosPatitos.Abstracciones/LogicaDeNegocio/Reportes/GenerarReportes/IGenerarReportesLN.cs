namespace BancoLosPatitos.Abstracciones.LogicaDeNegocio.Reportes.GenerarReportes
{
    public interface IGenerarReportesLN
    {
        // Genera o actualiza el reporte del mes actual de todos los comercios y devuelve cuántos procesó.
        int Generar();
    }
}
