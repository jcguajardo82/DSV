﻿using Soriana.FWK;
using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagement.Web.DAL.Actualizaciones;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.DTO;
using System.Configuration;
using System.Web.Script.Serialization;
using Microsoft.ServiceBus.Messaging;
using RestSharp;
using NPOI.SS.UserModel;
using ServicesManagement.Web.Models.Config;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class ActualizacionesController : Controller
    {
        // GET: Actualizaciones Contenido
        public ActionResult Actualizaciones()
        {
            return View();
        }
        // GET: Actualizaciones
        public ActionResult ActualizacionesContenido()
        {
            return View();
        }

        //upload imagenes
        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object 
            if (Request.Files.Count > 0)
            {
                try
                {

                    string servername = Request.Form["servername"].ToString();
                    string nombre = Request.Form["nombre"].ToString();
                    string extension = Request.Form["extension"].ToString();
                    //string tipoFolder = Request.Form["tipo"].ToString();
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        DateTime dt = DateTime.Now;
                        string dateTime = dt.ToString("yyyyMMddHHmmssfff");

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        //fname = string.Format("{0}_{1}_{2}", nombre, "contenido", dateTime + "." + extension);
                        fname = string.Format("{0}_{1}", nombre, "." + extension);


                        // Get the complete folder path and store the file inside it.  
                        var path = Path.Combine(Server.MapPath("~/Files/"), fname);
                        var pathServerName = servername + "/Files/" + fname;

                        file.SaveAs(path);

                        RestClient restClient = new RestClient(System.Configuration.ConfigurationManager.AppSettings["api_Upload_Files"]);
                        RestRequest restRequest = new RestRequest("/Soriana_Upload_Files?Folder=dsv");
                        restRequest.RequestFormat = DataFormat.Json;
                        restRequest.Method = Method.POST;
                        restRequest.AddHeader("Authorization", "Authorization");
                        restRequest.AddHeader("Content-Type", "multipart/form-data");
                        restRequest.AddFile("content", path);
                        var response = restClient.Execute(restRequest);


                        if (response.StatusDescription.Equals("Bad Request"))
                        {

                            return Json("¡Ocurrio un ERROR al cargar el Archivo!");

                        }

                        // DALManualesOperativos.spManualTitles_iUP(int.Parse(idManual), int.Parse(idOwner), ManualDesc, string.Empty, string.Empty, true, fname, DateTime.Now, User.Identity.Name);

                        //DALManualesOperativos.spManualTitles_iUP(int.Parse(idManual), int.Parse(idOwner), ManualDesc, string.Empty, string.Empty, true, pathServerName, DateTime.Now, User.Identity.Name);
                    }
                    // Returns message that successfully uploaded  
                    return Json("¡Archivo cargado con Exito!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
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
                        decimal.Parse(fileExcel[i - 1]), int.Parse(fileExcel[i]), int.Parse(fileExcel[i + 2]), 0, int.Parse(fileExcel[i + 1]),
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

        public FileResult ExcelInventario()

        {


            var usr = DataTableToModel.ConvertTo<Usuario>(DALConfig.Autenticar_sUP(User.Identity.Name).Tables[0]).FirstOrDefault();

          

            var d = DALActualizaciones.upCorpOMS_Cns_UeNoStock(usr.IdOwner, usr.IdTienda).Tables[0];

            string nombreArchivo = "Inventario";

            //Excel to create an object file

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();


            //Add a sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            IRow rowHead = sheet1.CreateRow(0);

            //Here you can set a variety of styles seemingly font color backgrounds, but not very convenient, there is not set
            //Sheet1 head to add the title of the first row
            for (int i = 0; i < d.Columns.Count; i++)
            {
                rowHead.CreateCell(i, CellType.String).SetCellValue(d.Columns[i].ColumnName.ToString());
            }


            //The data is written progressively sheet1 each row
            for (int i = 0; i < d.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < d.Columns.Count; j++)
                {
                    row.CreateCell(j, CellType.String).SetCellValue(d.Rows[i][j].ToString());
                }
            }


            for (int i = 0; i < d.Columns.Count; i++)
            {
                sheet1.AutoSizeColumn(i);
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
    }

}