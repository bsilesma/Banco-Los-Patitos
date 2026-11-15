using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Reportes.GenerarReportes;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.AccesoADatos.Contexto;
using BancoLosPatitos.AccesoADatos.Entidades;
using BancoLosPatitos.LogicaDeNegocio.Bitacora.RegistrarEvento;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BancoLosPatitos.LogicaDeNegocio.Reportes.GenerarReportes
{
    public class GenerarReportesLN : IGenerarReportesLN
    {
        private const decimal DivisorDePorcentaje = 100m;

        public int Generar()
        {
            DateTime ahora = DateTime.Now;
            DateTime inicioDelMes = new DateTime(ahora.Year, ahora.Month, 1);
            DateTime inicioDelMesSiguiente = inicioDelMes.AddMonths(1);

            using (var contexto = new BancoLosPatitosContext())
            using (var transaccion = contexto.Database.BeginTransaction())
            {
                try
                {
                    IRegistrarEventoLN bitacora = new RegistrarEventoLN(contexto);
                    List<Comercio> comercios = contexto.Comercios.OrderBy(x => x.IdComercio).ToList();

                    foreach (Comercio comercio in comercios)
                    {
                        ReporteMensual calculado = CalcularReporte(contexto, comercio.IdComercio, inicioDelMes, inicioDelMesSiguiente, ahora);
                        GuardarReporte(contexto, bitacora, comercio, calculado, inicioDelMes, inicioDelMesSiguiente);
                    }

                    transaccion.Commit();

                    return comercios.Count;
                }
                catch
                {
                    transaccion.Rollback();
                    throw;
                }
            }
        }

        private static ReporteMensual CalcularReporte(BancoLosPatitosContext contexto, int idComercio, DateTime inicioDelMes, DateTime inicioDelMesSiguiente, DateTime fechaDelReporte)
        {
            // Los SINPE llegan al teléfono de la caja, por eso se buscan por los teléfonos del comercio.
            List<string> telefonos = contexto.Cajas
                .Where(x => x.IdComercio == idComercio)
                .Select(x => x.TelefonoSINPE)
                .Distinct()
                .ToList();

            var sinpesDelMes = contexto.Sinpes
                .Where(x => telefonos.Contains(x.TelefonoDestinatario)
                    && x.FechaDeRegistro >= inicioDelMes
                    && x.FechaDeRegistro < inicioDelMesSiguiente);

            int cantidadDeCajas = contexto.Cajas.Count(x => x.IdComercio == idComercio);
            int cantidadDeSinpes = sinpesDelMes.Count();
            // Sum sobre decimal? evita el error de EF cuando no hay filas.
            decimal montoRecaudado = sinpesDelMes.Select(x => (decimal?)x.Monto).Sum() ?? 0m;

            Configuracion configuracion = contexto.Configuraciones
                .FirstOrDefault(x => x.IdComercio == idComercio);
            int porcentajeDeComision = configuracion == null ? 0 : configuracion.Comision;

            return new ReporteMensual
            {
                IdComercio = idComercio,
                CantidadDeCajas = cantidadDeCajas,
                MontoTotalRecaudado = montoRecaudado,
                CantidadDeSINPES = cantidadDeSinpes,
                MontoTotalComision = Math.Round(montoRecaudado * porcentajeDeComision / DivisorDePorcentaje, 2),
                FechaDelReporte = fechaDelReporte
            };
        }

        // Si ya hay reporte del comercio en el mes actual se actualiza; si no, se crea.
        private static void GuardarReporte(BancoLosPatitosContext contexto, IRegistrarEventoLN bitacora, Comercio comercio, ReporteMensual calculado, DateTime inicioDelMes, DateTime inicioDelMesSiguiente)
        {
            ReporteMensual existente = contexto.ReportesMensuales
                .FirstOrDefault(x => x.IdComercio == comercio.IdComercio
                    && x.FechaDelReporte >= inicioDelMes
                    && x.FechaDelReporte < inicioDelMesSiguiente);

            if (existente == null)
            {
                contexto.ReportesMensuales.Add(calculado);
                contexto.SaveChanges();

                bitacora.Registrar(new EventoBitacoraDto
                {
                    TablaDeEvento = TablasDeEvento.ReportesMensuales,
                    TipoDeEvento = TiposDeEvento.Registrar,
                    DescripcionDeEvento = "Se generó el reporte mensual del comercio " + comercio.Nombre + ".",
                    DatosAnteriores = calculado
                });

                return;
            }

            ReporteMensual anterior = Copiar(existente);

            existente.CantidadDeCajas = calculado.CantidadDeCajas;
            existente.MontoTotalRecaudado = calculado.MontoTotalRecaudado;
            existente.CantidadDeSINPES = calculado.CantidadDeSINPES;
            existente.MontoTotalComision = calculado.MontoTotalComision;
            existente.FechaDelReporte = calculado.FechaDelReporte;

            contexto.SaveChanges();

            bitacora.Registrar(new EventoBitacoraDto
            {
                TablaDeEvento = TablasDeEvento.ReportesMensuales,
                TipoDeEvento = TiposDeEvento.Editar,
                DescripcionDeEvento = "Se actualizó el reporte mensual del comercio " + comercio.Nombre + ".",
                DatosAnteriores = anterior,
                DatosPosteriores = existente
            });
        }

        // Copia los valores antes de modificarlos, porque la entidad original cambia al actualizar.
        private static ReporteMensual Copiar(ReporteMensual reporte)
        {
            return new ReporteMensual
            {
                IdReporte = reporte.IdReporte,
                IdComercio = reporte.IdComercio,
                CantidadDeCajas = reporte.CantidadDeCajas,
                MontoTotalRecaudado = reporte.MontoTotalRecaudado,
                CantidadDeSINPES = reporte.CantidadDeSINPES,
                MontoTotalComision = reporte.MontoTotalComision,
                FechaDelReporte = reporte.FechaDelReporte
            };
        }
    }
}
