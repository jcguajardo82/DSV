using ServicesManagement.Web.DAL.Almacenes;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.Almacenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class AlmacenesController : Controller
    {
        // GET: SolicitudGuiasReenvio
        public ActionResult Almacenes()
        {
            return View();
        }

        public ActionResult GetSuppliers()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<SuppliersWHForAproval>(DALAlmacenes.SuppliersWHForAproval_sUP().Tables[0]);
                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetSuppliersWHStock(string idSupplierWH, string idSupplierWHCode)
        {
            try
            {
                var list = DataTableToModel.ConvertTo<SuppliersWHStock>(DALAlmacenes.SuppliersWHStock_sUP(int.Parse(idSupplierWH), int.Parse(idSupplierWHCode)).Tables[0]);
                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateSuppliersWH(string idSupplierWH, string idSupplierWHCode, string bitenable, string enableCause)
        {
            try
            {
                DALAlmacenes.SuppliersWH_uUP(int.Parse(idSupplierWH), int.Parse(idSupplierWHCode), bool.Parse(bitenable), enableCause, User.Identity.Name);
                var result = new { Success = true };
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