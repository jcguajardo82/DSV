using Soriana.FWK;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagement.Web.DAL.Actualizaciones;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.DTO;
using System.Configuration;
using System.Web.Script.Serialization;
using Microsoft.ServiceBus.Messaging;

namespace ServicesManagement.Web.Controllers
{
    public class ActualizacionesController : Controller
    {
        // GET: Actualizaciones
        public ActionResult Actualizaciones()
        {
            return View();
        }
        public ActionResult InsertaArchivo(string ArchivoCSV)
        {
            try
            {
                String readString = ArchivoCSV;
                string[] fileExcel = readString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                int elemFile = fileExcel.Length;

                var insEnc = DALActualizaciones.SuppliersWHStockHeader_iUP(int.Parse(fileExcel[1]), int.Parse(fileExcel[2]), 
                    int.Parse(fileExcel[fileExcel.Length - 1]), int.Parse(fileExcel[fileExcel.Length - 2]), fileExcel[3], fileExcel[4]);

                var delInv = DALActualizaciones.SuppliersWHStockDetail_dUP(int.Parse(fileExcel[1]), int.Parse(fileExcel[2]));
                var delDMInv = DALActualizaciones.up_Corp_Vendor_Del_Inventario(int.Parse(fileExcel[1]), int.Parse(fileExcel[2]));

                for (int i = 7; i + 9 <= fileExcel.Length; i = i + 5)
                {
                    var insDMInv = DALActualizaciones.up_Corp_Vendor_Ins_Inventario(int.Parse(fileExcel[1]), int.Parse(fileExcel[2]),
                        decimal.Parse(fileExcel[i - 1]), int.Parse(fileExcel[i]), int.Parse(fileExcel[i + 2]), 0, int.Parse(fileExcel[i + 1]),
                        fileExcel[3], fileExcel[4]);
                    int bitInsertDMOk = insDMInv.Tables[0].Rows[0][0].ToString() == "OK" ? 1 : 0; 
                    var insInv = DALActualizaciones.SuppliersWHStockDetail_iUP(int.Parse(fileExcel[1]), int.Parse(fileExcel[2]), 
                        decimal.Parse(fileExcel[i-1]), int.Parse(fileExcel[i]), int.Parse(fileExcel[i + 2]), 0, int.Parse(fileExcel[i + 1]), 
                        fileExcel[3], fileExcel[4], bitInsertDMOk);
                }

                SupplierStockDto supplierStockDto = CreateMessage(fileExcel);

                SendMessage(supplierStockDto);

                var result = new { Success = true, resp = readString };
                return Json(result, JsonRequestBehavior.AllowGet);


            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        private void SendMessage(SupplierStockDto supplierStockDto)
        {
            var json = new JavaScriptSerializer().Serialize(supplierStockDto); 

            string ServiceBusConnectionString = ConfigurationManager.ConnectionStrings["ServiceBus"].ConnectionString;
            string QueueName = ConfigurationSettings.AppSettings["stockQueueName"];
            
            var queueClient = QueueClient.CreateFromConnectionString(ServiceBusConnectionString, QueueName);
            BrokeredMessage message = new BrokeredMessage(json);
            queueClient.Send(message);
        }

        private SupplierStockDto CreateMessage(string[] fileExcel)
        {
            SupplierStockDto supplierStockDto = new SupplierStockDto();

            supplierStockDto.idSupWH = int.Parse(fileExcel[1]);
            supplierStockDto.idSupWCode = int.Parse(fileExcel[2]);
            supplierStockDto.stockL = int.Parse(fileExcel[fileExcel.Length - 1]);
            supplierStockDto.stockCod = int.Parse(fileExcel[fileExcel.Length - 2]);
            supplierStockDto.StockDate = fileExcel[3];
            supplierStockDto.StockTime = fileExcel[4];

            supplierStockDto.Stock = new List<ProductStock>();

            for (int i = 7; i + 9 <= fileExcel.Length; i = i + 5)
            {
                ProductStock productStock = new ProductStock();
                productStock.barCode = decimal.Parse(fileExcel[i - 1]);
                productStock.stockLvl = int.Parse(fileExcel[i]);
                productStock.qtyStkS = int.Parse(fileExcel[i + 2]);
                productStock.qtyStkFSale = 0;
                productStock.qtyStkRsrvd = int.Parse(fileExcel[i + 1]);

                supplierStockDto.Stock.Add(productStock);
            }
            return supplierStockDto;
        }
    }

}