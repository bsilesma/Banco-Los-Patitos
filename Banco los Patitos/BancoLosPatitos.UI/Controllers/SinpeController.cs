using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Cajas.ObtenerCaja;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Sinpes.ListarSinpePorCaja;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Sinpes.RegistrarSinpe;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Sinpes.SincronizarSinpe;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.LogicaDeNegocio.Cajas.ObtenerCaja;
using BancoLosPatitos.LogicaDeNegocio.Sinpes.ListarSinpePorCaja;
using BancoLosPatitos.LogicaDeNegocio.Sinpes.RegistrarSinpe;
using BancoLosPatitos.LogicaDeNegocio.Sinpes.SincronizarSinpe;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BancoLosPatitos.UI.Controllers
{
    public class SinpeController : ControladorBase
    {
        private readonly IRegistrarSinpeLN _registrarSinpe;
        private readonly IListarSinpePorCajaLN _listarSinpePorCaja;
        private readonly IObtenerCajaLN _obtenerCaja;
        private readonly ISincronizarSinpeLN _sincronizarSinpe;

        public SinpeController()
        {
            _registrarSinpe = new RegistrarSinpeLN();
            _listarSinpePorCaja = new ListarSinpePorCajaLN();
            _obtenerCaja = new ObtenerCajaLN();
            _sincronizarSinpe = new SincronizarSinpeLN();
        }

        protected override string TablaDeEvento
        {
            get { return TablasDeEvento.Sinpe; }
        }

        public ActionResult ListarSinpePorCaja(int id)
        {
            // Los mensajes vienen de SincronizarSinpe, que redirige a esta vista.
            ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Error = TempData["Error"];

            try
            {
                CajaDto caja = _obtenerCaja.BuscarPorId(id);

                if (caja == null)
                {
                    return HttpNotFound();
                }

                CargarCaja(caja);
                List<SinpeDto> laListaDeSinpe = _listarSinpePorCaja.ObtenerPorTelefono(caja.TelefonoSINPE);

                return View(laListaDeSinpe);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);

                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult RegistrarSinpe()
        {
            return View(new SinpeDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarSinpe(SinpeDto sinpe)
        {
            try
            {
                int idCaja = _registrarSinpe.Registrar(sinpe);

                return RedirectToAction("ListarSinpePorCaja", new { id = idCaja });
            }
            catch (ReglaDeNegocioException ex)
            {
                ViewBag.Error = ex.Message;

                return View(sinpe);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);
                ViewBag.Error = MensajeDeErrorInesperado;

                return View(sinpe);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SincronizarSinpe(int idSinpe, int idCaja)
        {
            try
            {
                _sincronizarSinpe.Sincronizar(idSinpe);

                TempData["Mensaje"] = "El SINPE se sincronizó correctamente.";
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

            return RedirectToAction("ListarSinpePorCaja", new { id = idCaja });
        }

        // La lista muestra a qué caja pertenecen los pagos y permite volver a sus cajas.
        private void CargarCaja(CajaDto caja)
        {
            ViewBag.IdCaja = caja.IdCaja;
            ViewBag.IdComercio = caja.IdComercio;
            ViewBag.NombreDeLaCaja = caja.Nombre;
            ViewBag.TelefonoSINPE = caja.TelefonoSINPE;
        }
    }
}
