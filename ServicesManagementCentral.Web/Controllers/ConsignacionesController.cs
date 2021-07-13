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
    [Authorize]
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
                var list = DataTableToModel.ConvertTo<ConsignacionesAdm>(DALConsignacionesAdm.upCorpAlmacen_Cns_Consigments(User.Identity.Name).Tables[0]);
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
                var list = DataTableToModel.ConvertTo<ConsignacionesCEDIS>(DALConsignacionesCEDIS.upCorpAlmacen_Cns_ConsigmentsCEDIS(User.Identity.Name).Tables[0]);
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

        #region Consignaciones Proveedor Pendientes
        public ActionResult Consignaciones_Proveedores()
        {

            return View();
           
        }


        public ActionResult GetconsignacionesProveedor()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<ConsignacionesProveedor>(DALConsignacionesProveedor.upCorpAlmacen_Cns_ConsigmentsProveedor(User.Identity.Name).Tables[0]);
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


        #region Consignaciones Proveedor concluidos
        public ActionResult Consignaciones_ProveedoresConcluidos()
        {

            return View();

        }


        public ActionResult GetconsignacionesProveedorConcluidos()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<ConsignacionesProveedor>(DALConsignacionesProveedor.upCorpAlmacen_Cns_ConsigmentsProveedorConcluido(User.Identity.Name).Tables[0]);
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