using System;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
                    int idOrderNo = int.Parse(Request.Form["idOrderNo"].ToString());
                    string servername = Request.Form["servername"].ToString();

                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        System.IO.Stream str, str1; String strmContents;
                        Int32 counter, strLen, strRead, strRead1;
                        // Create a Stream object.
                        //str = Request.InputStream;
                        //// Find number of bytes in stream.
                        //strLen = Convert.ToInt32(str.Length);
                        //// Create a byte array.
                        //byte[] strArr = new byte[strLen];
                        //// Read stream into byte array.
                        //strRead = str.Read(strArr, 0, strLen);

                        //// Convert byte array to a text string.
                        //strmContents = "";
                        //for (counter = 0; counter < strLen; counter++)
                        //{
                        //    strmContents = strmContents + strArr[counter].ToString();
                        //}
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        //DateTime dt = DateTime.Now;
                        //string dateTime = dt.ToString("yyyyMMddHHmmssfff");

                        HttpPostedFileBase file = files[i];
                        str1 = file.InputStream;
                        byte[] strArr1 = new byte[Convert.ToInt32(file.ContentLength)];
                        // Read stream into byte array.
                        strRead1 = str1.Read(strArr1, 0, Convert.ToInt32(file.ContentLength));

                        //Grabar a tabla
                        DALProcesoReciboDevoluciones.upCorpOms_Ins_UeNoDevolEvidencia(idUeNo, idOrderNo, User.Identity.Name, strArr1);

                        //string fname;

                        //// Checking for Internet Explorer  
                        //if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        //{
                        //    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        //    fname = testfiles[testfiles.Length - 1];
                        //}
                        //else
                        //{
                        //    fname = file.FileName;
                        //}

                        //fname = string.Format("{0}_{1}_{2}", idUeNo, "devolucion", dateTime);

                        //// Get the complete folder path and store the file inside it.  
                        //var path = Path.Combine(Server.MapPath("~/Files/"), fname);
                        //var pathServerName = servername + "/Files/" + fname;

                        //file.SaveAs(path);

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
        // GET: ProcesoReciboDevoluciones
        public ActionResult ProcesoReciboDevolucionesCEDIS()
        {
            var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
            int idOwner = 3;

            //if (ds.Tables[0].Rows.Count == 0)
            //{
            //    idOwner = 2;
            //}
            //else
            //{
            //    foreach (DataRow item in ds.Tables[0].Rows)
            //    {
            //        UserRolModel rol = new UserRolModel();
            //        rol.idRol = item["rol"].ToString();
            //        rol.nombreRol = item["nombreRol"].ToString();
            //        Session["UserRol"] = rol;
            //        if (item["idOwner"].ToString() != "")
            //        {
            //            idOwner = int.Parse(item["idOwner"].ToString());
            //        }
            //        else
            //        {
            //            idOwner = 4;
            //        }
            //    }
            //}

            var FecIni = DateTime.Now.AddDays(-7);
            var FecFin = DateTime.Now;

            ViewBag.FecIni = FecIni.ToString("yyyy/MM/dd");
            ViewBag.FecFin = FecFin.ToString("yyyy/MM/dd");

            if (Request.QueryString["FecIni"] != null && Request.QueryString["FecFin"] != null)
            {
                FecIni = Convert.ToDateTime(Request.QueryString["FecIni"].ToString());
                FecFin = Convert.ToDateTime(Request.QueryString["FecFin"].ToString());
            }

            Session["ListaConsignaciones"] =
                DALProcesoReciboDevoluciones.upCorpOMS_Cns_UeNoDevolProcess(idOwner, User.Identity.Name, FecIni, FecFin);
            return View();
        }
        // GET: ProcesoReciboDevoluciones
        public ActionResult ProcesoReciboDevolucionesDST()
        {
            var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
            int idOwner = 2;

            //if (ds.Tables[0].Rows.Count == 0)
            //{
            //    idOwner = 2;
            //}
            //else
            //{
            //    foreach (DataRow item in ds.Tables[0].Rows)
            //    {
            //        UserRolModel rol = new UserRolModel();
            //        rol.idRol = item["rol"].ToString();
            //        rol.nombreRol = item["nombreRol"].ToString();
            //        Session["UserRol"] = rol;
            //        if (item["idOwner"].ToString() != "")
            //        {
            //            idOwner = int.Parse(item["idOwner"].ToString());
            //        }
            //        else
            //        {
            //            idOwner = 4;
            //        }
            //    }
            //}

            var FecIni = DateTime.Now.AddDays(-7);
            var FecFin = DateTime.Now;

            ViewBag.FecIni = FecIni.ToString("yyyy/MM/dd");
            ViewBag.FecFin = FecFin.ToString("yyyy/MM/dd");

            if (Request.QueryString["FecIni"] != null && Request.QueryString["FecFin"] != null)
            {
                FecIni = Convert.ToDateTime(Request.QueryString["FecIni"].ToString());
                FecFin = Convert.ToDateTime(Request.QueryString["FecFin"].ToString());
            }

            Session["ListaConsignaciones"] =
                DALProcesoReciboDevoluciones.upCorpOMS_Cns_UeNoDevolProcess(idOwner, User.Identity.Name, FecIni, FecFin);
            return View();
        }
        public ActionResult ProcesoReciboDevolucionesDSV()
        {
            var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
            int idOwner = 4;

            //if (ds.Tables[0].Rows.Count == 0)
            //{
            //    idOwner = 2;
            //}
            //else
            //{
            //    foreach (DataRow item in ds.Tables[0].Rows)
            //    {
            //        UserRolModel rol = new UserRolModel();
            //        rol.idRol = item["rol"].ToString();
            //        rol.nombreRol = item["nombreRol"].ToString();
            //        Session["UserRol"] = rol;
            //        if (item["idOwner"].ToString() != "")
            //        {
            //            idOwner = int.Parse(item["idOwner"].ToString());
            //        }
            //        else
            //        {
            //            idOwner = 4;
            //        }
            //    }
            //}

            var FecIni = DateTime.Now.AddDays(-7);
            var FecFin = DateTime.Now;

            ViewBag.FecIni = FecIni.ToString("yyyy/MM/dd");
            ViewBag.FecFin = FecFin.ToString("yyyy/MM/dd");

            if (Request.QueryString["FecIni"] != null && Request.QueryString["FecFin"] != null)
            {
                FecIni = Convert.ToDateTime(Request.QueryString["FecIni"].ToString());
                FecFin = Convert.ToDateTime(Request.QueryString["FecFin"].ToString());
            }

            Session["ListaConsignaciones"] =
                DALProcesoReciboDevoluciones.upCorpOMS_Cns_UeNoDevolProcess(idOwner, User.Identity.Name, FecIni, FecFin);
            return View();
        }

        //Combo Condiciones
        public ActionResult ListCondiciones(int idOwnertmp)
        {
            try
            {
                var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
                int idOwner = idOwnertmp;

                //if (ds.Tables[0].Rows.Count == 0)
                //{
                //    idOwner = 2;
                //}
                //else
                //{
                //    foreach (DataRow item in ds.Tables[0].Rows)
                //    {
                //        UserRolModel rol = new UserRolModel();
                //        rol.idRol = item["rol"].ToString();
                //        rol.nombreRol = item["nombreRol"].ToString();
                //        Session["UserRol"] = rol;
                //        if (item["idOwner"].ToString() != "")
                //        {
                //            idOwner = int.Parse(item["idOwner"].ToString());
                //        }
                //        else
                //        {
                //            idOwner = 4;
                //        }
                //    }
                //}

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
        public ActionResult ListDevoluciones(int idOwnertmp)
        {
            try
            {
                var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
                int idOwner = idOwnertmp;

                //if (ds.Tables[0].Rows.Count == 0)
                //{
                //    idOwner = 2;
                //}
                //else
                //{
                //    foreach (DataRow item in ds.Tables[0].Rows)
                //    {
                //        UserRolModel rol = new UserRolModel();
                //        rol.idRol = item["rol"].ToString();
                //        rol.nombreRol = item["nombreRol"].ToString();
                //        Session["UserRol"] = rol;
                //        if (item["idOwner"].ToString() != "")
                //        {
                //            idOwner = int.Parse(item["idOwner"].ToString());
                //        }
                //        else
                //        {
                //            idOwner = 4;
                //        }
                //    }
                //}

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
                        if (item["idOwner"].ToString() != "")
                        {
                            idOwner = int.Parse(item["idOwner"].ToString());
                        }
                        else
                        {
                            idOwner = 4;
                        }
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

        //listar productos de  consignación
        public ActionResult LstProdImagenes(string UeNo)
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
                        if (item["idOwner"].ToString() != "")
                        {
                            idOwner = int.Parse(item["idOwner"].ToString());
                        }
                        else
                        {
                            idOwner = 4;
                        }
                    }
                }

                var listaImagenes = DataTableToModel.ConvertTo<upCorpOMS_Cns_UeNoDevolEvidencia>(DALProcesoReciboDevoluciones.upCorpOMS_Cns_UeNoDevolEvidencia(UeNo).Tables[0]);
                var lstImagenes = DALProcesoReciboDevoluciones.upCorpOMS_Cns_UeNoDevolEvidencia(UeNo).Tables[0];
                var x = 0;
                foreach (DataRow item in lstImagenes.Rows)
                {
                    byte[] data = (byte[])(item["Evidence"]);
                    string base64String = Convert.ToBase64String(data);
                    //listaImagenes[x].Evidence = data;
                    listaImagenes[x].strImg = base64String;
                    x += 1;
                }

                var result = new { Success = true, resp = listaImagenes };
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