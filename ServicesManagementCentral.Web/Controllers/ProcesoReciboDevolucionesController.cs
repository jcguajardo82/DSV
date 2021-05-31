using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagement.Web.DAL.ProcesoReciboDevoluciones;
using ServicesManagement.Web.Helpers;

namespace ServicesManagement.Web.Controllers
{
    public class ProcesoReciboDevolucionesController : Controller
    {
        // GET: ProcesoReciboDevoluciones
        public ActionResult ProcesoReciboDevoluciones()
        {
            int idOwner = 3;
            Session["ListaConsignaciones"] = DALProcesoReciboDevoluciones.upCorpOMS_Cns_UeNoDevolProcess(idOwner);
            return View();
        }

        //Aceptar Devolución
        public ActionResult AceptarDevolucion(string UeNo, int OrderNo, string IdTrackingService, string TrackingType, decimal Barcode, string CreationId)
        {
            try
            {
                var aceptadaDevolucion = DALProcesoReciboDevoluciones.upCorpOMS_Ins_UeNoDevolProcess(UeNo, OrderNo, IdTrackingService, TrackingType, Barcode, CreationId).Tables[0].Rows[0][0];
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