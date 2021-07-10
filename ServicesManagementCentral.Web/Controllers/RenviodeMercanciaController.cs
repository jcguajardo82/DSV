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
            return View();
        }

        public ActionResult GetreenviomciaProveedor()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<ReenvioMciaProveedor>(DALReenvioMciaProveedores.upCorpOMS_Cns_UeNoReShipment().Tables[0]);
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

        public ActionResult GetreenviomciaCEDIS()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<ReenvioMciaCEDIS>(DALReenvioMciaCEDIS.upCorpOMS_Cns_UeNoReshipmentCedis().Tables[0]);
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