using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class DevolucionesController : Controller
    {
        // GET: Devoluciones
        public ActionResult Devoluciones()
        {
            return View();
        }
    }
}