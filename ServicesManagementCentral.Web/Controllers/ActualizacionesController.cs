using Soriana.FWK;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagement.Web.DAL.Actualizaciones;
using ServicesManagement.Web.Helpers;

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

                for (int i = 7; i + 9 <= fileExcel.Length; i = i + 5)
                {
                    var insInv = DALActualizaciones.SuppliersWHStockDetail_iUP(int.Parse(fileExcel[1]), int.Parse(fileExcel[2]), 
                        decimal.Parse(fileExcel[i-1]), int.Parse(fileExcel[i]), int.Parse(fileExcel[i + 2]), 0, int.Parse(fileExcel[i + 1]), 
                        fileExcel[3], fileExcel[4]);
                }

                var result = new { Success = true, resp = readString };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }

}