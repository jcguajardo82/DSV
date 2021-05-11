using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class AltaProveedorController : Controller
    {
        // GET: ManualesOperativos
        public ActionResult AltaProveedor()
        {
            return View();
        }

        public ActionResult GrabarProveedor()
        {
            try
            {
                var list = "";

                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

    }
}