using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Comercios.EditarComercio;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Comercios.ListarComercios;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Comercios.ObtenerComercio;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Comercios.RegistrarComercio;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.LogicaDeNegocio.Comercios.EditarComercio;
using BancoLosPatitos.LogicaDeNegocio.Comercios.ListarComercios;
using BancoLosPatitos.LogicaDeNegocio.Comercios.ObtenerComercio;
using BancoLosPatitos.LogicaDeNegocio.Comercios.RegistrarComercio;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BancoLosPatitos.UI.Controllers
{
    public class ComercioController : ControladorBase
    {
        private readonly IListarComerciosLN _listarComercios;
        private readonly IObtenerComercioLN _obtenerComercio;
        private readonly IRegistrarComercioLN _registrarComercio;
        private readonly IEditarComercioLN _editarComercio;

        public ComercioController()
        {
            _listarComercios = new ListarComerciosLN();
            _obtenerComercio = new ObtenerComercioLN();
            _registrarComercio = new RegistrarComercioLN();
            _editarComercio = new EditarComercioLN();
        }

        protected override string TablaDeEvento
        {
            get { return TablasDeEvento.Comercios; }
        }

        public ActionResult ListarComercios()
        {
            try
            {
                List<ComercioDto> laListaDeComercios = _listarComercios.Obtener();

                return View(laListaDeComercios);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);
                ViewBag.Error = MensajeDeErrorInesperado;

                return View(new List<ComercioDto>());
            }
        }

        public ActionResult DetallesComercio(int id)
        {
            try
            {
                ComercioDto comercio = _obtenerComercio.BuscarPorId(id);

                if (comercio == null)
                {
                    return HttpNotFound();
                }

                return View(comercio);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);

                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult RegistrarComercio()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarComercio(ComercioDto comercio)
        {
            try
            {
                _registrarComercio.Registrar(comercio);

                return RedirectToAction("ListarComercios");
            }
            catch (ReglaDeNegocioException ex)
            {
                ViewBag.Error = ex.Message;

                return View(comercio);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);
                ViewBag.Error = MensajeDeErrorInesperado;

                return View(comercio);
            }
        }

        [HttpGet]
        public ActionResult EditarComercio(int id)
        {
            try
            {
                ComercioDto comercio = _obtenerComercio.BuscarPorId(id);

                if (comercio == null)
                {
                    return HttpNotFound();
                }

                return View(comercio);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);

                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarComercio(ComercioDto comercio)
        {
            try
            {
                _editarComercio.Editar(comercio);

                return RedirectToAction("ListarComercios");
            }
            catch (ReglaDeNegocioException ex)
            {
                ViewBag.Error = ex.Message;

                return View(comercio);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);
                ViewBag.Error = MensajeDeErrorInesperado;

                return View(comercio);
            }
        }
    }
}
