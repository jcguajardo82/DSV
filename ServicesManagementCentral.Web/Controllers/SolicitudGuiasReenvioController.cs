using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class SolicitudGuiasReenvioController : Controller
    {
        // GET: SolicitudGuiasReenvio
        public ActionResult SolicitudGuiasReenvio()
        {
            return View();
        }
    }
}