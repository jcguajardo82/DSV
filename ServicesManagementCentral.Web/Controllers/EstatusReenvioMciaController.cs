using ServicesManagement.Web.DAL.EstatusReenvioMcia;
using ServicesManagement.Web.DAL.ReenvioMcia;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.EstatusReenvioMcia;
using ServicesManagement.Web.Models.ReemvioMcia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class EstatusReenvioMciaController : Controller
    {
        #region EstatusReenvioMcia CEDIS


        // GET: SolicitudGuiasReenvio
        public ActionResult EstatusReenvioMcia()
        {
            return View();

        }

        // GET: RenviodeMercancia

        //public ActionResult GetreenviomciaCEDIS()
        //{
        //    try
        //    {
        //        var list = DataTableToModel.ConvertTo<EstatusReenvioMcia>(DALReenvioMciaCEDIS.upCorpOMS_Cns_UeNoReshipmentCedis().Tables[0]);
        //        var result = new { Success = true, resp = list };
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception x)
        //    {
        //        var result = new { Success = false, Message = x.Message };
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //}

        #endregion

        #region EstatusReenvioMcia DSV


        //GET: SolicitudGuiasReenvio


        //GET: RenviodeMercancia

        public ActionResult GetreenviomciaDSV()
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


        public ActionResult GetDetalleConsignacionDSV(string consignacion)
        {
            try
            {
                List<DetalleEstatusReenvioMcia> list = new List<DetalleEstatusReenvioMcia>();
                var ds = DALReenvioMciaProveedores.upCorpOMS_Cns_UeNoReshipmentDetail(consignacion);

                if (ds.Tables.Count > 0)
                    list = DataTableToModel.ConvertTo<DetalleEstatusReenvioMcia>(ds.Tables[0]);

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