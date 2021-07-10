using ServicesManagement.Web.DAL.NivelExistencia;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.NivelExistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class NivelExistenciaController : Controller
    {
        // GET: NivelExistencia
        public ActionResult NivelExistencia()
        {
            List<upCorpOMS_Cns_UeNoTotalsByOrder> list = new List<upCorpOMS_Cns_UeNoTotalsByOrder>();

            list = DataTableToModel.ConvertTo<upCorpOMS_Cns_UeNoTotalsByOrder>(DALNivelExistencia.upCorpOMS_Cns_UeNoStockLevels().Tables[0]);

            ViewBag.Orders = list;

            return View();
        }
    }
}