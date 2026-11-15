using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Configuraciones.EditarConfiguracion;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Configuraciones.ListarComerciosSinConfiguracion;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Configuraciones.ListarConfiguraciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Configuraciones.ObtenerConfiguracion;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Configuraciones.RegistrarConfiguracion;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.LogicaDeNegocio.Configuraciones.EditarConfiguracion;
using BancoLosPatitos.LogicaDeNegocio.Configuraciones.ListarComerciosSinConfiguracion;
using BancoLosPatitos.LogicaDeNegocio.Configuraciones.ListarConfiguraciones;
using BancoLosPatitos.LogicaDeNegocio.Configuraciones.ObtenerConfiguracion;
using BancoLosPatitos.LogicaDeNegocio.Configuraciones.RegistrarConfiguracion;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BancoLosPatitos.UI.Controllers
{
    public class ConfiguracionController : ControladorBase
    {
        private readonly IListarConfiguracionesLN _listarConfiguraciones;
        private readonly IObtenerConfiguracionLN _obtenerConfiguracion;
        private readonly IRegistrarConfiguracionLN _registrarConfiguracion;
        private readonly IEditarConfiguracionLN _editarConfiguracion;
        private readonly IListarComerciosSinConfiguracionLN _listarComerciosSinConfiguracion;

        public ConfiguracionController()
        {
            _listarConfiguraciones = new ListarConfiguracionesLN();
            _obtenerConfiguracion = new ObtenerConfiguracionLN();
            _registrarConfiguracion = new RegistrarConfiguracionLN();
            _editarConfiguracion = new EditarConfiguracionLN();
            _listarComerciosSinConfiguracion = new ListarComerciosSinConfiguracionLN();
        }

        protected override string TablaDeEvento
        {
            get { return TablasDeEvento.Configuraciones; }
        }

        public ActionResult ListarConfiguraciones()
        {
            try
            {
                List<ConfiguracionDto> laListaDeConfiguraciones = _listarConfiguraciones.Obtener();

                return View(laListaDeConfiguraciones);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);
                ViewBag.Error = MensajeDeErrorInesperado;

                return View(new List<ConfiguracionDto>());
            }
        }

        [HttpGet]
        public ActionResult RegistrarConfiguracion()
        {
            try
            {
                CargarComerciosDisponibles();

                return View();
            }
            catch (Exception ex)
            {
                RegistrarError(ex);

                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarConfiguracion(ConfiguracionDto configuracion)
        {
            try
            {
                _registrarConfiguracion.Registrar(configuracion);

                return RedirectToAction("ListarConfiguraciones");
            }
            catch (ReglaDeNegocioException ex)
            {
                ViewBag.Error = ex.Message;
                CargarComerciosDisponibles();

                return View(configuracion);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);
                ViewBag.Error = MensajeDeErrorInesperado;
                CargarComerciosDisponibles();

                return View(configuracion);
            }
        }

        [HttpGet]
        public ActionResult EditarConfiguracion(int id)
        {
            try
            {
                ConfiguracionDto configuracion = _obtenerConfiguracion.BuscarPorId(id);

                if (configuracion == null)
                {
                    return HttpNotFound();
                }

                return View(configuracion);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);

                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarConfiguracion(ConfiguracionDto configuracion)
        {
            try
            {
                _editarConfiguracion.Editar(configuracion);

                return RedirectToAction("ListarConfiguraciones");
            }
            catch (ReglaDeNegocioException ex)
            {
                ViewBag.Error = ex.Message;

                return View(configuracion);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);
                ViewBag.Error = MensajeDeErrorInesperado;

                return View(configuracion);
            }
        }

        // El formulario de registro solo ofrece comercios activos que aún no tienen configuración.
        private void CargarComerciosDisponibles()
        {
            List<ComercioDto> comercios = _listarComerciosSinConfiguracion.Obtener();

            ViewBag.Comercios = new SelectList(comercios, "IdComercio", "Nombre");
        }
    }
}
