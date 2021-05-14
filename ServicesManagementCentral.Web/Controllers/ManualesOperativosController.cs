using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagement.Web.DAL.ManualesOperativos;

namespace ServicesManagement.Web.Controllers
{
    public class ManualesOperativosController : Controller
    {
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


        #region ManualesOperativos

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
            return View();
        }
    }





}