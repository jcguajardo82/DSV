using ServicesManagement.Web.DAL.TotalOrdenes;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.TotalOrdenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class TotalOrdenesController : Controller
    {
        // GET: SolicitudGuiasReenvio
        public ActionResult TotalOrdenes()
        {
            List<upCorpOMS_Cns_UeNoTotalsByOrder> list = new List<upCorpOMS_Cns_UeNoTotalsByOrder>();

            list = DataTableToModel.ConvertTo<upCorpOMS_Cns_UeNoTotalsByOrder>(DALTotalOrdenes.upCorpOMS_Cns_UeNoTotalsByOrder().Tables[0]);

            ViewBag.Orders = list;

            return View();
        }
    }
}