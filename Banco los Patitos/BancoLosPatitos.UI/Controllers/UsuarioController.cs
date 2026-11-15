using BancoLosPatitos.Abstracciones.Excepciones;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Comercios.ObtenerComercio;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Usuarios.EditarUsuario;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Usuarios.ListarUsuarios;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuario;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Usuarios.RegistrarUsuario;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.LogicaDeNegocio.Comercios.ObtenerComercio;
using BancoLosPatitos.LogicaDeNegocio.Usuarios.EditarUsuario;
using BancoLosPatitos.LogicaDeNegocio.Usuarios.ListarUsuarios;
using BancoLosPatitos.LogicaDeNegocio.Usuarios.ObtenerUsuario;
using BancoLosPatitos.LogicaDeNegocio.Usuarios.RegistrarUsuario;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BancoLosPatitos.UI.Controllers
{
    public class UsuarioController : ControladorBase
    {
        private readonly IListarUsuariosLN _listarUsuarios;
        private readonly IObtenerUsuarioLN _obtenerUsuario;
        private readonly IRegistrarUsuarioLN _registrarUsuario;
        private readonly IEditarUsuarioLN _editarUsuario;
        private readonly IObtenerComercioLN _obtenerComercio;

        public UsuarioController()
        {
            _listarUsuarios = new ListarUsuariosLN();
            _obtenerUsuario = new ObtenerUsuarioLN();
            _registrarUsuario = new RegistrarUsuarioLN();
            _editarUsuario = new EditarUsuarioLN();
            _obtenerComercio = new ObtenerComercioLN();
        }

        protected override string TablaDeEvento
        {
            get { return TablasDeEvento.Usuarios; }
        }

        public ActionResult ListarUsuarios(int idComercio)
        {
            try
            {
                ComercioDto comercio = _obtenerComercio.BuscarPorId(idComercio);

                if (comercio == null)
                {
                    return HttpNotFound();
                }

                CargarComercio(comercio);
                List<UsuarioDto> laListaDeUsuarios = _listarUsuarios.ObtenerPorComercio(idComercio);

                return View(laListaDeUsuarios);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);

                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult RegistrarUsuario(int idComercio)
        {
            try
            {
                ComercioDto comercio = _obtenerComercio.BuscarPorId(idComercio);

                if (comercio == null)
                {
                    return HttpNotFound();
                }

                CargarComercio(comercio);

                return View(new UsuarioDto { IdComercio = idComercio });
            }
            catch (Exception ex)
            {
                RegistrarError(ex);

                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarUsuario(UsuarioDto usuario)
        {
            try
            {
                _registrarUsuario.Registrar(usuario);

                return RedirectToAction("ListarUsuarios", new { idComercio = usuario.IdComercio });
            }
            catch (ReglaDeNegocioException ex)
            {
                ViewBag.Error = ex.Message;
                CargarComercio(_obtenerComercio.BuscarPorId(usuario.IdComercio));

                return View(usuario);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);
                ViewBag.Error = MensajeDeErrorInesperado;
                CargarComercio(_obtenerComercio.BuscarPorId(usuario.IdComercio));

                return View(usuario);
            }
        }

        [HttpGet]
        public ActionResult EditarUsuario(int id)
        {
            try
            {
                UsuarioDto usuario = _obtenerUsuario.BuscarPorId(id);

                if (usuario == null)
                {
                    return HttpNotFound();
                }

                CargarComercio(_obtenerComercio.BuscarPorId(usuario.IdComercio));

                return View(usuario);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);

                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario(UsuarioDto usuario)
        {
            try
            {
                _editarUsuario.Editar(usuario);

                return RedirectToAction("ListarUsuarios", new { idComercio = usuario.IdComercio });
            }
            catch (ReglaDeNegocioException ex)
            {
                ViewBag.Error = ex.Message;
                CargarComercio(_obtenerComercio.BuscarPorId(usuario.IdComercio));

                return View(usuario);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);
                ViewBag.Error = MensajeDeErrorInesperado;
                CargarComercio(_obtenerComercio.BuscarPorId(usuario.IdComercio));

                return View(usuario);
            }
        }

        // Las vistas muestran el nombre del comercio al que pertenecen los usuarios.
        private void CargarComercio(ComercioDto comercio)
        {
            ViewBag.IdComercio = comercio == null ? 0 : comercio.IdComercio;
            ViewBag.NombreDelComercio = comercio == null ? string.Empty : comercio.Nombre;
        }
    }
}
