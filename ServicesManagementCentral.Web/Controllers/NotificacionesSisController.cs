using ServicesManagement.Web.DAL.NotificacionesSis;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.NotificacionesSis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class NotificacionesSisController : Controller
    {
        // GET: NotificacionesSis
        public ActionResult NotificacionesCat()
        {
            return View();
        }

        public ActionResult GetNotificacionesSis()
        {
            try
            {


                //var lst = DataTableToModel.ConvertTo<ImagenNotificacion>(DALNotificacionesSis.ImagenNotificacion_Cns().Tables[0]);
                var lst = lstImagenNotificacion(DALNotificacionesSis.ImagenNotificacion_Cns().Tables[0]);

                var result = new { Success = true, resp = lst };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetNotificacionesSisId(int IdImagen)
        {
            try
            {


                //var lst = DataTableToModel.ConvertTo<ImagenNotificacion>(DALNotificacionesSis.ImagenNotificacion_Cns().Tables[0]);
                var lst = lstImagenNotificacion(DALNotificacionesSis.ImagenNotificacion_CnsbyId(IdImagen).Tables[0]).FirstOrDefault();

                var result = new { Success = true, resp = lst };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        

        public List<ImagenNotificacion> lstImagenNotificacion(DataTable dt)
        {

            List<ImagenNotificacion> lst = new List<ImagenNotificacion>();

            foreach (DataRow item in dt.Rows)
            {

                //ImagenNotificacion img = new ImagenNotificacion();

                //img.IdImagen = int.Parse(item["IdImagen"].ToString());
                //img.Titulo = item["Titulo"].ToString();

                //byte[] data = (byte[])(item["Imagen"]);
                //string base64String = Convert.ToBase64String(data);

                //img.strImagen = base64String;
                //img.BitActivo = item["BitActivo"].ToString();
                //img.FechaIni = item["FechaIni"].ToString();
                //img.FechaFin = item["FechaFin"].ToString();
                //img.CreatedDate = item["CreatedDate"].ToString();
                //img.CreationUser = item["CreationUser"].ToString();
                //img.ModifDate = item["ModifDate"].ToString();
                //img.ModifUser = item["ModifUser"].ToString();
                //lst.Add(img);
                lst.Add(
                    new ImagenNotificacion
                    {
                        IdImagen = int.Parse(item["IdImagen"].ToString()),
                        Titulo = item["Titulo"].ToString().Trim(),
                        strImagen = Convert.ToBase64String((byte[])(item["Imagen"])),
                        BitActivo = item["BitActivo"].ToString(),
                        FechaIni = Convert.ToDateTime( item["FechaIni"].ToString()).ToString("dd/MM/yyyy"),
                        FechaFin = Convert.ToDateTime(item["FechaFin"].ToString()).ToString("dd/MM/yyyy"),
                        CreatedDate = item["CreatedDate"].ToString(),
                        CreationUser = item["CreationUser"].ToString().Trim(),
                        ModifDate = item["ModifDate"].ToString(),
                        ModifUser = item["ModifUser"].ToString(),
                    }
                    );

            }

            return lst;
        }
        public ActionResult AddNotificacionesSis()
        {
            string Titulo = Request.Form["Titulo"].ToString();
            string FechaIni = Request.Form["FechaIni"].ToString();
            string FechaFin = Request.Form["FechaFin"].ToString();
            int IdImagen = int.Parse(Request.Form["IdImagen"].ToString());
            bool bitActivo = Convert.ToBoolean(Request.Form["Activo"].ToString());
            // Checking no of files injected in Request object 
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        System.IO.Stream str, str1; String strmContents;
                        Int32 counter, strLen, strRead, strRead1;

                        HttpPostedFileBase file = files[i];
                        str1 = file.InputStream;
                        byte[] strArr1 = new byte[Convert.ToInt32(file.ContentLength)];
                        // Read stream into byte array.
                        strRead1 = str1.Read(strArr1, 0, Convert.ToInt32(file.ContentLength));

                        //Grabar a tabla
                        if (IdImagen == 0)
                        {
                            DALNotificacionesSis.ImagenNotificacion_Ins(Titulo, strArr1, FechaIni, FechaFin, User.Identity.Name);
                        }
                        else {
                            DALNotificacionesSis.ImagenNotificacion_Up(IdImagen,Titulo, strArr1, FechaIni, FechaFin, User.Identity.Name);
                        }
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
                DALNotificacionesSis.ImagenNotificacion_Up(IdImagen, Titulo, null, FechaIni, FechaFin, User.Identity.Name);
                return Json("No files selected.");
            }
        }


        public ActionResult NotificacionSis() {

            var lst = lstImagenNotificacion(DALNotificacionesSis.ImagenNotificacion_CnsbyDates().Tables[0]);

           


            return View(lst);
        }

        public ActionResult Finaliza()
        {
            if (Session["loginTienda"] != null)
            {
                Session["Id_Num_UN"] = Session["loginTienda"].ToString();
                Session["Desc_Num_UN"] = "";

                System.Data.DataSet ds = DALServicesM.GetUN();

                foreach (System.Data.DataRow r in ds.Tables[0].Rows)
                {
                    if (Session["loginTienda"].ToString().Trim().Equals(r[0].ToString().Trim()))
                    {
                        Session["Desc_Num_UN"] = r[1].ToString();
                    }
                }

                return RedirectToAction("OrdenSeleccionada", "Ordenes");
                //return RedirectToAction("Index", "Ordenes");
            }
            else
            {
                return RedirectToAction("Index", "CPanel");
            }

        }
    }
}