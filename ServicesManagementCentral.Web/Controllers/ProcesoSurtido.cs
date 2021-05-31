using ServicesManagement.Web.DAL.Embarques;
using ServicesManagement.Web.DAL.ProcesoSurtido;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.ProcesoSurtido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class ProcesoSurtidoController : Controller
    {
        // GET: SolicitudGuiasReenvio 
        public ActionResult ProcesoSurtido()
        {

            int OrderNo = 101141812;
            string UeNo = "101141812-1";
            var ds = DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcess(UeNo, OrderNo);

            //ViewBag.OrderNo = OrderNo; 
            //ViewBag.UeNo = UeNo; 
            TempData["OrderNo"] = OrderNo;
            TempData["UeNo"] = UeNo;


            ViewBag.MotCan = DataTableToModel.ConvertTo<OrderFacts_UE_CancelCauses>(DALProcesoSurtido.upCorpOms_Cns_UeCancelCauses(2).Tables[0]);


            if (ds.Tables.Count == 1)
            {
                ViewBag.Mensaje = DataTableToModel.ConvertTo<msj>(ds.Tables[0]).FirstOrDefault();
            }
            else
            {

                ViewBag.UeNoSupplyProcess = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoSupplyProcess>(ds.Tables[0]);
            }

            return View();
        }

        [HttpPost]
        public ActionResult GetSolicitarGuia()
        {
            try
            {
                int OrderNo = 101141812;
                string UeNo = "101141812-1";
                msj msg = new msj();
                List<upCorpOms_Cns_UeNoTracking.Guia> guias = new List<upCorpOms_Cns_UeNoTracking.Guia>();
                List<upCorpOms_Cns_UeNoTracking.Item> items = new List<upCorpOms_Cns_UeNoTracking.Item>();


                var ds = DALEmbarques.upCorpOms_Cns_UeNoTracking(UeNo, OrderNo);
                if (ds.Tables.Count == 1)
                {
                    msg = DataTableToModel.ConvertTo<msj>(ds.Tables[0]).FirstOrDefault();
                }
                else
                {
                    guias = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoTracking.Guia>(ds.Tables[0]);
                    items = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoTracking.Item>(ds.Tables[1]);
                }




                var result = new
                {
                    Success = true
                    ,
                    Message = msg
                    ,
                    guias = guias
                    ,
                    items = items
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Cancelar(string Cause_Desc, string IdCause)
        {
            try
            {

                bool successs = false;
                string msg = "Error al obtener los datos de la consignación.";
                if (TempData.ContainsKey("OrderNo") && TempData.ContainsKey("UeNo"))
                {
                    DALProcesoSurtido.upCorpOms_Del_UeNoSupplyProcess(int.Parse(TempData["OrderNo"].ToString()), Cause_Desc, int.Parse(IdCause), TempData["UeNo"].ToString());
                    successs = true;
                }


                var result = new
                {
                    Success = successs
                    ,
                    Message = msg

                };


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