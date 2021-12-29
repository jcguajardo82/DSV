using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagementCentral.Web.Controllers.Api;
using ServicesManagement.Web.Models;
using ServicesManagement.Web.DAL.ActualizacionesInventario;
using ServicesManagement.Web.Helpers;
using System.IO;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class ActualizacionesInventarioController : Controller
    {
        // GET: SolicitudGuiasReenvio
        public ActionResult ActualizacionesInventario()
        {
            // EXPORTACION A EXCEL
            string excel = CrearExcel();
            MemoryStream stream = null;

            stream = ExcelTools.LoadAndConvertToMemStream(excel);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename= Reporte.xlsm");

            stream.WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();


            if (System.IO.File.Exists(excel))
                System.IO.File.Delete(excel);
            return View();
        }

        public ActionResult NiveldeServicio()
        {
            return View();
        }

        public ActionResult NotasdeCredito()
        {
            return View();
        }

        public ActionResult ExportToExcelCVPTL(string WorkOrder)
        {
            string Cv = string.Empty;
            string Partida = string.Empty;

            string GetCVP = string.Empty;

            GetCVP = WorkOrder;
            Cv = GetCVP.Split('/')[0];
            Partida = GetCVP.Split('/')[1];

            // EXPORTACION A EXCEL
            string excel = CrearExcel();
            MemoryStream stream = null;

            stream = ExcelTools.LoadAndConvertToMemStream(excel);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename= Reporte.xlsx");

            stream.WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();


            if (System.IO.File.Exists(excel))
                System.IO.File.Delete(excel);
            return View();
        }

        public string CrearExcel()
        {

            OfficeOpenXml.ExcelWorksheet xlsWorkSheet = null;
            //     OfficeOpenXml.ExcelWorksheet xlsWorkSheetSumary = null;
            OfficeOpenXml.ExcelWorkbook workBook = null;
            OfficeOpenXml.ExcelPackage xlsApp = null;
            string fileSavedPath = "fileName";
            bool mismaFila = false;
            int fila = 1, vuelta = 0, piezas = 0, filaSumas = 0;
            decimal metros = 0, ton = 0;

            string templatePath = Server.MapPath("~/Files/");

            // OSR SRI.356915 {
            fileSavedPath = ExcelTools.OpenExcelTemplate(ref workBook, ref xlsApp, templatePath, "DSV Macro v1.42");
            ExcelTools.SelectWorkSheet(ref workBook, ref xlsWorkSheet, "Datos");
            // ExcelTools.SelectWorkSheet(ref workBook, ref xlsWorkSheetSumary, "Resumen");

            xlsWorkSheet.Name = "Datos";



                                    ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 1, "Chocolate", false, 11, "Frutiger 45 Light", true,
                                               System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                                               OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                                    ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 2, "Chocolate", false, 11, "Frutiger 45 Light", true,
                                                                   System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                                                                   OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                                    ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 3, "Chocolate", false, 11, "Frutiger 45 Light", true,
                                                                   System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                                                                   OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                                    ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 4, "Chocolate", false, 11, "Frutiger 45 Light", true,
                                                                   System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                                                                   OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                                    ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 5, "Chocolate", false, 11, "Frutiger 45 Light", true,
                                                                   System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                                                                   OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

                                    ExcelTools.InsertCell2(ref xlsApp, ref xlsWorkSheet, fila, 6, "Chocolate", false, 11, "Frutiger 45 Light", true,
                                                                   System.Drawing.Color.White, System.Drawing.Color.Black, true, null, OfficeOpenXml.Style.ExcelBorderStyle.Dashed,
                                                                   OfficeOpenXml.Style.ExcelHorizontalAlignment.Center);

            if (xlsApp != null && xlsWorkSheet != null)
            {
                //string fileName = templateName + "_" + new Random().Next(0, 100000);
                ExcelTools.SaveFile(ref workBook, ref xlsApp);
            }

            return fileSavedPath;
        }
    }
}