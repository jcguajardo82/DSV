using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Globalization;
using OfficeOpenXml;
using System.Drawing;

//using Aspose.Cells;
//using Aspose.Cells.Rendering;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Xml.Linq;
using System.Xml;

namespace ServicesManagement.Web
{
    public static class ExcelTools
    {
        //public static void OpenExcelTemplate(ref Excel.Workbook workBook, ref Excel.Application xlsApp, string templatePath, string templateName)
        //{
        //    xlsApp = new Excel.Application();

        //    workBook = xlsApp.Workbooks.Add(templatePath + templateName + ".xlt");

        //}

        public static string OpenExcelTemplate(ref OfficeOpenXml.ExcelWorkbook workBook, ref OfficeOpenXml.ExcelPackage xlsApp, string templatePath, string templateName)
        {
            string fileName = templatePath + "\\generated_file" + new Random().Next(0, 10000) + ".xlsm";

            FileInfo newFile = new FileInfo(fileName);
            FileInfo fileTemplate = new FileInfo(templatePath + templateName + ".xlsm");

            xlsApp = new OfficeOpenXml.ExcelPackage(newFile, fileTemplate);
            workBook = xlsApp.Workbook;

            return fileName;
        }

        //public static void OpenExcelTemplate(ref Aspose.Cells.Workbook workBook, string templatePath, string templateName)
        //{
        //    Aspose.Cells.License license = new Aspose.Cells.License();
        //    var StreamLicence = License.OLStream;
        //    license.SetLicense(StreamLicence); 

        //    workBook = new Workbook(templatePath + templateName + ".xlsm"); 
        //}


        //public static void SelectWorkSheet(ref Excel.Workbook workBook, ref Excel.Worksheet xlsWorkSheet, string workSheetName)
        //{
        //    if (string.IsNullOrEmpty(workSheetName)) workSheetName = "Hoja1";
        //    if (workSheetName.Length > 30) workSheetName = workSheetName.Substring(0, 29);

        //    xlsWorkSheet = workBook.Worksheets[workSheetName];
        //    xlsWorkSheet.Name = workSheetName;
        //}

        public static void SelectWorkSheet(ref OfficeOpenXml.ExcelWorkbook workBook, ref OfficeOpenXml.ExcelWorksheet xlsWorkSheet, string workSheetName)
        {
            if (string.IsNullOrEmpty(workSheetName)) workSheetName = "Datos";
            if (workSheetName.Length > 30) workSheetName = workSheetName.Substring(0, 29);

            xlsWorkSheet = workBook.Worksheets[workSheetName];
            xlsWorkSheet.Name = workSheetName;
        }

        //public static string SaveFile(ref Excel.Workbook workBook, ref Excel.Application excelApplication, string savePath, string fileName)
        //{


        //    workBook.SaveAs(savePath + fileName + ".xls");

        //    workBook.Close(false);
        //    excelApplication.Quit();

        //    return savePath + fileName + ".xls";
        //}

        public static void SaveFile(ref OfficeOpenXml.ExcelWorkbook workBook, ref OfficeOpenXml.ExcelPackage excelApplication)
        {
            excelApplication.Save();
            excelApplication.Dispose();
        }

        public static MemoryStream SaveFileAS(ref OfficeOpenXml.ExcelWorkbook workBook, ref OfficeOpenXml.ExcelPackage excelApplication)
        {
            MemoryStream file = new MemoryStream();
            excelApplication.SaveAs(file); 
            excelApplication.Dispose();
            return file;
        }

        //public static void InsertCell(ref Excel.Application excelApplication, ref Excel.Worksheet xlsWorkSheet, long row, long column, object cellContent, bool fontBold,
        //                             int fontSize, string font, bool setStyle, Excel.XlColorIndex backColor = Excel.XlColorIndex.xlColorIndexNone, bool border = false, Excel.XlLineStyle borderLine = Excel.XlLineStyle.xlLineStyleNone, Excel.XlBorderWeight borderWitdh = Excel.XlBorderWeight.xlThin, Excel.XlHAlign horizontalAlign = Excel.XlHAlign.xlHAlignCenter)
        //{
        //    string value = string.Empty;

        //    if (cellContent != null)
        //    {
        //        value = Convert.ToString(cellContent, CultureInfo.InvariantCulture);
        //    }

        //    xlsWorkSheet.Cells[row, column].Value = value;
        //    xlsWorkSheet.Cells[row, column].Font.Bold = fontBold;
        //    xlsWorkSheet.Cells[row, column].Font.Size = fontSize;
        //    xlsWorkSheet.Cells[row, column].Font.Name = font;

        //    if (setStyle)
        //    {
        //        xlsWorkSheet.Cells[row, column].Interior.ColorIndex = backColor;
        //        xlsWorkSheet.Cells[row, column].Style.IncludeBorder = border;
        //        xlsWorkSheet.Cells[row, column].Borders.LineStyle = borderLine;
        //        xlsWorkSheet.Cells[row, column].Borders.Weight = borderWitdh;
        //        xlsWorkSheet.Cells[row, column].HorizontalAlignment = horizontalAlign;
        //    }
        //}


        public static void InsertCell2(
                                ref OfficeOpenXml.ExcelPackage excelApplication, ref OfficeOpenXml.ExcelWorksheet xlsWorkSheet,
                                long row, long column, object cellContent, bool fontBold,
                                int fontSize, string font, bool setStyle,
                                System.Drawing.Color backColor, System.Drawing.Color setColor,
                                bool border = false,
                                object borderLine = null,
                                OfficeOpenXml.Style.ExcelBorderStyle borderStyle = OfficeOpenXml.Style.ExcelBorderStyle.Thin,
                                OfficeOpenXml.Style.ExcelHorizontalAlignment horizontalAlign = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center)
        {
            int irow = Convert.ToInt32(row, CultureInfo.InvariantCulture);
            int icolumn = Convert.ToInt32(column, CultureInfo.InvariantCulture);

            var cel = xlsWorkSheet.Cells[irow, icolumn];
            var orow = xlsWorkSheet.Row(irow);
            var ocolum = xlsWorkSheet.Column(icolumn);

            cel.Value = cellContent;
            cel.Style.HorizontalAlignment = horizontalAlign;
            cel.Style.Font.Bold = fontBold;
            cel.Style.Font.Size = fontSize;
            cel.Style.Font.Name = font;
            cel.Style.Font.Color.SetColor(setColor);
            //cel.AutoFitColumns();

            orow.CustomHeight = false;
            //orow.Style.WrapText = true;
            //orow.Style.ShrinkToFit = true;

            ocolum.BestFit = true;
            ocolum.AutoFit();
            //ocolum.Style.WrapText = true;
            //ocolum.Style.ShrinkToFit = true;

            if (setStyle)
            {
                var fill = cel.Style.Fill;
                fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(backColor);

                var oborder = cel.Style.Border;
                oborder.Bottom.Style = borderStyle;
                oborder.Top.Style = borderStyle;
                oborder.Left.Style = borderStyle;
                oborder.Right.Style = borderStyle;
            }
        }

        public static void InsertCell4(
                                ref OfficeOpenXml.ExcelPackage excelApplication, ref OfficeOpenXml.ExcelWorksheet xlsWorkSheet,
                                long row, long column, object cellContent, bool fontBold,
                                int fontSize, string font, bool setStyle,
                                System.Drawing.Color backColor,
                                bool border = false,
                                object borderLine = null,
                                OfficeOpenXml.Style.ExcelBorderStyle borderStyle = OfficeOpenXml.Style.ExcelBorderStyle.Thin,
                                OfficeOpenXml.Style.ExcelHorizontalAlignment horizontalAlign = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center)
        {
            int irow = Convert.ToInt32(row, CultureInfo.InvariantCulture);
            int icolumn = Convert.ToInt32(column, CultureInfo.InvariantCulture);

            var cel = xlsWorkSheet.Cells[irow, icolumn];
            var orow = xlsWorkSheet.Row(irow);
            var ocolum = xlsWorkSheet.Column(icolumn);

            cel.Value = cellContent;
            cel.Style.HorizontalAlignment = horizontalAlign;
            cel.Style.Font.Bold = fontBold;
            cel.Style.Font.Size = fontSize;
            cel.Style.Font.Name = font;
            //cel.AutoFitColumns();

            orow.CustomHeight = false;
            orow.Style.WrapText = true;
            orow.Style.ShrinkToFit = true;

            //ocolum.BestFit = true;
            //ocolum.AutoFit();
            //ocolum.Style.WrapText = true;
            //ocolum.Style.ShrinkToFit = true;

            if (setStyle)
            {
                var fill = cel.Style.Fill;
                fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(backColor);

                var oborder = cel.Style.Border;
                oborder.Bottom.Style = borderStyle;
                oborder.Top.Style = borderStyle;
                oborder.Left.Style = borderStyle;
                oborder.Right.Style = borderStyle;
            }
        }
        public static void CellBorderAndFont(ref OfficeOpenXml.ExcelPackage excelApplication, ref OfficeOpenXml.ExcelWorksheet xlsWorkSheet, string cellAdress,
                                bool fontBold, int fontSize, string font,
                                System.Drawing.Color backColor, bool merge = false,
                                OfficeOpenXml.Style.ExcelBorderStyle topBorderStyle = OfficeOpenXml.Style.ExcelBorderStyle.None,
                                OfficeOpenXml.Style.ExcelBorderStyle leftBorderStyle = OfficeOpenXml.Style.ExcelBorderStyle.None,
                                OfficeOpenXml.Style.ExcelBorderStyle rigthBorderStyle = OfficeOpenXml.Style.ExcelBorderStyle.None,
                                OfficeOpenXml.Style.ExcelBorderStyle bottomBorderStyle = OfficeOpenXml.Style.ExcelBorderStyle.None,
                                OfficeOpenXml.Style.ExcelHorizontalAlignment horizontalAlign = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center,
                                OfficeOpenXml.Style.ExcelVerticalAlignment verticalAlign = OfficeOpenXml.Style.ExcelVerticalAlignment.Top)
        {
            var cel = xlsWorkSheet.Cells[cellAdress];

            cel.Style.HorizontalAlignment = horizontalAlign;
            cel.Style.VerticalAlignment = verticalAlign;
            cel.Style.Font.Bold = fontBold;
            cel.Style.Font.Size = fontSize;
            cel.Style.Font.Name = font;

            cel.Merge = merge;


            var fill = cel.Style.Fill;
            fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(backColor);


            var oborder = cel.Style.Border;
            oborder.Bottom.Style = bottomBorderStyle;
            oborder.Top.Style = topBorderStyle;
            oborder.Left.Style = leftBorderStyle;
            oborder.Right.Style = rigthBorderStyle;

        }


        public static MemoryStream LoadAndConvertToMemStream(string excelPathName)
        {
            byte[] result = System.IO.File.ReadAllBytes(excelPathName);
            return new MemoryStream(result);
        }

        private static readonly HashSet<int> _LockedPosts = new HashSet<int>();
        //private static int print_numbers = 0;
        /// <summary>
        /// Imprime documentos de Excel a partir de los origenes de datos:  FileFormatType, Stream, MemoryStream, string (Ruta completa al archivo)
        /// </summary>
        /// <param name="file">Archivo que se va a imprimir, tipos permitidos: FileFormatType, Stream, MemoryStream, string</param>
        /// <param name="WorksheetIndex">ïndice de la hoja de trabajo que se va a imprimir, valor cero para la primera del libro</param>
        /// <param name="printerName">Nombre de la impresora local donde se envia la impresión</param>
        /// <param name="fileQueueName">Nombre que debe aparecer en la cola de impresión para el archivo que se está imprimiendo</param>
        //public static void PrintExcelWorkSheet(dynamic file, int WorksheetIndex, string printerName, string fileQueueName)
        //{
        //    if (file == null)
        //    {
        //        throw new NullReferenceException("Archivo no espeficicado!.");
        //    }

        //    if (string.IsNullOrEmpty(printerName))
        //    {
        //        throw new NullReferenceException("Impresora no espeficicada!.");
        //    }

        //    if (string.IsNullOrEmpty(fileQueueName))
        //    {
        //        throw new NullReferenceException("Nomobre de archivo para la cola de impresion no espeficicada!.");
        //    }

        //    if (file.GetType() != typeof(FileFormatType) && file.GetType() != typeof(Stream)
        //        && file.GetType() != typeof(MemoryStream) && file.GetType() != typeof(string))
        //    {
        //        throw new FormatException("Formato no aceptado!.");
        //    }

        //    try
        //    {
        //        //Bloquearlo en cada llamada hasta que se mande a imprimir, porque luego se genera error de concurrencia a la impresora con otras impresiones que se envian
        //        lock (_LockedPosts)
        //        {
        //            if (file.GetType() == typeof(MemoryStream) || file.GetType() == typeof(Stream))
        //            {
        //                file.Position = 0;
        //            }
                    
        //            Aspose.Cells.License license = new Aspose.Cells.License();
        //            var lic = License.OLStream;
        //            license.SetLicense(lic); 

        //            Workbook workbook = new Workbook(file);
        //            Worksheet worksheet;
        //            worksheet = workbook.Worksheets[WorksheetIndex];


        //            Aspose.Cells.Rendering.ImageOrPrintOptions options = new Aspose.Cells.Rendering.ImageOrPrintOptions();
        //            options.PrintingPage = PrintingPageType.Default;

        //            SheetRender sr = new SheetRender(worksheet, options);
        //            sr.ToPrinter(printerName, workbook.FileName);

        //            //PrintExcelWorkSheetMemory(file, 0, printerName, fileQueueName, worksheet, null);
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        #region EXTLJB Insertar imagen de Codigo de Barras 28-04-2015
        //public static void InsertImage(ref OfficeOpenXml.ExcelWorksheet xlsWorkSheet,
        //                                Int32 row, Int32 column, Int32 rowOffSetPixel,
        //                                Int32 columnOffSetPixel, MemoryStream image,
        //                                Size imageSize)
        //{
        //    Image returnImage = Image.FromStream(image);
        //    Image imageResize = ImageTool.ResizeImage(returnImage,imageSize,true);

        //    var picture = xlsWorkSheet.Drawings.AddPicture("ImageName", imageResize);
        //    picture.SetPosition(row, rowOffSetPixel, column, columnOffSetPixel);
        //}
        public static void InsertImage(ref OfficeOpenXml.ExcelWorksheet xlsWorkSheet,
                                        Int32 row, Int32 column, Int32 rowOffSetPixel,
                                        Int32 columnOffSetPixel, MemoryStream image)
        {
            Image returnImage = Image.FromStream(image);

            var picture = xlsWorkSheet.Drawings.AddPicture("ImageName", returnImage);
            picture.SetPosition(row, rowOffSetPixel, column, columnOffSetPixel);
        }

        //static SheetRender sr;
        //static PrintDocument pd;
        //public static void PrintExcelWorkSheetMemory(dynamic file, int WorksheetIndex, string printerName, string fileQueueName, Worksheet worksheet, PaperSizeType? paperSize = null)
        //{
        //    if (file == null)
        //    {
        //        throw new NullReferenceException("Archivo no espeficicado!.");
        //    }

        //    if (string.IsNullOrEmpty(printerName))
        //    {
        //        throw new NullReferenceException("Impresora no espeficicada!.");
        //    }

        //    if (string.IsNullOrEmpty(fileQueueName))
        //    {
        //        throw new NullReferenceException("Nomobre de archivo para la cola de impresion no espeficicada!.");
        //    }

        //    if (file.GetType() != typeof(FileFormatType) && file.GetType() != typeof(Stream)
        //        && file.GetType() != typeof(MemoryStream) && file.GetType() != typeof(string))
        //    {
        //        throw new FormatException("Formato no aceptado!.");
        //    }

        //    try
        //    {
        //        //Bloquearlo en cada llamada hasta que se mande a imprimir, porque luego se genera error de concurrencia a la impresora con otras impresiones que se envian
        //        lock (_LockedPosts)
        //        {
        //            _LockedPosts.Add(++print_numbers);

        //            Workbook workbook = new Workbook(file);
        //            worksheet = workbook.Worksheets[WorksheetIndex];
        //            worksheet = workbook.Worksheets[WorksheetIndex];
                    
        //            string pdfFileName = AppPath() + "\\App_Files\\TemplatesForGeneratExcel\\_tmp_" + workbook.FileName + ".pdf";

        //            workbook.Save(pdfFileName, SaveFormat.Pdf);

        //            //Aspose.Cells.Rendering.ImageOrPrintOptions options = new Aspose.Cells.Rendering.ImageOrPrintOptions();
        //            //options.PrintingPage = PrintingPageType.Default; 

        //            //SheetRender sr = new SheetRender(worksheet, options); 
        //            //sr.ToPrinter(printerName, workbook.FileName);

        //            //GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

        //            //GemBox.Spreadsheet.ExcelFile.Load(file, GemBox.Spreadsheet.LoadOptions.XlsxDefault).Print();
                     
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //} 

        public static string AppPath()
        { 

            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath;


            int i = path.IndexOf("\\bin\\");
            path = path.Remove(i);

            i = path.IndexOf("\\bin");
            if (i > 0)
            {
                path = path.Remove(i);
            }

            i = path.IndexOf("//bin//");
            if (i > 0)
            {
                path = path.Remove(i);
            }

            i = path.IndexOf("//bin");
            if (i > 0)
            {
                path = path.Remove(i);
            }

            i = path.IndexOf("/bin/");
            if (i > 0)
            {
                path = path.Remove(i);
            }

            i = path.IndexOf("/bin");
            if (i > 0)
            {
                path = path.Remove(i);
            }


            //XDocument docContactos = XDocument.Load(path + "/App_Data/Printers/Printers.xml"﻿﻿);

            return path;
        }
        #endregion

        #region Convertir XLSx a XML
        public static XmlDocument Xlsx2Xml(MemoryStream mb, string workSheetName) {
            OfficeOpenXml.ExcelPackage xlsApp = new OfficeOpenXml.ExcelPackage(mb);
            var xml = xlsApp.Workbook.Worksheets[workSheetName].WorksheetXml;

            return xml;
        }
        #endregion
    }
}
