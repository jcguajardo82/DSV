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
using System.Data;
using ServicesManagement.Web.Models.ManualesOp;

namespace ServicesManagement.Web.Controllers
{
    public class ManualesOperativosController : Controller
    {
        
        #region ManualesOperativos
        // GET: ManualesOperativos
        public ActionResult ManualesOperativos()
        {
            var tipoalmacen = DALManualesOperativos.Llenartipoalmacen();

            var manualesOperativosModels = tipoalmacen.Tables[0].AsEnumerable().Select(item => new SelectListItem { Value = (item.Field<int>("idOwner")).ToString(), Text = item.Field<string>("ownerName") }) as IEnumerable<SelectListItem>;
            var vacio = new List<SelectListItem>() {
                new SelectListItem { Value = "0", Text = "-Seleccione una opcion-" }
            } as IEnumerable<SelectListItem>;

            vacio = vacio.Union(manualesOperativosModels);

            return View(vacio);
        }

        public ActionResult CargaPDF(int idManual, int idOwner)
        {
            try
            {
                var list = DALManualesOperativos.MostrarPDF(idManual, idOwner);
                string URL = list.Tables[0].Rows[0]["manualFilename"].ToString();

                if (string.IsNullOrWhiteSpace(URL))
                    return Json(new { Success = false, Message = "No se encontro la informacion del manual." }, JsonRequestBehavior.AllowGet);

                var result = new { Success = true, Message = "OK", URL };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }


        }

        #endregion


        public ActionResult ManualesOperativos_CargaManual()
        {
            // var list = DALManualesOperativos.Llenartipoalmacen();
            var list = DataTableToModel.ConvertTo<ManualesOperativosModels>(DALManualesOperativos.Llenartipoalmacen().Tables[0]);
            ViewBag.Owners = list;
            ViewBag.Manuals = DataTableToModel.ConvertTo<ManualTypesModel>(DALManualesOperativos.spManualTypes_sUP().Tables[0]);


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

        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object 
            if (Request.Files.Count > 0)
            {
                try
                {

                    var idOwner = Request.Form["idOwner"].ToString();
                    var idManual = Request.Form["idManual"].ToString();
                    var ManualDesc = Request.Form["ManualDesc"].ToString();
                    var ownerName = Request.Form["ownerName"].ToString();
                    string servername = Request.Form["servername"].ToString();

                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

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

                        fname = string.Format("{0}_{1}_{2}", ownerName, ManualDesc, fname);

                        // Get the complete folder path and store the file inside it.  
                        var path = Path.Combine(Server.MapPath("~/Files/"), fname);
                        var pathServerName = servername + "/Files/" + fname;

                        file.SaveAs(path);

                       // DALManualesOperativos.spManualTitles_iUP(int.Parse(idManual), int.Parse(idOwner), ManualDesc, string.Empty, string.Empty, true, fname, DateTime.Now, User.Identity.Name);

                        DALManualesOperativos.spManualTitles_iUP(int.Parse(idManual), int.Parse(idOwner), ManualDesc, string.Empty, string.Empty, true, pathServerName, DateTime.Now, User.Identity.Name);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
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
    }


}