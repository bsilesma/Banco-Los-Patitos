using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Cajas.EditarCaja;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Cajas.ListarCajas;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Cajas.ObtenerCaja;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Cajas.RegistrarCaja;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Comercios.ObtenerComercio;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.LogicaDeNegocio.Cajas.EditarCaja;
using BancoLosPatitos.LogicaDeNegocio.Cajas.ListarCajas;
using BancoLosPatitos.LogicaDeNegocio.Cajas.ObtenerCaja;
using BancoLosPatitos.LogicaDeNegocio.Cajas.RegistrarCaja;
using BancoLosPatitos.LogicaDeNegocio.Comercios.ObtenerComercio;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BancoLosPatitos.UI.Controllers
{
    public class CajaController : ControladorBase
    {
        private readonly IListarCajasLN _listarCajas;
        private readonly IObtenerCajaLN _obtenerCaja;
        private readonly IRegistrarCajaLN _registrarCaja;
        private readonly IEditarCajaLN _editarCaja;
        private readonly IObtenerComercioLN _obtenerComercio;

        public CajaController()
        {
            _listarCajas = new ListarCajasLN();
            _obtenerCaja = new ObtenerCajaLN();
            _registrarCaja = new RegistrarCajaLN();
            _editarCaja = new EditarCajaLN();
            _obtenerComercio = new ObtenerComercioLN();
        }

        protected override string TablaDeEvento
        {
            get { return TablasDeEvento.Cajas; }
        }

        public ActionResult ListarCajas(int idComercio)
        {
            try
            {
                ComercioDto comercio = _obtenerComercio.BuscarPorId(idComercio);

                if (comercio == null)
                {
                    return HttpNotFound();
                }

                CargarComercio(comercio);
                List<CajaDto> laListaDeCajas = _listarCajas.ObtenerPorComercio(idComercio);

                return View(laListaDeCajas);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);

                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult RegistrarCaja(int idComercio)
        {
            try
            {
                ComercioDto comercio = _obtenerComercio.BuscarPorId(idComercio);

                if (comercio == null)
                {
                    return HttpNotFound();
                }

                CargarComercio(comercio);

                return View(new CajaDto { IdComercio = idComercio });
            }
            catch (Exception ex)
            {
                RegistrarError(ex);

                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarCaja(CajaDto caja)
        {
            try
            {
                _registrarCaja.Registrar(caja);

                return RedirectToAction("ListarCajas", new { idComercio = caja.IdComercio });
            }
            catch (ReglaDeNegocioException ex)
            {
                ViewBag.Error = ex.Message;
                CargarComercio(_obtenerComercio.BuscarPorId(caja.IdComercio));

                return View(caja);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);
                ViewBag.Error = MensajeDeErrorInesperado;

                return View(caja);
            }
        }

        [HttpGet]
        public ActionResult EditarCaja(int id)
        {
            try
            {
                CajaDto caja = _obtenerCaja.BuscarPorId(id);

                if (caja == null)
                {
                    return HttpNotFound();
                }

                CargarComercio(_obtenerComercio.BuscarPorId(caja.IdComercio));

                return View(caja);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);

                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCaja(CajaDto caja)
        {
            try
            {
                _editarCaja.Editar(caja);

                return RedirectToAction("ListarCajas", new { idComercio = caja.IdComercio });
            }
            catch (ReglaDeNegocioException ex)
            {
                ViewBag.Error = ex.Message;
                CargarComercio(_obtenerComercio.BuscarPorId(caja.IdComercio));

                return View(caja);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);
                ViewBag.Error = MensajeDeErrorInesperado;

                return View(caja);
            }
        }

        // Las vistas muestran el nombre del comercio al que pertenecen las cajas.
        private void CargarComercio(ComercioDto comercio)
        {
            ViewBag.IdComercio = comercio == null ? 0 : comercio.IdComercio;
            ViewBag.NombreDelComercio = comercio == null ? string.Empty : comercio.Nombre;
        }
    }
}
