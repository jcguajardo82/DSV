using ServicesManagement.Web.DAL.PriorizacionInventario;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.PriorizacionInventario;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class PrioridadInventarioController : Controller
    {
        // GET: PrioridadInventario
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetPrioridadInventario()
        {
            try
            {
                DataSet ds = DALPriorizacionInventario.PrioridadInventario();
                List<PriorizacionInventarioModel> listC = DataTableToModel.ConvertTo<PriorizacionInventarioModel>(ds.Tables[0]);
                var result = new { Success = true, json = listC };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult PrioridadInventario(string json)
        {
            try
            {
                var result= new Object();
                DataSet ds = DALPriorizacionInventario.PrioridadInventario(json, User.Identity.Name);

                if (ds.Tables[0].Rows[0][0].ToString().Equals("OK"))
                    result = new { Success = true };
                else
                    result = new { Success = false, Message = "Hubo un error al Insertar" };
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