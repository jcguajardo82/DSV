using ServicesManagement.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using ServicesManagement.Web.Helpers;
using System.IO;

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
                //return Json(result, JsonRequestBehavior.AllowGet);
                return new JsonResult { Data = result, MaxJsonLength = Int32.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public FileResult Excel()

        {


            var d = DALServicesM.upCorpOms_Sel_ShipmentProcessedFromWMS().Tables[0];

            string nombreArchivo = "PedidosConGuiasGeneradas";

            //Excel to create an object file

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //Add a sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");


            //Here you can set a variety of styles seemingly font color backgrounds, but not very convenient, there is not set
            //Sheet1 head to add the title of the first row
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("Fecha de Creacion de consignacion");
            row1.CreateCell(1).SetCellValue("Cuenta");
            row1.CreateCell(2).SetCellValue("Cantidad");
            row1.CreateCell(3).SetCellValue("Referencia2");
            row1.CreateCell(4).SetCellValue("Cliente");
            row1.CreateCell(5).SetCellValue("Direccion1");
            row1.CreateCell(6).SetCellValue("Colonia");
            row1.CreateCell(7).SetCellValue("Poblacion");
            row1.CreateCell(8).SetCellValue("Codigo Postal");
            row1.CreateCell(9).SetCellValue("Telefono");
            row1.CreateCell(10).SetCellValue("Contacto");
            row1.CreateCell(11).SetCellValue("PK");
            row1.CreateCell(12).SetCellValue("Guia de Ratreo");
            row1.CreateCell(13).SetCellValue("Paqueteria");

            //The data is written progressively sheet1 each row

            for (int i = 0; i < d.Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(d.Rows[i]["FechaConsignacion"].ToString());
                rowtemp.CreateCell(1).SetCellValue(d.Rows[i]["Cuenta"].ToString());
                rowtemp.CreateCell(2).SetCellValue(d.Rows[i]["cantidad"].ToString());
                rowtemp.CreateCell(3).SetCellValue(d.Rows[i]["referencia2"].ToString());
                rowtemp.CreateCell(4).SetCellValue(d.Rows[i]["siglasCliente"].ToString());
                rowtemp.CreateCell(5).SetCellValue(d.Rows[i]["direccion"].ToString());
                rowtemp.CreateCell(6).SetCellValue(d.Rows[i]["colonia"].ToString());
                rowtemp.CreateCell(7).SetCellValue(d.Rows[i]["poblacion"].ToString());
                rowtemp.CreateCell(8).SetCellValue(d.Rows[i]["codigoPostal"].ToString());
                rowtemp.CreateCell(9).SetCellValue(d.Rows[i]["telefono"].ToString());
                rowtemp.CreateCell(10).SetCellValue(d.Rows[i]["contacto"].ToString());
                rowtemp.CreateCell(11).SetCellValue(d.Rows[i]["ucc"].ToString());
                rowtemp.CreateCell(12).SetCellValue(d.Rows[i]["IdTrackingService"].ToString());
                rowtemp.CreateCell(13).SetCellValue(d.Rows[i]["TrackingServiceName"].ToString());

            }

            //  Write to the client 

            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            book.Write(ms);

            ms.Seek(0, SeekOrigin.Begin);

            DateTime dt = DateTime.Now;

            string dateTime = dt.ToString("yyyyMMddHHmmssfff");

            string fileName = nombreArchivo + "_" + dateTime + ".xls";

            return File(ms, "application/vnd.ms-excel", fileName);

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
        public FileResult ExcelRequests()

        {


            var d = DALServicesM.upCorpOms_Sel_ShipmentRequestsFromWMS().Tables[0];

            string nombreArchivo = "OrdenesPendientesPorEmpacar";

            //Excel to create an object file

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //Add a sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");


            //Here you can set a variety of styles seemingly font color backgrounds, but not very convenient, there is not set
            //Sheet1 head to add the title of the first row
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("Cuenta");
            row1.CreateCell(1).SetCellValue("Cantidad");
            row1.CreateCell(2).SetCellValue("Referencia2");
            row1.CreateCell(3).SetCellValue("Cliente");
            row1.CreateCell(4).SetCellValue("Direccion1");
            row1.CreateCell(5).SetCellValue("Colonia");
            row1.CreateCell(6).SetCellValue("Poblacion");
            row1.CreateCell(7).SetCellValue("Codigo Postal");
            row1.CreateCell(8).SetCellValue("Telefono");
            row1.CreateCell(9).SetCellValue("Contacto");
            row1.CreateCell(10).SetCellValue("PK");

            //The data is written progressively sheet1 each row

            for (int i = 0; i < d.Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(d.Rows[i]["Cuenta"].ToString());
                rowtemp.CreateCell(1).SetCellValue(d.Rows[i]["cantidad"].ToString());
                rowtemp.CreateCell(2).SetCellValue(d.Rows[i]["referencia2"].ToString());
                rowtemp.CreateCell(3).SetCellValue(d.Rows[i]["siglasCliente"].ToString());
                rowtemp.CreateCell(4).SetCellValue(d.Rows[i]["direccion"].ToString());
                rowtemp.CreateCell(5).SetCellValue(d.Rows[i]["colonia"].ToString());
                rowtemp.CreateCell(6).SetCellValue(d.Rows[i]["poblacion"].ToString());
                rowtemp.CreateCell(7).SetCellValue(d.Rows[i]["codigoPostal"].ToString());
                rowtemp.CreateCell(8).SetCellValue(d.Rows[i]["telefono"].ToString());
                rowtemp.CreateCell(9).SetCellValue(d.Rows[i]["contacto"].ToString());
                rowtemp.CreateCell(10).SetCellValue(d.Rows[i]["ucc"].ToString());
            }

            //  Write to the client 

            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            book.Write(ms);

            ms.Seek(0, SeekOrigin.Begin);

            DateTime dt = DateTime.Now;

            string dateTime = dt.ToString("yyyyMMddHHmmssfff");

            string fileName = nombreArchivo + "_" + dateTime + ".xls";

            return File(ms, "application/vnd.ms-excel", fileName);

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