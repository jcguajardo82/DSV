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
    }
}