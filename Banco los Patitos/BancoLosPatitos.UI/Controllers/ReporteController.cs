using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Reportes.GenerarReportes;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Reportes.ListarReportes;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.LogicaDeNegocio.Reportes.GenerarReportes;
using BancoLosPatitos.LogicaDeNegocio.Reportes.ListarReportes;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BancoLosPatitos.UI.Controllers
{
    public class ReporteController : ControladorBase
    {
        private readonly IListarReportesLN _listarReportes;
        private readonly IGenerarReportesLN _generarReportes;

        public ReporteController()
        {
            _listarReportes = new ListarReportesLN();
            _generarReportes = new GenerarReportesLN();
        }

        protected override string TablaDeEvento
        {
            get { return TablasDeEvento.ReportesMensuales; }
        }

        public ActionResult ListarReportes()
        {
            // Los mensajes vienen de GenerarReportes, que redirige a esta vista.
            ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Error = TempData["Error"];

            try
            {
                List<ReporteMensualDto> laListaDeReportes = _listarReportes.Obtener();

                return View(laListaDeReportes);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);
                ViewBag.Error = MensajeDeErrorInesperado;

                return View(new List<ReporteMensualDto>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerarReportes()
        {
            try
            {
                int comerciosProcesados = _generarReportes.Generar();

                TempData["Mensaje"] = "Se generaron los reportes del mes actual para " + comerciosProcesados + " comercio(s).";
            }
            catch (ReglaDeNegocioException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                RegistrarError(ex);
                TempData["Error"] = MensajeDeErrorInesperado;
            }

            return RedirectToAction("ListarReportes");
        }
    }
}
