using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServicesManagement.Web.DAL.Embarques;
using ServicesManagement.Web.DAL.ProcesoSurtido;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models;
using ServicesManagement.Web.Models.ProcesoSurtido;
using Soriana.OMS.Ordenes.Common.DTO;
using System;
using System.Collections.Generic;
using System.Data;
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
        // comentario: 2022-04-08
        public ActionResult ProcesoSurtidoDSV()
        {
            int Tipoenvio = 0;
            int OrderNo = 0;
            string UeNo = string.Empty;

            if (Request.QueryString["Tipoenvio"] != null) {
                Tipoenvio = int.Parse(Request.QueryString["Tipoenvio"].ToString());
            }

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

            Session["lstGuiasDetalle"] = null;
            var ds = DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcess(UeNo, OrderNo);
            ViewBag.OrderNo = OrderNo;
            ViewBag.UeNo = UeNo;

            ViewBag.PSCarriers = DataTableToModel.ConvertTo<upCorpOms_cns_MonitorPSCarriers>(DALProcesoSurtido.upCorpOms_cns_MonitorPSCarriers().Tables[0]);
            ViewBag.MotCan = DataTableToModel.ConvertTo<OrderFacts_UE_CancelCauses>(DALProcesoSurtido.upCorpOms_Cns_UeCancelCauses(4).Tables[0]);
            var enc = DataTableToModel.ConvertTo<Encabezado>(DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcessHeader(UeNo, OrderNo).Tables[0]).FirstOrDefault();
            ViewBag.Header = enc;
            ViewBag.PorProcesar = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoSupplyProcessSel>(DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcessSel(UeNo, OrderNo).Tables[0]);
            ViewBag.Vehiculos = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoConsigmentsVehicles>(DALProcesoSurtido.upCorpOms_Cns_UeNoConsigmentsVehicles(UeNo, OrderNo, enc.idSupplierWH, enc.IdSupplierWHCode).Tables[0]);

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
                ViewBag.Encabezado = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoSupplyProcess>(ds.Tables[2]);

                if (ds.Tables.Count == 3)
                    ViewBag.Guias = GetGuias(ds.Tables[1], UeNo);

                var lstTracking = DataTableToModel.ConvertTo<UenoTracking>(DALEmbarques.upCorpOms_Cns_UeNoTracking(UeNo, OrderNo).Tables[0]);
                List<UenoTracking> lstTrackingF = new List<UenoTracking>();
                foreach (var item in lstTracking)
                {
                    UenoTracking newItem = new UenoTracking();
                    newItem.declaredValue = item.declaredValue;
                    newItem.IdTrackingService = item.IdTrackingService;
                    newItem.PackageHeight = item.PackageHeight;
                    newItem.PackageLength = item.PackageLength;
                    newItem.PackageWeight = item.PackageWeight;
                    newItem.PackageWidth = item.PackageWidth;
                    newItem.Quantity = item.Quantity;
                    newItem.ShippingMethod = item.ShippingMethod;
                    newItem.IdTracking = item.IdTracking;
                    newItem.PesoReal = GetPesoReal(item.IdTrackingService);

                    lstTrackingF.Add(newItem);
                }

                Session["OrderTrackings"] = lstTrackingF;
            }

            return View();
        }
        public ActionResult ConfirmarOrden(int OrderNo)
        {
            try
            {
                //string UserCreate = User.Identity.Name;
                DALProcesoSurtido.upCorpOms_upd_MonitorPSSurtidoEmpaque(OrderNo, 1);

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
        public ActionResult IniciarProceso(int OrderNo, string TrackingServiceName, string IdTrackingService, string TrackingServiceStatus)
        {
            try
            {
                //string UserCreate = User.Identity.Name;
                DALProcesoSurtido.upCorpOms_upd_MonitorPSEmbarque(OrderNo, TrackingServiceName, IdTrackingService, TrackingServiceStatus);

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

            Session["lstGuiasDetalle"] = null;
            var ds = DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcess(UeNo, OrderNo);
            ViewBag.OrderNo = OrderNo;
            ViewBag.UeNo = UeNo;


            ViewBag.MotCan = DataTableToModel.ConvertTo<OrderFacts_UE_CancelCauses>(DALProcesoSurtido.upCorpOms_Cns_UeCancelCauses(4).Tables[0]);
            var enc = DataTableToModel.ConvertTo<Encabezado>(DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcessHeader(UeNo, OrderNo).Tables[0]).FirstOrDefault();
            ViewBag.Header = enc;
            ViewBag.PorProcesar = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoSupplyProcessSel>(DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcessSel(UeNo, OrderNo).Tables[0]);
            ViewBag.Vehiculos = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoConsigmentsVehicles>(DALProcesoSurtido.upCorpOms_Cns_UeNoConsigmentsVehicles(UeNo, OrderNo, enc.idSupplierWH, enc.IdSupplierWHCode).Tables[0]);

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
                ViewBag.Encabezado = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoSupplyProcess>(ds.Tables[2]);

                if (ds.Tables.Count == 3)
                    ViewBag.Guias = GetGuias(ds.Tables[1], UeNo);

                var lstTracking = DataTableToModel.ConvertTo<UenoTracking>(DALEmbarques.upCorpOms_Cns_UeNoTracking(UeNo, OrderNo).Tables[0]);
                List<UenoTracking> lstTrackingF = new List<UenoTracking>();
                foreach (var item in lstTracking)
                {
                    UenoTracking newItem = new UenoTracking();
                    newItem.declaredValue = item.declaredValue;
                    newItem.IdTrackingService = item.IdTrackingService;
                    newItem.PackageHeight = item.PackageHeight;
                    newItem.PackageLength = item.PackageLength;
                    newItem.PackageWeight = item.PackageWeight;
                    newItem.PackageWidth = item.PackageWidth;
                    newItem.Quantity = item.Quantity;
                    newItem.ShippingMethod = item.ShippingMethod;
                    newItem.IdTracking = item.IdTracking;
                    newItem.PesoReal = GetPesoReal(item.IdTrackingService);

                    lstTrackingF.Add(newItem);
                }

                Session["OrderTrackings"] = lstTrackingF;
            }

            return View();
        }
        private List<Guias_por_OrderFact> GetGuias(DataTable dt, string UeNo)
        {
            var lstGuias = DataTableToModel.ConvertTo<Guias_por_OrderFact>(dt);
            List<Guias_por_OrderFact> lst = new List<Guias_por_OrderFact>();

            foreach (var item in lstGuias)
            {
                Guias_por_OrderFact newItem = new Guias_por_OrderFact();
                newItem.IdTrackingService = item.IdTrackingService;
                newItem.PackageType = item.PackageType;
                newItem.PackageWeight = GetGuiasDetails(UeNo, item.IdTrackingService);

                lst.Add(newItem);
            }

            return lst;
        }
        private decimal? GetGuiasDetails(string UeNo, string idTrackingService)
        {
            List<upCorpOms_Cns_UeNoSupplyProcess> lst = new List<upCorpOms_Cns_UeNoSupplyProcess>();
            var ds = DALProcesoSurtido.upCorpOms_Cns_UeNoTrackingProducts(UeNo, idTrackingService);
            var listDetails = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoSupplyProcess>(ds.Tables[0]);
            decimal pesoVol = 0;
            foreach (var itemD in listDetails)
            {
                upCorpOms_Cns_UeNoSupplyProcess newItem = new upCorpOms_Cns_UeNoSupplyProcess();

                var dsProd = DALServicesM.GetDimensionsByProduct(itemD.SKU.ToString());
                var product = DataTableToModel.ConvertTo<ProductDimensionsModel>(dsProd.Tables[0]).FirstOrDefault();
                pesoVol = 0;
                if (product.UNIDAD_PES.Equals("KG"))
                    pesoVol = (product.ALTO * product.ANCHO * product.LARGO) / 5000;
                else
                    pesoVol = ((product.ALTO * product.ANCHO * product.LARGO) / 5000) / 1000;

                newItem.Descripcion = itemD.Descripcion;
                newItem.SKU = itemD.SKU;
                newItem.Piezas = itemD.Piezas;
                newItem.IdTrackingService = idTrackingService;

                if (pesoVol > product.PESO)
                    newItem.PesoReal = (pesoVol * decimal.Parse(itemD.Piezas.ToString()));
                else
                    newItem.PesoReal = (product.PESO * decimal.Parse(itemD.Piezas.ToString()));


                lst.Add(newItem);

            }

            if (Session["lstGuiasDetalle"] != null)
            {
                var lstDetalle = (List<upCorpOms_Cns_UeNoSupplyProcess>)Session["lstGuiasDetalle"];

                lstDetalle.AddRange(lst);

                Session["lstGuiasDetalle"] = lstDetalle;
            }
            else
                Session["lstGuiasDetalle"] = lst;

            var pesoRealTot = lst.Where(x => x.IdTrackingService == idTrackingService).Sum(x => x.PesoReal);

            return pesoRealTot;

        }
        private decimal? GetPesoReal(string idTrackingService)
        {
            decimal? pesoReal = 0;
            try
            {
                if (Session["lstGuiasDetalle"] != null)
                {
                    var lstDetalle = (List<upCorpOms_Cns_UeNoSupplyProcess>)Session["lstGuiasDetalle"];
                    var list = lstDetalle.Where(x => x.IdTrackingService == idTrackingService);

                    pesoReal = list.Sum(x => x.PesoReal);
                }

            }
            catch (Exception x)
            {

            }

            return pesoReal;
        }
        [HttpPost]
        public ActionResult GetDetails(string idTrackingService)
        {
            try
            {
                if (Session["lstGuiasDetalle"] != null)
                {
                    var lstDetalle = (List<upCorpOms_Cns_UeNoSupplyProcess>)Session["lstGuiasDetalle"];
                    var list = lstDetalle.Where(x => x.IdTrackingService == idTrackingService);

                    var result = new
                    {
                        Success = true
                        ,
                        data = list
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new
                    {
                        Success = false
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
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
                string UserCreate = User.Identity.Name;
                DALProcesoSurtido.upCorpOms_Del_UeNoSupplyProcess(OrderNo, Cause_Desc, int.Parse(IdCause), UeNo, UserCreate);

                Correos.Correos.Correo8A(OrderNo, 2);
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

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["api_FinalizarSurtidoDSV"];
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
                    a.UnidadMedida = p[0].UnidadMedida.Length == 0 ? " " : p[0].UnidadMedida;    // UnitMeasure
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
                //Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_FinalizarSurtido"], "", json2);
                Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_FinalizarSurtidoDSV"], "", json2);

                //Llamado Generar Nueva orden de compra
                string apiUrl2 = System.Configuration.ConfigurationManager.AppSettings["GeneracionNvaOrden_API"]; 
                var req = new { OrderID = UeNo.ToString() };

                string json3 = JsonConvert.SerializeObject(new { OrderID = UeNo.ToString() });
                string PPSRequest = JsonConvert.SerializeObject(req);
                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "in_data: " + json3, false, null);
                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Request: " + apiUrl2, false, null);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Soriana.FWK.FmkTools.RestResponse r2 = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, apiUrl2, "", json3);

                JObject json = JObject.Parse(r2.message);
                if (json.TryGetValue("errorCode", out JToken v1))
                {
                    if (((Newtonsoft.Json.Linq.JValue)v1).Value.ToString() == "99")
                    {
                        throw new Exception(r.message);
                    }
                }
                //fin


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
           , string idOwner, int idSupplierWH, int idSupplierWHCode, string SerialId, string BateryId, string EntryBy, string EntryDate, string PetitionId
           , string MotorId, string RepuveId, string Year, int ColorId, string ModelId, string BrandId, string CylinderCapacityId)
        {
            try
            {

                DALProcesoSurtido.upCorpOms_Ins_UeNoConsigmentsVehicles(
                     UeNo, OrderNo
                   , idOwner, idSupplierWH, idSupplierWHCode, SerialId, BateryId, EntryBy, Convert.ToDateTime(EntryDate), PetitionId
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