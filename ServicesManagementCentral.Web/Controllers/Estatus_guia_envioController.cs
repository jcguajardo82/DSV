using ServicesManagement.Web.DAL.EstatusGuiasEnvio;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.EstatusGuiasEnvio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class Estatus_guia_envioController : Controller
    {



        #region EstatusGuiaEnvioAdmin


        // GET: Estatus_guia_envio
        public ActionResult EstatusGuiaEnvio()
        {
            return View();
        }


        public ActionResult Getestatusreenvioenviomcia()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<EstatusGuiasEnvio>(DALEstatusGuiasEnvio.upCorpOMS_Cns_UeNoShipmentStatus().Tables[0]);
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

     
        #region EstatusGuiaEnvioCEDIS


        // GET: Estatus_guia_envio
        public ActionResult EstatusGuiaEnvioCEDIS()
        {
            return View();
        }


        public ActionResult GetestatusreenvioenviomciaCEDIS()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<EstatusGuiasEnvio>(DALEstatusGuiasEnvio.upCorpOMS_Cns_UeNoShipmentStatusCEDIS().Tables[0]);
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


        #region EstatusGuiaEnvioProveedor


        // GET: Estatus_guia_envio
        public ActionResult EstatusGuiaEnvioProveedor()
        {
            return View();
        }


        public ActionResult GetestatusreenvioenviomciaProveedor()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<EstatusGuiasEnvio>(DALEstatusGuiasEnvio.upCorpOMS_Cns_UeNoShipmentStatusProveedor().Tables[0]);
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