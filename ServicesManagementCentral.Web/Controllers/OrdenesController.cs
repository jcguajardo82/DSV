using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using ServicesManagement.Web.Models.ProcesoSurtido;
using ServicesManagement.Web.DAL.ProcesoSurtido;
using ServicesManagement.Web.DAL.Correos;
using ServicesManagement.Web.Models.Estafeta;

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
        public long Barcode { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Pieces { get; set; }
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

        public ActionResult MonitorOrden()
        {
            var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
            int idOwner = 0;
            string tipoAlmacen = null;
            int un = 0;
            int vista = 1;

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    UserRolModel rol = new UserRolModel();
                    rol.idRol = item["rol"].ToString();
                    rol.nombreRol = item["nombreRol"].ToString();
                    Session["UserRol"] = rol;
                    if (item["idOwner"].ToString() != "")
                    {
                        idOwner = int.Parse(item["idOwner"].ToString());
                        vista = 2;
                        switch (idOwner)
                        {
                            case 2:
                                tipoAlmacen = "DST";
                                break;
                            case 3:
                                tipoAlmacen = "CEDIS";
                                break;
                            case 4:
                                tipoAlmacen = "DSV";
                                break;
                        }
                    }
                    else
                    {
                        idOwner = 0;
                        vista = 1;
                        tipoAlmacen = "";
                    }
                    if (item["idTienda"].ToString() != "")
                    {
                        un = int.Parse(item["idTienda"].ToString());
                        vista = 2;
                    }
                    else
                    {
                        un = 0;
                        vista = 1;
                    }
                }
            }
            Session["listaOrdersSurtir"] = DALServicesM.GetListaSurtirM(tipoAlmacen, un, vista);
            Session["listaOrdersEmbarcar"] = DALServicesM.GetListaEmbarcarM(tipoAlmacen, un, vista);

            return View();
        }
        public ActionResult MonitorOrdenDST()
        {
            var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
            int idOwner = 0;
            string tipoAlmacen = null;
            int un = 0;
            int vista = 1;

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    UserRolModel rol = new UserRolModel();
                    rol.idRol = item["rol"].ToString();
                    rol.nombreRol = item["nombreRol"].ToString();
                    Session["UserRol"] = rol;
                    if (item["idOwner"].ToString() != "")
                    {
                        idOwner = int.Parse(item["idOwner"].ToString());
                        vista = 2;
                        switch (idOwner)
                        {
                            case 2:
                                tipoAlmacen = "DST";
                                break;
                            case 3:
                                tipoAlmacen = "CEDIS";
                                break;
                            case 4:
                                tipoAlmacen = "DSV";
                                break;
                        }
                    }
                    else
                    {
                        idOwner = 0;
                        vista = 1;
                        tipoAlmacen = "";
                    }
                    if (item["idTienda"].ToString() != "")
                    {
                        un = int.Parse(item["idTienda"].ToString());
                        vista = 2;
                    }
                    else
                    {
                        un = 0;
                        vista = 2;
                    }
                }
            }

            if (Session["Id_Num_UN"] != null)
            {
                un = int.Parse(Session["Id_Num_UN"].ToString());
                Session["listaOrdersSurtir"] = DALServicesM.GetListaSurtirM("DST", un, 2);
                Session["listaOrdersEmbarcar"] = DALServicesM.GetListaEmbarcarM("DST", un, 2);
            }
            else
            {
                //un = int.Parse(Session["Id_Num_UN"].ToString());
                //Session["listaOrdersSurtir"] = DALServicesM.GetListaSurtirM("DST", un, vista);
                //Session["listaOrdersEmbarcar"] = DALServicesM.GetListaEmbarcarM("DST", un, vista);
                return RedirectToAction("Index", "Ordenes");
            }

            return View();
        }
        public ActionResult MonitorOrdenDSV()
        {
            var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
            int idOwner = 0;
            string tipoAlmacen = null;
            int un = 0;
            int vista = 1;

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    UserRolModel rol = new UserRolModel();
                    rol.idRol = item["rol"].ToString();
                    rol.nombreRol = item["nombreRol"].ToString();
                    Session["UserRol"] = rol;
                    if (item["idOwner"].ToString() != "")
                    {
                        idOwner = int.Parse(item["idOwner"].ToString());
                        vista = 2;
                        switch (idOwner)
                        {
                            case 2:
                                tipoAlmacen = "DST";
                                break;
                            case 3:
                                tipoAlmacen = "CEDIS";
                                break;
                            case 4:
                                tipoAlmacen = "DSV";
                                break;
                        }
                    }
                    else
                    {
                        idOwner = 0;
                        vista = 1;
                        tipoAlmacen = "";
                    }
                    if (item["idTienda"].ToString() != "")
                    {
                        un = int.Parse(item["idTienda"].ToString());
                        vista = 2;
                    }
                    else
                    {
                        un = 0;
                        vista = 2;
                    }
                }
            }

            Session["listaOrdersSurtir"] = DALServicesM.GetListaSurtirM("DSV", un, vista);
            Session["listaOrdersEmbarcar"] = DALServicesM.GetListaEmbarcarM("DSV", un, vista);

            return View();
        }
        public ActionResult MonitorOrdenCEDIS()
        {
            var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
            int idOwner = 0;
            string tipoAlmacen = null;
            int un = 0;
            int vista = 1;

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    UserRolModel rol = new UserRolModel();
                    rol.idRol = item["rol"].ToString();
                    rol.nombreRol = item["nombreRol"].ToString();
                    Session["UserRol"] = rol;
                    if (item["idOwner"].ToString() != "")
                    {
                        idOwner = int.Parse(item["idOwner"].ToString());
                        vista = 2;
                        switch (idOwner)
                        {
                            case 2:
                                tipoAlmacen = "DST";
                                break;
                            case 3:
                                tipoAlmacen = "CEDIS";
                                break;
                            case 4:
                                tipoAlmacen = "DSV";
                                break;
                        }
                    }
                    else
                    {
                        idOwner = 0;
                        vista = 2;
                        tipoAlmacen = "";
                    }
                    if (item["idTienda"].ToString() != "")
                    {
                        un = int.Parse(item["idTienda"].ToString());
                        vista = 2;
                    }
                    else
                    {
                        un = 0;
                        vista = 2;
                    }
                }
            }

            //if (Session["Id_Num_UN"] != null)
            //{
            //    un = int.Parse(Session["Id_Num_UN"].ToString());
            Session["listaOrdersSurtir"] = DALServicesM.GetListaSurtirMCEDIS("CEDIS", un, vista);
            Session["listaOrdersEmbarcar"] = DALServicesM.GetListaEmbarcarMCEDIS("CEDIS", un, vista);
            //}
            //else
            //{
            //    //un = int.Parse(Session["Id_Num_UN"].ToString());
            //    Session["listaOrdersSurtir"] = DALServicesM.GetListaSurtirM("CEDIS", un, vista);
            //    Session["listaOrdersEmbarcar"] = DALServicesM.GetListaEmbarcarM("CEDIS", un, vista);
            //    //return RedirectToAction("Index", "Ordenes");
            //}

            return View();
        }
        public ActionResult MonitorOrdenADMIN()
        {
            Session["listaOrdersSurtir"] = DALServicesM.GetListaSurtirMADMIN("ADMIN", 0, 1);
            Session["listaOrdersEmbarcar"] = DALServicesM.GetListaEmbarcarMADMIN("ADMIN", 0, 1);

            return View();
        }

        public ActionResult GenerarNvaOrden(string OrderId)
        {
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["GeneracionNvaOrden_API"]; var req = new { OrderID = OrderId.ToString() };

                string json2 = JsonConvert.SerializeObject(new { OrderID = OrderId.ToString() });
                string PPSRequest = JsonConvert.SerializeObject(req);
                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "in_data: " + json2, false, null);
                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Request: " + apiUrl, false, null);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, apiUrl, "", json2);

                JObject json = JObject.Parse(r.message);
                if (json.TryGetValue("errorCode", out JToken v1))
                {
                    if (((Newtonsoft.Json.Linq.JValue)v1).Value.ToString() == "99")
                    {
                        throw new Exception(r.message);
                    }
                }

                //if (json.TryGetValue("statusCode", out JToken v2))
                //{
                //    if (((Newtonsoft.Json.Linq.JValue)v2).Value.ToString() == "200")
                //    {
                //        throw new Exception(r.message);
                //    }
                //}

                if (r.code != "00")
                {
                    throw new Exception(r.message);
                }

                var result = new { Success = true };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message, urlRedirect = "" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
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
            Session["ordenACancelar"] = null;
            if (!string.IsNullOrEmpty(order) & Session["Id_Num_UN"] != null)
            {
                DataSet ds;
                ds = DALServicesM.upCorpOms_Cns_OrdersByOrderNoInit(order);
                var ueType = ds.Tables[0].Rows[0]["UeType"].ToString();

                if (ueType.Equals("CEDIS") || ueType.Equals("DSV"))
                    return RedirectToAction("OrdenSeleccionada", "Ordenes", new { Ueno = order });

                Session["OrderSelected"] = ds;
                Session["listS"] = DALServicesM.GetSurtidores(Session["Id_Num_UN"].ToString());
                Session["StatusDescription"] = ds.Tables[0].Rows[0]["StatusDescription"];

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

        public ActionResult GetOrderDetalle(string order, string cancelacion)
        {
            try
            {
                if (!string.IsNullOrEmpty(order))
                {
                    System.Data.DataSet ds = DALServicesM.GetOrdersByOrderNo(order);

                    Session["OrderSelected"] = ds;

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        if (cancelacion != null)
                            Session["ordenACancelar"] = order;
                        else
                            Session["ordenACancelar"] = null;

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
            }
            catch
            {
                var result = new { Success = false, Message = "Error" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return View();

        }

        [HttpPost]
        public JsonResult FinalizarSurtido(string OrderNo, string tId, string trans, string ue, string store, string status, string manual)
        {

            try
            {

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["api_FinalizarSurtido"];

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
        public JsonResult FinalizarEmbarqueDSV(string OrderNo, string tId, string trans, string ue, string store, string status
    , string bolsas, string contenedores, string hieleras, string checkPinPad)
        {

            try
            {

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["api_FinalizarEmbarqueDSV"];

                //metodo mio
                InformacionOrden o = new InformacionOrden();

                o.Orden = new InformacionDetalleOrden();
                o.Orden.NumeroOrden = OrderNo;

                o.Orden.EstatusUnidadEjecucion = "3";   //status;
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

                Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_FinalizarEmbarqueDSV"], "", json2);

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

                        //Pedido Surtido Terminado
                        if (DALCorreos.GetMetodoEnvio_sUp(int.Parse(OrderNo)).Tables[0].Rows[0][0].ToString().Contains("domicilio"))
                        {
                            var s = ue.Split('-');
                            Correos.Correos.Correo7(int.Parse(s[0]));
                        }
                        else
                        {
                            Correos.Correos.Correo15(int.Parse(OrderNo));
                        }

                        #endregion
                    }
                }
                else
                {
                    //foreach (string value in array)
                    {
                        #region MyRegion

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

                        //Pedido Surtido Terminado falta validación DST
                        if (DALCorreos.GetMetodoEnvio_sUp(int.Parse(OrderNo)).Tables[0].Rows[0][0].ToString().Contains("domicilio"))
                        {
                            var s = ue.Split('-');
                            Correos.Correos.Correo7(int.Parse(s[0]));
                        }
                        else
                        {
                            Correos.Correos.Correo15(int.Parse(OrderNo));
                        }

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
                        var maxDate = DateTime.Now.AddMinutes(-5);
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

                    DataTable dt = DALServicesM.GetTableDeliveredStatus();
                    dt.Rows.Add(ue, store, item.ShipperName, item.fechaE + " " + item.timeD, item.nombre, item.nombre, item.comentarios);
                    DALServicesM.OrdersToDeliveredStatus(dt);

                    //string json2 = string.Empty;
                    //JavaScriptSerializer js = new JavaScriptSerializer();
                    ////json2 = js.Serialize(o);
                    //js = null;
                    //json2 = JsonConvert.SerializeObject(o);
                    //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    //Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "in_data: " + json2, false, null);

                    //Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Request: " + apiUrl, false, null);

                    //Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_Finaliza_Entrega"], "", json2);

                    //Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Response : " + r.code + "-Message : " + r.message, false, null);

                    if (d.Tables[0].Rows[0]["DeliveryType"].ToString() == "Entrega a domicilio")
                        if (DALCorreos.GetMetodoEnvio_sUp(int.Parse(orderNo)).Tables[0].Rows[0][0].ToString().Contains("domicilio"))
                        {
                            //Confirmación de Entrega en domicilio DeliveryType == Entrega a domicilio
                            Correos.Correos.Correo16A(int.Parse(orderNo));
                        }
                        else
                        {
                            //Confirmación de Entrega en Tienda DeliveryType != Entrega a domicilio
                            Correos.Correos.Correo16(int.Parse(orderNo));
                        }
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

        public ActionResult ConsultaDetalleMonitor(string order)
        {

            if (string.IsNullOrEmpty(order))
            {
                return RedirectToAction("MonitorOrdenADMIN", "Ordenes");
            }
            else
            {
                DataSet ds;

                ds = DALServicesM.GetOrdersByOrderNo(order);

                var ueType = ds.Tables[0].Rows[0]["UeType"].ToString();

                int IdOwner = 0;

                if (ueType.Equals("SETC"))
                    IdOwner = 1;
                if (ueType.Equals("DST"))
                    IdOwner = 2;
                if (ueType.Equals("CEDIS"))
                    IdOwner = 3;
                if (ueType.Equals("DSV"))
                    IdOwner = 4;

                ViewBag.MotCanCD = DataTableToModel.ConvertTo<OrderFacts_UE_CancelCauses>(DALProcesoSurtido.upCorpOms_Cns_UeCancelCauses(IdOwner).Tables[0]);
                Session["OrderSelected"] = ds;
                Session["OrderStatus"] = ds.Tables[0].Rows[0]["StatusDescription"].ToString();

            }
            return View();

        }

        public ActionResult ConsultaDetalleMonitorJson(string order)
        {

            if (string.IsNullOrEmpty(order))
            {
                return RedirectToAction("MonitorOrdenADMIN", "Ordenes");
            }
            else
            {
                DataSet ds;
                ds = DALServicesM.GetOrdersByOrderNo(order);

                var itemLst = new List<ItemsGuia>();
                foreach (DataRow item in ds.Tables[5].Rows)
                {
                    itemLst.Add(
                        new ItemsGuia
                        {
                            Barcode = item["Barcode"].ToString(),
                            ProductId = item["ProductId"].ToString(),
                            ProductName = item["ProductName"].ToString()
                        }
                        ); 
                }
                var result = new { Success = true, items = itemLst };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ReasignarOrden(string OrderNo, string UeNo, string opReasignar)
        {
            try
            {
                if (string.IsNullOrEmpty(OrderNo))
                {
                    return RedirectToAction("MonitorOrdenADMIN", "Ordenes");
                }
                else
                {
                    int IdOwner = int.Parse(opReasignar);
                    string proveedorGanador = string.Empty;
                    string postalCodeCliente = string.Empty;

                    DataSet ds, ds1, ds2, ds3, ds4;
                    ds = DALProcesoSurtido.valida_Punto_de_Venta(int.Parse(OrderNo));
                    //1.- La orden no debe haber pasado por Punto de Venta
                    bool esValid = bool.Parse(ds.Tables[0].Rows[0]["esValid"].ToString());
                    if (esValid)
                    {
                        var proveedorLst = new List<NuevoProveedorReasignado>();
                        var productoLst = new List<productosProveedor>();
                        //2.- Inventario Disponible
                        ds1 = DALProcesoSurtido.upCorpOMS_Cns_ItemsToSupply(int.Parse(OrderNo));
                        postalCodeCliente = ds1.Tables[0].Rows[0]["PostalCode"].ToString();
                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {
                            int productId = int.Parse(ds1.Tables[0].Rows[i]["ProductId"].ToString());
                            string prodcutName = ds1.Tables[0].Rows[i]["ProductName"].ToString();
                            decimal quantity = decimal.Parse(ds1.Tables[0].Rows[i]["Quantity"].ToString());
                            switch (IdOwner)
                            {
                                case 2:
                                    ds2 = DALProcesoSurtido.upCorpDM_Cns_InvDispStoreNum(productId, quantity);
                                    if (ds2.Tables[0].Rows.Count > 0)
                                    {
                                        //Centro_WERKS Material_MATNR
                                        for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                                        {
                                            productoLst.Add(
                                                new productosProveedor
                                                {
                                                    Producto = ds2.Tables[0].Rows[j]["Material_MATNR"].ToString(),
                                                    Cantidad = quantity,
                                                    Costo = decimal.Parse(ds1.Tables[0].Rows[i]["Quantity"].ToString()) * decimal.Parse(ds1.Tables[0].Rows[i]["Price"].ToString()),
                                                    Peso = 0,
                                                    Volumen = 0
                                                }
                                                    );
                                            proveedorLst.Add(
                                                new NuevoProveedorReasignado
                                                {
                                                    Proveedor = ds2.Tables[0].Rows[j]["Centro_WERKS"].ToString(),
                                                    Distancia = 0,
                                                    productosTot = productoLst.Count,
                                                    Productos = productoLst,
                                                    PesoTot = 0,
                                                    VolumenTot = 0,
                                                    CostoTot = 0
                                                }
                                                );
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("No se cuenta con inventario disponible para el producto: " + productId.ToString() + " " + prodcutName + " .");
                                    }
                                    break;
                                case 3:
                                    ds2 = DALProcesoSurtido.upCorpDM_Cns_InvDispCEDISNum(productId, quantity);
                                    if (ds2.Tables[0].Rows.Count > 0)
                                    {
                                        //Centro_WERKS Material_MATNR
                                        for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                                        {
                                            productoLst.Add(
                                                new productosProveedor
                                                {
                                                    Producto = ds2.Tables[0].Rows[j]["Material_MATNR"].ToString(),
                                                    Cantidad = quantity,
                                                    Costo = decimal.Parse(ds1.Tables[0].Rows[i]["Quantity"].ToString()) * decimal.Parse(ds1.Tables[0].Rows[i]["Price"].ToString()),
                                                    Peso = 0,
                                                    Volumen = 0
                                                }
                                                );
                                            proveedorLst.Add(
                                                new NuevoProveedorReasignado
                                                {
                                                    Proveedor = ds2.Tables[0].Rows[j]["Centro_WERKS"].ToString(),
                                                    Distancia = 0,
                                                    productosTot = productoLst.Count,
                                                    Productos = productoLst,
                                                    PesoTot = 0,
                                                    VolumenTot = 0,
                                                    CostoTot = 0
                                                }
                                                );
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("No se cuenta con inventario disponible para el producto: " + productId.ToString() + " " + prodcutName + " .");
                                    }
                                    break;
                                case 4:
                                    ds2 = DALProcesoSurtido.upCorpDM_Cns_InvDispSupplierWH(productId, quantity);
                                    if (ds2.Tables[0].Rows.Count > 0)
                                    {
                                        //NumProv_LIFNR Material_MATNR
                                        for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                                        {
                                            productoLst.Add(
                                                new productosProveedor
                                                {
                                                    Producto = ds2.Tables[0].Rows[j]["Material_MATNR"].ToString(),
                                                    Cantidad = quantity,
                                                    Costo = decimal.Parse(ds1.Tables[0].Rows[i]["Quantity"].ToString()) * decimal.Parse(ds1.Tables[0].Rows[i]["Price"].ToString()),
                                                    Peso = 0,
                                                    Volumen = 0
                                                }
                                                );
                                            proveedorLst.Add(
                                                new NuevoProveedorReasignado
                                                {
                                                    Proveedor = ds2.Tables[0].Rows[j]["NumProv_LIFNR"].ToString(),
                                                    Distancia = 0,
                                                    productosTot = productoLst.Count,
                                                    Productos = productoLst,
                                                    PesoTot = 0,
                                                    VolumenTot = 0,
                                                    CostoTot = 0
                                                }
                                                );
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("No se cuenta con inventario disponible para el producto: " + productId.ToString() + " " + prodcutName + " .");
                                    }
                                    break;
                            }
                        }
                        //3.- Menor distancia
                        decimal distancia = 0;
                        for (int k = 0; k < proveedorLst.Count; k++)
                        {
                            //Llamado a SP que calcula la distancia
                            proveedorGanador = proveedorLst[k].Proveedor;
                            distancia = proveedorLst[k].Distancia;
                            ds3 = DALProcesoSurtido.upCorpOMS_Cns_DistanciaClienteProveedor(int.Parse(OrderNo), proveedorLst[k].Proveedor, postalCodeCliente);
                            for (int j = 0; j < proveedorLst.Count; j++)
                            {
                                proveedorLst[k].Distancia = decimal.Parse(ds3.Tables[0].Rows[0]["Distancia"].ToString());
                                proveedorLst[k].CodigoPostal = ds3.Tables[0].Rows[0]["CodigoPostal"].ToString();
                            }

                            //4.- Cobertura
                            for (int h = 0; h < proveedorLst[k].Productos.Count; h++)
                            {
                                string productReenviar = proveedorLst[k].Productos[h].Producto;
                                ds4 = DALServicesM.GetDimensionsByProducts(productReenviar);
                                if (ds4.Tables[0].Rows.Count > 0)
                                {
                                    proveedorLst[k].Productos[h].Peso = decimal.Parse(ds4.Tables[0].Rows[0]["Peso"].ToString()) * proveedorLst[k].Productos[h].Cantidad;
                                    proveedorLst[k].Productos[h].Volumen = decimal.Parse(ds4.Tables[0].Rows[0]["PesoVol"].ToString()) * proveedorLst[k].Productos[h].Cantidad;
                                    proveedorLst[k].PesoTot = proveedorLst[k].PesoTot + proveedorLst[k].Productos[h].Peso;
                                    proveedorLst[k].VolumenTot = proveedorLst[k].VolumenTot + proveedorLst[k].Productos[h].Volumen;
                                    proveedorLst[k].CostoTot = proveedorLst[k].CostoTot + proveedorLst[k].Productos[h].Costo;
                                }
                                else
                                {
                                    throw new Exception("No se encuentra el producto: " + proveedorLst[k].Productos[h].Producto + " .");
                                }
                            }

                            if (proveedorLst[k].Distancia > distancia)
                            {
                                proveedorGanador = proveedorLst[k].Proveedor;
                                distancia = proveedorLst[k].Distancia;
                            }
                        }                        
                    }

                    //var result = new { Success = true, items = itemLst };
                    var result = new { Success = true, Reasignado = proveedorGanador };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ConsultaDetalle(string order)
        {

            if (string.IsNullOrEmpty(order))
            {
                return RedirectToAction("OrdenSeleccionada", "Ordenes");
            }
            else if (Session["Id_Num_UN"] == null && Session["ordenACancelar"] == null)
            {
                return RedirectToAction("Index", "Ordenes");
            }
            else

            {
                DataSet ds;

                ds = DALServicesM.GetOrdersByOrderNo(order);

                var ueType = ds.Tables[0].Rows[0]["UeType"].ToString();

                int IdOwner = 0;

                if (ueType.Equals("SETC"))
                    IdOwner = 1;
                if (ueType.Equals("DST"))
                    IdOwner = 2;
                if (ueType.Equals("CEDIS") || ueType.Equals("DSV"))
                    return RedirectToAction("OrdenSeleccionada", "Ordenes", new { Ueno = order });

                ViewBag.MotCanCD = DataTableToModel.ConvertTo<OrderFacts_UE_CancelCauses>(DALProcesoSurtido.upCorpOms_Cns_UeCancelCauses(IdOwner).Tables[0]);
                Session["OrderSelected"] = ds;
                Session["OrderStatus"] = ds.Tables[0].Rows[0]["StatusDescription"].ToString();

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


                            Correos.Correos.Correo8A(int.Parse(OrderNo), 2);
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
        public JsonResult CancelOrder2(string OrderNo, string pass, string Id_Num_MotCan = "0", string motivoCancelacion = "", string UeNo = "")
        {
            try
            {
                string msj = "Clave incorrecta";
                bool isSucces = false;
                //if (Session["Id_Num_UN"] != null)
                //{
                DataSet ds = DALServicesM.GetPassCan2(1, UeNo);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString().Equals(pass))
                    {
                        DALServicesM.CancelaOrden_Uup2(int.Parse(OrderNo), motivoCancelacion, int.Parse(Id_Num_MotCan), UeNo, User.Identity.Name);

                        isSucces = true;
                        //Cancelación de Productos SETC
                        Correos.Correos.Correo8A(int.Parse(OrderNo), 2);
                    }
                }

                var result = new { Success = isSucces, Message = msj };
                return Json(result, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    var result = new { Success = false, json = "Caduco la sesión" };
                //    RedirectToAction("Index", "Ordenes");
                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}
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

        [HttpPost]
        public ActionResult AddEmbalaje(List<PackageMeasure> Paquetes, string UeNo, int OrderNo, List<ProductEmbalaje> Products, string TrackingType = "Normal")
        {
            string tarifa = string.Empty, paqueteria = string.Empty, guia = string.Empty, servicioPaq = string.Empty, service = string.Empty, trackUrl = string.Empty, requestRecoleccion = string.Empty;
            try
            {
                int enviaCom = int.Parse(DALServicesM.ActiveEnviaCom().Tables[0].Rows[0][0].ToString());

                #region Guias

                List<string> folios = new List<string>();
                foreach (PackageMeasure item in Paquetes)
                {
                    var FolioDisp = DALEmbarques.upCorpOms_Cns_NextTracking().Tables[0].Rows[0]["NextTracking"].ToString();
                    folios.Add(FolioDisp);
                    List<CarrierRequest> lstCarrierRequests = new List<CarrierRequest>();

                    EliminarTarifasAnteriores(UeNo, OrderNo);
                    CrearCotizacionesLogyt(Products, OrderNo);

                    decimal decimalPeso = decimal.Round(decimal.Parse(Session["SumPeso"].ToString()));
                    int peso = decimal.ToInt32(decimalPeso);

                    if (enviaCom == 1 && peso <= 70)
                    {
                        DataSet dsCarriers = DALServicesM.CarriersPorTransportista("envia.com");
                        foreach (DataRow row in dsCarriers.Tables[0].Rows)
                        {
                            var tarifaCarrier = CreateGuiaCotizador(UeNo, OrderNo, 1, row["Carrier"].ToString(), peso);

                            if (tarifaCarrier.Carrier != null)
                            {
                                GuardarTarifas(UeNo, OrderNo, tarifaCarrier.msj);
                                lstCarrierRequests.Add(tarifaCarrier);
                            }
                        }
                    }
                    int type = 1;

                    long cotizeId = 0;
                    if (item.Tipo.Equals("CJA") || item.Tipo.Equals("EMB") || item.Tipo.Equals("STC"))
                        type = 4;

                    DataSet dsCarrier = DALServicesM.CarrierRates(OrderNo);

                    foreach (DataRow row in dsCarrier.Tables[0].Rows)
                    {
                        paqueteria = row["Carrier"].ToString();
                        service = row["Service"].ToString();
                        cotizeId = long.Parse(row["cotizeId"].ToString());

                        if (paqueteria.Contains("Logyt"))
                        {
                            guia = CreateGuiaLogyt(UeNo, OrderNo, peso, type);

                            servicioPaq = "Logyt-Estafeta"; //esta variable sera dinamica
                        }
                        if (paqueteria.Equals("Estafeta"))
                        {
                            DataSet dsEstafeta = DALServicesM.EstafetaActive();
                            if (dsEstafeta != null)
                            {
                                if (dsEstafeta.Tables[0].Rows[0][1].ToString().ToLower().Equals("estafeta"))
                                    guia = CreateGuiaEstafeta(UeNo, OrderNo, peso, type);
                                else
                                    guia = CreateGuiaEstafetaAPI(UeNo, OrderNo, decimalPeso, Products, dsCarrier, null);
                            }
                            else
                                throw new Exception("No hay Canal de Estafeta");
                            //guia = CreateGuiaEstafeta(UeNo, OrderNo, peso, type);
                            //guia = CreateGuiaEstafetaAPI(OrderNo, decimalPeso, Products, dsCarrier, null);

                            servicioPaq = "Soriana-Estafeta"; //esta variable sera dinamica
                        }
                        if (!paqueteria.Equals("Estafeta") && !paqueteria.Contains("Logyt"))
                        {
                            var request = lstCarrierRequests.Where(x => x.Carrier == paqueteria).FirstOrDefault().requests[0];
                            guia = CreateGuiaEnvia(request, service, peso);
                            if (guia != "ERROR")
                            {
                                servicioPaq = "Envia-" + paqueteria;
                                trackUrl = guia.Split(',')[2];
                                requestRecoleccion = lstCarrierRequests.Where(x => x.Carrier == paqueteria).FirstOrDefault().requests[1];
                            }
                        }
                        if (guia != "ERROR")
                            break;
                    }

                    if (guia == "ERROR")
                        throw new Exception("No se pudo generar la guia con ninguna de las paqueterias. ");

                    string GuiaEstatus = "CREADA";

                    var cabeceraGuia = DALEmbarques.upCorpOms_Ins_UeNoTracking(UeNo, OrderNo, FolioDisp, TrackingType,
                    item.Tipo, item.Largo, item.Ancho, item.Alto, peso,
                    User.Identity.Name, servicioPaq, guia.Split(',')[0], guia.Split(',')[1], GuiaEstatus, null, trackUrl).Tables[0].Rows[0][0];
                    DALServicesM.CarrierSelected(OrderNo, cotizeId);

                    if (!string.IsNullOrEmpty(requestRecoleccion))
                    {
                        var responseRecoleccion = RecoleccionEnvia(requestRecoleccion);

                        if (responseRecoleccion.Contains("carrier"))
                            DALServicesM.GuardarPickUp(UeNo, OrderNo, FolioDisp, responseRecoleccion, User.Identity.Name, null, null, null);
                        else
                        {
                            RecoleccionRequestModel request = JsonConvert.DeserializeObject<RecoleccionRequestModel>(requestRecoleccion);
                            DALServicesM.GuardarPickUp(UeNo, OrderNo, FolioDisp, "--", User.Identity.Name, paqueteria, request.shipment.pickup.date, request.origin.postalCode);
                        }
                    }
                }

                //DESCOMENTAR
                foreach (var folio in folios)
                {
                    foreach (var p in Products)
                    {
                        DALEmbarques.upCorpOms_Ins_UeNoTrackingDetail(UeNo, OrderNo, folio, TrackingType,
                        p.ProductId, p.Barcode, p.ProductName, User.Identity.Name);
                    }

                }

                // 2021-12-15: llamado a la clase de los corres, especificamente al template 7 para confirmacion de Envio.
                //var s = UeNo.Split('-');
                //Correos.Correos.Correo7(int.Parse(s[0]));
                Correos.Correos.Correo7(OrderNo);

                #endregion


                var result = new { Success = true };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        private void CrearCotizacionesLogyt(List<ProductEmbalaje> Products, int orderNo)
        {
            string productsAll = string.Empty;
            bool bigTicket = false;
            decimal sumPeso = 0, width = 0, length = 0, heigth = 0;
            DataTable dt = DALServicesM.GetTableProducts();

            foreach (var p in Products)
            {
                productsAll += p.ProductId.ToString() + ",";
            }
            List<WeightByProducts> lstPesos = DataTableToModel.ConvertTo<WeightByProducts>(DALServicesM.GetDimensionsByProducts(productsAll).Tables[0]);

            Session["ListWeightByProducts"] = lstPesos;

            foreach (var item in lstPesos)
            {
                if (item.PesoVol > item.Peso)
                {
                    if (item.PesoVol > 70)
                        bigTicket = true;

                    var piezas = Products.Where(x => x.ProductId == item.Product).FirstOrDefault().Pieces;
                    sumPeso += (item.PesoVol * piezas);
                    width += item.Width;
                    length += item.Lenght;
                    heigth += item.Height;

                    dt.Rows.Add(item.Product, item.PesoVol, item.Cve_CategSAP, item.Cve_GciaCategSAP, item.Cve_GpoCategSAP, item.Desc_CategSAP);
                }
                else
                {
                    if (item.Peso > 70)
                        bigTicket = true;

                    var piezas = Products.Where(x => x.ProductId == item.Product).FirstOrDefault().Pieces;
                    sumPeso += (item.Peso * piezas);
                    width += item.Width;
                    length += item.Lenght;
                    heigth += item.Height;

                    dt.Rows.Add(item.Product, item.Peso, item.Cve_CategSAP, item.Cve_GciaCategSAP, item.Cve_GpoCategSAP, item.Desc_CategSAP);
                }


            }

            if (sumPeso < 1)
                sumPeso = 1;

            var widthRound = decimal.Round(width);
            if (widthRound > 1)
                Session["SumWidth"] = decimal.ToInt32(widthRound);
            else
                Session["SumWidth"] = 1;

            var lengthRound = decimal.Round(length);
            if (lengthRound > 1)
                Session["SumLength"] = decimal.ToInt32(lengthRound);
            else
                Session["SumLength"] = 1;

            var heightRound = decimal.Round(heigth);
            if (heightRound > 1)
                Session["SumHeigth"] = decimal.ToInt32(heightRound);
            else
                Session["SumHeigth"] = 1;

            Session["SumPeso"] = sumPeso;

            DALServicesM.GuardarTarifasLogyt(orderNo, sumPeso, bigTicket, dt);

        }
        public ActionResult AddEmbalajePendiente(List<ShipmentToTrackingModel> Paquetes)
        {
            string tarifa = string.Empty, paqueteria = string.Empty, guia = string.Empty, servicioPaq = string.Empty, service = string.Empty, trackUrl = string.Empty, requestRecoleccion = string.Empty;
            try
            {
                int enviaCom = int.Parse(DALServicesM.ActiveEnviaCom().Tables[0].Rows[0][0].ToString());
                foreach (ShipmentToTrackingModel item in Paquetes)
                {
                    #region Guias
                    //List<string> folios = new List<string>();
                    var FolioDisp = DALEmbarques.upCorpOms_Cns_NextTracking().Tables[0].Rows[0]["NextTracking"].ToString();

                    List<CarrierRequest> lstCarrierRequests = new List<CarrierRequest>();

                    EliminarTarifasAnteriores(item.ueNo, item.orderNo);
                    CrearCotizacionesLogytPendiente(item);

                    var widthRound = decimal.Round(item.ancho);
                    if (widthRound > 1)
                        Session["SumWidth"] = decimal.ToInt32(widthRound);
                    else
                        Session["SumWidth"] = 1;

                    var lengthRound = decimal.Round(item.largo);
                    if (lengthRound > 1)
                        Session["SumLength"] = decimal.ToInt32(lengthRound);
                    else
                        Session["SumLength"] = 1;

                    var heightRound = decimal.Round(item.alto);
                    if (heightRound > 1)
                        Session["SumHeigth"] = decimal.ToInt32(heightRound);
                    else
                        Session["SumHeigth"] = 1;


                    decimal decimalPeso = decimal.Round(decimal.Parse(Session["SumPeso"].ToString()));
                    decimal decimalRound = decimal.Round(decimalPeso);
                    if (decimalRound == 0)
                        decimalRound = 1;

                    int peso = decimal.ToInt32(decimalPeso);

                    if (enviaCom == 1 && peso <= 70)
                    {
                        DataSet dsCarriers = DALServicesM.CarriersPorTransportista("envia.com");
                        foreach (DataRow row in dsCarriers.Tables[0].Rows)
                        {
                            var tarifaCarrier = CreateGuiaCotizador(item.ueNo, item.orderNo, 1, row["Carrier"].ToString(), peso);

                            if (tarifaCarrier.Carrier != null)
                            {
                                GuardarTarifas(item.ueNo, item.orderNo, tarifaCarrier.msj);
                                lstCarrierRequests.Add(tarifaCarrier);
                            }
                        }
                    }
                    long cotizeId = 0;
                    int type = 1;

                    if (item.tipoEmpaque.Equals("CJA") || item.tipoEmpaque.Equals("EMB") || item.tipoEmpaque.Equals("STC"))
                        type = 4;

                    DataSet dsCarrier = DALServicesM.CarrierRates(item.orderNo);

                    foreach (DataRow row in dsCarrier.Tables[0].Rows)
                    {
                        paqueteria = row["Carrier"].ToString();
                        service = row["Service"].ToString();
                        cotizeId = long.Parse(row["cotizeId"].ToString());

                        if (paqueteria.Contains("Logyt"))
                        {
                            guia = CreateGuiaLogyt(item.ueNo, item.orderNo, peso, type);

                            servicioPaq = "Logyt-Estafeta"; //esta variable sera dinamica
                        }
                        if (paqueteria.Equals("Estafeta"))
                        {
                            DataSet dsEstafeta = DALServicesM.EstafetaActive();
                            if (dsEstafeta != null)
                            {
                                if (dsEstafeta.Tables[0].Rows[0][1].ToString().ToLower().Equals("estafeta"))
                                    guia = CreateGuiaEstafeta(item.ueNo, item.orderNo, peso, type);
                                else
                                    guia = CreateGuiaEstafetaAPI(item.ueNo, item.orderNo, decimalPeso, null, dsCarrier, item);
                            }
                            else
                                throw new Exception("No hay Canal de Estafeta");

                            servicioPaq = "Soriana-Estafeta"; //esta variable sera dinamica
                        }
                        if (!paqueteria.Equals("Estafeta") && !paqueteria.Contains("Logyt"))
                        {
                            var request = lstCarrierRequests.Where(x => x.Carrier == paqueteria).FirstOrDefault().requests[0];
                            guia = CreateGuiaEnvia(request, service, peso);
                            if (guia != "ERROR")
                            {
                                servicioPaq = "Envia-" + paqueteria;
                                trackUrl = guia.Split(',')[2];
                                requestRecoleccion = lstCarrierRequests.Where(x => x.Carrier == paqueteria).FirstOrDefault().requests[1];
                            }
                        }
                        if (guia != "ERROR")
                            break;
                    }

                    if (guia == "ERROR")
                        throw new Exception("No se pudo generar la guia de la orden " + item.orderNo + " con ninguna de las paqueterias.");


                    string GuiaEstatus = "CREADA";
                    //TarifaModel tarifaSeleccionada = new TarifaModel();
                    //tarifaSeleccionada = SeleccionarTarifaMasEconomica(UeNo, OrderNo);

                    //var cabeceraGuia = DALEmbarques.upCorpOms_Ins_UeNoTracking(UeNo, OrderNo, IdTracking, TrackingType,
                    //PackageType, PackageLength, PackageWidth, PackageHeight, PackageWeight,
                    //User.Identity.Name, guia.Split(',')[0], guia.Split(',')[1]).Tables[0].Rows[0][0];

                    //var cabeceraGuia = DALEmbarques.upCorpOms_Ins_UeNoTracking(UeNo, OrderNo, FolioDisp, TrackingType,
                    //item.Tipo, item.Largo, item.Ancho, item.Alto, item.Peso,
                    //User.Identity.Name, servicioPaq, guia.Split(',')[0], guia.Split(',')[1], GuiaEstatus, null, trackUrl).Tables[0].Rows[0][0];

                    var cabeceraGuia = DALEmbarques.upCorpOms_Ins_UeNoTracking(item.ueNo, item.orderNo, FolioDisp, "Normal",
                     item.tipoEmpaque, item.largo, item.ancho, item.alto, peso,
                     User.Identity.Name, servicioPaq, guia.Split(',')[0], guia.Split(',')[1], GuiaEstatus, item.ucc, trackUrl).Tables[0].Rows[0][0];

                    DALEmbarques.upCorpOms_Ins_UeNoTrackingDetail(item.ueNo, item.orderNo, FolioDisp, "Normal",
                     item.productId, item.barcode, "000000000", User.Identity.Name);

                    DALServicesM.UCCProcesada(item.ucc);
                    DALServicesM.CarrierSelected(item.orderNo, cotizeId);

                    if (!string.IsNullOrEmpty(requestRecoleccion))
                    {
                        var responseRecoleccion = RecoleccionEnvia(requestRecoleccion);

                        if (responseRecoleccion.Contains("carrier"))
                            DALServicesM.GuardarPickUp(item.ueNo, item.orderNo, FolioDisp, responseRecoleccion, User.Identity.Name, null, null, null);
                        else
                        {
                            RecoleccionRequestModel request = JsonConvert.DeserializeObject<RecoleccionRequestModel>(requestRecoleccion);
                            DALServicesM.GuardarPickUp(item.ueNo, item.orderNo, FolioDisp, "--", User.Identity.Name, paqueteria, request.shipment.pickup.date, request.origin.postalCode);
                        }
                    }
                    // 2021-12-15: llamado a la clase de los corres, especificamente al template 7 para confirmacion de Envio.
                    Correos.Correos.Correo7(item.orderNo);
                    //var s = item.ueNo.Split('-');
                    //Correos.Correos.Correo7(int.Parse(s[0]));

                    #endregion

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
        private void CrearCotizacionesLogytPendiente(ShipmentToTrackingModel Paquete)
        {
            string productsAll = string.Empty;
            bool bigTicket = false;
            decimal sumPeso = 0;
            DataTable dt = DALServicesM.GetTableProducts();

            productsAll = Paquete.productId.ToString();

            List<WeightByProducts> lstPesos = DataTableToModel.ConvertTo<WeightByProducts>(DALServicesM.GetDimensionsByProducts(productsAll).Tables[0]);

            Session["ListWeightByProducts"] = lstPesos;

            foreach (var item in lstPesos)
            {
                if (item.PesoVol > item.Peso)
                {
                    if (item.PesoVol > 70)
                        bigTicket = true;

                    var piezas = Paquete.piezas;
                    sumPeso = sumPeso + (item.PesoVol * piezas);
                    dt.Rows.Add(item.Product, item.PesoVol, item.Cve_CategSAP, item.Cve_GciaCategSAP, item.Cve_GpoCategSAP, item.Desc_CategSAP);
                }
                else
                {
                    if (item.Peso > 70)
                        bigTicket = true;

                    var piezas = Paquete.piezas;
                    sumPeso = sumPeso + (item.Peso * piezas);
                    dt.Rows.Add(item.Product, item.Peso, item.Cve_CategSAP, item.Cve_GciaCategSAP, item.Cve_GpoCategSAP, item.Desc_CategSAP);
                }

            }
            if (sumPeso < 1)
                sumPeso = 1;

            Session["SumPeso"] = sumPeso;

            //DataSet ds = DALServicesM.OrdersLogistics(Paquete.orderNo, sumPeso, bigTicket);
            DALServicesM.GuardarTarifasLogyt(Paquete.orderNo, sumPeso, bigTicket, dt);
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
                string[] carriers = { "redpack", "carssa", "sendex", "noventa9minutos" };
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
                string guia = CreateGuiaEstafeta(UeNo, OrderNo, int.Parse(PackageWeight.ToString()), type);

                string servicioPaq = "Logyt-Estafeta";
                string GuiaEstatus = "CREADA";
                //TarifaModel tarifaSeleccionada = new TarifaModel();
                //tarifaSeleccionada = SeleccionarTarifaMasEconomica(UeNo, OrderNo);

                //var cabeceraGuia = DALEmbarques.upCorpOms_Ins_UeNoTracking(UeNo, OrderNo, IdTracking, TrackingType,
                //PackageType, PackageLength, PackageWidth, PackageHeight, PackageWeight,
                //User.Identity.Name, guia.Split(',')[0], guia.Split(',')[1]).Tables[0].Rows[0][0];

                var cabeceraGuia = DALEmbarques.upCorpOms_Ins_UeNoTracking(UeNo, OrderNo, IdTracking, TrackingType,
                PackageType, PackageLength, PackageWidth, PackageHeight, PackageWeight,
                User.Identity.Name, servicioPaq, guia.Split(',')[0], guia.Split(',')[1], GuiaEstatus, null, "").Tables[0].Rows[0][0];

                // 2021-12-15: llamado a la clase de los corres, especificamente al template 7 para confirmacion de Envio.
                //Correos.Correos.Correo7(OrderNo);
                var s = UeNo.Split('-');
                Correos.Correos.Correo7(int.Parse(s[0]));

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

        private string RequestEstafeta(DataSet ds, decimal weight, List<ProductEmbalaje> Products, DataSet dsCarrier, ShipmentToTrackingModel packageCEDIS, string UeNo)
        {
            string json = string.Empty;

            RequestEstafetaModel model = new RequestEstafetaModel();
            LabelDefinition label = new LabelDefinition();

            WayBillDocument wayBill = new WayBillDocument();
            wayBill.aditionalInfo = "Informacion Adicional";
            wayBill.content = "Contenido";
            wayBill.costCenter = "SPMXA12345";
            wayBill.customerShipmentId = null;
            var reference = ds.Tables[0].Rows[0]["addressReference1"].ToString() + " " + ds.Tables[0].Rows[0]["addressReference2"].ToString();
            wayBill.referenceNumber = reference.Length > 30 ? reference.Substring(0, 29) : reference;
            wayBill.groupShipmentId = null;

            label.wayBillDocument = wayBill;

            ItemDescription itemDescription = new ItemDescription();
            itemDescription.parcelId = 1;
            itemDescription.weight = weight;
            itemDescription.height = int.Parse(Session["SumHeigth"].ToString()) > 190 ? 190 : int.Parse(Session["SumHeigth"].ToString());
            itemDescription.length = int.Parse(Session["SumLength"].ToString()) > 240 ? 240 : int.Parse(Session["SumLength"].ToString());
            itemDescription.width = int.Parse(Session["SumWidth"].ToString()) > 120 ? 120 : int.Parse(Session["SumWidth"].ToString());

            Merchandises merchandises = new Merchandises();
            merchandises.totalGrossWeight = weight;
            merchandises.weightUnitCode = "KGM";

            List<Merchandise> lstMerchandises = new List<Merchandise>();
            List<WeightByProducts> lstWeightByProducts = (List<WeightByProducts>)Session["ListWeightByProducts"];
            if (Products != null)
            {
                foreach (var product in Products)
                {
                    var total = 0.0;
                    var quantity = 0.0;

                    Merchandise merchandise = new Merchandise();

                    foreach (DataRow r in ds.Tables[2].Rows)
                    {
                        if (r["PosBarCode"].ToString().Equals(product.Barcode.ToString()))
                        {
                            total = double.Parse(r["Total"].ToString());
                            quantity = double.Parse(r["PosBarCode"].ToString());

                        }
                    }
                    merchandise.merchandiseValue = total;
                    merchandise.currency = "MXN";
                    merchandise.productServiceCode = lstWeightByProducts.Where(x => x.Product == long.Parse(product.ProductId.ToString())).FirstOrDefault().SAT_IdProdServ;
                    merchandise.merchandiseQuantity = product.Pieces;
                    merchandise.measurementUnitCode = lstWeightByProducts.Where(x => x.Product == long.Parse(product.ProductId.ToString())).FirstOrDefault().SAT_UniMedProd;
                    merchandise.tariffFraction = "12345678";
                    merchandise.UUIDExteriorTrade = "ABCDed02-a12A-B34B-c56C-c5abcdef61F2";
                    merchandise.isInternational = false;
                    merchandise.isImport = false;
                    merchandise.isHazardousMaterial = false;
                    merchandise.hazardousMaterialCode = "M0035";
                    merchandise.packagingCode = "4A";

                    lstMerchandises.Add(merchandise);
                }
            }
            else
            {
                var total = 0.0;
                var quantity = 0.0;

                Merchandise merchandise = new Merchandise();

                foreach (DataRow r in ds.Tables[2].Rows)
                {
                    if (r["PosBarCode"].ToString().Equals(packageCEDIS.barcode.ToString()))
                    {
                        total = double.Parse(r["Total"].ToString());
                        quantity = double.Parse(r["PosBarCode"].ToString());

                    }
                }
                merchandise.merchandiseValue = total;
                merchandise.currency = "MXN";
                merchandise.productServiceCode = lstWeightByProducts.Where(x => x.Product == long.Parse(packageCEDIS.productId.ToString())).FirstOrDefault().SAT_IdProdServ;
                merchandise.merchandiseQuantity = packageCEDIS.piezas;
                merchandise.measurementUnitCode = lstWeightByProducts.Where(x => x.Product == long.Parse(packageCEDIS.productId.ToString())).FirstOrDefault().SAT_UniMedProd;
                merchandise.tariffFraction = "12345678";
                merchandise.UUIDExteriorTrade = "ABCDed02-a12A-B34B-c56C-c5abcdef61F2";
                merchandise.isInternational = false;
                merchandise.isImport = false;
                merchandise.isHazardousMaterial = false;
                merchandise.hazardousMaterialCode = "M0035";
                merchandise.packagingCode = "4A";

                lstMerchandises.Add(merchandise);
            }

            merchandises.merchandise = lstMerchandises;
            itemDescription.merchandises = merchandises;

            label.itemDescription = itemDescription;

            ServiceConfiguration serviceConfiguration = new ServiceConfiguration();
            serviceConfiguration.quantityOfLabels = 1;
            serviceConfiguration.serviceTypeId = "70";
            serviceConfiguration.salesOrganization = System.Configuration.ConfigurationManager.AppSettings["salesOrganization"];
            serviceConfiguration.effectiveDate = ds.Tables[1].Rows[0]["effectiveDate"].ToString();
            serviceConfiguration.originZipCodeForRouting = ds.Tables[1].Rows[0]["PostalCode"].ToString();
            // validar si tiene seguro---------------
            if (double.Parse(dsCarrier.Tables[0].Rows[0][3].ToString()) > 0)
            {
                serviceConfiguration.isInsurance = true;

                Insurance insurance = new Insurance();
                insurance.contentDescription = "Mercancias Generales";
                insurance.declaredValue = double.Parse(dsCarrier.Tables[0].Rows[0][3].ToString());

                serviceConfiguration.insurance = insurance;

            }
            else
                serviceConfiguration.isInsurance = false;


            serviceConfiguration.isReturnDocument = false;
            ReturnDocument returnDocument = new ReturnDocument();
            returnDocument.serviceId = "60";
            returnDocument.type = "DRFZ";

            serviceConfiguration.returnDocument = returnDocument;

            label.serviceConfiguration = serviceConfiguration;

            Location location = new Location();
            location.isDRAAlternative = false;

            Origin origin = new Origin();
            Contact contact = new Contact();
            contact.corporateName = ds.Tables[0].Rows[0]["supplierName"].ToString();
            contact.contactName = ds.Tables[0].Rows[0]["commInfoName"].ToString();
            contact.cellPhone = ds.Tables[0].Rows[0]["commInfoPhone"].ToString();
            contact.telephone = ds.Tables[0].Rows[0]["commInfoPhone"].ToString();
            contact.phoneExt = ds.Tables[0].Rows[0]["commInfoExt"].ToString();
            contact.email = ds.Tables[0].Rows[0]["commInfoEmail"].ToString();

            origin.contact = contact;

            Address address = new Address();
            address.bUsedCode = false;
            address.roadTypeCode = "001";
            address.roadTypeAbbName = "RUTA1";
            address.roadName = ds.Tables[0].Rows[0]["addressStreet"].ToString();
            address.townshipCode = "08-019";
            address.townshipName = ds.Tables[0].Rows[0]["addressCity"].ToString();
            address.settlementTypeCode = "001";
            address.settlementTypeAbbName = ds.Tables[0].Rows[0]["addressCol"].ToString().Length > 5 ? ds.Tables[0].Rows[0]["addressCol"].ToString().Substring(0, 4) : ds.Tables[0].Rows[0]["addressCol"].ToString();
            address.settlementName = ds.Tables[0].Rows[0]["addressCol"].ToString();
            address.stateCode = "02";
            address.stateAbbName = ds.Tables[0].Rows[0]["Abrev_Estado"].ToString();
            address.zipCode = ds.Tables[0].Rows[0]["addressPostalCode"].ToString();
            address.countryCode = "484";
            address.countryName = "MEX";
            address.addressReference = ds.Tables[0].Rows[0]["addressReference1"].ToString();
            address.betweenRoadName1 = ds.Tables[0].Rows[0]["addressReference2"].ToString();
            address.betweenRoadName2 = " ";
            address.latitude = ds.Tables[0].Rows[0]["Latitude"].ToString();
            address.longitude = ds.Tables[0].Rows[0]["Longitude"].ToString();
            address.externalNum = ds.Tables[0].Rows[0]["addressNumberExt"].ToString();
            address.indoorInformation = ds.Tables[0].Rows[0]["addressNumberInt"].ToString();
            address.nave = " ";
            address.platform = " ";
            address.localityCode = "02";
            address.localityName = ds.Tables[0].Rows[0]["addressCity"].ToString();

            origin.address = address;

            location.origin = origin;


            Destination destination = new Destination();
            destination.isDeliveryToPUDO = false;
            destination.deliveryPUDOCode = "";

            HomeAddress homeAddress = new HomeAddress();
            Contact contactD = new Contact();
            contactD.corporateName = UeNo;
            contactD.contactName = ds.Tables[1].Rows[0]["CustomerName"].ToString();
            contactD.cellPhone = ds.Tables[1].Rows[0]["Phone"].ToString();
            contactD.telephone = ds.Tables[1].Rows[0]["Phone"].ToString();
            contactD.phoneExt = "";//ds.Tables[1].Rows[0]["commInfoExt"].ToString();
            contactD.email = ds.Tables[1].Rows[0]["Email"].ToString();

            homeAddress.contact = contactD;

            Address addressD = new Address();
            addressD.bUsedCode = false;
            addressD.roadTypeCode = "001";
            addressD.roadTypeAbbName = "RUTA1";
            addressD.roadName = ds.Tables[1].Rows[0]["Address1"].ToString();
            addressD.townshipCode = "08-019";
            addressD.townshipName = ds.Tables[1].Rows[0]["City"].ToString();
            addressD.settlementTypeCode = "001";
            addressD.settlementTypeAbbName = ds.Tables[1].Rows[0]["Address2"].ToString().Length > 5 ? ds.Tables[1].Rows[0]["Address2"].ToString().Substring(0, 4) : ds.Tables[1].Rows[0]["Address2"].ToString();
            addressD.settlementName = ds.Tables[1].Rows[0]["Address2"].ToString();
            addressD.stateCode = "02";
            addressD.stateAbbName = ds.Tables[1].Rows[0]["Abrev_Estado"].ToString();
            addressD.zipCode = ds.Tables[1].Rows[0]["PostalCode"].ToString();
            addressD.countryCode = "484";
            addressD.countryName = "MEX";
            addressD.addressReference = ds.Tables[1].Rows[0]["Reference"].ToString();
            addressD.betweenRoadName1 = " ";
            addressD.betweenRoadName2 = " ";
            addressD.latitude = ds.Tables[1].Rows[0]["Latitude"].ToString();
            addressD.longitude = ds.Tables[1].Rows[0]["Longitude"].ToString();
            addressD.externalNum = " ";
            addressD.indoorInformation = " ";
            addressD.nave = " ";
            addressD.platform = " ";
            addressD.localityCode = "02";
            addressD.localityName = ds.Tables[1].Rows[0]["City"].ToString();

            homeAddress.address = addressD;

            destination.homeAddress = homeAddress;
            location.destination = destination;

            Notified notified = new Notified();
            notified.notifiedTaxIdCode = "notifiedTaxCode";
            notified.notifiedTaxCountry = "MEX";
            Residence residence = new Residence();
            Contact contactN = new Contact();
            contactN.corporateName = ds.Tables[1].Rows[0]["CustomerName"].ToString();
            contactN.contactName = ds.Tables[1].Rows[0]["CustomerName"].ToString();
            contactN.cellPhone = ds.Tables[1].Rows[0]["Phone"].ToString();
            contactN.telephone = ds.Tables[1].Rows[0]["Phone"].ToString();
            contactN.phoneExt = "";//ds.Tables[1].Rows[0]["commInfoExt"].ToString();
            contactN.email = ds.Tables[1].Rows[0]["Email"].ToString();

            residence.contact = contactN;

            Address addressN = new Address();
            addressN.bUsedCode = false;
            addressN.roadTypeCode = "001";
            addressN.roadTypeAbbName = "RUTA1";
            addressN.roadName = ds.Tables[1].Rows[0]["Address1"].ToString();
            addressN.townshipCode = "08-019";
            addressN.townshipName = ds.Tables[1].Rows[0]["City"].ToString();
            addressN.settlementTypeCode = "001";
            addressN.settlementTypeAbbName = ds.Tables[1].Rows[0]["Address2"].ToString().Length > 5 ? ds.Tables[1].Rows[0]["Address2"].ToString().Substring(0, 4) : ds.Tables[1].Rows[0]["Address2"].ToString();
            addressN.settlementName = ds.Tables[1].Rows[0]["Address2"].ToString();
            addressN.stateCode = "02";
            addressN.stateAbbName = ds.Tables[1].Rows[0]["Abrev_Estado"].ToString();
            addressN.zipCode = ds.Tables[1].Rows[0]["PostalCode"].ToString();
            addressN.countryCode = "484";
            addressN.countryName = "MEX";
            addressN.addressReference = ds.Tables[1].Rows[0]["Reference"].ToString();
            addressN.betweenRoadName1 = " ";
            addressN.betweenRoadName2 = " ";
            addressN.latitude = ds.Tables[1].Rows[0]["Latitude"].ToString();
            addressN.longitude = ds.Tables[1].Rows[0]["Longitude"].ToString();
            addressN.externalNum = " ";
            addressN.indoorInformation = " ";
            addressN.nave = " ";
            addressN.platform = " ";
            addressN.localityCode = "02";
            addressN.localityName = ds.Tables[1].Rows[0]["City"].ToString();

            residence.address = addressN;

            notified.residence = residence;

            location.notified = notified;
            label.location = location;

            model.labelDefinition = label;

            json = JsonConvert.SerializeObject(model);

            return json;
        }
        private string RequestEstafetaLTL(DataSet ds, decimal weight, List<ProductEmbalaje> Products, DataSet dsCarrier, ShipmentToTrackingModel packageCEDIS, string UeNo)
        {
            string json = string.Empty;

            RequestEstafetaLtlModel model = new RequestEstafetaLtlModel();
            LabelDefinitionLtl label = new LabelDefinitionLtl();

            WayBillDocumentLtl wayBill = new WayBillDocumentLtl();
            wayBill.aditionalInfo = "Informacion Adicional";
            wayBill.content = "Contenido";
            wayBill.costCenter = "SPMXA12345";
            wayBill.customerShipmentId = null;
            var reference = ds.Tables[0].Rows[0]["addressReference1"].ToString() + " " + ds.Tables[0].Rows[0]["addressReference2"].ToString();
            wayBill.referenceNumber = reference.Length > 30 ? reference.Substring(0, 29) : reference;
            wayBill.groupShipmentId = null;

            ItemDescription itemDescription = new ItemDescription();
            itemDescription.parcelId = 5;
            itemDescription.weight = weight;
            itemDescription.height = int.Parse(Session["SumHeigth"].ToString());
            itemDescription.length = int.Parse(Session["SumLength"].ToString());
            itemDescription.width = int.Parse(Session["SumWidth"].ToString());

            Pallet pallet = new Pallet();
            pallet.merchandise = "NATIONAL";
            pallet.genericContent = "Mercancias Generales (" + UeNo +")";
            if (itemDescription.length <= 120 && itemDescription.width <= 105)
                pallet.type = "SIMPLE";
            else
                pallet.type = "DOUBLE";
            wayBill.pallet = pallet;
            label.wayBillDocument = wayBill;

            Merchandises merchandises = new Merchandises();
            merchandises.totalGrossWeight = weight;
            merchandises.weightUnitCode = "KGM";

            List<Merchandise> lstMerchandises = new List<Merchandise>();
            List<WeightByProducts> lstWeightByProducts = (List<WeightByProducts>)Session["ListWeightByProducts"];
            if (Products != null)
            {
                foreach (var product in Products)
                {
                    var total = 0.0;
                    var quantity = 0.0;

                    Merchandise merchandise = new Merchandise();

                    foreach (DataRow r in ds.Tables[2].Rows)
                    {
                        if (r["PosBarCode"].ToString().Equals(product.Barcode.ToString()))
                        {
                            total = double.Parse(r["Total"].ToString());
                            quantity = double.Parse(r["PosBarCode"].ToString());

                        }
                    }
                    merchandise.merchandiseValue = total;
                    merchandise.currency = "MXN";
                    merchandise.productServiceCode = lstWeightByProducts.Where(x => x.Product == long.Parse(product.ProductId.ToString())).FirstOrDefault().SAT_IdProdServ;
                    merchandise.merchandiseQuantity = product.Pieces;
                    merchandise.measurementUnitCode = lstWeightByProducts.Where(x => x.Product == long.Parse(product.ProductId.ToString())).FirstOrDefault().SAT_UniMedProd;
                    merchandise.tariffFraction = "12345678";
                    merchandise.UUIDExteriorTrade = "ABCDed02-a12A-B34B-c56C-c5abcdef61F2";
                    merchandise.isInternational = false;
                    merchandise.isImport = false;
                    merchandise.isHazardousMaterial = false;
                    merchandise.hazardousMaterialCode = "M0035";
                    merchandise.packagingCode = "4A";

                    lstMerchandises.Add(merchandise);
                }
            }
            else
            {
                var total = 0.0;
                var quantity = 0.0;

                Merchandise merchandise = new Merchandise();

                foreach (DataRow r in ds.Tables[2].Rows)
                {
                    if (r["PosBarCode"].ToString().Equals(packageCEDIS.barcode.ToString()))
                    {
                        total = double.Parse(r["Total"].ToString());
                        quantity = double.Parse(r["PosBarCode"].ToString());

                    }
                }
                merchandise.merchandiseValue = total;
                merchandise.currency = "MXN";
                merchandise.productServiceCode = lstWeightByProducts.Where(x => x.Product == long.Parse(packageCEDIS.productId.ToString())).FirstOrDefault().SAT_IdProdServ;
                merchandise.merchandiseQuantity = packageCEDIS.piezas;
                merchandise.measurementUnitCode = lstWeightByProducts.Where(x => x.Product == long.Parse(packageCEDIS.productId.ToString())).FirstOrDefault().SAT_UniMedProd;
                merchandise.tariffFraction = "12345678";
                merchandise.UUIDExteriorTrade = "ABCDed02-a12A-B34B-c56C-c5abcdef61F2";
                merchandise.isInternational = false;
                merchandise.isImport = false;
                merchandise.isHazardousMaterial = false;
                merchandise.hazardousMaterialCode = "M0035";
                merchandise.packagingCode = "4A";

                lstMerchandises.Add(merchandise);
            }
            merchandises.merchandise = lstMerchandises;
            itemDescription.merchandises = merchandises;

            label.itemDescription = itemDescription;

            ServiceConfiguration serviceConfiguration = new ServiceConfiguration();
            serviceConfiguration.quantityOfLabels = 1;
            serviceConfiguration.serviceTypeId = "L0";
            serviceConfiguration.salesOrganization = System.Configuration.ConfigurationManager.AppSettings["salesOrganization"];
            serviceConfiguration.effectiveDate = ds.Tables[1].Rows[0]["effectiveDate"].ToString();
            serviceConfiguration.originZipCodeForRouting = ds.Tables[1].Rows[0]["PostalCode"].ToString();
            // validar si tiene seguro---------------
            if (double.Parse(dsCarrier.Tables[0].Rows[0][3].ToString()) > 0)
            {
                serviceConfiguration.isInsurance = true;

                Insurance insurance = new Insurance();
                insurance.contentDescription = "Mercancias Generales";
                insurance.declaredValue = double.Parse(dsCarrier.Tables[0].Rows[0][3].ToString());

                serviceConfiguration.insurance = insurance;

            }
            else
                serviceConfiguration.isInsurance = false;

            serviceConfiguration.isReturnDocument = false;
            ReturnDocument returnDocument = new ReturnDocument();
            returnDocument.serviceId = "60";
            returnDocument.type = "DRFZ";

            serviceConfiguration.returnDocument = returnDocument;

            label.serviceConfiguration = serviceConfiguration;

            Location location = new Location();
            location.isDRAAlternative = false;

            Origin origin = new Origin();
            Contact contact = new Contact();
            contact.corporateName = ds.Tables[0].Rows[0]["supplierName"].ToString();
            contact.contactName = ds.Tables[0].Rows[0]["commInfoName"].ToString();
            contact.cellPhone = ds.Tables[0].Rows[0]["commInfoPhone"].ToString();
            contact.telephone = ds.Tables[0].Rows[0]["commInfoPhone"].ToString();
            contact.phoneExt = ds.Tables[0].Rows[0]["commInfoExt"].ToString();
            contact.email = ds.Tables[0].Rows[0]["commInfoEmail"].ToString();

            origin.contact = contact;

            Address address = new Address();
            address.bUsedCode = false;
            address.roadTypeCode = "001";
            address.roadTypeAbbName = "RUTA1";
            address.roadName = ds.Tables[0].Rows[0]["addressStreet"].ToString();
            address.townshipCode = "08-019";
            address.townshipName = ds.Tables[0].Rows[0]["addressCity"].ToString();
            address.settlementTypeCode = "001";
            address.settlementTypeAbbName = ds.Tables[0].Rows[0]["addressCol"].ToString().Length > 5 ? ds.Tables[0].Rows[0]["addressCol"].ToString().Substring(0, 4) : ds.Tables[0].Rows[0]["addressCol"].ToString();
            address.settlementName = ds.Tables[0].Rows[0]["addressCol"].ToString();
            address.stateCode = "02";
            address.stateAbbName = ds.Tables[0].Rows[0]["Abrev_Estado"].ToString();
            address.zipCode = ds.Tables[0].Rows[0]["addressPostalCode"].ToString();
            address.countryCode = "484";
            address.countryName = "MEX";
            address.addressReference = ds.Tables[0].Rows[0]["addressReference1"].ToString();
            address.betweenRoadName1 = ds.Tables[0].Rows[0]["addressReference2"].ToString();
            address.betweenRoadName2 = " ";
            address.latitude = ds.Tables[0].Rows[0]["Latitude"].ToString();
            address.longitude = ds.Tables[0].Rows[0]["Longitude"].ToString();
            address.externalNum = ds.Tables[0].Rows[0]["addressNumberExt"].ToString();
            address.indoorInformation = ds.Tables[0].Rows[0]["addressNumberInt"].ToString();
            address.nave = " ";
            address.platform = " ";
            address.localityCode = "02";
            address.localityName = ds.Tables[0].Rows[0]["addressCity"].ToString();

            origin.address = address;

            location.origin = origin;


            Destination destination = new Destination();
            destination.isDeliveryToPUDO = false;
            destination.deliveryPUDOCode = "";

            HomeAddress homeAddress = new HomeAddress();
            Contact contactD = new Contact();
            contactD.corporateName = ds.Tables[1].Rows[0]["CustomerName"].ToString();
            contactD.contactName = ds.Tables[1].Rows[0]["CustomerName"].ToString();
            contactD.cellPhone = ds.Tables[1].Rows[0]["Phone"].ToString();
            contactD.telephone = ds.Tables[1].Rows[0]["Phone"].ToString();
            contactD.phoneExt = "";//ds.Tables[1].Rows[0]["commInfoExt"].ToString();
            contactD.email = ds.Tables[1].Rows[0]["Email"].ToString();

            homeAddress.contact = contactD;

            Address addressD = new Address();
            addressD.bUsedCode = false;
            addressD.roadTypeCode = "001";
            addressD.roadTypeAbbName = "RUTA1";
            addressD.roadName = ds.Tables[1].Rows[0]["Address1"].ToString();
            addressD.townshipCode = "08-019";
            addressD.townshipName = ds.Tables[1].Rows[0]["City"].ToString();
            addressD.settlementTypeCode = "001";
            addressD.settlementTypeAbbName = ds.Tables[1].Rows[0]["Address2"].ToString().Length > 5 ? ds.Tables[1].Rows[0]["Address2"].ToString().Substring(0, 4) : ds.Tables[1].Rows[0]["Address2"].ToString();
            addressD.settlementName = ds.Tables[1].Rows[0]["Address2"].ToString();
            addressD.stateCode = "02";
            addressD.stateAbbName = ds.Tables[1].Rows[0]["Abrev_Estado"].ToString();
            addressD.zipCode = ds.Tables[1].Rows[0]["PostalCode"].ToString();
            addressD.countryCode = "484";
            addressD.countryName = "MEX";
            addressD.addressReference = ds.Tables[1].Rows[0]["Reference"].ToString();
            addressD.betweenRoadName1 = " ";
            addressD.betweenRoadName2 = " ";
            addressD.latitude = ds.Tables[1].Rows[0]["Latitude"].ToString();
            addressD.longitude = ds.Tables[1].Rows[0]["Longitude"].ToString();
            addressD.externalNum = " ";
            addressD.indoorInformation = " ";
            addressD.nave = " ";
            addressD.platform = " ";
            addressD.localityCode = "02";
            addressD.localityName = ds.Tables[1].Rows[0]["City"].ToString();

            homeAddress.address = addressD;

            destination.homeAddress = homeAddress;
            location.destination = destination;

            Notified notified = new Notified();
            notified.notifiedTaxIdCode = "notifiedTaxCode";
            notified.notifiedTaxCountry = "MEX";
            Residence residence = new Residence();
            Contact contactN = new Contact();
            contactN.corporateName = ds.Tables[1].Rows[0]["CustomerName"].ToString();
            contactN.contactName = ds.Tables[1].Rows[0]["CustomerName"].ToString();
            contactN.cellPhone = ds.Tables[1].Rows[0]["Phone"].ToString();
            contactN.telephone = ds.Tables[1].Rows[0]["Phone"].ToString();
            contactN.phoneExt = "";//ds.Tables[1].Rows[0]["commInfoExt"].ToString();
            contactN.email = ds.Tables[1].Rows[0]["Email"].ToString();

            residence.contact = contactN;

            Address addressN = new Address();
            addressN.bUsedCode = false;
            addressN.roadTypeCode = "001";
            addressN.roadTypeAbbName = "RUTA1";
            addressN.roadName = ds.Tables[1].Rows[0]["Address1"].ToString();
            addressN.townshipCode = "08-019";
            addressN.townshipName = ds.Tables[1].Rows[0]["City"].ToString();
            addressN.settlementTypeCode = "001";
            addressN.settlementTypeAbbName = ds.Tables[1].Rows[0]["Address2"].ToString().Length > 5 ? ds.Tables[1].Rows[0]["Address2"].ToString().Substring(0, 4) : ds.Tables[1].Rows[0]["Address2"].ToString();
            addressN.settlementName = ds.Tables[1].Rows[0]["Address2"].ToString();
            addressN.stateCode = "02";
            addressN.stateAbbName = ds.Tables[1].Rows[0]["Abrev_Estado"].ToString();
            addressN.zipCode = ds.Tables[1].Rows[0]["PostalCode"].ToString();
            addressN.countryCode = "484";
            addressN.countryName = "MEX";
            addressN.addressReference = ds.Tables[1].Rows[0]["Reference"].ToString();
            addressN.betweenRoadName1 = " ";
            addressN.betweenRoadName2 = " ";
            addressN.latitude = ds.Tables[1].Rows[0]["Latitude"].ToString();
            addressN.longitude = ds.Tables[1].Rows[0]["Longitude"].ToString();
            addressN.externalNum = " ";
            addressN.indoorInformation = " ";
            addressN.nave = " ";
            addressN.platform = " ";
            addressN.localityCode = "02";
            addressN.localityName = ds.Tables[1].Rows[0]["City"].ToString();

            residence.address = addressN;

            notified.residence = residence;

            location.notified = notified;
            label.location = location;

            model.labelDefinition = label;

            json = JsonConvert.SerializeObject(model);

            return json;
        }
        public string CreateGuiaEstafetaAPI(string UeNo, int OrderNo, decimal weight, List<ProductEmbalaje> Products, DataSet dsCarrier, ShipmentToTrackingModel packageCEDIS)
        {
            string result = string.Empty;
            string json = string.Empty, url = string.Empty;
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
                parametros.Add("@OrderNo", OrderNo);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Cns_EstafetaInfo]", false, parametros);

                if (weight <= 70)
                {
                    json = RequestEstafeta(ds, weight, Products, dsCarrier, packageCEDIS, UeNo);
                    url = System.Configuration.ConfigurationSettings.AppSettings["Estafeta_API"];
                }
                else
                {
                    json = RequestEstafetaLTL(ds, weight, Products, dsCarrier, packageCEDIS, UeNo);
                    url = System.Configuration.ConfigurationSettings.AppSettings["EstafetaLTL_API"];
                }
                Soriana.FWK.FmkTools.RestResponse r2 = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, url, "", json);

                string msg = r2.message;

                ResponseEstafetaModel re = JsonConvert.DeserializeObject<ResponseEstafetaModel>(r2.message);

                string pdfcadena2 = Convert.ToBase64String(re.data, Base64FormattingOptions.None);

                //return re.Guia + "," + re.pdf;
                result = re.labelPetitionResult.result.description + "," + pdfcadena2;

            }

            catch
            {
                //throw new Exception("La generacion de la Guia por Estafeta Fallo. " + ex.Message);
                result = "ERROR";
            }

            return result;
        }
        public string CreateGuiaEstafeta(string UeNo, int OrderNo, int weight, int typeId)
        {
            string result = string.Empty;
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

                    //m.serviceTypeId = "60";
                    //m.serviceTypeId = System.Configuration.ConfigurationManager.AppSettings["val_serviceTypeId"];
                    m.serviceTypeId = r["ServiceType"].ToString();
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
                    //m.DestinationInfo.corporateName = r["CustomerName"].ToString();
                    m.DestinationInfo.corporateName = r["UeNo"].ToString();
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

            }

            catch
            {
                //throw new Exception("La generacion de la Guia por Estafeta Fallo. " + ex.Message);
                result = "ERROR";
            }
            return result;
        }
        public string CreateGuiaLogyt(string UeNo, int OrderNo, int weight, int typeId)
        {
            string result = string.Empty;
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

                LogytRequestModel m = new LogytRequestModel();
                foreach (DataRow r in dsO.Tables[0].Rows)
                {


                    m.Origin = new LogytAddressModel();

                    m.Origin.Address1 = r["address1"].ToString();
                    m.Origin.Address2 = r["address2"].ToString();
                    m.Origin.City = r["city"].ToString();
                    m.Origin.ContactName = r["contactName"].ToString();
                    m.Origin.CorporateName = r["corporateName"].ToString();
                    m.Origin.CustomerNumber = r["customerNumber"].ToString();
                    m.Origin.Neighborhood = r["neighborhood"].ToString();
                    m.Origin.PhoneNumber = r["phone"].ToString();
                    m.Origin.State = r["state"].ToString();
                    m.Origin.ZipCode = r["zipCode"].ToString();

                }

                foreach (DataRow r in ds.Tables[0].Rows)
                {

                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    //m.ServiceType = System.Configuration.ConfigurationManager.AppSettings["val_serviceTypeId"];
                    m.ServiceType = r["ServiceType"].ToString();
                    if (weight >= 70)
                    {
                        m.ServiceType = "L0";
                    }


                    m.Destination = new LogytAddressModel();

                    m.Destination.Address1 = r["Address1"].ToString();
                    m.Destination.Address2 = r["Address2"].ToString();
                    m.Destination.City = r["City"].ToString();
                    m.Destination.ContactName = r["CustomerName"].ToString();
                    //m.Destination.corporateName = r["CustomerName"].ToString();
                    m.Destination.CorporateName = r["UeNo"].ToString();
                    m.Destination.CustomerNumber = r["CustomerNo"].ToString();
                    m.Destination.Neighborhood = r["NameReceives"].ToString();
                    m.Destination.PhoneNumber = r["Phone"].ToString();
                    m.Destination.State = r["StateCode"].ToString();
                    m.Destination.ZipCode = r["PostalCode"].ToString();

                    m.Reference = r["Reference"].ToString();
                    //m.originZipCodeForRouting = r["PostalCode"].ToString();
                    m.Weight = weight; // lo capturado en el modal
                    m.Volume = weight;

                    string json2 = JsonConvert.SerializeObject(m);

                    Soriana.FWK.FmkTools.RestResponse r2 = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_Logyt_Guia"], "", json2);

                    string msg = r2.message;
                    string pdfcadena2 = string.Empty;

                    LogytResponseModels re = JsonConvert.DeserializeObject<LogytResponseModels>(r2.message);


                    if (!Convert.ToBoolean(re.Error))
                        pdfcadena2 = Convert.ToBase64String(re.Labels[0].PDF, Base64FormattingOptions.None);
                    else
                    {
                        string msgDetail = string.Empty;
                        foreach (var itemMsg in re.Messages)
                        {
                            msgDetail += itemMsg + ". ";
                        }
                        throw new Exception(msgDetail);
                    }
                    //return re.Guia + "," + re.pdf;
                    result = re.Labels[0].Folios[0] + "," + pdfcadena2;

                }

            }
            catch
            {
                //throw new Exception("La generacion de la Guia por Logyt Fallo. " + ex.Message);
                result = "ERROR";
            }
            return result;
        }
        private string RequestEnvia(string json, string service)
        {
            string _jsonFinal = string.Empty;

            CotizadorRequestModel request = JsonConvert.DeserializeObject<CotizadorRequestModel>(json);

            request.shipment.service = service;

            _jsonFinal = JsonConvert.SerializeObject(request);

            return _jsonFinal;
        }
        private string RequestLTLEnvia(string json, string service)
        {
            string _jsonFinal = string.Empty;

            CotizadorRequestLTLModel request = JsonConvert.DeserializeObject<CotizadorRequestLTLModel>(json);

            request.shipment.service = service;

            _jsonFinal = JsonConvert.SerializeObject(request);

            return _jsonFinal;
        }
        public string CreateGuiaEnvia(string request, string service, int weight)
        {
            string result = string.Empty, _json = string.Empty;
            try
            {
                if (weight > 70)
                    _json = RequestLTLEnvia(request, service);
                else
                    _json = RequestEnvia(request, service);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                Soriana.FWK.FmkTools.RestResponse r2 = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_Envia_Guia"], "", _json);

                string msg = r2.message;

                //string msg = "{ 'meta': 'generate', 'data': [{'carrier': 'fedex','service': 'express','trackingNumber': '794693403268','trackUrl': 'https://test.envia.com/rastreo?label=794693403268&cntry_code=mx', 'label': 'https://s3.us-east-2.amazonaws.com/envia-staging/uploads/fedex/79469340326840461b0130d92379.pdf', 'additionalFiles': [],'totalPrice': 434,'currentBalance': 1580, 'currency': 'MXN'} ]}";


                EnviaResponseModel re = JsonConvert.DeserializeObject<EnviaResponseModel>(msg);


                result = re.data[0].trackingNumber + "," + re.data[0].label + "," + re.data[0].trackUrl;
            }
            catch
            {
                //throw new Exception("La generacion de la Guia por Envia.Com Fallo. " + ex.Message);
                result = "ERROR";
            }

            return result;
        }
        public string RecoleccionEnvia(string request)
        {
            string result = string.Empty;
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                Soriana.FWK.FmkTools.RestResponse r2 = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_Envia_Recoleccion"], "", request);

                result = r2.message;

            }
            catch
            {
                //throw new Exception("La generacion de la Guia por Envia.Com Fallo. " + ex.Message);
                result = "ERROR";
            }

            return result;
        }
        //public string CreateGuiaLogyt(string UeNo, int OrderNo, int weight, int typeId)
        //{
        //    var ServiceTypeId = 1;
        //    DataSet ds = new DataSet();
        //    DataSet dsO = new DataSet();

        //    string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
        //    if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
        //    {
        //        conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
        //    }


        //    try
        //    {
        //        Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

        //        System.Collections.Hashtable parametros = new System.Collections.Hashtable();
        //        parametros.Add("@UeNo", UeNo);
        //        parametros.Add("@OrderNo", OrderNo);

        //        ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Sel_EstafetaInfo]", false, parametros);



        //        System.Collections.Hashtable parametros2 = new System.Collections.Hashtable();
        //        parametros2.Add("@UeNo", UeNo);


        //        dsO = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Cns_UeNoOriginInfo]", false, parametros2);

        //    }
        //    catch (SqlException ex)
        //    {
        //        return "ERRSQL";
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return "ERR";
        //    }

        //    LogytRequestModel m = new LogytRequestModel();
        //    foreach (DataRow r in dsO.Tables[0].Rows)
        //    {


        //        m.Origin = new LogytAddressModel();

        //        m.Origin.Address1 = r["address1"].ToString();
        //        m.Origin.Address2 = r["address2"].ToString();
        //        m.Origin.City = r["city"].ToString();
        //        m.Origin.ContactName = r["contactName"].ToString();
        //        m.Origin.CorporateName = r["corporateName"].ToString();
        //        m.Origin.CustomerNumber = r["customerNumber"].ToString();
        //        m.Origin.Neighborhood = r["neighborhood"].ToString();
        //        m.Origin.PhoneNumber = r["phone"].ToString();
        //        m.Origin.State = r["state"].ToString();
        //        m.Origin.ZipCode = r["zipCode"].ToString();

        //    }

        //    foreach (DataRow r in ds.Tables[0].Rows)
        //    {

        //        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //        //m.ServiceType = System.Configuration.ConfigurationManager.AppSettings["val_serviceTypeId"];
        //        m.ServiceType = r["ServiceType"].ToString();
        //        if (weight >= 70)
        //        {
        //            m.ServiceType = "L0";
        //        }


        //        m.Destination = new LogytAddressModel();

        //        m.Destination.Address1 = r["Address1"].ToString();
        //        m.Destination.Address2 = r["Address2"].ToString();
        //        m.Destination.City = r["City"].ToString();
        //        m.Destination.ContactName = r["CustomerName"].ToString();
        //        //m.Destination.corporateName = r["CustomerName"].ToString();
        //        m.Destination.CorporateName = r["UeNo"].ToString();
        //        m.Destination.CustomerNumber = r["CustomerNo"].ToString();
        //        m.Destination.Neighborhood = r["NameReceives"].ToString();
        //        m.Destination.PhoneNumber = r["Phone"].ToString();
        //        m.Destination.State = r["StateCode"].ToString();
        //        m.Destination.ZipCode = r["PostalCode"].ToString();

        //        m.Reference = r["Reference"].ToString();
        //        //m.originZipCodeForRouting = r["PostalCode"].ToString();
        //        m.Weight = weight; // lo capturado en el modal
        //        m.Volume = weight;

        //        string json2 = JsonConvert.SerializeObject(m);

        //        Soriana.FWK.FmkTools.RestResponse r2 = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_Logyt_Guia"], "", json2);

        //        string msg = r2.message;

        //        LogytResponseModels re = JsonConvert.DeserializeObject<LogytResponseModels>(r2.message);

        //        string pdfcadena2 = Convert.ToBase64String(re.Labels[0].PDF, Base64FormattingOptions.None);

        //        //return re.Guia + "," + re.pdf;
        //        return re.Labels[0].Folios[0] + "," + pdfcadena2;

        //    }

        //    return string.Empty;

        //}
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
        private string RequestLTLCotizador(DataSet ds)
        {
            string _json = string.Empty;

            CotizadorRequestLTLModel requestCotizador = new CotizadorRequestLTLModel();

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
            packages.content = "Mercancias Generales";
            packages.declaredValue = 0;
            packages.insurance = 0;
            var pesoRound = decimal.Round(Convert.ToDecimal(Session["SumPeso"].ToString()));
            var peso = decimal.ToInt32(pesoRound);
            packages.weight = peso;
            packages.weightUnit = "KG";
            packages.lenghtUnit = "CM";
            packages.type = "pallet";
            DimensionsModel dimensions = new DimensionsModel();
            dimensions.height = int.Parse(Session["SumHeigth"].ToString());
            dimensions.length = int.Parse(Session["SumLength"].ToString());
            dimensions.width = int.Parse(Session["SumWidth"].ToString());
            packages.dimensions = dimensions;

            List<PackageModel> lstPackages = new List<PackageModel>();
            lstPackages.Add(packages);

            requestCotizador.packages = lstPackages;

            foreach (DataRow r in ds.Tables[2].Rows)
            {
                ShipmentLTLModel shipment = new ShipmentLTLModel();

                shipment.carrier = r["carrier"].ToString();
                shipment.type = 2;

                Pickup pickup = new Pickup();
                pickup.timeFrom = "09:00";
                pickup.timeTo = "14:00";
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);
                pickup.date = tomorrow.ToString("yyyy") + "-" + tomorrow.ToString("MM") + "-" + tomorrow.ToString("dd");

                shipment.pickup = pickup;

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
            List<string> additionals = new List<string>(3);
            additionals.Add("pickup_appointment");
            additionals.Add("delivery_schedule");
            additionals.Add("dry");

            requestCotizador.additionalServices = additionals;

            _json = JsonConvert.SerializeObject(requestCotizador);


            return _json;
        }
        private string[] RequestCotizador(DataSet ds)
        {
            string _json = string.Empty, _jsonRecoleccion = string.Empty;
            string[] result = new string[2];

            CotizadorRequestModel requestCotizador = new CotizadorRequestModel();
            RecoleccionRequestModel requestRecoleccion = new RecoleccionRequestModel();

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

                requestRecoleccion.origin = origin;

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

            foreach (DataRow r in ds.Tables[2].Rows)
            {
                ShipmentLTLModel shipment = new ShipmentLTLModel();

                shipment.carrier = r["carrier"].ToString();
                shipment.type = 1;

                Pickup pickup = new Pickup();
                pickup.timeFrom = "9";
                pickup.timeTo = "14";
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);
                pickup.date = tomorrow.ToString("yyyy") + "-" + tomorrow.ToString("MM") + "-" + tomorrow.ToString("dd");
                pickup.instructions = "Recolectar en direccion indicada";
                pickup.totalPackages = 1;
                pickup.totalWeight = decimal.Round(Convert.ToDecimal(Session["SumPeso"].ToString()));

                shipment.pickup = pickup;

                requestRecoleccion.shipment = shipment;
            }

            PackageModel packages = new PackageModel();
            packages.amount = 1;
            packages.content = "Mercancias Generales";
            packages.declaredValue = 0;
            packages.insurance = 0;
            var pesoRound = decimal.Round(Convert.ToDecimal(Session["SumPeso"].ToString()));
            var peso = decimal.ToInt32(pesoRound);
            packages.weight = peso;
            packages.weightUnit = "KG";
            packages.lenghtUnit = "CM";
            packages.type = "box";
            DimensionsModel dimensions = new DimensionsModel();
            dimensions.height = int.Parse(Session["SumHeigth"].ToString());
            dimensions.length = int.Parse(Session["SumLength"].ToString());
            dimensions.width = int.Parse(Session["SumWidth"].ToString());
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
            SettingsRecoleccionModel settingsRecoleccionModel = new SettingsRecoleccionModel();
            settingsRecoleccionModel.currency = "MXN";
            settingsRecoleccionModel.labelFormat = "pdf";

            requestRecoleccion.settings = settingsRecoleccionModel;

            _json = JsonConvert.SerializeObject(requestCotizador);
            result[0] = _json;
            _jsonRecoleccion = JsonConvert.SerializeObject(requestRecoleccion);
            result[1] = _jsonRecoleccion;

            return result;
        }
        public CarrierRequest CreateGuiaCotizador(string UeNo, int OrderNo, int type, string carrier, int weight)
        {
            string[] _json = new string[2];
            DataSet ds = new DataSet();
            CarrierRequest carrierRequest = new CarrierRequest();

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
                return carrierRequest;
            }
            catch (System.Exception ex)
            {
                return carrierRequest;
            }

            //if (weight > 70)
            //    _json = RequestLTLCotizador(ds);
            //else
            _json = RequestCotizador(ds);

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            Soriana.FWK.FmkTools.RestResponse r2 = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_Cotizador_Guia"], "", _json[0]);

            string msg = r2.message;

            if (msg.Contains("costSummary"))
            {
                CotizadorResponseModel re = JsonConvert.DeserializeObject<CotizadorResponseModel>(r2.message);
                carrierRequest.Carrier = carrier;
                carrierRequest.requests = _json;
                carrierRequest.msj = msg;
            }

            return carrierRequest;
        }


        //el detalle producto a producto
        public ActionResult DetalleProdaProd(string UeNo, int OrderNo, string IdTracking, string TrackingType,
            int ProductId, long Barcode, string ProductName,
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
            string urlPdf = string.Empty;
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
                                urlPdf = ds.Tables[0].Rows[0][1].ToString();
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

                var result = new { Success = true, pdf = pdfBase, url = urlPdf };
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