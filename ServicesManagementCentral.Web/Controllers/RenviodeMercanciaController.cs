using ServicesManagement.Web.DAL.ReenvioMcia;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.ReemvioMcia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class RenviodeMercanciaController : Controller
    {


        #region RenviodeMercanciaProveedor

        // GET: RenviodeMercancia
        public ActionResult RenviodeMercancia()
        {
            ViewBag.FecIni = DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd");
            ViewBag.FecFin = DateTime.Now.ToString("yyyy/MM/dd");
            return View();
        }

        public ActionResult GetreenviomciaProveedor(DateTime FecIni,DateTime FecFin)
        {
            try
            {
                var list = DataTableToModel.ConvertTo<ReenvioMciaProveedor>(
                    DALReenvioMciaProveedores.upCorpOMS_Cns_UeNoReShipment(User.Identity.Name,FecIni,FecFin).Tables[0]);
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


        #region ReenviodeMercanciaCEDIS

        // GET: RenviodeMercancia
        public ActionResult ReenviodeMercanciaCEDIS()
        {
            return View();
        }

        public ActionResult GetreenviomciaCEDIS(DateTime FecIni,DateTime FecFin)
        {
            try
            {
                var list = DataTableToModel.ConvertTo<ReenvioMciaCEDIS>(
                    DALReenvioMciaCEDIS.upCorpOMS_Cns_UeNoReshipmentCedis(User.Identity.Name,FecIni,FecFin).Tables[0]);
                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult GetDetalleConsignacionCedis(string consignacionR)
        {
            try
            {
                List<DetalleReenvioMciaCEDIS> list = new List<DetalleReenvioMciaCEDIS>();
                var ds = DALReenvioMciaCEDIS.upCorpOMS_Cns_UeNoReshipmentDetail(consignacionR);

                if (ds.Tables.Count > 0)
                    list = DataTableToModel.ConvertTo<DetalleReenvioMciaCEDIS>(ds.Tables[0]);

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