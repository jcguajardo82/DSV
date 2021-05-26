using System;
using ServicesManagement.Web.DAL.Consignaciones;
using ServicesManagement.Web.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagement.Web.Models.Consignaciones;

namespace ServicesManagement.Web.Controllers
{
    public class ConsignacionesController : Controller
    {

        #region Consignaciones Administrador
        public ActionResult Consignaciones()
        {
            return View();
        }

        public ActionResult GetconsignacionesAdm()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<ConsignacionesAdm>(DALConsignacionesAdm.upCorpAlmacen_Cns_Consigments().Tables[0]);
                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult GetDetalleConsignacionAdm(string consignacion)
        {
            try
            {
                List<DetalleConsignaciones> list = new List<DetalleConsignaciones>();
                var ds = DALConsignacionesAdm.upCorpAlmacen_Cns_ConsigmentsDetail(consignacion);

                if (ds.Tables.Count > 0)
                    list = DataTableToModel.ConvertTo<DetalleConsignaciones>(ds.Tables[0]);

                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Consignaciones CEDIS
        public ActionResult Consignaciones_CEDIS()
        {

            return View();

        }


        public ActionResult GetconsignacionesCEDIS()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<ConsignacionesCEDIS>(DALConsignacionesCEDIS.upCorpAlmacen_Cns_ConsigmentsCEDIS().Tables[0]);
                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Consignaciones Proveedor
        public ActionResult Consignaciones_Proveedores()
        {

            return View();
           
        }


        public ActionResult GetconsignacionesProveedor()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<ConsignacionesProveedor>(DALConsignacionesProveedor.upCorpAlmacen_Cns_ConsigmentsProveedor().Tables[0]);
                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


    }
}