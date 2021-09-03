using Newtonsoft.Json;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using ServicesManagement.Web.Models;
using Soriana.OMS.Ordenes.Common.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using ServicesManagement.Web.DAL.Embarques;
using ServicesManagement.Web.Helpers;
using System.Configuration;
using System.Data.SqlClient;
using ServicesManagement.Web.Models.Cotizador;

namespace ServicesManagement.Web.Controllers
{
    #region modelos producto

    public class CodigoModels
    {

        public string ItemId { get; set; } //": "7501055313594",
        public string Description { get; set; } //": "REFRESCO COCA COLA 473 ML LAT(NO_ACT)",
        public string Id_Num_SKU { get; set; } //": "3059640",
        public string NormalPrice { get; set; } //": 8.5,
        public string OfferPrice { get; set; } //": 8.5,
        public string ValidTo { get; set; } //": 0.0,
        public string POSDiscount { get; set; } //": 0.0,
        public string OwnBrand { get; set; } //": 0.0,

        public string ForeignExchangeRate { get; set; } //": 19.8,
        public string ForeignNormalPrice { get; set; } //": 168.3,
        public string ForeignOfferPrice { get; set; } //": 0.0,

        public List<TemplatesModels> Templates { get; set; }
    }

    public class TemplatesModels
    {
        public string Id { get; set; }
        public string Template { get; set; }
    }

    public class InCodigoModels
    {

        public string CliId { get; set; } = "";
        public string StoreId { get; set; }
        public string ItemId { get; set; }

    }

    public class Dias
    {
        public string fecha { get; set; }
        public string NombeDia { get; set; }
    }

    public class FinEmbarque
    {
        public string nombre { get; set; }
        public string comentarios { get; set; }
        public string fechaE { get; set; }
        public string timeD { get; set; }
        public string fechaAsigTrans { get; set; }
        public string ShipperName { get; set; }
        public string OrderNo { get; set; }
        public string flagE { get; set; }
        public string idex { get; set; }
    }


    public class PackageMeasure
    {
        public string Tipo { get; set; }
        public decimal Peso { get; set; }
        public decimal Largo { get; set; }
        public decimal Ancho { get; set; }
        public decimal Alto { get; set; }
      
    }

    public class ProductEmbalaje
    {
        public decimal Barcode { get; set; }
        public decimal ProductId { get; set; }
        public string ProductName { get; set; }
    }
    #endregion
    [Authorize]
    public class OrdenesController : Controller
    {
        string UrlApi = "";

        // GET: Ordenes
        public ActionResult Index()

        {
            try
            {
                Session["listaUN"] = DALServicesM.GetUN();
                return View();
            }
            catch (Exception x)
            {
                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.ERROR, "", false, x);
                throw;
            }
        }

        public ActionResult MenuOrdenes()
        {
            if (Session["Id_Num_UN"] == null)
            {
                return RedirectToAction("index");
            }
            else { return RedirectToAction("OrdenSeleccionada"); }
        }

        public ActionResult SeleccionTda(string un = "", string desc_un = "", string UnPerm_Info = "")
        {
            Session["Id_Num_UN"] = un;
            Session["Desc_Num_UN"] = desc_un;
            // nuevo parametro
            Session["UnPerm_Info"] = UnPerm_Info;

            return RedirectToAction("OrdenSeleccionada");

        }

        public ActionResult OrdenSeleccionada()
        {
            if (Session["Id_Num_UN"] != null)
            {

                string un = Session["Id_Num_UN"].ToString();

                Session["listaOrdersSurtir"] = DALServicesM.GetListaSurtir(un);
                Session["listaOrdersEmbarcar"] = DALServicesM.GetListaEmbarcar(un);
                Session["listTrans"] = DALServicesM.GetCarriersUN(int.Parse(un));
                //agregado DST
                Session["listTransDST"] = DALServicesM.GetCarriersUNDTS(int.Parse(un));



            }
            else
            {
                return RedirectToAction("Index", "Ordenes");
            }

            return View();

        }

        public ActionResult OrdenDetalle(string order)
        {

            if (!string.IsNullOrEmpty(order) & Session["Id_Num_UN"] != null)
            {
                Session["OrderSelected"] = DALServicesM.upCorpOms_Cns_OrdersByOrderNoInit(order);
                Session["listS"] = DALServicesM.GetSurtidores(Session["Id_Num_UN"].ToString());
            }
            else
            {
                return RedirectToAction("Index", "Ordenes");
            }

            return View();
        }

        public ActionResult DespOrden()
        {
            return View();
        }

        public ActionResult GetOrderDetalle(string order)
        {
            if (!string.IsNullOrEmpty(order))
            {
                System.Data.DataSet ds = DALServicesM.GetOrdersByOrderNo(order);

                Session["OrderSelected"] = ds;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    var result = new { Success = true, message = Url.Action("ConsultaDetalle", "Ordenes") };

                    return Json(result, JsonRequestBehavior.AllowGet);

                    //RedirectToAction("ConsultaDetalle", "Ordenes", new { order = order });
                }
                else
                {
                    var result = new { Success = false, Message = "Alta exitosa" };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }


            }
            return View();

        }

        [HttpPost]
        public JsonResult FinalizarSurtido(string OrderNo, string tId, string trans, string ue, string store, string status, string manual)
        {

            try
            {

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["api_FinalizarSurtido"];
                //using (HttpClient client = new HttpClient())
                //{
                //    client.BaseAddress = new Uri(apiUrl);
                //    client.DefaultRequestHeaders.Accept.Clear();
                //    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //    var in_data = "{\"order-no\": {0},\"surtidor-orden\":{\"surtidor-id\":{1},\"surtidor-name\":\"{2}\"}}".Replace("{0}", OrderNo).Replace("{1}", tId).Replace("{2}", trans);

                //                    var in_data2 = "{	\"order\":    {\"order-no\":\"{0}\",\"order\":\"\",\"store-num\":0,\"ue-no\":\"\",\"status-ue\":\"\",\"order-date\":\"\",\"order-delivery-date\":\"\",\"created-by\":\"\",\"delivery-type\":\"\",\"ue-type\":\"\",\"additionalPoints\":\"\",\"redeemedPoints\":\"\" "
                //+"     ,\"ismanualpicking\":  "
                //+ "	}  "
                //+ "    ,\"payment\":{ \"method - payment\":\"\",\"card - number\":}             "
                //+ "    ,\"customer\":{ \"customer - no\":\"\",\"customer - name\":\"\",\"phone\":\"\"}             "
                //+ "    ,\"shipments\":{ \"address1\":\"\",\"address2\":\"\",\"city\":\"\",\"state - code\":\"\",\"postal - code\":\"\",\"reference\":\"\",\"name - receives\":\"\"}             "
                //+ "	,\"product - lineitem - to - supply\":[]             "
                //+ "	,\"product-lineitem-assorted\":[]             "
                //+ "	,\"detail-payment\":{\"total\":0.0,\"num-points\":0,\"num-cashier\":0,\"num-pos\":0,\"transaction-id\":0}             "
                //+ "	,\"shipper\":{\"shipper-name\":null,\"shipping-date\":null,\"transaction-no\":null,\"traking-no\":null,\"num-bags\":0,\"num-coolers\":0,\"num-containers\":0,\"terminal\":null}             "
                //+ "	,\"delivery\":{\"delivery-date\":null,\"id-receive\":null,\"name-receive\":null,\"comments\":null}             "
                //+ "	,\"orden-pos\":{             "
                //+ "									\"order-status\":{\"order-no\":,\"store-num\":,\"order-status\":,\"order-status-desc\":\"\",\"order-date-mov\":\"\",\"order-rounding\":0}             "
                //+ "									,\"products\":[]             "
                //+ "									,\"method-payment\":{\"order-no\":,\"payment-method-id\":,\"payment-method-desc\":\"\",\"payment-method-account\":\"\",\"payment-method-amount\":0.00,\"payment-method-exp-date\":\"\",\"payment-method-bit-encrypted\":false,\"payment-method-encrypt-account\":\"\",\"payment-method-encrypt-exp-date\":\"\",\"payment-method-id-key\":\"\"}             "
                //+ "									,\"client\":{             "
                //+ "												\"order-no\":,\"customer-no\":,\"customer-first-name\":\"\",\"customer-lastname1\":\"\",\"customer-lastname2\":\"\",\"customer-personal-id\":\"\",\"customer-total-points\":0}             "
                //+ "									,			\"promotions\":[{\"pos-barcode\":0,\"order-no\":{0},\"pos-additional-points\":0,\"pos-accum-points-exch\":0.0,\"pos-redemption-points\":0,\"pos-amount-exch\":0.0,\"pos-percent-applied\":0.0}]             "
                //+ "								  }   "
                //+ "	,\"surtidor-orden\":{\"surtidor-id\":{1},\"surtidor-name\":\"{2}\"}  "
                //+ "}".Replace("{0}", OrderNo).Replace("{1}", tId).Replace("{2}", trans); 



                //InformacionOrden o = new InformacionOrden();

                //o.Orden = new InformacionDetalleOrden();
                //o.Orden.NumeroOrden = OrderNo;
                //o.Surtidor = new InformacionSurtidor();
                //o.Surtidor.SurtidorID = Convert.ToInt32(tId);
                //o.Surtidor.NombreSurtidor = trans;




                //    HttpContent c = new StringContent(JsonConvert.SerializeObject(o).ToString(), System.Text.Encoding.UTF8, "application/json");


                //    Soriana.FWK.Log.clsLogManagerFWK.WriteMessage_Loger(JsonConvert.SerializeObject(o).ToString());

                //    //Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "in_data: " + in_data2, false, null);
                //    //Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "in_data: " + JsonConvert.SerializeObject(o).ToString(), false, null);


                //    Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Url: " + apiUrl, false, null);


                //    //HttpResponseMessage response = await client.GetAsync(apiUrl);
                //    Uri u = new Uri(apiUrl);

                //    HttpRequestMessage request = new HttpRequestMessage
                //    {
                //        Method = HttpMethod.Post,
                //        RequestUri = u,
                //        Content = c
                //    };

                //    HttpResponseMessage resultApi = await client.SendAsync(request);


                //    if (resultApi.IsSuccessStatusCode)
                //    {
                //        var data = await resultApi.Content.ReadAsStringAsync();

                //        Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "IsSuccessStatusCode :" + data, false, null);

                //        // var jsonResult = JsonConvert.DeserializeObject(data).ToString();

                //        //var resp = JsonConvert.DeserializeObject<List<SupplierModel>>(data);



                //        var result2 = new { Success = true };
                //        return Json(result2, JsonRequestBehavior.AllowGet);
                //    }
                //    else //web api sent error response 
                //    {
                //        //log response status here..
                //        Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "IsSuccessStatusCode False: " + resultApi.StatusCode, false, null);

                //        var result1 = new { Success = false, Message = resultApi.StatusCode };
                //        return Json(result1, JsonRequestBehavior.AllowGet);

                //    }

                //}




                //metodo mio
                InformacionOrden o = new InformacionOrden();

                o.Orden = new InformacionDetalleOrden();
                o.Orden.NumeroOrden = OrderNo;
                o.Orden.EsPickingManual = manual.Equals("1") ? true : false;
                o.Orden.EstatusUnidadEjecucion = status;
                o.Orden.NumeroUnidadEjecucion = ue;
                o.Orden.NumeroTienda = Convert.ToInt32(store);
                o.Surtidor = new InformacionSurtidor();
                o.Surtidor.SurtidorID = Convert.ToInt32(tId);
                o.Surtidor.NombreSurtidor = trans;


                string json2 = string.Empty;
                JavaScriptSerializer js = new JavaScriptSerializer();
                //json2 = js.Serialize(o);


                json2 = JsonConvert.SerializeObject(o);

                js = null;

                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "in_data: " + json2, false, null);

                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Request: " + apiUrl, false, null);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_FinalizarSurtido"], "", json2);

                //Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestAzureMS(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_FinalizarSurtido"], "", json2);

                //var r =RestClient_2.RestClient_2.RequestAzure(System.Configuration.ConfigurationSettings.AppSettings["api_FinalizarSurtido"].ToString(), json2);



                //RestClient_2.Rest_3 r3 = new RestClient_2.Rest_3(f);



                //var client = new  WebMvc.Content.RestClient();

                //client.EndPoint = System.Configuration.ConfigurationSettings.AppSettings["api_FinalizarSurtido"];
                //client.Method = HttpVerb.POST;
                //client.PostData = json2;

                //var json = client.MakeRequest();

                //string respon = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(json);


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
        public JsonResult FinalizarEmbarque(string OrderNo, string tId, string trans, string ue, string store, string status
    , string bolsas, string contenedores, string hieleras, string checkPinPad)
        {

            try
            {

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["api_FinalizarEmbarque"];

                //metodo mio
                InformacionOrden o = new InformacionOrden();

                o.Orden = new InformacionDetalleOrden();
                o.Orden.NumeroOrden = OrderNo;

                o.Orden.EstatusUnidadEjecucion = status;
                o.Orden.NumeroUnidadEjecucion = ue;
                o.Orden.NumeroTienda = Convert.ToInt32(store);
                o.Expedidor.NombreExpedidor = "";

                o.Expedidor.NumeroBolsas = Convert.ToInt32(bolsas);
                o.Expedidor.NumeroContenedores = Convert.ToInt32(contenedores);
                o.Expedidor.NumeroEnfriadores = Convert.ToInt32(hieleras);

                if (!string.IsNullOrEmpty(checkPinPad)) { o.Expedidor.Terminal = checkPinPad; }
                else { o.Expedidor.Terminal = "0"; }


                string json2 = string.Empty;
                JavaScriptSerializer js = new JavaScriptSerializer();
                //json2 = js.Serialize(o);
                js = null;
                json2 = JsonConvert.SerializeObject(o);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "in_data: " + json2, false, null);

                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Request: " + apiUrl, false, null);

                Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_FinalizarEmbarque"], "", json2);

                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Response : " + r.code + "-Message : " + r.message, false, null);

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
        public JsonResult FinalizarTransportista(string OrderNo, string tId, string trans, string ue, string store, string status)
        {

            try
            {
                System.Collections.Hashtable listOrders = new System.Collections.Hashtable();

                string[] array = OrderNo.Split(',');



                int index_orders = 0;
                foreach (string value in array)
                {

                    if (!string.IsNullOrEmpty(value))
                    {

                        index_orders++;

                        if (!listOrders.ContainsKey(value))
                        {

                            listOrders.Add(value, "ok");
                        }
                    }
                }



                if (index_orders == 1)
                {
                    foreach (string value in array)
                    {
                        #region MyRegion
                        OrderNo = value;
                        System.Data.DataSet d = DALServicesM.GetOrdersByOrderNo(OrderNo);

                        foreach (System.Data.DataRow r1 in d.Tables[0].Rows)
                        {

                            OrderNo = r1["OrderNo"].ToString();
                            status = r1["StatusUe"].ToString();
                            ue = r1["UeNo"].ToString();
                            store = r1["StoreNum"].ToString();

                        }


                        string apiUrl = System.Configuration.ConfigurationManager.AppSettings["api_FinalizarTrans"];

                        //metodo mio
                        InformacionOrden o = new InformacionOrden();

                        o.Orden = new InformacionDetalleOrden();

                        // o.Orden.NumeroOrden = OrderNo.Replace("-","");
                        o.Orden.NumeroOrden = OrderNo;

                        o.Orden.EstatusUnidadEjecucion = status;
                        o.Orden.NumeroUnidadEjecucion = ue;
                        o.Orden.NumeroTienda = Convert.ToInt32(store);
                        o.Expedidor.NombreExpedidor = trans;
                        o.Expedidor.NumeroTransaccion = tId;
                        //o.Expedidor.MultipleOrdenes = true;

                        //foreach (string no in OrderNo.Split(','))
                        //{
                        //    o.Expedidor.NumeroOrdenes.Add(no);

                        //}
                        //o.Expedidor.NumeroBolsas = 1;
                        //o.Expedidor.NumeroContenedores = 1;
                        //o.Expedidor.NumeroEnfriadores = 1;


                        string json2 = string.Empty;
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        //json2 = js.Serialize(o);
                        js = null;
                        json2 = JsonConvert.SerializeObject(o);
                        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "in_data: " + json2, false, null);

                        Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Request: " + apiUrl, false, null);

                        Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_FinalizarTrans"], "", json2);

                        Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Response : " + r.code + "-Message : " + r.message, false, null);


                        #endregion
                    }
                }
                else
                {
                    //foreach (string value in array)
                    {
                        #region MyRegion

                        //foreach (DictionaryEntry de in listOrders)
                        //{
                        //    OrderNo = de.Key.ToString();
                        //}


                        //System.Data.DataSet d = DALServicesM.GetOrdersByOrderNo(OrderNo);
                        System.Data.DataSet d = DALServicesM.GetOrdersByOrderNo(array[0]);
                        //OrderNo = array[0];
                        foreach (System.Data.DataRow r1 in d.Tables[0].Rows)
                        {
                            OrderNo = r1["OrderNo"].ToString();
                            status = r1["StatusUe"].ToString();
                            ue = r1["UeNo"].ToString();
                            store = r1["StoreNum"].ToString();

                        }


                        string apiUrl = System.Configuration.ConfigurationManager.AppSettings["api_FinalizarTrans"];

                        //metodo mio
                        InformacionOrden o = new InformacionOrden();

                        o.Orden = new InformacionDetalleOrden();

                        //o.Orden.NumeroOrden = array[0].Replace("-","");
                        o.Orden.NumeroOrden = OrderNo;

                        o.Orden.EstatusUnidadEjecucion = status;
                        o.Orden.NumeroUnidadEjecucion = ue;
                        o.Orden.NumeroTienda = Convert.ToInt32(store);
                        o.Expedidor.NombreExpedidor = trans;

                        o.Expedidor.MultipleOrdenes = true;
                        o.Expedidor.NumeroOrdenes = new List<string>();
                        o.Expedidor.NumeroTransaccion = tId;
                        //foreach (string no in OrderNo.Split(','))
                        //{
                        //    if (string.IsNullOrEmpty(no)) { continue; }
                        //    if (!OrderNo.Equals(no))
                        //    {
                        //        o.Expedidor.NumeroOrdenes.Add(no);
                        //    }
                        //}

                        foreach (string value in array)
                        {
                            if (!value.Equals(array[0]))
                            {

                                 d = DALServicesM.GetOrdersByOrderNo(value);
                                //OrderNo = array[0];
                                foreach (System.Data.DataRow r1 in d.Tables[0].Rows)
                                {
                                   

                                    o.Expedidor.NumeroOrdenes.Add(r1["OrderNo"].ToString());
                                }


                                
                            }
                        }


                        //o.Expedidor.NumeroBolsas = 1;
                        //o.Expedidor.NumeroContenedores = 1;
                        //o.Expedidor.NumeroEnfriadores = 1;


                        string json2 = string.Empty;
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        //json2 = js.Serialize(o);
                        js = null;
                        json2 = JsonConvert.SerializeObject(o);
                        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "in_data: " + json2, false, null);


                        Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Request: " + apiUrl, false, null);

                        Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_FinalizarTrans"], "", json2);

                        Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Response : " + r.code + "-Message : " + r.message, false, null);


                        #endregion
                    }
                }






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
        public ActionResult FinalizarEntrega(List<FinEmbarque> Guias)
        {

            try
            {
                string mensaje = "Alta exitosa";
                bool succes = true;
                foreach (FinEmbarque item in Guias)
                {

                    #region Validacion Fecha
                    CultureInfo ci = new CultureInfo("Es-Es");//

                    try
                    {
                        var input = Convert.ToDateTime(item.fechaE + " " + item.timeD);
                        var maxDate = DateTime.Now.AddHours(-5);
                        var minDate = Convert.ToDateTime(item.fechaAsigTrans); ;


                        //return input >= minDate && input <= maxDate;


                        if (input < minDate || input > maxDate)
                        {
                            var result2 = new
                            {
                                Success = false,
                                Message = string.Format("ORDEN:{3} .-La hora de entrega debe estar dentro de los horarios: {0} - {1} . Hora ingresada : {2}"
                                , minDate, maxDate, input, item.OrderNo)
                                ,
                                Index = item.idex
                            };

                            return Json(result2, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    #endregion

                    #region LLamado al Api
                    string status = string.Empty, ue = string.Empty, store = string.Empty;

                    string orderNo = string.Empty;

                    System.Data.DataSet d = DALServicesM.GetOrdersByOrderNo(item.OrderNo);

                    foreach (System.Data.DataRow r1 in d.Tables[0].Rows)
                    {

                        status = r1["StatusUe"].ToString();
                        ue = r1["UeNo"].ToString();
                        store = r1["StoreNum"].ToString();
                        orderNo = r1["OrderNo"].ToString();
                    }


                    string apiUrl = System.Configuration.ConfigurationManager.AppSettings["api_Finaliza_Entrega"];

                    //metodo mio
                    InformacionOrden o = new InformacionOrden();

                    o.Orden = new InformacionDetalleOrden();
                    o.Orden.NumeroOrden = orderNo; // item.OrderNo;


                    o.Orden.EstatusUnidadEjecucion = status;
                    o.Orden.NumeroUnidadEjecucion = ue;
                    o.Orden.NumeroTienda = Convert.ToInt32(store);
                    o.Expedidor.NombreExpedidor = item.ShipperName;

                    o.Entrega.IdentificadorQuienRecibe = item.nombre;
                    o.Entrega.FechaEntrega = item.fechaE + " " + item.timeD;

                    o.Entrega.Comentarios = item.comentarios;
                    o.Entrega.NombreQuienRecibe = item.nombre;

                    o.Entrega.OrdenEntregada = item.flagE.Equals("N") ? false : true;
                    //o.Expedidor.NumeroBolsas = 1;
                    //o.Expedidor.NumeroContenedores = 1;
                    //o.Expedidor.NumeroEnfriadores = 1;


                    string json2 = string.Empty;
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    //json2 = js.Serialize(o);
                    js = null;
                    json2 = JsonConvert.SerializeObject(o);
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "in_data: " + json2, false, null);


                    Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Request: " + apiUrl, false, null);

                    Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_Finaliza_Entrega"], "", json2);

                    Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Response : " + r.code + "-Message : " + r.message, false, null);
                    #endregion
                }

                var result = new { Success = succes, Message = mensaje };

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception x)
            {
                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.ERROR, "", false, x);

                var result = new { Success = false, Message = x.Message, Index = 0 };

                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }

        /*
        [HttpPost]
        public JsonResult FinalizarEntrega(string OrderNo, string tId, string trans, string ue, string store
            , string status, string ide, string fechaD, string timeD, string comments, string flagE, string fecAsigTrans)
        {

            try
            {
                CultureInfo ci = new CultureInfo("Es-Es");//

                try
                {
                    var input = Convert.ToDateTime(fechaD + " " + timeD);
                    var maxDate = DateTime.Now.AddHours(-5);
                    var minDate = Convert.ToDateTime(fecAsigTrans); ;


                    //return input >= minDate && input <= maxDate;


                    if (input < minDate || input > maxDate)
                    {
                        var result2 = new
                        {
                            Success = false,
                            Message = string.Format("La hora de entrega debe estar dentro de los horarios: {0} - {1} . Hora ingresada : {2}"
                            , minDate, maxDate, input)
                        };

                        return Json(result2, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception)
                {

                    throw new Exception("La hora de entrega debe estar dentro de los horarios de servicio");
                }

                System.Data.DataSet d = DALServicesM.GetOrdersByOrderNo(OrderNo);

                foreach (System.Data.DataRow r1 in d.Tables[0].Rows)
                {

                    status = r1["StatusUe"].ToString();
                    ue = r1["UeNo"].ToString();
                    store = r1["StoreNum"].ToString();

                }


                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["api_Finaliza_Entrega"];

                //metodo mio
                InformacionOrden o = new InformacionOrden();

                o.Orden = new InformacionDetalleOrden();
                o.Orden.NumeroOrden = OrderNo;


                o.Orden.EstatusUnidadEjecucion = status;
                o.Orden.NumeroUnidadEjecucion = ue;
                o.Orden.NumeroTienda = Convert.ToInt32(store);
                o.Expedidor.NombreExpedidor = trans;

                o.Entrega.IdentificadorQuienRecibe = ide;
                o.Entrega.FechaEntrega = fechaD + " " + timeD;

                o.Entrega.Comentarios = comments;
                o.Entrega.NombreQuienRecibe = ide;

                o.Entrega.OrdenEntregada = flagE.Equals("N") ? false : true;
                //o.Expedidor.NumeroBolsas = 1;
                //o.Expedidor.NumeroContenedores = 1;
                //o.Expedidor.NumeroEnfriadores = 1;


                string json2 = string.Empty;
                JavaScriptSerializer js = new JavaScriptSerializer();
                //json2 = js.Serialize(o);
                js = null;
                json2 = JsonConvert.SerializeObject(o);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "in_data: " + json2, false, null);


                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Request: " + apiUrl, false, null);

                Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_Finaliza_Entrega"], "", json2);

                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Response : " + r.code + "-Message : " + r.message, false, null);

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
        */

        public ActionResult ConsultaDetalle(string order)
        {

            if (string.IsNullOrEmpty(order))
            {
                return RedirectToAction("OrdenSeleccionada", "Ordenes");
            }
            else if (Session["Id_Num_UN"] == null)
            {
                return RedirectToAction("Index", "Ordenes");
            }
            else

            {
                Session["OrderSelected"] = DALServicesM.GetOrdersByOrderNo(order);
            }
            return View();

        }

        public ActionResult Embarque(string order)
        {
            string txtchek;
            if (string.IsNullOrEmpty(order))
            {
                return RedirectToAction("OrdenSeleccionada", "Ordenes");
            }
            else if (Session["Id_Num_UN"] == null)
            {
                return RedirectToAction("Index", "Ordenes");
            }
            else
            {
                Session["OrderSelected"] = DALServicesM.GetOrdersByOrderNo(order);
                System.Data.DataSet dOP = (System.Data.DataSet)Session["OrderSelected"];

                txtchek = dOP.Tables[0].Rows[0]["UeType"].ToString().Trim();

                //if (dOP.Tables[0].Rows[0]["UeType"].ToString().Trim() == "SETC")
                if (txtchek == "SETC")
                {
                    EmbarqueSETC(order);
                }
                var txtUeNo = dOP.Tables[0].Rows[0]["UeNo"].ToString();
                int decOrder = int.Parse(dOP.Tables[0].Rows[0]["OrderNo"].ToString());
                Session["OrderPackages"] = DALEmbarques.upCorpOms_Cns_UeNoTracking(txtUeNo, decOrder);
            }

            return View();

        }

        public ActionResult EmbarqueSETC(string order)
        {
            if (string.IsNullOrEmpty(order))
            {
                return RedirectToAction("OrdenSeleccionada", "Ordenes");
            }
            else if (Session["Id_Num_UN"] == null)
            {
                return RedirectToAction("Index", "Ordenes");
            }
            else
            {
                Session["OrderSelected"] = DALServicesM.GetOrdersByOrderNo(order);
            }
            return View();

        }
        public ActionResult RecepcionGuiaEmbarque(string guia)
        {
            if (string.IsNullOrEmpty(guia))
            {
                return RedirectToAction("OrdenSeleccionada", "Ordenes");
            }
            else if (Session["Id_Num_UN"] == null)
            {
                return RedirectToAction("Index", "Ordenes");
            }
            else
            {
                Session["OrderSelected"] = DALServicesM.GetOrdersByGuiaEmb(guia);
                GenerarCodigoQR(guia);
            }
            return View();

        }
        public ActionResult GenerarCodigoQR(string guia)
        {
            try
            {
                string readString = guia;
                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                QrCode qrCode = new QrCode();

                qrEncoder.TryEncode(guia, out qrCode);

                GraphicsRenderer renderer = new GraphicsRenderer(new FixedCodeSize(400, QuietZoneModules.Zero), Brushes.Black, Brushes.White);

                MemoryStream ms = new MemoryStream();

                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
                var imageTemporal = new Bitmap(ms);
                var imagen = new Bitmap(imageTemporal, new Size(new Point(200, 200)));
                //(Image)imageTemporal.BackgroundImage = imagen;

                // Guardar en el disco duro la imagen (Carpeta del proyecto)
                var filename = guia + "_.png";
                string pngGuia = Path.Combine(Server.MapPath("~/Files/"), filename);
                imagen.Save(pngGuia, ImageFormat.Png);

                var result = new { Success = true, resp = pngGuia };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public JsonResult GetCodigoApi(string cod, string cant, string clave, string medida, string action, string order, string comentarios, string UeNo)
        {
            try
            {
                InCodigoModels c = new InCodigoModels();
                c.StoreId = Session["Id_Num_UN"].ToString();
                c.ItemId = cod;

                string json2 = string.Empty;
                JavaScriptSerializer js = new JavaScriptSerializer();
                //json2 = js.Serialize(o);
                js = null;
                json2 = JsonConvert.SerializeObject(c);
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;



                //var url = $"http://localhost:8080/item/{id}";
                var request = (HttpWebRequest)WebRequest.Create(System.Configuration.ConfigurationSettings.AppSettings["api_GetCodigo"]);
                //var url = $"http://localhost:8080/items";
                //var request = (HttpWebRequest)WebRequest.Create(url);
                //string json = $"{{\"data\":\"{data}\"}}";
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json2);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream strReader = response.GetResponseStream())
                        {
                            if (strReader == null) { }
                            else
                            {
                                using (StreamReader objReader = new StreamReader(strReader))
                                {
                                    string responseBody = objReader.ReadToEnd();
                                    // Do something with responseBody
                                    Console.WriteLine(responseBody);
                                }
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    // Handle error
                }




                Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_GetCodigo"], "", json2);

                if (r.code.ToLower().Contains("error"))
                {
                    var result2 = new { Success = false, Message = "El codigo no existe. Favor de teclearlo correctamente" };
                    return Json(result2, JsonRequestBehavior.AllowGet);
                }

                if (r.code.Trim().Equals("99"))
                {
                    var result2 = new { Success = false, Message = "Problemas de comunicacion con el servicio de consulta de Productos , favor de avisar al administrador , endpoint : " + System.Configuration.ConfigurationSettings.AppSettings["api_GetCodigo"] };
                    return Json(result2, JsonRequestBehavior.AllowGet);
                }


                CodigoModels codigo = JsonConvert.DeserializeObject<CodigoModels>(r.message);

                InformacionOrden o = new InformacionOrden();

                InformacionProductoSuministrar p = new InformacionProductoSuministrar();

                p.Cantidad = Convert.ToDouble(cant);
                p.CodigoBarra = cod;
                p.DescripcionArticulo = codigo.Description;
                p.FueSuministrado = false;
                p.NumeroOrden = order;
                p.Precio = Convert.ToDouble(codigo.NormalPrice);
                p.UnidadMedida = medida;
                p.IdentificadorProducto = codigo.Id_Num_SKU;

                o.ProductosSuministrar.Add(p);

                //DALServicesM.UpdProductsToOrder(order, cod, p.DescripcionArticulo, cant, p.Precio.ToString(), p.Precio.ToString(), p.IdentificadorProducto, medida, comentarios);
                DALServicesM.UpdProductsToOrder(order, cod, p.DescripcionArticulo, cant, p.Precio.ToString(), p.Precio.ToString(), p.IdentificadorProducto
                    , medida, action.Trim().Equals("HB") ? "hacia abajo" : "hacia arriba", UeNo); //comentarios);
                var result = new { Success = true, json = codigo.Description };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public List<UNModel> ListadoTdas()
        {

            try
            {

                var jsonResponse = "";

                var res = JsonConvert.DeserializeObject<List<UNModel>>(jsonResponse);

                return res;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Traspaso

        [HttpGet]
        public JsonResult GetOrdenesTraspaso(string Id_Num_Un)
        {
            try
            {
                //OrderSelected
                // Session["OrderSelected"] = DALServicesM.GetOrdersByOrderNo(order);
                var list = ConvertTo<OrderSelected>(DALServicesM.GetOrdersByUn(Id_Num_Un).Tables[0]);
                var result = new { Success = true, json = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpGet]
        public ActionResult GetOrdenesByOrderNoTras(string OrderNo)
        {

            try
            {
                //OrderSelected
                // Session["OrderSelected"] = DALServicesM.GetOrdersByOrderNo(order);
                var ds = DALServicesM.GetOrdersByOrderNo(OrderNo);
                OrderSelected orderSelected = new OrderSelected();


                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    orderSelected.Address1 = row["Address1"].ToString();
                    orderSelected.Address2 = row["Address2"].ToString();
                    orderSelected.PostalCode = row["PostalCode"].ToString();
                    orderSelected.StateCode = row["StateCode"].ToString();
                    orderSelected.CustomerName = row["CustomerName"].ToString();
                    orderSelected.Phone = row["Phone"].ToString();
                }



                var result = new { Success = true, json = orderSelected };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }


        public JsonResult GetTdasTaspaso()
        {
            try
            {
                if (Session["Id_Num_UN"] == null)
                {
                    var result1 = new { Success = false, Message = "OKSession" };
                    return Json(result1, JsonRequestBehavior.AllowGet);
                }
                int idNumUn = int.Parse(Session["Id_Num_UN"].ToString());
                var tdas = ConvertTo<UNModel>(DALServicesM.GetUNTraspaso(idNumUn).Tables[0]);
                var tdasFiltradas = tdas.Where(p => p.Id_Num_UN != idNumUn);
                //var data = JsonConvert.SerializeObject<List<UNModel>>(tdas);
                var result = new { Success = true, json = tdas };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public ActionResult TraspasoOrden(string NumOrden, string NumUnNva, string pass = "")
        {
            try
            {

                if (Session["Id_Num_UN"] != null)
                {
                    var dt = DALServicesM.GetPassCan(int.Parse(Session["Id_Num_UN"].ToString()));

                    string dbPass = string.Empty;
                    foreach (DataTable table in dt.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            dbPass = row[0].ToString();
                        }
                    }


                    if (dbPass.Equals(pass))
                    {
                        var msj = string.Format("Orden {0}. Cambio de tienda. Anterior:  {1}. Nueva: {2}"
                           , NumOrden, NumUnNva, Session["Id_Num_UN"].ToString());

                        DataSet ds = DALServicesM.TraspasaOrdenDeTienda(NumOrden, int.Parse(Session["Id_Num_UN"].ToString()));

                        var result = new { Success = true, Message = msj };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        var result = new { Success = false, Message = "Las credenciales son incorrectas" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var result = new { Success = false, Message = "Caduco la sesión" };
                    return RedirectToAction("Index", "Ordenes");
                    //return Json(result, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Etiquetas
        [HttpPost]
        public async Task<JsonResult> AddEtiqueta(string OrderNo, string Posicion)
        {

            try
            {



                var content = new EtiquetaModel
                {
                    OrderNo = int.Parse(OrderNo)
                    ,
                    Posicion = int.Parse(Posicion)
                };
                var json = JsonConvert.SerializeObject(content);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                string apiUrl = string.Format("{0}api/Oredenes/AddEtiqueta", UrlApi);


                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));



                    HttpResponseMessage response = await client.PostAsync(apiUrl, stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                        //var data = await response.Content.ReadAsStringAsync();
                        // var jsonResult = JsonConvert.DeserializeObject(data).ToString();

                        //var resp = JsonConvert.DeserializeObject<List<SupplierModel>>(data);
                        var result = new { Success = true };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else //web api sent error response 
                    {
                        //log response status here..

                        var result = new { Success = false, Message = response.StatusCode };
                        return Json(result, JsonRequestBehavior.AllowGet);

                    }

                }

            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion

        #region Actualiza Forma de Pago

        [HttpGet]
        public JsonResult GetMotivoCambioFP()

        {
            try
            {
                DataSet ds = DALServicesM.GetMotivoCambioFP();

                var listC = ConvertTo<MotivoCambioFP>(ds.Tables[0]);

                var result = new { Success = true, json = listC };
                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult UpdateFormaPago(string Id_Num_Orden, string Id_Num_MotCmbFP, string Obs_CambioFP = "")

        {
            try
            {
                DataSet ds = DALServicesM.UpdFormaPago(int.Parse(Id_Num_Orden), int.Parse(Id_Num_MotCmbFP), Obs_CambioFP);

                var result = new { Success = true, json = "Cambio con éxito" };
                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Observaciones
        [HttpPost]
        public JsonResult DelObservaciones(string OrderNo, string Id_Cnsc_CarObs, string CnscOrder)
        {
            try
            {
                DataSet ds = DALServicesM.DelObservaciones(int.Parse(OrderNo), int.Parse(Id_Cnsc_CarObs), int.Parse(CnscOrder));

                var result = new { Success = true, json = "Cambio con éxito" };
                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult AddObservaciones(string OrderNo, string Desc_CarObs)
        {
            try
            {
                DataSet ds = DALServicesM.AddObservaciones(int.Parse(OrderNo), Desc_CarObs);

                var result = new { Success = true, json = "Cambio con éxito" };
                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region ListaPasillosEspeciales
        public ActionResult ListaPasillosEsp(string order)
        {

            if (!string.IsNullOrEmpty(order) & Session["Id_Num_UN"] != null)
            {
                Session["OrderPasillos"] = DALServicesM.GetPasillosEspeciales(int.Parse(order));
            }
            else
            {
                return RedirectToAction("Index", "Ordenes");
            }

            return View();
        }
        #endregion

        #region Cancelacion de Orden
        [HttpPost]
        public JsonResult CancelOrder(string OrderNo, string pass, string Id_Num_MotCan = "0", string motivoCancelacion = "", string UeNo = "")
        {
            try
            {
                string msj = "Clave incorrecta";
                bool isSucces = false;
                if (Session["Id_Num_UN"] != null)
                {
                    DataSet ds = DALServicesM.GetPassCan(int.Parse(Session["Id_Num_UN"].ToString()));

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        if (ds.Tables[0].Rows[0][0].ToString().Equals(pass))
                        {
                            DALServicesM.CancelaOrden_Uup(int.Parse(OrderNo), motivoCancelacion, int.Parse(Id_Num_MotCan), UeNo);

                            isSucces = true;
                        }
                    }

                    var result = new { Success = isSucces, Message = msj };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { Success = false, json = "Caduco la sesión" };
                    RedirectToAction("Index", "Ordenes");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        //CatMotivoCan_Sup
        [HttpGet]
        public JsonResult CatMotivoCan()
        {
            try
            {

                DataSet ds = DALServicesM.CatMotivoCan_Sup();

                if (Session["Id_Num_UN"] != null)
                {


                    var listC = ConvertTo<MotivosCancel>(ds.Tables[0]);
                    var result = new { Success = true, json = listC };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { Success = false, json = "OKS" };
                    RedirectToAction("Index", "Ordenes");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region Reportes
        public ActionResult RptDiarioAct(string fechaReporte)
        {
            //DateTime fecIni = Convert.ToDateTime(string.Format("{0} 00:00:00", fechaReporte))
            //           , fecFin = Convert.ToDateTime(string.Format("{0} 23:59:59", fechaReporte));

            string fecIni = (string.Format("{0} 00:00:00", fechaReporte))
                       , fecFin = (string.Format("{0} 23:59:59", fechaReporte));

            try
            {
                if (Session["Id_Num_UN"] != null)
                {
                    int idUn = int.Parse(Session["Id_Num_UN"].ToString());

                    Session.Remove("OEntregadas");
                    Session.Remove("OCanceladas");
                    Session.Remove("OPendientesEnt");
                    Session.Remove("OPendientesSurt");
                    Session.Remove("OEnTransito");
                    Session["OEntregadas"] = DALServicesM.RepDiaSurtido(1, fecIni, fecFin, 0, idUn);
                    Session["OCanceladas"] = DALServicesM.RepDiaSurtido(3, fecIni, fecFin, 0, idUn);
                    Session["OPendientesEnt"] = DALServicesM.RepDiaSurtido(4, fecIni, fecFin, 0, idUn);
                    Session["OPendientesSurt"] = DALServicesM.RepDiaSurtido(5, fecIni, fecFin, 0, idUn);
                }
                else
                {
                    return RedirectToAction("Index", "Ordenes");
                }
            }
            catch (Exception)
            {

            }

            ViewBag.fecIni = fecIni;
            return View();
        }

        public ActionResult RptRangoFec(string fechaIni, string fechaFin
            , string isEntregadas, string isCanceladas, string isTransito, string isPensSur, string isPenEnt)
        {

            //DateTime fecIni = Convert.ToDateTime(string.Format("{0} 00:00:00", fechaIni))
            //     , fecFin = Convert.ToDateTime(string.Format("{0} 23:59:59", fechaFin));
            string fecIni = (string.Format("{0} 00:00:00", fechaIni))
        , fecFin = (string.Format("{0} 23:59:59", fechaFin));

            try
            {

                if (Session["Id_Num_UN"] != null)
                {
                    Session.Remove("OEntregadasRng");
                    Session.Remove("OCanceladasRng");
                    Session.Remove("OPendientesEntRng");
                    Session.Remove("OPendientesSurtRng");
                    Session.Remove("OEnTransitoRng");
                    int idUn = int.Parse(Session["Id_Num_UN"].ToString());
                    if (bool.Parse(isEntregadas))
                    {
                        Session["OEntregadasRng"] = DALServicesM.RepDiaSurtido(1, fecIni, fecFin, 0, idUn);
                    }
                    if (bool.Parse(isCanceladas))
                    { Session["OCanceladasRng"] = DALServicesM.RepDiaSurtido(3, fecIni, fecFin, 0, idUn); }
                    if (bool.Parse(isPenEnt))
                    { Session["OPendientesEntRng"] = DALServicesM.RepDiaSurtido(4, fecIni, fecFin, 0, idUn); }
                    if (bool.Parse(isPensSur))
                    { Session["OPendientesSurtRng"] = DALServicesM.RepDiaSurtido(5, fecIni, fecFin, 0, idUn); }
                    if (bool.Parse(isTransito))
                    { Session["OEnTransitoRng"] = DALServicesM.RepDiaSurtido(6, fecIni, fecFin, 0, idUn); }

                }
                else
                {
                    return RedirectToAction("Index", "Ordenes");
                }
            }
            catch (Exception)
            {

            }

            ViewBag.fecIni = fecIni;
            ViewBag.fecFin = fecFin;
            return View();
        }
        #endregion

        #region Actuliza Hora Orden

        public string NombreMesDia(DateTime fecha)
        {
            string result = string.Empty;
            string mes = string.Empty;
            switch (fecha.Month)
            {
                case 1:
                    mes = "Enero";
                    break;
                case 2:
                    mes = "Febrero";
                    break;
                case 3:
                    mes = "Marzo";
                    break;
                case 4:
                    mes = "Abril";
                    break;
                case 5:
                    mes = "Mayo";
                    break;
                case 6:
                    mes = "Junio";
                    break;
                case 7:
                    mes = "Julio";
                    break;
                case 8:
                    mes = "Agosto";
                    break;
                case 9:
                    mes = "Septiembre";
                    break;
                case 10:
                    mes = "Octubre";
                    break;
                case 11:
                    mes = "Noviembre";
                    break;
                case 12:
                    mes = "Diciembre";
                    break;
            }

            result = string.Format("{0} {1}", fecha.Day, mes);
            return result;
        }

        public List<Dias> HoraEntrga(string fechaOriginal, string fechaSelec)
        {
            List<Dias> dias = new List<Dias>();
            int hora = 9;
            if (fechaSelec.ToUpper().Equals("HOY"))
            {
                //var fec = Convert.ToDateTime(fechaOriginal).ToString("dd/MM/yyyy") + " " + DateTime.Now.AddHours(-5).Hour.ToString() + ":00";
                var fec = DateTime.Now.AddHours(-5).ToString();
                hora = Convert.ToDateTime(fec).AddHours(3).Hour;
            }

            if (hora > 8 & hora < 21)
            {
                for (int i = hora; i < 22; i++)
                {
                    Dias dia = new Dias();

                    dia.fecha = string.Format("{0}:01:00", i);
                    dia.NombeDia = string.Format("{0}:01:00 - {1}:00:00", i, i + 1);

                    dias.Add(dia);
                }
            }
            return dias;
        }

        public ActionResult GetDias(string fechaOriginal)
        {
            try
            {
                if (Session["Id_Num_UN"] != null)
                {
                    List<Dias> dias = new List<Dias>();
                    CultureInfo ci = new CultureInfo("Es-Es");
                    for (int i = 0; i < 7; i++)
                    {
                        Dias dia = new Dias();
                        if (i == 0)
                        {
                            dia.fecha = DateTime.Now.AddHours(-5).ToString("yyyy/MM/dd");
                            dia.NombeDia = string.Format("HOY - {0}", NombreMesDia(DateTime.Now.AddHours(-5)));//ci.DateTimeFormat.DayNames[DateTime.Now.Date.Day];
                        }
                        else
                        {
                            dia.fecha = DateTime.Now.AddHours(-5).AddDays(i).ToString("yyyy/MM/dd");

                            dia.NombeDia = string.Format("{0} - {1}", ci.DateTimeFormat.DayNames[(int)DateTime.Now.AddHours(-5).Date.AddDays(i).DayOfWeek].ToUpper()
                                , NombreMesDia(DateTime.Now.AddHours(-5).AddDays(i)));
                        }
                        dias.Add(dia);
                    }

                    var horas = HoraEntrga(fechaOriginal, "HOY");
                    var result = new { Success = true, json = dias, horas = horas };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return RedirectToAction("Index", "Ordenes");
                }
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetHorasEntrega(string fechaOriginal, string fechaSelec)
        {

            try
            {
                if (Session["Id_Num_UN"] != null)
                {

                    var dias = HoraEntrga(fechaOriginal, fechaSelec);
                    var result = new { Success = true, json = dias };
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return RedirectToAction("Index", "Ordenes");
                }
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ActualizaFecEntre(string fecha, string rangoHora, string UeNo
            , string fechaOriginal, string pass)
        {
            try
            {
                if (Session["Id_Num_UN"] != null)
                {

                    var dt = DALServicesM.GetPassCan(int.Parse(Session["Id_Num_UN"].ToString()));

                    string dbPass = string.Empty;
                    foreach (DataTable table in dt.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            dbPass = row[0].ToString();
                        }
                    }

                    if (dbPass.Equals(pass))
                    {
                        var nuevaFecEnt = (fecha + " " + rangoHora);
                        DataSet ds = DALServicesM.Fec_Entrega_Uup(UeNo, nuevaFecEnt);

                        string msj = string.Format("Orden: {0}.Cambio de fecha de entrega. Anterior {1}. Nueva {2}", UeNo
                            , fechaOriginal, nuevaFecEnt);

                        var result = new { Success = true, Message = msj };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        var result = new { Success = false, Message = "Las credenciales son incorrectas" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }


                }
                else
                {
                    return RedirectToAction("Index", "Ordenes");
                }
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region common
        public List<T> ConvertTo<T>(DataTable datatable) where T : new()
        {
            List<T> Temp = new List<T>();

            try
            {
                List<string> columnsNames = new List<string>();
                foreach (DataColumn DataColumn in datatable.Columns)
                    columnsNames.Add(DataColumn.ColumnName);
                Temp = datatable.AsEnumerable().ToList().ConvertAll<T>(row => getObject<T>(row, columnsNames));
                return Temp;
            }
            catch
            {
                return Temp;
            }
        }

        public T getObject<T>(DataRow row, List<string> columnsName) where T : new()
        {

            T obj = new T();

            try
            {
                string columnname = "";
                string value = "";
                PropertyInfo[] Properties;
                Properties = typeof(T).GetProperties();
                foreach (PropertyInfo objProperty in Properties)
                {
                    columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());
                    if (!string.IsNullOrEmpty(columnname))
                    {
                        value = row[columnname].ToString();

                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            {
                                value = row[columnname].ToString().Replace("$", "").Replace(",", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[columnname].ToString().Replace("%", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                            }
                        }
                    }
                }

                return obj;
            }
            catch
            {
                return obj;
            }
        }
        #endregion

        #region Embarques


        public ActionResult AddEmbalaje(List<PackageMeasure> Paquetes, string UeNo, int OrderNo,List<ProductEmbalaje> Products, string contentType, string TrackingType ="Normal")
        {
            string tarifa = string.Empty;
            try
            {
                List<string> folios = new List<string>();
                foreach (PackageMeasure item in Paquetes)
                {
                    #region Guias
                    var FolioDisp = DALEmbarques.upCorpOms_Cns_NextTracking().Tables[0].Rows[0]["NextTracking"].ToString();
                    folios.Add(FolioDisp);
                    string[] carriers = { "redpack", "carssa", "sendex", "noventa9minutos" };
                    List<string> lstCarriers = new List<string>(carriers);

                    EliminarTarifasAnteriores(UeNo, OrderNo);
                    foreach (var carrier in lstCarriers)
                    {
                        tarifa = CreateGuiaCotizador(UeNo, OrderNo, 1, carrier);

                        if (!tarifa.Equals("error"))
                            GuardarTarifas(UeNo, OrderNo, tarifa);
                    }

                    int peso = decimal.ToInt32(item.Peso);
                    int type = 1;

                    if (item.Tipo.Equals("CJA") || item.Tipo.Equals("EMB") || item.Tipo.Equals("STC"))
                        type = 4;

                    string guia = CreateGuiaEstafeta(UeNo, OrderNo, peso, type);

                    string servicioPaq = "estafeta";
                    string GuiaEstatus = "CREADA";
                    //TarifaModel tarifaSeleccionada = new TarifaModel();
                    //tarifaSeleccionada = SeleccionarTarifaMasEconomica(UeNo, OrderNo);

                    //var cabeceraGuia = DALEmbarques.upCorpOms_Ins_UeNoTracking(UeNo, OrderNo, IdTracking, TrackingType,
                    //PackageType, PackageLength, PackageWidth, PackageHeight, PackageWeight,
                    //User.Identity.Name, guia.Split(',')[0], guia.Split(',')[1]).Tables[0].Rows[0][0];

                    var cabeceraGuia = DALEmbarques.upCorpOms_Ins_UeNoTracking(UeNo, OrderNo, FolioDisp, TrackingType,
                    item.Tipo, item.Largo, item.Ancho, item.Alto, item.Peso,
                    User.Identity.Name, servicioPaq, guia.Split(',')[0], guia.Split(',')[1], GuiaEstatus, contentType).Tables[0].Rows[0][0];

                    #endregion
                }


                foreach (var folio in folios)
                {
                    foreach (var p in Products)
                    {
                        DALEmbarques.upCorpOms_Ins_UeNoTrackingDetail(UeNo, OrderNo, folio, TrackingType,
                        p.ProductId, p.Barcode, p.ProductName, User.Identity.Name);
                    }

                }

                var result = new { Success = true };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //Cabeceras y productos
        public ActionResult LstCabecerasGuiasProds(string UeNo, int OrderNo)
        {
            try
            {
                var cabecerasGuia = DALEmbarques.upCorpOms_Cns_UeNoTracking(UeNo, OrderNo).Tables[0].Rows[0][0];
                var productosOrden = DALEmbarques.upCorpOms_Cns_UeNoTracking(UeNo, OrderNo).Tables[0].Rows[0][0];
                var result = new { Success = true, resp = cabecerasGuia, prod = productosOrden };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        //consulta que devuelve siguiente folio disponible de guias de embarque.
        public ActionResult SigFolioDisp()
        {
            try
            {
                var FolioDisp = DALEmbarques.upCorpOms_Cns_NextTracking().Tables[0].Rows[0]["NextTracking"];
                var result = new { Success = true, resp = FolioDisp };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        //listar productos de  paquete
        public ActionResult LstProdPaquete(string UeNo, int OrderNo, string IdTracking)
        {
            try
            {
                var listaProd = DataTableToModel.ConvertTo<UeNoTrackingDetail>(DALEmbarques.upCorpOms_Cns_UeNoTrackingDetail(UeNo, OrderNo, IdTracking).Tables[0]);
                var result = new { Success = true, resp = listaProd };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        //borrar paquete
        public ActionResult BorrarPaquete(string UeNo, int OrderNo, string IdTracking, string TrackingType)
        {
            try
            {
                var paqueteBorrado = DALEmbarques.upCorpOms_Del_UeNoTrackingFull(UeNo, OrderNo, IdTracking, TrackingType).Tables[0].Rows[0][0];
                var result = new { Success = true, resp = paqueteBorrado };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        //la cabecera de la guia
        public ActionResult CreacionCabeceraGuia(string UeNo, int OrderNo, string IdTracking, string TrackingType,
            string PackageType, decimal PackageLength, decimal PackageWidth, decimal PackageHeight, decimal PackageWeight,
            string CreationId)
        {
            string tarifa = string.Empty;
            try
            {
                string[] carriers = {"redpack", "carssa", "sendex", "noventa9minutos" };
                List<string> lstCarriers = new List<string>(carriers);





                
                
                //EliminarTarifasAnteriores(UeNo, OrderNo);
                //foreach (var carrier in lstCarriers)
                //{
                //    tarifa = CreateGuiaCotizador(UeNo, OrderNo, 1, carrier);

                //    if (!tarifa.Equals("error"))
                //        GuardarTarifas(UeNo, OrderNo, tarifa);
                //}
                int type = 1;

                if (PackageType.Equals("Paquete"))
                    type = 4;
                string guia = CreateGuiaEstafeta(UeNo, OrderNo,int.Parse(PackageWeight.ToString()),type);

                string servicioPaq = "estafeta";
                string GuiaEstatus = "CREADA";
                //TarifaModel tarifaSeleccionada = new TarifaModel();
                //tarifaSeleccionada = SeleccionarTarifaMasEconomica(UeNo, OrderNo);

                //var cabeceraGuia = DALEmbarques.upCorpOms_Ins_UeNoTracking(UeNo, OrderNo, IdTracking, TrackingType,
                //PackageType, PackageLength, PackageWidth, PackageHeight, PackageWeight,
                //User.Identity.Name, guia.Split(',')[0], guia.Split(',')[1]).Tables[0].Rows[0][0];

                var cabeceraGuia = DALEmbarques.upCorpOms_Ins_UeNoTracking(UeNo, OrderNo, IdTracking, TrackingType,
                PackageType, PackageLength, PackageWidth, PackageHeight, PackageWeight,
                User.Identity.Name, servicioPaq, guia.Split(',')[0], guia.Split(',')[1], GuiaEstatus, null).Tables[0].Rows[0][0];



                //var result = new { Success = true, resp = cabeceraGuia };
                var result = new { Success = true, Message = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        public string EliminarTarifasAnteriores(string UeNo, int OrderNo)
        {

            DataSet ds = new DataSet();
            string result = string.Empty;

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@UeNo", UeNo);
                parametros.Add("@OrderNo", OrderNo);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Del_UeNoRates]", false, parametros);

            }
            catch (SqlException ex)
            {
               result = "ERRSQL";
            }
            catch (System.Exception ex)
            {
                result = "ERR";
            }

            return result;
        }
        public TarifaModel SeleccionarTarifaMasEconomica(string UeNo, int OrderNo)
        {

            DataSet ds = new DataSet();
            TarifaModel tarifa = new TarifaModel();
            string result = string.Empty;

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@UeNo", UeNo);
                parametros.Add("@OrderNo", OrderNo);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Sel_UeNoRates]", false, parametros);

                tarifa.Carrier = ds.Tables[0].Rows[0]["Carrier"].ToString();
                tarifa.currency = ds.Tables[0].Rows[0]["currency"].ToString();
                tarifa.Price = decimal.Parse(ds.Tables[0].Rows[0]["currency"].ToString());
            }
            catch (SqlException ex)
            {
                result = "ERRSQL";
            }
            catch (System.Exception ex)
            {
                result = "ERR";
            }

            return tarifa;
        }

        public string CreateGuiaEstafeta(string UeNo, int OrderNo, int weight, int typeId)
        {
            var ServiceTypeId = 1;
            DataSet ds = new DataSet();
            DataSet dsO = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@UeNo", UeNo);
                parametros.Add("@OrderNo", OrderNo);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Sel_EstafetaInfo]", false, parametros);



                System.Collections.Hashtable parametros2 = new System.Collections.Hashtable();
                parametros2.Add("@UeNo", UeNo);


                dsO = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Cns_UeNoOriginInfo]", false, parametros2);

            }
            catch (SqlException ex)
            {
                return "ERRSQL";
            }
            catch (System.Exception ex)
            {
                return "ERR";
            }

            EstafetaRequestModel m = new EstafetaRequestModel();
            foreach (DataRow r in dsO.Tables[0].Rows)
            {


                m.OriginInfo = new AddressModel();

                m.OriginInfo.address1 = r["address1"].ToString();
                m.OriginInfo.address2 = r["address2"].ToString();
                m.OriginInfo.cellPhone = r["cellPhone"].ToString();
                m.OriginInfo.city = r["city"].ToString();
                m.OriginInfo.contactName = r["contactName"].ToString();
                m.OriginInfo.corporateName = r["corporateName"].ToString();
                m.OriginInfo.customerNumber = r["customerNumber"].ToString();
                m.OriginInfo.neighborhood = r["neighborhood"].ToString();
                m.OriginInfo.phoneNumber = r["phone"].ToString();
                m.OriginInfo.state = r["state"].ToString();
                m.OriginInfo.zipCode = r["zipCode"].ToString();
                
            }

            foreach (DataRow r in ds.Tables[0].Rows)
            {

                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    m.serviceTypeId = "70";

                    if (weight >= 70)
                    {
                        m.serviceTypeId = "L0";
                    }

                    m.DestinationInfo = new AddressModel();

                    m.DestinationInfo.address1 = r["Address1"].ToString();
                    m.DestinationInfo.address2 = r["Address2"].ToString();
                    m.DestinationInfo.cellPhone = r["Phone"].ToString();
                    m.DestinationInfo.city = r["City"].ToString();
                    m.DestinationInfo.contactName = r["CustomerName"].ToString();
                    m.DestinationInfo.corporateName = r["CustomerName"].ToString();
                    m.DestinationInfo.customerNumber = r["CustomerNo"].ToString();
                    m.DestinationInfo.neighborhood = r["NameReceives"].ToString();
                    m.DestinationInfo.phoneNumber = r["Phone"].ToString();
                    m.DestinationInfo.state = r["StateCode"].ToString();
                    m.DestinationInfo.zipCode = r["PostalCode"].ToString();
                    
                    m.reference = r["Reference"].ToString();
                    m.originZipCodeForRouting = r["PostalCode"].ToString();
                    m.weight = weight; // lo capturado en el modal
                    m.parcelTypeId = typeId; // 1 - sobre, 4 - paquete
                    m.effectiveDate = r["effectiveDate"].ToString();

                //OrderNo
                //    CnscOrder
                //    StoreNum
                //    UeNo
                //    StatusUe
                //    OrderDate
                //    OrderTime
                //    CustomerNo  
                //    CustomerName 
                //    Phone   
                //    Address1 
                //    Address2    
                //    City 
                //    StateCode   
                //    PostalCode 
                //    Reference   
                //    NameReceives 
                //    Total   


                string json2 = JsonConvert.SerializeObject(m);

                    Soriana.FWK.FmkTools.RestResponse r2 = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_Estafeta_Guia"], "", json2);

                    string msg = r2.message;

                    ResponseModels re = JsonConvert.DeserializeObject<ResponseModels>(r2.message);
                    
                    string pdfcadena2 = Convert.ToBase64String(re.pdf, Base64FormattingOptions.None);

                //return re.Guia + "," + re.pdf;
                return re.Guia + "," + pdfcadena2;

            }

            return string.Empty;

        }
        public string GuardarTarifas(string UeNo, int OrderNo, string json)
        {

            DataSet ds = new DataSet();
            string result = string.Empty;
            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@UeNo", UeNo);
                parametros.Add("@OrderNo", OrderNo);
                parametros.Add("@json", json);
                parametros.Add("@createdUser", "system");

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Ins_UeNoRates]", false, parametros);

            }
            catch (SqlException ex)
            {
                return "ERRSQL";
            }
            catch (System.Exception ex)
            {
                return "ERR";
            }

            return "ok";
        }
            public string CreateGuiaCotizador(string UeNo, int OrderNo, int type, string carrier)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@UeNo", UeNo);
                parametros.Add("@carrier", carrier);
                parametros.Add("@type", type);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Cns_UeNoCotizeInfo]", false, parametros);

            }
            catch (SqlException ex)
            {
                return "ERRSQL";
            }
            catch (System.Exception ex)
            {
                return "ERR";
            }

            CotizadorRequestModel requestCotizador = new CotizadorRequestModel();

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                AddressCotzModel origin = new AddressCotzModel();

                origin.city = r["city"].ToString();
                origin.company = r["company"].ToString();
                origin.country = r["country"].ToString();
                origin.district = r["district"].ToString();
                origin.email = r["email"].ToString();
                origin.name = r["name"].ToString();
                origin.number = r["number"].ToString();
                origin.phone = r["phone"].ToString();
                origin.postalCode = r["postalCode"].ToString();
                origin.reference = r["reference"].ToString();
                origin.state = r["state"].ToString();
                origin.street = r["street"].ToString();

                CoordinatesModel coordinates = new CoordinatesModel();
                coordinates.latitudde = r["latitude"].ToString();
                coordinates.longitude = r["longitude"].ToString();

                origin.coordinates = coordinates;

                requestCotizador.origin = origin;
                
            }
            foreach (DataRow r in ds.Tables[1].Rows)
            {
                AddressCotzModel destination = new AddressCotzModel();

                destination.city = r["city"].ToString();
                destination.company = r["company"].ToString();
                destination.country = r["country"].ToString();
                destination.district = r["district"].ToString();
                destination.email = r["email"].ToString();
                destination.name = r["name"].ToString();
                destination.number = r["number"].ToString();
                destination.phone = r["phone"].ToString();
                destination.postalCode = r["postalCode"].ToString();
                destination.reference = r["reference"].ToString();
                destination.state = r["state"].ToString();
                destination.street = r["street"].ToString();

                CoordinatesModel coordinates = new CoordinatesModel();
                coordinates.latitudde = r["latitude"].ToString();
                coordinates.longitude = r["longitude"].ToString();

                destination.coordinates = coordinates;

                requestCotizador.destination = destination;
            }


            PackageModel packages = new PackageModel();
            packages.amount = 1;
            packages.content = "zapatos";
            packages.declaredValue = 0;
            packages.insurance = 0;
            packages.weight = 1;
            packages.weightUnit = "KG";
            packages.lenghtUnit = "CM";
            packages.type = "box";
            DimensionsModel dimensions = new DimensionsModel();
            dimensions.height = 20;
            dimensions.length = 11;
            dimensions.width = 15;
            packages.dimensions = dimensions;

            List<PackageModel> lstPackages = new List<PackageModel>();
            lstPackages.Add(packages);

            requestCotizador.packages = lstPackages;

            foreach (DataRow r in ds.Tables[2].Rows)
            {
                ShipmentModel shipment = new ShipmentModel();

                shipment.carrier = r["carrier"].ToString();
                shipment.type = int.Parse(r["type"].ToString());
                
                requestCotizador.shipment = shipment;
            }

            foreach (DataRow r in ds.Tables[3].Rows)
            {
                SettingsModel settings = new SettingsModel();

                settings.cashOnDelivery = r["cashOnDelivery"].ToString();
                settings.comments = r["comments"].ToString();
                settings.currency = r["currency"].ToString();
                settings.printFormat = r["printFormat"].ToString();
                settings.printSize = r["printSize"].ToString();

                requestCotizador.settings = settings;
            }

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string json2 = JsonConvert.SerializeObject(requestCotizador);

            Soriana.FWK.FmkTools.RestResponse r2 = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_Cotizador_Guia"], "", json2);

            string msg = r2.message;

            if (msg.Contains("costSummary"))
            {
                CotizadorResponseModel re = JsonConvert.DeserializeObject<CotizadorResponseModel>(r2.message);
                return msg;
            }
            else
                return "error";

        }


        //el detalle producto a producto
        public ActionResult DetalleProdaProd(string UeNo, int OrderNo, string IdTracking, string TrackingType,
            decimal ProductId, decimal Barcode, string ProductName,
            string CreationId)
        {
            try
            {
                var detalleProd = DALEmbarques.upCorpOms_Ins_UeNoTrackingDetail(UeNo, OrderNo, IdTracking, TrackingType,
            ProductId, Barcode, ProductName,
            User.Identity.Name).Tables[0].Rows[0][0];
                var result = new { Success = true, resp = detalleProd };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetPdfGuia(string guia)
        {
            string pdfBase = string.Empty;
            try
            {
                DataSet ds = new DataSet();

                string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
                if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
                {
                    conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
                }

                try
                {
                    Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                    System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                    parametros.Add("@IdTrackingService", guia);

                    ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Cns_UeNoTrackingPDF]", false, parametros);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                pdfBase = ds.Tables[0].Rows[0][0].ToString();
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {

                    throw ex;
                }
                catch (System.Exception ex)
                {

                    throw ex;
                }

                var result = new { Success = true, pdf = pdfBase };
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