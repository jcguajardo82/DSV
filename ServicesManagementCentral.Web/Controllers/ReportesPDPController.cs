using ServicesManagement.Web.Models.ReportesPDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class ReportesPDPController : Controller
    {
        // GET: ReportesPDP
        #region FormaPago

       
        public ActionResult FormaPago()
        {
            return View();
        }

        public ActionResult GetFormaPago()
        {
            try
            {
                var list = new List<ReportesPDP>(); //DataTableToModel.ConvertTo<SuppliersWHForAproval>(DALAlmacenes.SuppliersWHForAproval_sUP().Tables[0]);
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
    }
}