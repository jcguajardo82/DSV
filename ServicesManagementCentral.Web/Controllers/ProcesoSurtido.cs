using Newtonsoft.Json;
using ServicesManagement.Web.DAL.Embarques;
using ServicesManagement.Web.DAL.ProcesoSurtido;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.ProcesoSurtido;
using Soriana.OMS.Ordenes.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class ProcesoSurtidoController : Controller
    {
        // GET: SolicitudGuiasReenvio 
        public ActionResult ProcesoSurtido()
        {

            int OrderNo = 0;
            string UeNo = string.Empty;


            //ViewBag.OrderNo = OrderNo; 
            //ViewBag.UeNo = UeNo; 

            if (Request.QueryString["OrderNo"] != null && Request.QueryString["UeNo"] != null)
            {
                OrderNo = int.Parse(Request.QueryString["OrderNo"].ToString());
                UeNo = Request.QueryString["UeNo"].ToString();
            }
            else
            {
                var msj = new msj();
                msj.mensaje = "No existe la consignacion.";

                ViewBag.Mensaje = msj;

                return View();
            }

            var ds = DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcess(UeNo, OrderNo);
            ViewBag.OrderNo = OrderNo;
            ViewBag.UeNo = UeNo;


            ViewBag.MotCan = DataTableToModel.ConvertTo<OrderFacts_UE_CancelCauses>(DALProcesoSurtido.upCorpOms_Cns_UeCancelCauses(4).Tables[0]);
            var enc = DataTableToModel.ConvertTo<Encabezado>(DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcessHeader(UeNo, OrderNo).Tables[0]).FirstOrDefault();
            ViewBag.Header = enc;
            ViewBag.PorProcesar = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoSupplyProcessSel>(DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcessSel(UeNo, OrderNo).Tables[0]);
            ViewBag.Vehiculos = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoConsigmentsVehicles>(DALProcesoSurtido.upCorpOms_Cns_UeNoConsigmentsVehicles(UeNo, OrderNo, enc.idSupplierWH, enc.IdSupplierWHCode).Tables[0]);
            Session["OrderPackages"] = DALEmbarques.upCorpOms_Cns_UeNoTracking(UeNo, OrderNo);



            if (ds.Tables.Count == 1)
            {
                ViewBag.Mensaje = DataTableToModel.ConvertTo<msj>(ds.Tables[0]).FirstOrDefault();
            }
            else
            {
                var list = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoSupplyProcess>(ds.Tables[0]);

                var totSup = list.Where(x => x.Suplido == true).ToList();


                if (list.Count == totSup.Count)
                {
                    ViewBag.ArtSup = true;
                }

                ViewBag.UeNoSupplyProcess = list;
                ViewBag.Encabezado = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoSupplyProcess>(ds.Tables[1]);
            }

            return View();
        }

        [HttpPost]
        public ActionResult GetSolicitarGuia()
        {
            try
            {
                int OrderNo = 101184110;
                string UeNo = "101184110-4";
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
        public ActionResult Cancelar(string Cause_Desc, string IdCause, string UeNo, int OrderNo)
        {
            try
            {

                DALProcesoSurtido.upCorpOms_Del_UeNoSupplyProcess(OrderNo, Cause_Desc, int.Parse(IdCause), UeNo);


                var result = new
                {
                    Success = true               
                };


                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Consignacion(List<string> items, string UeNo, string OrderNo, string NombreSurtidor, string SurtidorID)
        {
            try
            {

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["api_FinalizarSurtido"];
                System.Data.DataSet d = DALServicesM.GetOrdersByOrderNo(UeNo);
                string status = string.Empty, store = string.Empty;
                foreach (System.Data.DataRow r1 in d.Tables[0].Rows)
                {

                    status = r1["StatusUe"].ToString();
                    //ue = r1["UeNo"].ToString();
                    store = r1["StoreNum"].ToString();

                }
                //metodo mio
                InformacionOrden o = new InformacionOrden();

                o.Orden = new InformacionDetalleOrden();
                o.Orden.NumeroOrden = OrderNo;
                o.Orden.EsPickingManual = false;
                o.Orden.EstatusUnidadEjecucion = "0";
                o.Orden.NumeroUnidadEjecucion = UeNo;
                o.Orden.NumeroTienda = Convert.ToInt32(store);
                o.Surtidor = new InformacionSurtidor();
                o.Surtidor.SurtidorID = Convert.ToInt32(SurtidorID);
                o.Surtidor.NombreSurtidor = NombreSurtidor;

                var prodcuts = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoSupplyProcess>(DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcess(UeNo, int.Parse(OrderNo)).Tables[0]);

                foreach (var item in items)
                {
                    var a = new InformacionProductoSuministrado();

                    var p = prodcuts.Where(x => x.EAN == item).ToList();


                    a.IdentificadorProducto = p[0].SKU.ToString();                              // Product-ID
                    a.CodigoBarra = p[0].EAN;                                                   // BarCode
                    a.DescripcionArticulo = p[0].Descripcion;                                   // Product-Name
                    a.Cantidad = p[0].Piezas;                                                   // Quantity

                    a.Precio = p[0].Precio;                                                     // Price
                    a.Observaciones = p[0].Observaciones;                                       // Observations
                    a.UnidadMedida = p[0].UnidadMedida.Length == 0 ? " ": p[0].UnidadMedida;    // UnitMeasure
                    a.NumeroOrden = p[0].OrderNo.ToString();                                    // OrderNo
                    //  a.FueSuministrado = true;
                    //  a.Cantidad = p[0].Piezas;

                    o.ProductosSuministrados.Add(a);
                }



                string json2 = string.Empty;
                JavaScriptSerializer js = new JavaScriptSerializer();
                //json2 = js.Serialize(o);


                json2 = JsonConvert.SerializeObject(o);

                js = null;

                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "in_data: " + json2, false, null);

                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Request: " + apiUrl, false, null);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_FinalizarSurtido"], "", json2);




                var result = new { Success = true, Message = "Alta exitosa" };

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception x)
            {
                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.ERROR, "", false, x);

                var result = new { Success = false, Message = x.Message };

                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public ActionResult SaveVehiculo(string UeNo, int OrderNo
           , string idOwner, int idSupplierWH, int idSupplierWHCode, string SerialId, string BateryId,string EntryBy, string EntryDate, string PetitionId
           , string MotorId,string RepuveId, string Year, int ColorId, string ModelId, string BrandId, string CylinderCapacityId)
        {
            try
            {

                    DALProcesoSurtido.upCorpOms_Ins_UeNoConsigmentsVehicles(
                         UeNo, OrderNo
                       , idOwner, idSupplierWH, idSupplierWHCode, SerialId, BateryId,EntryBy, Convert.ToDateTime(EntryDate), PetitionId
                       , MotorId, RepuveId, Year, ColorId, ModelId, BrandId, CylinderCapacityId, User.Identity.Name);           
                var result = new
                {
                    Success = true         
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