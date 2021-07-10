using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class AutorizacionesRMAController : Controller
    {
        // GET: AutorizacionesRMA
        public ActionResult AutorizacionesRMA()
        {
            return View();
        }
    }
}