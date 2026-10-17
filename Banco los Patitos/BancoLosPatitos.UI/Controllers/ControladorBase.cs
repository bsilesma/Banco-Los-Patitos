using BancoLosPatitos.Abstracciones.LogicaDeNegocio.Bitacora.RegistrarEvento;
using BancoLosPatitos.Abstracciones.ModelosParaUI;
using BancoLosPatitos.LogicaDeNegocio.Bitacora.RegistrarEvento;
using System;
using System.Web.Mvc;

namespace BancoLosPatitos.UI.Controllers
{
    public abstract class ControladorBase : Controller
    {
        protected const string MensajeDeErrorInesperado = "Ocurrió un error inesperado. Intente de nuevo.";

        private readonly IRegistrarEventoLN _registrarEvento;

        protected ControladorBase()
        {
            _registrarEvento = new RegistrarEventoLN();
        }

        protected abstract string TablaDeEvento { get; }

        protected void RegistrarError(Exception excepcion)
        {
            try
            {
                _registrarEvento.Registrar(new EventoBitacoraDto
                {
                    TablaDeEvento = TablaDeEvento,
                    TipoDeEvento = TiposDeEvento.Error,
                    DescripcionDeEvento = excepcion.GetBaseException().Message,
                    StackTrace = excepcion.StackTrace
                });
            }
            catch (Exception)
            {
                // Si la bitácora falla, el usuario igual recibe el mensaje de error.
            }
        }
    }
}
