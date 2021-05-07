using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class ManualesOperativosController : Controller
    {
        // GET: ManualesOperativos
        public ActionResult ManualesOperativos()
        {
            return View();
        }
        public ActionResult ManualesOperativos_CargaManual()
        {
            return View();
        }
    }

}