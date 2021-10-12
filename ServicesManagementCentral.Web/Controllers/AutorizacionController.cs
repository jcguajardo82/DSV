using ServicesManagement.Web.DAL.Autorizacion;
using ServicesManagement.Web.DAL.CallCenter;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models;
using ServicesManagement.Web.Models.Autorizacion;
using ServicesManagement.Web.Models.CallCenter;
using System;
using System.Collections.Generic;
using System.Data;
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

        public ActionResult AutorizacionSup()
        {
            return View();
        }


        public ActionResult GetOrdenes(string accion = "")
        {
            try
            {

                var result = new
                {
                    Success = true,
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

        public ActionResult GetDetalle(int OrderSF, string accion)
        {
            try
            {

                var ds = DALAutorizacion.upCorpOms_Cns_OrdersByItems(OrderSF, accion);
                var lst = DataTableToModel.ConvertTo<upCorpOms_Cns_OrdersByItems>(ds.Tables[0]);
                var item = DataTableToModel.ConvertTo<Header>(ds.Tables[1]).FirstOrDefault();
                var images = DataTableToModel.ConvertTo<Tbl_OrdenFotosRMA_sUp>(DALCallCenter.tbl_OrdenFotosRMA_sUp(OrderSF).Tables[0]);

                var result = new
                {
                    Success = true,
                    resp = lst,
                    head = item,
                    imgs = images
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        public List<Tbl_OrdenFotosRMA_sUp> lstImagen(DataTable dt)
        {
            List<Tbl_OrdenFotosRMA_sUp> lst = new List<Tbl_OrdenFotosRMA_sUp>();

            foreach (DataRow item in dt.Rows)
            {


                lst.Add(
                    new Tbl_OrdenFotosRMA_sUp
                    {
                        Id_fotoRMA = (item["IdImagen"].ToString()),
                        ordenRMA = item["ordenRMA"].ToString().Trim(),
                        FotoURL = Convert.ToBase64String((byte[])(item["FotoURL"]))
                    }
                    );

            }

            return lst;
        }

        public ActionResult SetAut(int IdProceso, string Comentario
           , string IdAccion, int Id_cancelacion, int OrderId)
        {
            try
            {
                // DALAutorizacion.BitacoraAutRma_iUp( IdEstatusAut, User.Identity.Name, Comentario, orderId, Id_cancelacion );
                //Cancelación de Orden por Supervisor
                if (IdProceso == 2)
                {
                    //var ds = DALCallCenter.up_Corp_cns_OrderInfo(OrderId);
                    //var detalle = DataTableToModel.ConvertTo<ServicesManagement.Web.Models.CallCenter.OrderDetail>(ds.Tables[1]);

                    //var ShipmentId = detalle[0].ShipmentId;
                    //DALAutorizacion.upCorpOms_Del_UeNoSupplyProcess(OrderId, Comentario, 1, ShipmentId);
                }

                DALAutorizacion.BitacoraAutRma_iUp_v2(IdProceso, IdAccion, Comentario, Id_cancelacion, User.Identity.Name);

                var result = new { Success = true };

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