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
        // GET: NivelExistencia - 2021-09-23
        public ActionResult NivelExistencia()
        {
            ViewBag.FecIni = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            ViewBag.FecFin = DateTime.Now.ToString("yyyy/MM/dd");
            ViewBag.IdOwner = 0;
            ViewBag.IdTienda = 0;


            List<upCorpOMS_Cns_UeNoTotalsByOrder> list = new List<upCorpOMS_Cns_UeNoTotalsByOrder>();

            DateTime FecIni = DateTime.Now.AddDays(-7), FecFin = DateTime.Now;
            int IdOwner = 0, IdTienda = 0;

            if (Request.QueryString["FecIni"] != null)
            {
                FecIni = Convert.ToDateTime(Request.QueryString["FecIni"].ToString());
                ViewBag.FecIni = FecIni.ToString("yyyy/MM/dd");
            }

            if (Request.QueryString["FecFin"] != null)
            {
                FecFin = Convert.ToDateTime(Request.QueryString["FecFin"].ToString());
                ViewBag.FecFin = FecFin.ToString("yyyy/MM/dd");
            }

            if (Request.QueryString["IdOwner"] != null)
            {
                IdOwner = int.Parse(Request.QueryString["IdOwner"].ToString());
                ViewBag.IdOwner = IdOwner;
            }

            if (Request.QueryString["IdTienda"] != null)
            {
                IdTienda = int.Parse(Request.QueryString["IdTienda"].ToString());
                ViewBag.IdTienda = IdTienda;
            }

            list = DataTableToModel.ConvertTo<upCorpOMS_Cns_UeNoTotalsByOrder>(
                DALNivelExistencia.upCorpOMS_Cns_UeNoStockLevels(FecIni, FecFin, IdOwner, IdTienda).Tables[0]);

            ViewBag.Orders = list;

            return View();
        }
    }
}