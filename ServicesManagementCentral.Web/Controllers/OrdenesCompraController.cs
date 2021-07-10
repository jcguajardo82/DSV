using ServicesManagement.Web.DAL.OrdenesCompra;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.OrdenesCompra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class OrdenesCompraController : Controller
    {
        // GET: OrdenesCompra
        public ActionResult OrdenesCompra()
        {
            List<upCorpOMS_Cns_UeNoShoppingOrders> list = new List<upCorpOMS_Cns_UeNoShoppingOrders>();

           list = DataTableToModel.ConvertTo<upCorpOMS_Cns_UeNoShoppingOrders>(DALOrdenesCompra.upCorpOMS_Cns_UeNoShoppingOrders().Tables[0]);

            ViewBag.Orders = list;

            return View();
        }

      
        public ActionResult GetOrdenesCompraDetalle(string Consignacion)
        {
            try
            {
                List<upCorpOMS_Cns_UeNoShoppingOrdersDetail> list = new List<upCorpOMS_Cns_UeNoShoppingOrdersDetail> ();

                var ds = DALOrdenesCompra.upCorpOMS_Cns_UeNoShoppingOrdersDetail(Consignacion);

                if (ds.Tables.Count > 0)
                {
                    list = DataTableToModel.ConvertTo<upCorpOMS_Cns_UeNoShoppingOrdersDetail>(ds.Tables[0]);
                }
               
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