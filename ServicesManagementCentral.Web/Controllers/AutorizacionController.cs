using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class AutorizacionController : Controller
    {
        // GET: Autorizacion
        public ActionResult Autorizacion()
        {
            return View();
        }
    }
}