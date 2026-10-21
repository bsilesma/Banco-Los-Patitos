using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Cajas.ObtenerCaja;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Sinpes.ListarSinpePorCaja;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Sinpes.RegistrarSinpe;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.LogicaDeNegocio.Cajas.ObtenerCaja;
using BancoLosPatitos.LogicaDeNegocio.Sinpes.ListarSinpePorCaja;
using BancoLosPatitos.LogicaDeNegocio.Sinpes.RegistrarSinpe;
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

        public SinpeController()
        {
            _registrarSinpe = new RegistrarSinpeLN();
            _listarSinpePorCaja = new ListarSinpePorCajaLN();
            _obtenerCaja = new ObtenerCajaLN();
        }

        protected override string TablaDeEvento
        {
            get { return TablasDeEvento.Sinpe; }
        }

        public ActionResult ListarSinpePorCaja(int id)
        {
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

        // La lista muestra a qué caja pertenecen los pagos y permite volver a sus cajas.
        private void CargarCaja(CajaDto caja)
        {
            ViewBag.IdComercio = caja.IdComercio;
            ViewBag.NombreDeLaCaja = caja.Nombre;
            ViewBag.TelefonoSINPE = caja.TelefonoSINPE;
        }
    }
}
