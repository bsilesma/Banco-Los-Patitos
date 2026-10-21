using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BancoLosPatitos.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "SINPE Empresarial para comercios afiliados.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Estamos para ayudarle con su afiliación y sus pagos.";

            return View();
        }
    }
}