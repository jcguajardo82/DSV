using ServicesManagement.Web.DAL.Autorizacion;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models;
using ServicesManagement.Web.Models.Autorizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class AutorizacionController : Controller
    {
        // GET: Autorizacion
        public ActionResult Autorizacion()
        {
            return View();
        }

        public ActionResult AutorizacionCedis()
        {
            return View();
        }

        public ActionResult AutorizacionDsv()
        {
            return View();
        }

        public ActionResult AutorizacionDst()
        {
            return View();
        }

        public ActionResult AutorizacionAdmon()
        {
            return View();
        }

        public ActionResult GetOrdenes(string accion="") {
            try
            {

                var result = new
                {
                    Success = true ,
                    resp = DataTableToModel.ConvertTo<AutorizacionShow>(DALAutorizacion.up_Corp_cns_tbl_OrdenCancelada(accion).Tables[0])
                };         
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDetalle(int OrderSF ,string accion)
        {
            try
            {
                var result = new
                {
                    Success = true,
                    resp = DataTableToModel.ConvertTo<upCorpOms_Cns_OrdersByItems>(DALAutorizacion.upCorpOms_Cns_OrdersByItems(OrderSF, accion).Tables[0])
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SetAut(int IdEstatusAut, string Comentario
           , string orderId, int Id_cancelacion)
        {
            try
            {
                DALAutorizacion.BitacoraAutRma_iUp( IdEstatusAut, User.Identity.Name, Comentario, orderId, Id_cancelacion );
                var result = new  { Success = true };

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