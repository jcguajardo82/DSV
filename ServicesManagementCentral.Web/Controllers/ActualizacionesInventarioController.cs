using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class ActualizacionesInventarioController : Controller
    {
        // GET: SolicitudGuiasReenvio
        public ActionResult ActualizacionesInventario()
        {
            return View();
        }

        public ActionResult NiveldeServicio()
        {
            return View();
        }

        public ActionResult NotasdeCredito()
        {
            return View();
        }
    }
}