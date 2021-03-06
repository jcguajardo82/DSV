using Soriana.FWK;
using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.DAL.Actualizaciones;
using ServicesManagement.Web.Models.DTO;
using System.Configuration;
using System.Web.Script.Serialization;
using Microsoft.ServiceBus.Messaging;
using RestSharp;
using NPOI.SS.UserModel;
using ServicesManagement.Web.Models.Config;
using ExcelDataReader;
using System.Globalization;

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
        public ActionResult ActualizacionesExcel()
        {
            // EXPORTACION A EXCEL
            string excel = CrearExcel();
            MemoryStream stream = null;

            stream = ExcelTools.LoadAndConvertToMemStream(excel);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename= DSV Macro v1.42.xlsm");

            stream.WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();


            if (System.IO.File.Exists(excel))
                System.IO.File.Delete(excel);
            return View();
        }
        // GET: Actualizaciones
        public string CrearExcel()
        {

            OfficeOpenXml.ExcelWorksheet xlsWorkSheet = null;
            //     OfficeOpenXml.ExcelWorksheet xlsWorkSheetSumary = null;
            OfficeOpenXml.ExcelWorkbook workBook = null;
            OfficeOpenXml.ExcelPackage xlsApp = null;
            string fileSavedPath = "fileName";

            int fila = 2;
            DataSet ds;
            string templatePath = Server.MapPath("~/Files/");

            // OSR SRI.356915 {
            fileSavedPath = ExcelTools.OpenExcelTemplate(ref workBook, ref xlsApp, templatePath, "DSV Macro v1.42");
            ExcelTools.SelectWorkSheet(ref workBook, ref xlsWorkSheet, "Datos");
            xlsWorkSheet.Name = "Datos";

            // ExcelTools.SelectWorkSheet(ref workBook, ref xlsWorkSheetSumary, "Resumen");

            var prov = DALActualizaciones.SuppliersByUser(User.Identity.Name).Tables[0].Rows[0][0].ToString();

            if (prov != "0")
            {
                ds = DALActualizaciones.InventarioPorProveedor(prov);

                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 1, row["Id_Num_CodBarra"].ToString(), false, 11, "Frutiger 45 Light", true,
                               System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                               OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                    ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 2, row["LABST"].ToString(), false, 11, "Frutiger 45 Light", true,
                                                   System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                                                   OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                    ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 3, row["Stock_Seguridad"].ToString(), false, 11, "Frutiger 45 Light", true,
                                                   System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                                                   OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                    ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 4, prov, false, 11, "Frutiger 45 Light", true,
                                                   System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                                                   OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                    ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 5, "1", false, 11, "Frutiger 45 Light", true,
                                                   System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                                                   OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                    fila++;
                }

            }else
                ExcelOriginal(ref xlsApp, ref xlsWorkSheet);


            if (xlsApp != null && xlsWorkSheet != null)
            {
                //string fileName = templateName + "_" + new Random().Next(0, 100000);
                ExcelTools.SaveFile(ref workBook, ref xlsApp);
            }

            return fileSavedPath;
        }
        private void ExcelOriginal(ref OfficeOpenXml.ExcelPackage xlsApp, ref OfficeOpenXml.ExcelWorksheet xlsWorkSheet)
        {
            int fila = 2;
            string proveedor = string.Empty, almacen = string.Empty;

            while (fila < 13)
            {
                if(fila == 2)
                {
                    proveedor = "12699";
                    almacen = "1";
                }
                else
                {
                    proveedor = null;
                    almacen = null;
                }
                ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 1, BarCodeOriginal(fila), false, 11, "Frutiger 45 Light", true,
                           System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                           OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 2, 200, false, 11, "Frutiger 45 Light", true,
                                               System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                                               OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 3, 0, false, 11, "Frutiger 45 Light", true,
                                               System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                                               OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 4, proveedor, false, 11, "Frutiger 45 Light", true,
                                               System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                                               OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 5, almacen, false, 11, "Frutiger 45 Light", true,
                                               System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                                               OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                fila++;
            }
        }

        private string BarCodeOriginal(int fila)
        {
            string barcode = string.Empty;
            if (fila == 2)
                barcode= "7501192151264";
            if (fila == 3)
                barcode = "7501192136728";
            if (fila == 4)
                barcode = "7501192143634";
            if (fila == 5)
                barcode = "7501192151387";
            if (fila == 6)
                barcode = "70896261168";
            if (fila == 7)
                barcode = "70896271969";
            if (fila == 8)
                barcode = "70896699701";
            if (fila == 9)
                barcode = "70896271983";
            if (fila == 10)
                barcode = "7501192151202";
            if (fila == 11)
                barcode = "70896700667";
            if (fila == 12)
                barcode = "7501192151226";

            return barcode;
        }
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

        public ActionResult InsertaArchivo_old(string ArchivoCSV)
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

                //SupplierStockDto supplierStockDto = CreateMessage(fileExcel);

                //SendMessage(supplierStockDto);

                var result = new { Success = true, resp = readString };
                return Json(result, JsonRequestBehavior.AllowGet);


            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult InsertaArchivo(HttpPostedFileBase ArchivoCSV)
        {
            try
            {
                int idSupWH = int.Parse(Request.Form["idSupWH"].ToString());
                int idSupWCode = int.Parse(Request.Form["idSupWCode"].ToString());
                decimal stockL = decimal.Parse(Request.Form["stockL"].ToString());
                decimal stockCod = decimal.Parse(Request.Form["stockCod"].ToString());
                DateTime dTime = Convert.ToDateTime(DateTime.Now.ToString(new CultureInfo("es-MX")), new CultureInfo("es-MX"));
                string StockDate = dTime.AddHours(-6).ToString("dd/MM/yyyy");
               
                TimeSpan time = TimeSpan.FromTicks(dTime.AddHours(-6).Ticks);
                string StockTime = time.Hours.ToString() + ":" + time.Minutes.ToString() + ":" + time.Seconds.ToString();
                //var fileData = GetDataFromFileGetDataFromFileExcel(importFile.InputStream);

                //string[] fileExcel = readString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                //int elemFile = fileExcel.Length;

                var insEnc = DALActualizaciones.SuppliersWHStockHeader_iUP(idSupWH, idSupWCode,
                    stockL, stockCod, StockDate, StockTime);

                var delInv = DALActualizaciones.SuppliersWHStockDetail_dUP(idSupWH, idSupWCode);
                var delDMInv = DALActualizaciones.up_Corp_Vendor_Del_Inventario(idSupWH, idSupWCode);

                var readString = GetDataFromFileExcel(ArchivoCSV.InputStream, idSupWH, idSupWCode, StockDate, StockTime);
                var dt = readString.ToDataTable();

                SupplierStockDto supplierStockDto = CreateMessage(dt, idSupWH, idSupWCode, stockL, stockCod, StockDate, StockTime);

                SendMessage(supplierStockDto);

                var result = new { Success = true, resp = "Carga exitosa!" };
                return Json(result, JsonRequestBehavior.AllowGet);


            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        private List<ProductStock> GetDataFromFileExcel(Stream stream, int idSupWH, int idSupWCode, string StockDate, string StockTime)
        {
            var List = new List<ProductStock>();
            try
            {
                using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
                {
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = false // To set First Row As Column Names    
                        }
                    });

                    if (dataSet.Tables.Count > 0)
                    {
                        var dataTable = dataSet.Tables[0];
                        foreach (DataRow objDataRow in dataTable.Rows)
                        {
                            if (objDataRow.ItemArray.All(x => string.IsNullOrEmpty(x?.ToString()))) continue;
                            if (objDataRow.ItemArray[0].ToString() == "DETAIL")
                            {
                                List.Add(new ProductStock()
                                {
                                    barCode = Convert.ToDecimal(objDataRow[1].ToString()),
                                    stockLvl = Convert.ToDecimal(objDataRow[2].ToString()),
                                    qtyStkS = Convert.ToDecimal(objDataRow[3].ToString()),
                                    qtyStkFSale = 0,
                                    qtyStkRsrvd = Convert.ToDecimal(objDataRow[4].ToString())
                                });
                                var insDMInv = DALActualizaciones.up_Corp_Vendor_Ins_Inventario(idSupWH, idSupWCode, Convert.ToDecimal(objDataRow[1].ToString()), Convert.ToDecimal(objDataRow[2].ToString()),
                                    Convert.ToDecimal(objDataRow[4].ToString()), 0, Convert.ToDecimal(objDataRow[3].ToString()), StockDate, StockTime);
                                int bitInsertDMOk = insDMInv.Tables[0].Rows[0][0].ToString() == "OK" ? 1 : 0;
                                var insInv = DALActualizaciones.SuppliersWHStockDetail_iUP(idSupWH, idSupWCode, Convert.ToDecimal(objDataRow[1].ToString()), Convert.ToDecimal(objDataRow[2].ToString()),
                                    Convert.ToDecimal(objDataRow[4].ToString()), 0, Convert.ToDecimal(objDataRow[3].ToString()), StockDate, StockTime, bitInsertDMOk);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return List;
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

        private SupplierStockDto CreateMessage(DataTable dataTable, int idSupWH, int idSupWCode, decimal stockL, decimal stockCod, string StockDate, string StockTime)
        {
            SupplierStockDto supplierStockDto = new SupplierStockDto();

            supplierStockDto.idSupWH = idSupWH;
            supplierStockDto.idSupWCode = idSupWCode;
            supplierStockDto.stockL = stockL;
            supplierStockDto.stockCod = stockCod;
            supplierStockDto.StockDate = StockDate;
            supplierStockDto.StockTime = StockTime;

            supplierStockDto.Stock = new List<ProductStock>();

            foreach (DataRow objDataRow in dataTable.Rows)
            {
                if (objDataRow.ItemArray.All(x => string.IsNullOrEmpty(x?.ToString()))) continue;
                    ProductStock productStock = new ProductStock();
                    productStock.barCode = Convert.ToDecimal(objDataRow[0].ToString());
                    productStock.stockLvl = Convert.ToDecimal(objDataRow[1].ToString());
                    productStock.qtyStkS = Convert.ToDecimal(objDataRow[3].ToString());
                    productStock.qtyStkFSale = 0;
                    productStock.qtyStkRsrvd = Convert.ToDecimal(objDataRow[4].ToString());

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