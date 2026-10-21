using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.ListarEventos;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.ObtenerEvento;
using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.LogicaDeNegocio.Bitacora.ListarEventos;
using BancoLosPatitos.LogicaDeNegocio.Bitacora.ObtenerEvento;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BancoLosPatitos.UI.Controllers
{
    public class BitacoraController : ControladorBase
    {
        private readonly IListarEventosLN _listarEventos;
        private readonly IObtenerEventoLN _obtenerEvento;

        public BitacoraController()
        {
            _listarEventos = new ListarEventosLN();
            _obtenerEvento = new ObtenerEventoLN();
        }

        protected override string TablaDeEvento
        {
            get { return TablasDeEvento.Bitacora; }
        }

        public ActionResult ListarEventos(int pagina = 1)
        {
            try
            {
                PaginaDeEventosDto laPaginaDeEventos = _listarEventos.ObtenerPagina(pagina);

                return View(laPaginaDeEventos);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);
                ViewBag.Error = MensajeDeErrorInesperado;

                return View(new PaginaDeEventosDto
                {
                    Eventos = new List<EventoRegistradoDto>(),
                    PaginaActual = 1,
                    TotalDePaginas = 1
                });
            }
        }

        public ActionResult DetallesEvento(int id)
        {
            try
            {
                EventoRegistradoDto evento = _obtenerEvento.BuscarPorId(id);

                if (evento == null)
                {
                    return HttpNotFound();
                }

                return View(evento);
            }
            catch (Exception ex)
            {
                RegistrarError(ex);

                return View("Error");
            }
        }
    }
}
