using ServicesManagement.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using ServicesManagement.Web.Helpers;

namespace ServicesManagement.Web.Controllers
{
    public class ShipmentRequestsWMSController : Controller
    {
        // GET: ShipmentRequestsWMS
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EnviosProcesados()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetShipmentProcessedFromWMS()
        {
            try
            {
                DataSet ds = DALServicesM.upCorpOms_Sel_ShipmentRequestsFromWMS();
                List<ShipmentRequestsFromWMSModel> listC = DataTableToModel.ConvertTo<ShipmentRequestsFromWMSModel>(ds.Tables[0]);
                var result = new { Success = true, json = listC };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult GetShipmentRequestsFromWMS()
        {
            try
            {
                DataSet ds = DALServicesM.upCorpOms_Sel_ShipmentRequestsFromWMS();
                List<ShipmentRequestsFromWMSModel> listC = DataTableToModel.ConvertTo<ShipmentRequestsFromWMSModel>(ds.Tables[0]);
                var result = new { Success = true, json = listC };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult GetShipmentPackingWMSByCode(string code)
        {
            try
            {
                DataSet ds = DALServicesM.GetShipmentPackingWMSByCode(code);
                List<ShipmentPackingModel> listC = DataTableToModel.ConvertTo<ShipmentPackingModel>(ds.Tables[0]);
                var result = new { Success = true, json = listC };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}