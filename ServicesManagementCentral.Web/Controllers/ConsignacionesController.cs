using System;
using ServicesManagement.Web.DAL.Consignaciones;
using ServicesManagement.Web.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagement.Web.Models.Consignaciones;

namespace ServicesManagement.Web.Controllers
{
    public class ConsignacionesController : Controller
    {


        #region Consignaciones Administrador
        public ActionResult Consignaciones()
        {
            return View();
        }

        public ActionResult GetconsignacionesAdm()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<ConsignacionesAdm>(DALConsignacionesAdm.upCorpAlmacen_Cns_Consigments().Tables[0]);
                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        public ActionResult Consignaciones_Proveedores()
        {
            return View();
        }


        public ActionResult Consignaciones_CEDIS()
        {
            return View();
        }
    }
}