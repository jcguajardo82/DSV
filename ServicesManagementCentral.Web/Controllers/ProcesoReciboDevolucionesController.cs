using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagementCentral.Web.Controllers.Api;
using ServicesManagement.Web.Models.ProcesoReciboDevoluciones;
using ServicesManagement.Web.DAL.ProcesoReciboDevoluciones;
using ServicesManagement.Web.Helpers;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class ProcesoReciboDevolucionesController : Controller
    {
        //upload imagenes
        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object 
            if (Request.Files.Count > 0)
            {
                try
                {

                    var idUeNo = Request.Form["idUeNo"].ToString();
                    string servername = Request.Form["servername"].ToString();

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

                        fname = string.Format("{0}_{1}_{2}", idUeNo, "devolucion", dateTime);

                        // Get the complete folder path and store the file inside it.  
                        var path = Path.Combine(Server.MapPath("~/Files/"), fname);
                        var pathServerName = servername + "/Files/" + fname;

                        file.SaveAs(path);

                        // DALManualesOperativos.spManualTitles_iUP(int.Parse(idManual), int.Parse(idOwner), ManualDesc, string.Empty, string.Empty, true, fname, DateTime.Now, User.Identity.Name);

                        //DALManualesOperativos.spManualTitles_iUP(int.Parse(idManual), int.Parse(idOwner), ManualDesc, string.Empty, string.Empty, true, pathServerName, DateTime.Now, User.Identity.Name);
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
        // GET: ProcesoReciboDevoluciones
        public ActionResult ProcesoReciboDevoluciones()
        {
            var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
            int idOwner = 1;

            if (ds.Tables[0].Rows.Count == 0)
            {
                idOwner = 2;
            }
            else
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    UserRolModel rol = new UserRolModel();
                    rol.idRol = item["rol"].ToString();
                    rol.nombreRol = item["nombreRol"].ToString();
                    Session["UserRol"] = rol;
                    idOwner = 4;
                }
            }

            var FecIni = DateTime.Now.AddDays(-7);
            var FecFin= DateTime.Now;

            ViewBag.FecIni = FecIni.ToString("yyyy/MM/dd");
            ViewBag.FecFin = FecFin.ToString("yyyy/MM/dd");

            if (Request.QueryString["FecIni"] != null && Request.QueryString["FecFin"] != null) {
                FecIni = Convert.ToDateTime(Request.QueryString["FecIni"].ToString());
                FecFin = Convert.ToDateTime(Request.QueryString["FecFin"].ToString());
            }

            Session["ListaConsignaciones"] = 
                DALProcesoReciboDevoluciones.upCorpOMS_Cns_UeNoDevolProcess(idOwner,User.Identity.Name, FecIni, FecFin);
            return View();
        }

        //Combo Condiciones
        public ActionResult ListCondiciones()
        {
            try
            {
                var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
                int idOwner = 1;

                if (ds.Tables[0].Rows.Count == 0)
                {
                    idOwner = 2;
                }
                else
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        UserRolModel rol = new UserRolModel();
                        rol.idRol = item["rol"].ToString();
                        rol.nombreRol = item["nombreRol"].ToString();
                        Session["UserRol"] = rol;
                        idOwner = 4;
                    }
                }

                var list = DataTableToModel.ConvertTo<upCorpOms_Cns_UeCondCauses>(DALProcesoReciboDevoluciones.upCorpOms_Cns_UeCondCauses(idOwner).Tables[0]);

                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        //Combo Devoluciones
        public ActionResult ListDevoluciones()
        {
            try
            {
                var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
                int idOwner = 1;

                if (ds.Tables[0].Rows.Count == 0)
                {
                    idOwner = 2;
                }
                else
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        UserRolModel rol = new UserRolModel();
                        rol.idRol = item["rol"].ToString();
                        rol.nombreRol = item["nombreRol"].ToString();
                        Session["UserRol"] = rol;
                        idOwner = 4;
                    }
                }
                var list = DataTableToModel.ConvertTo<upCorpOms_Cns_UeDevolCauses>(DALProcesoReciboDevoluciones.upCorpOms_Cns_UeDevolCauses(idOwner).Tables[0]);

                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        //listar productos de  consignación
        public ActionResult LstProdPaquete(string UeNo)
        {
            try
            {
                var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
                int idOwner = 1;

                if (ds.Tables[0].Rows.Count == 0)
                {
                    idOwner = 2;
                }
                else
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        UserRolModel rol = new UserRolModel();
                        rol.idRol = item["rol"].ToString();
                        rol.nombreRol = item["nombreRol"].ToString();
                        Session["UserRol"] = rol;
                        idOwner = 3;
                    }
                }

                var listaProd = DataTableToModel.ConvertTo<upCorpOMS_Cns_UeNoDevolDetail>(DALProcesoReciboDevoluciones.upCorpOMS_Cns_UeNoDevolDetail(UeNo).Tables[0]);
                var listCond = DataTableToModel.ConvertTo<upCorpOms_Cns_UeCondCauses>(DALProcesoReciboDevoluciones.upCorpOms_Cns_UeCondCauses(idOwner).Tables[0]);
                var listDev = DataTableToModel.ConvertTo<upCorpOms_Cns_UeDevolCauses>(DALProcesoReciboDevoluciones.upCorpOms_Cns_UeDevolCauses(idOwner).Tables[0]);
                var result = new { Success = true, resp = listaProd, lstCond = listCond, lstDev = listDev };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        //Aceptar Devolución
        public ActionResult AceptarDevolucion(string UeNo, int OrderNo, string IdTrackingService, string TrackingType, decimal Barcode, string CreationId,
            string PackageCondition, string ReturnedComment)
        {
            try
            {
                var aceptadaDevolucion = DALProcesoReciboDevoluciones.upCorpOMS_Ins_UeNoDevolProcess(UeNo, OrderNo, IdTrackingService, TrackingType, Barcode, User.Identity.Name,
                    PackageCondition, ReturnedComment).Tables[0].Rows[0][0];
                var result = new { Success = true, resp = aceptadaDevolucion };
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