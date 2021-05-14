using Soriana.FWK;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class ActualizacionesController : Controller
    {
        // GET: Actualizaciones
        public ActionResult Actualizaciones()
        {
            return View();
        }
        public ActionResult InsertaArchivo(string ArchivoCSV)
        {
            try
            {
                var archivoTmp = ArchivoCSV;
                var result = new { Success = true, resp = archivoTmp };
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