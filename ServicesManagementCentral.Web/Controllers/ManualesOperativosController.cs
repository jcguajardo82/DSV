using ServicesManagementCentral.Web.Models.SubirArchivos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using ServicesManagement.Web.DAL.Almacenes;
using ServicesManagement.Web.Models.Almacenes;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.DAL.ManualesOperativos;
using ServicesManagement.Web.Models;

namespace ServicesManagement.Web.Controllers
{
    public class ManualesOperativosController : Controller
    {

        // GET: ManualesOperativos
        public ActionResult ManualesOperativos()
        {
            return View();
        }
        public ActionResult ManualesOperativos_CargaManual()
        {
           // var list = DALManualesOperativos.Llenartipoalmacen();
           var list = DataTableToModel.ConvertTo<ManualesOperativosModels>(DALManualesOperativos.Llenartipoalmacen().Tables[0]);
            ViewBag.Owners = list;

            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();
            return View();

            // GET: Archivo

        }
        [HttpPost]
        public ActionResult Save(CargaManuales model)
        {
            string RutaSitio = Server.MapPath("~/");
            string PathExaminar = Path.Combine(RutaSitio + "/Files/Examinar.pdf");


            if (!ModelState.IsValid)
            {
                return View("ManualesOperativos_CargaManual", model);
            }

            model.Examinar.SaveAs(PathExaminar);


            @TempData["Message"] = "Se cargaron los archivos";
            return RedirectToAction("ManualesOperativos_CargaManual");

        }

        public ActionResult IniciarComboTipoAlmacen()
        {
            try
            {
                var dropdownVD = DataTableToModel.ConvertTo<Owners>(DALAltaProveedor.spOwners_sUP().Tables[0]);
                //DataTableToModel.ConvertTo<ServicesManagement.Web.Models.Almacenes.SPOwners_sUP>(DALAltaProveedor.spOwners_sUP().Tables[0])
                var result = new { Success = true, resp = dropdownVD };
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