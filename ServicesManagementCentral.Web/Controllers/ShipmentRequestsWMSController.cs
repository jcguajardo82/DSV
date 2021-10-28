﻿using ServicesManagement.Web.Models;
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
                DataSet ds = DALServicesM.upCorpOms_Sel_ShipmentProcessedFromWMS();
                List<upCorpOms_Sel_ShipmentProcessedFromWMSModel> listC = DataTableToModel.ConvertTo<upCorpOms_Sel_ShipmentProcessedFromWMSModel>(ds.Tables[0]);
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
        public ActionResult GetShipmentPackingWMSByCode(string code, string productId)
        {
            try
            {
                DataSet ds;
                
                //if (code.Equals("STC") || code.Equals("EMB"))
                    ds = DALServicesM.GetDimensionsByProduct(productId);
                //else
                //    ds = DALServicesM.GetShipmentPackingWMSByCode(code);
                List<ProductDimensionsModel> listC = DataTableToModel.ConvertTo<ProductDimensionsModel>(ds.Tables[0]);
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
        public ActionResult UCCProcessed(string ucc)
        {
            try
            {
                DataSet d = DALServicesM.UCCProcesada(ucc);

                var result = new { Success = true, json = "Ok" };
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