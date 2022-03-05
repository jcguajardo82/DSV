using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using ServicesManagement.Web.Models.ProcesadorPagosSoriana;
using ServicesManagement.Web.Models.ReportesPDP;
using Soriana.ProcesadorPagosWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    #region Modelos
    public class PagosModels
    {

        public string metodo { get; set; }
        public string request { get; set; }
        public string response { get; set; }
        public string fecha { get; set; }

    }

    public class productModel
    {


        public string descripcion { get; set; }
        public string promociones { get; set; }
        public string precio { get; set; }
        public string cantidad { get; set; }
        public string ajuste { get; set; }
        public string monto { get; set; }
        public string total { get; set; }


    }

    public class ProcessReponse
    {

        public string id { get; set; }
        public string razon { get; set; }
        public string code { get; set; }
        public string decision { get; set; }
        public string estatus { get; set; }

    }

    public class OrderPagoModels
    {
        public string orden { get; set; }
        public string request { get; set; }
        public string response { get; set; }
        public string fecha { get; set; }

        public string direccion { get; set; }
        public string telefono { get; set; }
        public string cliente { get; set; }
        public string email { get; set; }
        public string clienteId { get; set; }


        public List<productModel> productos { get; set; }
        public string total { get; set; } = "$0.0";
        public string dineroElectronico { get; set; } = "$0.0";
        public string dineroEfectivo { get; set; } = "$0.0";
        public string puntos { get; set; } = "$0.0";
        public string tarjeta { get; set; } = "$0.0";
        public string estatus { get; set; } = "$0.0";
        public List<ShipmentsModel> shipments { get; set; }

        public string jsonRequest { get; set; } = string.Empty;
        public RequestLealtad jsonLealtadRequest { get; set; } = null;
        public string jsonOmonelRequest { get; set; } = string.Empty;

        public bool LealtadSpecified { get; set; } = false;
        public ProcessReponse LealtadjsonResponse { get; set; } = null;
        public bool OmonelSpecified { get; set; } = false;
        public ProcessReponse OmoneljsonResponse { get; set; } = null;
        public bool SFSpecified { get; set; } = false;
        public ProcessReponse SFjsonResponse { get; set; } = null;
        public bool CaptureSpecified { get; set; } = false;
        public ProcessReponse CapturejsonResponse { get; set; } = null;
        public bool CreateDecisionSpecified { get; set; } = false;
        public ProcessReponse CreatejsonResponse { get; set; } = null;
        public bool EnrollmentSpecified { get; set; } = false;
        public ProcessReponse EnrollmentjsonResponse { get; set; } = null;
        public bool AutenticatedSpecified { get; set; } = false;
        public ProcessReponse AutenticatedjsonResponse { get; set; } = null;
        public bool ValidateSpecified { get; set; } = false;
        public ProcessReponse ValidatejsonResponse { get; set; } = null;
        public bool NotifySpecified { get; set; } = false;
        public ProcessReponse NotifyjsonResponse { get; set; } = null;
        public bool ProcessOrderSpecified { get; set; } = false;
        public ProcessReponse ProcessOrderjsonResponse { get; set; } = null;

        public bool ErrorSpecified { get; set; } = false;

    }

    public class RequestLealtad
    {
        public string Id_Cve_Orden { get; set; }//" : "",

        public string Id_Cve_GUID { get; set; }//" : "",

        public string Id_Cve_TokenCta { get; set; }//" : "",

        public string Cve_Operacion { get; set; }//" : "",

        public decimal Imp_Vta { get; set; }//" : 10.00,

        public int Cant_Puntos { get; set; }//" : 5,

        public decimal Imp_Comp { get; set; }//" : 10.00,

        public decimal Imp_DE { get; set; }//" : 10.00,

        public decimal Imp_Efvo { get; set; }//" :10.00,

        public decimal Imp_Cred { get; set; }//" : 10.00

        public string Cve_Accion { get; set; } = "DISMINUYE";
    }

    public class OmonelRequestModel
    {
        public string Id_Cve_Orden { get; set; }//" : "580210",

        public string Id_Cve_GUID { get; set; }//" :"37DEEF97-A79B-463B-A73D-6540009F9CA6",

        public string Id_Num_Cta { get; set; }//" : "2000100600457046",

        public string Cve_Operacion { get; set; }//" : "CANCELACION",

        public string Imp_Comp { get; set; }//" : "100.00",

        public string Cve_Accion { get; set; } = "DISMINUYE";//" : "AUMENTA"
    }

    public class RefoundRev
    {
        public string ResponseJson { get; set; }
        public string idPayment { get; set; }
        public string fec_movto { get; set; }
    }

    public class RefoundJson
    {
        public RefoundDetails refundAmountDetails { get; set; }
    }

    public class RefoundDetails
    {
        public string refundAmount { get; set; }
        public string currency { get; set; }
    }

    public class ReverseModel
    {
        public string FechaReverso { get; set; }
        public string HoraReverso { get; set; }
        public string MontoRverso { get; set; }
        public string IDReverso { get; set; }
    }

    public class Omonel_Auth
    {
        public string Cve_Autz { get; set; } = "";
        public string ShippingReferenceNumber { get; set; } = "";
    }

    #region Aprobacion Bancaria
    public class ResponseEmisor
    {
        public string DecisionEmisor { get; set; }
        public string CveReespuestaEmisor { get; set; }
        public string DescReespuestaEmisor { get; set; }
    }

    public class Emisor
    {
        public string id { get; set; }
        public string status { get; set; }
    }

    public class ApprovalCodeModel
    {
        public bool IsValid { get; set; } = false;
        public string Status { get; set; }
        public string ReasonCodes { get; set; }
        public responseObject ResponseObject { get; set; }
    }

    public class ShipmentDataEstatus
    {
        public string OrderId { get; set; } = "";
        public string shipmentAlias { get; set; } = "";
        public string status { get; set; } = "";
        public string CarrierName { get; set; } = "";
        public string DeliveryType { get; set; } = "";
    }

    public class responseObject
    {
        public ProcessorInformation processorInformation { get; set; }
    }

    public class ProcessorInformation
    {
        public string approvalCode { get; set; } = "";
    }

    public class ApprovalCodeJsonResponse
    {
        public string ResponseJson { get; set; }
    }
    #endregion

    #region APP
    public class OrdersAppModel
    {
        public string OrderNo { get; set; } = "";
        public string StatusUe { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public string Id_Num_Apl { get; set; } = "";
        public string MethodPayment { get; set; } = "";
        public string DeliveryType { get; set; } = "";
    }
    #endregion

    #endregion

    [Authorize]
    public class ReportesPDPController : Controller
    {
        // GET: ReportesPDP
        #region Reportes Procesador de Pagos
        #region FormaPago
        public ActionResult GetFormaPago()
        {
            try
            {
                var list = new List<ReportesPDP>(); //DataTableToModel.ConvertTo<SuppliersWHForAproval>(DALAlmacenes.SuppliersWHForAproval_sUP().Tables[0]);
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

        #region Autorizaciones Bancarias
        public ActionResult AutBancarias()
        {
            //Session["listaGrid"] = Newtonsoft.Json.JsonConvert.SerializeObject(GetAutorizaciones("", "", "AuthBancaria"));

            return View();
        }

        public ActionResult AutorizacionesBancariasbyOrder(string OrderReferenceNumber)
        {
            var Autorizacion = GetAutorizaciones(OrderReferenceNumber);

            var result = new { Success = true, resp = Autorizacion };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Canal de Compra
        public ActionResult Canaldecompra()
        {
            // Session["listaGrid"] = Newtonsoft.Json.JsonConvert.SerializeObject(GetCanalCompra("", ""));

            return View();
        }

        public ActionResult CanalComprabyOrder(string OrderReferenceNumber)
        {
            var CanalCompra = GetCanalCompra(OrderReferenceNumber);

            var result = new { Success = true, resp = CanalCompra };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Creditos
        public ActionResult Creditos()
        {
            //Session["listaGrid"] = Newtonsoft.Json.JsonConvert.SerializeObject(GetCreditos("", ""));

            return View();
        }

        public ActionResult CreditosbyOrder(string OrderReferenceNumber)
        {
            var Creditos = GetCreditos(OrderReferenceNumber);

            var result = new { Success = true, resp = Creditos };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Participacion Formas Pago
        public ActionResult FormaPago()
        {
            Session["listaGrid"] = Newtonsoft.Json.JsonConvert.SerializeObject(GetParticipacionFormasPago("", ""));

            return View();
        }

        public ActionResult GetParamDatesFormaPago(string FecIni, string FecFin)
        {
            var FormasPago = GetParticipacionFormasPago(FecIni, FecFin);

            var result = new { Success = true, resp = FormasPago };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Aprobaciones Marcas
        public ActionResult CanalPago()
        {
            Session["listaGrid"] = Newtonsoft.Json.JsonConvert.SerializeObject(GetAprobacionesMarcas());

            return View();
        }

        public ActionResult AutRM()
        {
            Session["listaGrid"] = Newtonsoft.Json.JsonConvert.SerializeObject(GetAprobacionesMarcas());

            return View();
        }
        #endregion

        #region Liquidaciones
        public ActionResult Liquidaciones()
        {
            //    //var a = ProcesaArchivos();

            //    //Session["listaGrid"] = Newtonsoft.Json.JsonConvert.SerializeObject(GetLiquidaciones("", "", "Liquidaciones"));

            return View();
        }

        public ActionResult LiquidacionesbyOrder(string OrderReferenceNumber)
        {
            var Liquidaciones = GetLiquidaciones(OrderReferenceNumber);

            var result = new { Success = true, resp = Liquidaciones };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Reversiones Saldo
        public ActionResult ReservacionesSaldo()
        {
            //Session["listaGrid"] = Newtonsoft.Json.JsonConvert.SerializeObject(GetReverso("", "", "Reversiones_Saldo"));

            return View();
        }

        public ActionResult ReversobyOrder(string OrderReferenceNumber)
        {
            var Liquidaciones = GetReverso(OrderReferenceNumber);

            var result = new { Success = true, resp = Liquidaciones };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Pago Tienda
        public ActionResult PagoTienda()
        {
            Session["listaGrid"] = Newtonsoft.Json.JsonConvert.SerializeObject(GetPaymentStore());

            return View();
        }
        #endregion
        #endregion

        #region Actions
        public ActionResult CanalGenerarOrden()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult TipoCatalogo()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult PagoLealtad()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult AutDM()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Validaciones()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult VolTransAfiliacion()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Reintentos()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult EstatusOrdenes()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult EstatusEntregas()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Detenciones()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Cancelaciones()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult IngresosAlmacen()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult EnviosEntregas()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult TotalTokens()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult BorradoTokens()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult CreacionTokens()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Bines()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Bancos()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult MoviemientosLealtad()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult ContraCargos()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Aclaraciones()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult MetodosEntrega()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Mensajerias()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult PagoTiendaEst()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult VentasCatalogo()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult EstatusPayment(string order)
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;
            Session["listaGrid"] = Newtonsoft.Json.JsonConvert.SerializeObject(GetOrdenes());

            return View();

        }

        public List<OrderPagoModels> GetOrdenes()
        {

            DataSet ds = new DataSet();

            var _ConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings:MercurioDB");
            //_ConnectionString = "Server=tcp:srvsqlmercurio.database.windows.net,1433;Initial Catalog=MercurioDesaDB;Persist Security Info=False;User ID=t_eliseogr;Password=El1530%.*314;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            // _ConnectionString = "Server=tcp:srvsqlmercurioqa.database.windows.net,1433;Initial Catalog=MercurioQaDB;Persist Security Info=False;User ID=t_eliseogr;Password=W3rcur10!QA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioPDPProdDB;Persist Security Info=False;User ID=ProcesadorPago;Password=W3rcur10PDP!#$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Application Name=PaymentOrderProcess;Min Pool Size=0;Max Pool Size=5;Pooling=true;";

            List<OrderPagoModels> lista = new List<OrderPagoModels>();

            try
            {

                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("up_Corp_sel_Ordenes", cnn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;


                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);

                        //DataTable dt = ds.Tables[0].Clone();

                        //dt.Rows.Add(ds.Tables[0].Select(""));

                        if (ds.Tables.Count > 0)
                        {

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow r in ds.Tables[0].Rows)
                                {

                                    OrderPagoModels p = new OrderPagoModels();

                                    p.request = r["RequestJson"].ToString();
                                    p.response = r["ResponseJson"].ToString();
                                    p.fecha = r["fec_movto"].ToString();

                                    JObject orde = JObject.Parse(r["RequestJson"].ToString());

                                    p.orden = orde["orderReferenceNumber"].ToString();
                                    p.total = "$" + orde["orderAmount"].ToString();
                                    p.dineroElectronico = "$" + (orde["customerLoyaltyRedeemElectronicMoney"] == null ? "" : orde["customerLoyaltyRedeemElectronicMoney"].ToString());
                                    p.dineroEfectivo = "$" + orde["customerLoyaltyRedeemMoney"].ToString();
                                    p.tarjeta = "$" + orde["orderAmount"].ToString();

                                    lista.Add(p);
                                }

                                return lista;
                            }
                        }

                    }
                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

            return lista;

        }

        public ActionResult GetEstatusPayment(string order)
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;

            DataSet ds = new DataSet();

            var _ConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings:MercurioDB");
            //_ConnectionString = "Server=tcp:srvsqlmercurio.database.windows.net,1433;Initial Catalog=MercurioDesaDB;Persist Security Info=False;User ID=t_eliseogr;Password=El1530%.*314;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //_ConnectionString = "Server=tcp:srvsqlmercurioqa.database.windows.net,1433;Initial Catalog=MercurioQaDB;Persist Security Info=False;User ID=t_eliseogr;Password=W3rcur10!QA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioPDPProdDB;Persist Security Info=False;User ID=ProcesadorPago;Password=W3rcur10PDP!#$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Application Name=PaymentOrderProcess;Min Pool Size=0;Max Pool Size=5;Pooling=true;";

            try
            {

                List<OrderPagoModels> listaP = new List<OrderPagoModels>();
                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("up_Corp_PPS_sel_GetOrderHistory", cnn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;

                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@order_no", SqlDbType.VarChar);
                        param.Value = order;


                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);

                        //DataTable dt = ds.Tables[0].Clone();

                        //dt.Rows.Add(ds.Tables[0].Select(""));

                        if (ds.Tables.Count > 0)
                        {

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                List<PagosModels> lista = new List<PagosModels>();
                                OrderPagoModels p = new OrderPagoModels();

                                foreach (DataRow r in ds.Tables[0].Rows)
                                {
                                    if (r["responsejson"] != System.DBNull.Value)
                                    {

                                        if (!string.IsNullOrEmpty(r["responsejson"].ToString().Trim()))
                                        {
                                            JObject out_response = JObject.Parse(r["responsejson"].ToString());

                                            if (out_response["responseCode"] != null)
                                            {
                                                if (string.IsNullOrEmpty(out_response["responseCode"].ToString()))
                                                {

                                                    if (out_response["responseCode"].ToString().Equals("202")) { p.ErrorSpecified = true; break; }
                                                }
                                            }

                                            if (out_response["responseCode"] != null)
                                            {
                                                if (!string.IsNullOrEmpty(out_response["responseCode"].ToString()))
                                                {
                                                    if (out_response["responseCode"].ToString().Equals("202")) { p.ErrorSpecified = true; break; }
                                                    if (out_response["responseCode"].ToString().Equals("400")) { p.ErrorSpecified = true; break; }
                                                }
                                            }

                                            if (out_response["responseError"] != null)
                                            {
                                                if (!string.IsNullOrEmpty(out_response["responseError"].ToString()))
                                                {
                                                    p.ErrorSpecified = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else { p.ErrorSpecified = true; break; }



                                    switch (r["method"].ToString())
                                    {

                                        // order_no
                                        //RequestJson
                                        //ResponseJson
                                        //method
                                        //fec_movto
                                        case "Descarga SF":
                                            //p.request = r["RequestJson"].ToString();
                                            //p.response = r["ResponseJson"].ToString();
                                            //p.fecha = r["fec_movto"].ToString();

                                            //JObject orde = JObject.Parse(r["RequestJson"].ToString());

                                            //p.telefono = orde["customerContact"] !=null ? orde["customerContact"].ToString() : "";
                                            //p.orden = orde["orderReferenceNumber"].ToString();
                                            //p.direccion = orde["customerAddress"].ToString();
                                            //p.cliente = orde["customerFirstName"].ToString() + orde["customerLastName"].ToString();
                                            //p.clienteId = orde["customerId"].ToString();
                                            //p.email = orde["customerEmail"].ToString();

                                            //p.total = "$" + orde["orderAmount"].ToString();
                                            //p.dineroElectronico = "$" + (orde["customerLoyaltyRedeemElectronicMoney"] == null ? "" : orde["customerLoyaltyRedeemElectronicMoney"].ToString());
                                            //p.dineroEfectivo = "$" + orde["customerLoyaltyRedeemMoney"].ToString();
                                            //p.tarjeta = "$" + orde["orderAmount"].ToString();
                                            break;
                                        case "LoyaltyRedeemMoney":
                                        case "LoyaltyRedeemElectronicMoney":


                                            if (r["RequestJson"].ToString().Contains("Id_Cve_Orden"))
                                            {

                                                JObject od = JObject.Parse(r["RequestJson"].ToString());
                                                JObject od1 = JObject.Parse(r["ResponseJson"].ToString());

                                                p.LealtadSpecified = true;

                                                p.dineroEfectivo = od["Imp_Efvo"].ToString();
                                                p.dineroElectronico = od["Imp_DE"].ToString();
                                                p.puntos = od["Cant_Puntos"].ToString();

                                                //{ "Id_Cve_Orden":"0000007005","Id_Cve_GUID":"STG_00006507","Id_Cve_TokenCta":"2000-XXXX-XXXX-2798","Cve_Operacion":"COMPRA"
                                                //,"Imp_Vta":207.5,"Cant_Puntos":0,"Imp_Comp":0.0,"Imp_DE":40.0,"Imp_Efvo":50.0,"Imp_Cred":0.0,"Cve_Accion":"DISMINUYE"}
                                                //{ "cve_Autz":"000000","bit_Error":true,"desc_Error":"TRANSACCION EN CERO NO SE PROCESARA"}

                                                if (od1["bit_Error"].ToString().ToLower().Equals("false"))
                                                {
                                                    p.jsonLealtadRequest = new RequestLealtad();
                                                    p.jsonLealtadRequest.Cve_Operacion = od["Id_Cve_Orden"].ToString();
                                                    p.jsonLealtadRequest.Imp_Comp = Convert.ToDecimal(od["Imp_Comp"].ToString());
                                                    p.jsonLealtadRequest.Imp_Cred = Convert.ToDecimal(od["Imp_Cred"].ToString());
                                                    p.jsonLealtadRequest.Imp_DE = Convert.ToDecimal(od["Imp_DE"].ToString());
                                                    p.jsonLealtadRequest.Imp_Efvo = Convert.ToDecimal(od["Imp_Efvo"].ToString());
                                                    p.jsonLealtadRequest.Imp_Vta = Convert.ToDecimal(od["Imp_Vta"].ToString());
                                                    p.jsonLealtadRequest.Cant_Puntos = Convert.ToInt32(od["Cant_Puntos"].ToString());
                                                    p.jsonLealtadRequest.Id_Cve_TokenCta = od["Id_Cve_TokenCta"].ToString();

                                                    p.LealtadjsonResponse = new ProcessReponse();
                                                    p.LealtadjsonResponse.code = "00";
                                                    p.LealtadjsonResponse.estatus = "ACEPTADA";
                                                    p.LealtadjsonResponse.id = od1["cve_Autz"].ToString();


                                                    p.LealtadjsonResponse.razon = "";
                                                    p.LealtadjsonResponse.decision = "";
                                                }
                                                else
                                                {
                                                    p.jsonLealtadRequest = new RequestLealtad();
                                                    p.jsonLealtadRequest.Cve_Operacion = od["Id_Cve_Orden"].ToString();
                                                    p.jsonLealtadRequest.Imp_Comp = Convert.ToDecimal(od["Imp_Comp"].ToString());
                                                    p.jsonLealtadRequest.Imp_Cred = Convert.ToDecimal(od["Imp_Cred"].ToString());
                                                    p.jsonLealtadRequest.Imp_DE = Convert.ToDecimal(od["Imp_DE"].ToString());
                                                    p.jsonLealtadRequest.Imp_Efvo = Convert.ToDecimal(od["Imp_Efvo"].ToString());
                                                    p.jsonLealtadRequest.Imp_Vta = Convert.ToDecimal(od["Imp_Vta"].ToString());
                                                    p.jsonLealtadRequest.Cant_Puntos = Convert.ToInt32(od["Cant_Puntos"].ToString());


                                                    p.LealtadjsonResponse = new ProcessReponse();
                                                    p.LealtadjsonResponse.code = "00";
                                                    p.LealtadjsonResponse.estatus = "RECHAZADA";
                                                    p.LealtadjsonResponse.id = od1["cve_Autz"].ToString();


                                                    p.LealtadjsonResponse.razon = "";
                                                    p.LealtadjsonResponse.decision = od1["desc_Error"].ToString();
                                                }

                                            }

                                            break;
                                        case "Start Process Payment":
                                            p.request = r["RequestJson"].ToString();
                                            p.response = r["ResponseJson"].ToString();
                                            p.fecha = r["fec_movto"].ToString();

                                            JObject orde1 = JObject.Parse(r["RequestJson"].ToString());

                                            p.telefono = orde1["customerContact"] != null ? orde1["customerContact"].ToString() : "";
                                            p.orden = orde1["orderReferenceNumber"].ToString();
                                            p.direccion = orde1["customerAddress"].ToString();
                                            p.cliente = orde1["customerFirstName"].ToString() + orde1["customerLastName"].ToString();
                                            p.clienteId = orde1["customerId"].ToString();
                                            p.email = orde1["customerEmail"].ToString();

                                            p.total = "$" + orde1["orderAmount"].ToString();
                                            p.dineroElectronico = "$" + (orde1["customerLoyaltyRedeemElectronicMoney"] == null ? "" : orde1["customerLoyaltyRedeemElectronicMoney"].ToString());
                                            p.dineroEfectivo = "$" + orde1["customerLoyaltyRedeemMoney"].ToString();
                                            p.tarjeta = "$" + orde1["orderAmount"].ToString();
                                            p.puntos = orde1["customerLoyaltyRedeemPoints"].ToString();

                                            p.shipments = new List<ShipmentsModel>();
                                            p.shipments = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ShipmentsModel>>(orde1["shipments"].ToString());

                                            break;
                                        case "createDecision":

                                            JObject orde3 = JObject.Parse(r["ResponseJson"].ToString());
                                            p.CreateDecisionSpecified = true;

                                            p.CreatejsonResponse = new ProcessReponse();
                                            p.CreatejsonResponse.estatus = orde3["status"].ToString();
                                            p.CreatejsonResponse.id = orde3["id"].ToString();

                                            if (orde3["errorInformation"] != null)
                                            {

                                                JObject error = JObject.Parse(r["ResponseJson"].ToString());

                                                p.CreatejsonResponse.razon = error["reason"].ToString();
                                                p.CreatejsonResponse.decision = error["message"].ToString();
                                            }

                                            break;


                                        case "EnrollmentPayment":

                                            JObject orde4 = JObject.Parse(r["ResponseJson"].ToString());
                                            p.EnrollmentSpecified = true;

                                            p.EnrollmentjsonResponse = new ProcessReponse();
                                            p.EnrollmentjsonResponse.estatus = orde4["status"].ToString();
                                            p.EnrollmentjsonResponse.id = orde4["id"].ToString();

                                            if (orde4["errorInformation"] != null)
                                            {

                                                JObject error = JObject.Parse(r["ResponseJson"].ToString());

                                                p.EnrollmentjsonResponse.razon = error["reason"].ToString();
                                                p.EnrollmentjsonResponse.decision = error["message"].ToString();
                                            }

                                            break;

                                        case "ValidateAuthentication":

                                            JObject orde2 = JObject.Parse(r["ResponseJson"].ToString());
                                            p.ValidateSpecified = true;

                                            p.ValidatejsonResponse = new ProcessReponse();
                                            p.ValidatejsonResponse.code = "";
                                            p.ValidatejsonResponse.estatus = orde2["status"].ToString();
                                            p.ValidatejsonResponse.id = orde2["id"].ToString();

                                            if (orde2["errorInformation"] != null)
                                            {

                                                JObject error = JObject.Parse(r["ResponseJson"].ToString());

                                                p.ValidatejsonResponse.razon = error["reason"].ToString();
                                                p.ValidatejsonResponse.decision = error["message"].ToString();
                                            }
                                            break;

                                        case "NotifyAutheticationPayment":
                                            break;
                                        case "capturePayment":
                                            break;
                                        case "processPayment":

                                            JObject orde5 = JObject.Parse(r["ResponseJson"].ToString());
                                            p.ProcessOrderSpecified = true;

                                            p.ProcessOrderjsonResponse = new ProcessReponse();
                                            p.ProcessOrderjsonResponse.estatus = orde5["status"].ToString();
                                            p.ProcessOrderjsonResponse.id = orde5["id"].ToString();

                                            if (orde5["errorInformation"] != null)
                                            {

                                                JObject error = JObject.Parse(r["ResponseJson"].ToString());

                                                p.ProcessOrderjsonResponse.razon = error["reason"].ToString();
                                                p.ProcessOrderjsonResponse.decision = error["message"].ToString();
                                            }
                                            else
                                            {
                                                p.ProcessOrderjsonResponse.razon = "";
                                                p.ProcessOrderjsonResponse.decision = "";
                                            }

                                            break;

                                            //NotifyAutheticationPayment
                                            //LoyaltyRedeemMoney
                                    }



                                }

                                var result1 = new { Success = true, order = p };

                                return Json(result1, JsonRequestBehavior.AllowGet);
                            }
                        }

                    }
                }

            }
            catch (System.Data.SqlClient.SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


            var result = new { Success = true, Message = "" };

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Descarga Reportes / by Order
        public List<ProcesadorPagosBase> GetAutorizaciones(string OrderReferenceNumber)
        {
            DataSet ds = new DataSet();
            List<ProcesadorPagosBase> LstppsBase = new List<ProcesadorPagosBase>();

            try
            {
                ds = GetConcentradoDatosBase(OrderReferenceNumber);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            ProcesadorPagosBase ppsBase = new ProcesadorPagosBase();

                            #region Mapping
                            #region Datos Orden
                            ppsBase.OrderReferenceNumber = row["OrderReferenceNumber"].ToString();          //Orden ID
                            ppsBase.PaymentTransactionID = row["PaymentTransactionID"].ToString();          //Transacción       
                            ppsBase.OrderAmount = row["OrderAmount"].ToString();                            //Monto Total Orden
                            if (row["OrderSaleChannel"].ToString() == "1")
                                ppsBase.OrderSaleChannel = "SFWEB";
                            else
                                ppsBase.OrderSaleChannel = row["OrderSaleChannel"].ToString();                  //Canal Compra
                            #endregion

                            #region Tokenizacion
                            ppsBase.Bank = row["Bank"].ToString();                                          //Banco
                            ppsBase.BinCode = row["BinCode"].ToString();
                            ppsBase.MaskCard = row["MaskCard"].ToString().Substring(15);                    // Sufijo hay q recortar
                            ppsBase.TypeOfCard = row["TypeOfCard"].ToString();                              //Tipo Tarjeta
                            ppsBase.PaymentMethod = row["PaymentMethod"].ToString();                        //Marca
                            ppsBase.CustomerFirstName = row["CustomerFirstName"].ToString();
                            ppsBase.CustomerLastName = row["CustomerLastName"].ToString();
                            #endregion

                            #region shipping
                            ppsBase.ShippingStoreId = row["shippingStoreId"].ToString();                    //Tipo ALmacen
                            ppsBase.ShippingReferenceNumber = row["ShippingReferenceNumber"].ToString();    //Consignación ID

                            if (row["ShippingReferenceNumber"].ToString() == "001-1")
                            {
                                ppsBase.ShippingDeliveryDesc = "SETC";
                                ppsBase.Catalogo = "SETC";
                                ppsBase.AffiliationType = "8655759";
                                ppsBase.Adquirente = "GETNET";
                            }

                            else
                            {
                                ppsBase.ShippingDeliveryDesc = "MG";
                                ppsBase.Catalogo = "MG";
                                ppsBase.AffiliationType = "1045441";
                                ppsBase.Adquirente = "EVO Payment";
                            }
                            #endregion

                            #region Lealtad
                            ppsBase.CustomerLoyaltyRedeemPoints = row["CustomerLoyaltyRedeemPoints"].ToString(); //punto aplicados
                            ppsBase.CustomerLoyaltyRedeemMoney = row["CustomerLoyaltyRedeemMoney"].ToString();  //efectivo disponble /dinero en efectivo                              
                            ppsBase.CustomerLoyaltyCardId = row["CustomerLoyaltyCardId"].ToString();
                            #endregion

                            #region GetTrace
                            var DatosExtra = GetTracePayment(ppsBase.OrderReferenceNumber, ppsBase.Catalogo);

                            if (DatosExtra.orderReferenceNumber != null)
                            {
                                string FechaOrden = DatosExtra.orderDateTime.Substring(0, 10);
                                string HoraOrden = DatosExtra.orderDateTime.Substring(11, 5);

                                ppsBase.OrderDate = FechaOrden;
                                ppsBase.OrderHour = HoraOrden;

                                ppsBase.orderAmount = DatosExtra.orderAmount;
                                ppsBase.TransactionReferenceID = DatosExtra.TransactionReferenceID;
                                ppsBase.IsAuthenticated = DatosExtra.IsAuthenticated;
                                ppsBase.IsAuthorized = DatosExtra.IsAuthorized;
                                ppsBase.Apply3DS = DatosExtra.Apply3DS;
                                ppsBase.MerchandiseType = DatosExtra.MerchandiseType;
                                ppsBase.PaymentTransactionService = DatosExtra.TransactionStatus;

                                if (DatosExtra.paymentType == "WALLET")
                                {
                                    ppsBase.paymentTypeJson = "PAYPAL";
                                }
                                else
                                {
                                    bool flagOmonel = DatosExtra.paymentToken.Contains("OMONEL");

                                    if (DatosExtra.paymentToken.Contains("OMONEL"))
                                    {
                                        ppsBase.paymentTypeJson = "OMONEL";
                                        ppsBase.Adquirente = "OMONEL";
                                    }

                                    else
                                    {
                                        ppsBase.paymentTypeJson = DatosExtra.paymentType;
                                        ppsBase.Adquirente = "EVO PAYMENT";
                                    }

                                }

                                if (DatosExtra.shipments.Count > 0)
                                {
                                    ppsBase.shippingDeliveryDesc = DatosExtra.shipments[0].shippingDeliveryDesc;
                                    ppsBase.shippingPaymentImport = DatosExtra.shipments[0].shippingPaymentImport;
                                    ppsBase.shippingPaymentInstallments = DatosExtra.shipments[0].shippingPaymentInstallments;
                                    ppsBase.shippingItemCategory = DatosExtra.shipments[0].Items[0].shippingItemCategory;
                                    ppsBase.shippingItemId = DatosExtra.shipments[0].Items[0].shippingItemId;
                                    ppsBase.shippingItemName = DatosExtra.shipments[0].Items[0].shippingItemName;
                                    ppsBase.ShippingItemTotal = DatosExtra.shipments[0].Items[0].ShippingItemTotal;
                                }
                            }
                            #endregion

                            #region ApprovalCode
                            var ApprovalCode = BL_GetApprovalCode(row["OrderReferenceNumber"].ToString());
                            ppsBase.TransactionAuthorizationId = ApprovalCode;
                            #endregion

                            #region Emisor
                            var EmisorResponse = GetDatosEmisor(ppsBase.OrderReferenceNumber);

                            ppsBase.DecisionEmisor = EmisorResponse.DecisionEmisor;
                            ppsBase.CveReespuestaEmisor = ApprovalCode; //EmisorResponse.CveReespuestaEmisor;
                            ppsBase.DescReespuestaEmisor = EmisorResponse.DescReespuestaEmisor;

                            if (EmisorResponse.DecisionEmisor == "AUTHORIZED")
                                ppsBase.DescReespuestaEmisor = "AUTHORIZED";
                            #endregion
                            #endregion

                            LstppsBase.Add(ppsBase);
                        }
                    }
                }
                else
                {
                    var LstOmonel = GetProcesadorPagosBase_Omonel(OrderReferenceNumber, "CanalCompra");

                    if (LstOmonel.Count > 0)
                    {
                        foreach (var omonel in LstOmonel)
                        {
                            LstppsBase.Add(omonel);
                        }
                    }
                    else
                    {
                        //TODO: APP
                    }
                }

                return LstppsBase;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ProcesadorPagosBase> GetCanalCompra(string OrderReferenceNumber)
        {
            DataSet ds = new DataSet();
            List<ProcesadorPagosBase> LstppsBase = new List<ProcesadorPagosBase>();

            string FechaOrden = string.Empty;
            string HoraOrden = string.Empty;

            try
            {

                ds = GetConcentradoDatosBase(OrderReferenceNumber);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            ProcesadorPagosBase ppsBase = new ProcesadorPagosBase();

                            #region Mapping
                            #region Order
                            ppsBase.OrderReferenceNumber = row["OrderReferenceNumber"].ToString();          //Orden ID
                            ppsBase.PaymentTransactionID = row["PaymentTransactionID"].ToString();          //Transacción
                            ppsBase.OrderAmount = row["OrderAmount"].ToString();                            //Monto Total Orden
                            #endregion

                            #region Tokenizacion
                            ppsBase.Bank = row["Bank"].ToString();                                          //Banco
                            ppsBase.BinCode = row["BinCode"].ToString();
                            ppsBase.MaskCard = row["MaskCard"].ToString().Substring(15);                    // Sufijo hay q recortar
                            ppsBase.TypeOfCard = row["TypeOfCard"].ToString();                              //Tipo Tarjeta
                            ppsBase.PaymentMethod = row["PaymentMethod"].ToString();                        //Marca
                            #endregion

                            #region shipping
                            ppsBase.ShippingStoreId = row["shippingStoreId"].ToString();                    //Tipo ALmacen
                            ppsBase.ShippingReferenceNumber = row["ShippingReferenceNumber"].ToString();    //Consignación ID

                            if (row["ShippingReferenceNumber"].ToString() == "001-1")
                            {
                                ppsBase.ShippingDeliveryDesc = "SETC";
                                ppsBase.Catalogo = "SETC";
                                ppsBase.AffiliationType = "8655759";
                                ppsBase.Adquirente = "GETNET";
                            }

                            else
                            {
                                ppsBase.ShippingDeliveryDesc = "MG";
                                ppsBase.Catalogo = "MG";
                                ppsBase.AffiliationType = "1045441";
                                ppsBase.Adquirente = "EVO Payment";
                            }
                            #endregion

                            #region Lealtad
                            ppsBase.CustomerLoyaltyRedeemPoints = row["CustomerLoyaltyRedeemPoints"].ToString(); //punto aplicados
                            ppsBase.CustomerLoyaltyRedeemMoney = row["CustomerLoyaltyRedeemMoney"].ToString();  //efectivo disponble /dinero en efectivo                              
                            ppsBase.CustomerLoyaltyCardId = row["CustomerLoyaltyCardId"].ToString();
                            #endregion

                            #region ApprovalCode
                            var ApprovalCode = BL_GetApprovalCode(row["OrderReferenceNumber"].ToString());
                            ppsBase.TransactionAuthorizationId = ApprovalCode;
                            #endregion

                            #region OUE
                            var oue = BL_Ordenes_APP(row["OrderReferenceNumber"].ToString());

                            if (oue.Id_Num_Apl == "22" || oue.Id_Num_Apl == "")
                            {
                                if (row["OrderSaleChannel"].ToString() == "1")
                                    ppsBase.OrderSaleChannel = "SFWEB";
                                else
                                    ppsBase.OrderSaleChannel = row["OrderSaleChannel"].ToString();                  //Canal Compra
                            }
                            else
                            {
                                ppsBase.OrderSaleChannel = "APP";                 //Canal Compra
                                ppsBase.TipoMobile = oue.CreatedBy;
                            }

                            ppsBase.DeliveryType = oue.DeliveryType;
                            #endregion

                            #region GetTrace
                            var DatosExtra = GetTracePayment(ppsBase.OrderReferenceNumber, ppsBase.Catalogo);

                            if (DatosExtra != null)
                            {
                                if (DatosExtra.orderReferenceNumber != null)
                                {
                                    if (oue.Id_Num_Apl == "22")
                                    {
                                        FechaOrden = DatosExtra.orderDateTime.Substring(0, 10);
                                        HoraOrden = DatosExtra.orderDateTime.Substring(11, 5);
                                    }
                                    else
                                    {
                                        var fec = DatosExtra.orderDateTime.Replace("-", "");
                                        FechaOrden = fec.Substring(0, 8);
                                        HoraOrden = fec.Substring(9, 5);
                                    }

                                    ppsBase.OrderDate = FechaOrden;
                                    ppsBase.OrderHour = HoraOrden;
                                    ppsBase.orderAmount = DatosExtra.orderAmount;
                                    ppsBase.TransactionReferenceID = DatosExtra.TransactionReferenceID;
                                    ppsBase.IsAuthenticated = DatosExtra.IsAuthenticated;
                                    ppsBase.IsAuthorized = DatosExtra.IsAuthorized;
                                    ppsBase.Apply3DS = DatosExtra.Apply3DS;
                                    ppsBase.MerchandiseType = DatosExtra.MerchandiseType;
                                    ppsBase.clientEmail = DatosExtra.customerEmail;
                                    ppsBase.TransactionStatus = DatosExtra.TransactionStatus;

                                    if (DatosExtra.paymentType == "WALLET")
                                    {
                                        ppsBase.paymentTypeJson = "PAYPAL";
                                    }
                                    else
                                    {
                                        bool flagOmonel = DatosExtra.paymentToken.Contains("OMONEL");

                                        if (DatosExtra.paymentToken.Contains("OMONEL"))
                                            ppsBase.paymentTypeJson = "OMONEL";
                                        else
                                            ppsBase.paymentTypeJson = DatosExtra.paymentType;
                                    }

                                    if (DatosExtra.shipments.Count > 0)
                                    {
                                        ppsBase.shippingDeliveryDesc = DatosExtra.shipments[0].shippingDeliveryDesc;
                                        ppsBase.shippingPaymentImport = DatosExtra.shipments[0].shippingPaymentImport;
                                        ppsBase.shippingPaymentInstallments = DatosExtra.shipments[0].shippingPaymentInstallments;
                                        ppsBase.ShippingFirstName = DatosExtra.shipments[0].shippingFirstName;
                                    }
                                }
                            }
                            #endregion

                            #region Estatus Shipment
                            var estatusShipment = GetEstatusShipment(row["OrderReferenceNumber"].ToString());

                            ppsBase.TipoAlmacen = estatusShipment.CarrierName;
                            ppsBase.EstatusEnvio = estatusShipment.status;
                            #endregion
                            #endregion

                            LstppsBase.Add(ppsBase);
                        }
                    }
                }
                else
                {
                    var LstOmonel = GetProcesadorPagosBase_Omonel(OrderReferenceNumber, "CanalCompra");

                    foreach (var omonel in LstOmonel)
                    {
                        LstppsBase.Add(omonel);
                    }
                }


                return LstppsBase;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ProcesadorPagosBase> GetLiquidaciones(string OrderReferenceNumber)
        {
            DataSet ds = new DataSet();
            List<ProcesadorPagosBase> LstppsBase = new List<ProcesadorPagosBase>();
            string FechaOrden = string.Empty;
            string HoraOrden = string.Empty;

            try
            {
                ds = GetConcentradoDatosBase(OrderReferenceNumber);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            ProcesadorPagosBase ppsBase = new ProcesadorPagosBase();

                            #region Mapping
                            #region Datos Orden
                            ppsBase.OrderReferenceNumber = row["OrderReferenceNumber"].ToString();          //Orden ID
                            ppsBase.PaymentTransactionID = row["PaymentTransactionID"].ToString();          //Transacción
                            //ppsBase.OrderSaleChannel = row["OrderSaleChannel"].ToString();                  //Canal Compra
                            #endregion

                            #region Tokenizacion
                            ppsBase.Bank = row["Bank"].ToString();                                          //Banco
                            ppsBase.BinCode = row["BinCode"].ToString();
                            ppsBase.MaskCard = row["MaskCard"].ToString().Substring(15);                    // Sufijo hay q recortar
                            ppsBase.TypeOfCard = row["TypeOfCard"].ToString();                              //Tipo Tarjeta
                            ppsBase.PaymentMethod = row["PaymentMethod"].ToString();                        //Marca
                            ppsBase.CustomerFirstName = row["CustomerFirstName"].ToString();
                            ppsBase.CustomerLastName = row["CustomerLastName"].ToString();
                            #endregion

                            #region shipping
                            ppsBase.ShippingStoreId = row["shippingStoreId"].ToString();                    //Tipo ALmacen
                            ppsBase.ShippingReferenceNumber = row["ShippingReferenceNumber"].ToString();    //Consignación ID

                            if (row["ShippingReferenceNumber"].ToString() == "001-1")
                            {
                                ppsBase.ShippingDeliveryDesc = "SETC";
                                ppsBase.Catalogo = "SETC";
                                ppsBase.AffiliationType = "8655759";
                                ppsBase.Adquirente = "GETNET";
                            }

                            else
                            {
                                ppsBase.ShippingDeliveryDesc = "MG";
                                ppsBase.Catalogo = "MG";
                                ppsBase.AffiliationType = "1045441";
                                ppsBase.Adquirente = "EVO Payment";
                            }
                            #endregion

                            #region Lealtad
                            ppsBase.CustomerLoyaltyRedeemPoints = row["CustomerLoyaltyRedeemPoints"].ToString(); //punto aplicados
                            ppsBase.CustomerLoyaltyRedeemMoney = row["CustomerLoyaltyRedeemMoney"].ToString();  //efectivo disponble /dinero en efectivo                              
                            ppsBase.CustomerLoyaltyCardId = row["CustomerLoyaltyCardId"].ToString();
                            #endregion

                            #region ApprovalCode
                            var ApprovalCode = BL_GetApprovalCode(row["OrderReferenceNumber"].ToString());
                            ppsBase.TransactionAuthorizationId = ApprovalCode;
                            #endregion

                            #region OUE
                            var oue = BL_Ordenes_APP(row["OrderReferenceNumber"].ToString());

                            if (oue.Id_Num_Apl == "22" || oue.Id_Num_Apl == "")
                            {
                                if (row["OrderSaleChannel"].ToString() == "1")
                                    ppsBase.OrderSaleChannel = "SFWEB";
                                else
                                    ppsBase.OrderSaleChannel = row["OrderSaleChannel"].ToString();                  //Canal Compra
                            }
                            else
                            {
                                ppsBase.OrderSaleChannel = "APP";                 //Canal Compra
                                ppsBase.TipoMobile = oue.CreatedBy;
                            }

                            ppsBase.DeliveryType = oue.DeliveryType;
                            #endregion

                            #region GetTrace
                            var DatosExtra = GetTracePayment(ppsBase.OrderReferenceNumber, ppsBase.Catalogo);
                            if (DatosExtra != null)
                            {
                                if (DatosExtra.orderReferenceNumber != null)
                                {
                                    if (oue.Id_Num_Apl == "22")
                                    {
                                        FechaOrden = DatosExtra.orderDateTime.Substring(0, 10);
                                        HoraOrden = DatosExtra.orderDateTime.Substring(11, 5);
                                    }
                                    else
                                    {
                                        var fec = DatosExtra.orderDateTime.Replace("-", "");
                                        FechaOrden = fec.Substring(0, 8);
                                        HoraOrden = fec.Substring(9, 5);
                                    }

                                    ppsBase.OrderDate = FechaOrden;
                                    ppsBase.OrderHour = HoraOrden;
                                    ppsBase.orderAmount = DatosExtra.orderAmount;
                                    ppsBase.MerchandiseType = DatosExtra.MerchandiseType;
                                    ppsBase.clientEmail = DatosExtra.customerEmail;
                                    ppsBase.TransactionStatus = DatosExtra.TransactionStatus;
                                    ppsBase.Apply3DS = DatosExtra.Apply3DS;
                                    ppsBase.PaymentTransactionService = DatosExtra.TransactionStatus;

                                    if (DatosExtra.paymentType == "WALLET")
                                    {
                                        ppsBase.paymentTypeJson = "PAYPAL";
                                    }
                                    else
                                    {
                                        bool flagOmonel = DatosExtra.paymentToken.Contains("OMONEL");

                                        if (DatosExtra.paymentToken.Contains("OMONEL"))
                                        {
                                            ppsBase.paymentTypeJson = "OMONEL";
                                            ppsBase.Adquirente = "OMONEL";
                                        }
                                        else
                                        {
                                            ppsBase.paymentTypeJson = DatosExtra.paymentType;
                                            ppsBase.Adquirente = "EVO PAYMENT";
                                        }
                                    }

                                    if (DatosExtra.shipments.Count > 0)
                                    {
                                        ppsBase.shippingDeliveryDesc = DatosExtra.shipments[0].shippingDeliveryDesc;
                                        ppsBase.shippingPaymentImport = DatosExtra.shipments[0].shippingPaymentImport;
                                        ppsBase.shippingPaymentInstallments = DatosExtra.shipments[0].shippingPaymentInstallments;
                                        ppsBase.shippingItemCategory = DatosExtra.shipments[0].Items[0].shippingItemCategory;
                                        ppsBase.shippingItemId = DatosExtra.shipments[0].Items[0].shippingItemId;
                                        ppsBase.shippingItemName = DatosExtra.shipments[0].Items[0].shippingItemName;
                                        ppsBase.ShippingItemTotal = DatosExtra.shipments[0].Items[0].ShippingItemTotal;

                                        ppsBase.ShippingFirstName = DatosExtra.shipments[0].shippingFirstName;
                                        ppsBase.ShippingLastName = DatosExtra.shipments[0].shippingLastName;

                                        #region Datos Liquidacion
                                        ppsBase.FechaLiquidacion = FechaOrden;
                                        ppsBase.HoraLiquidacion = HoraOrden;
                                        ppsBase.MontoLiquidacion = DatosExtra.shipments[0].shippingPaymentImport;
                                        ppsBase.LiquidacionManual = "";
                                        ppsBase.LiquidacionAutomatica = "True";
                                        ppsBase.IDTransaccionLiquidacion = DatosExtra.TransactionReferenceID; ;
                                        #endregion
                                    }
                                }
                            }

                            #endregion

                            #region Estatus Shipment
                            var estatusShipment = GetEstatusShipment(row["OrderReferenceNumber"].ToString());

                            ppsBase.TipoAlmacen = estatusShipment.CarrierName;
                            ppsBase.EstatusEnvio = estatusShipment.status;
                            #endregion
                            #endregion

                            if (DatosExtra != null)
                                LstppsBase.Add(ppsBase);
                        }
                    }
                }
                else
                {
                    var LstOmonel = GetProcesadorPagosBase_Omonel(OrderReferenceNumber, "AuthBancaria");

                    foreach (var omonel in LstOmonel)
                    {
                        LstppsBase.Add(omonel);
                    }
                }

                return LstppsBase;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProcesadorPagosBase> GetReverso(string OrderReferenceNumber)
        {
            DataSet ds = new DataSet();
            List<ProcesadorPagosBase> LstppsBase = new List<ProcesadorPagosBase>();
            string FechaOrden = string.Empty;
            string HoraOrden = string.Empty;

            try
            {
                ds = GetConcentradoDatosBase(OrderReferenceNumber);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            ProcesadorPagosBase ppsBase = new ProcesadorPagosBase();

                            #region Mapping
                            #region Datos Orden
                            ppsBase.OrderReferenceNumber = row["OrderReferenceNumber"].ToString();          //Orden ID
                            ppsBase.PaymentTransactionID = row["PaymentTransactionID"].ToString();          //Transacción
                            //ppsBase.OrderSaleChannel = row["OrderSaleChannel"].ToString();                  //Canal Compra
                            #endregion

                            #region Tokenizacion
                            ppsBase.Bank = row["Bank"].ToString();                                          //Banco
                            ppsBase.BinCode = row["BinCode"].ToString();
                            ppsBase.MaskCard = row["MaskCard"].ToString().Substring(15);                    // Sufijo hay q recortar
                            ppsBase.TypeOfCard = row["TypeOfCard"].ToString();                              //Tipo Tarjeta
                            ppsBase.PaymentMethod = row["PaymentMethod"].ToString();                        //Marca
                            ppsBase.CustomerFirstName = row["CustomerFirstName"].ToString();
                            ppsBase.CustomerLastName = row["CustomerLastName"].ToString();
                            #endregion

                            #region shipping
                            ppsBase.ShippingStoreId = row["shippingStoreId"].ToString();                    //Tipo ALmacen
                            ppsBase.ShippingReferenceNumber = row["ShippingReferenceNumber"].ToString();    //Consignación ID

                            if (row["ShippingReferenceNumber"].ToString() == "001-1")
                            {
                                ppsBase.ShippingDeliveryDesc = "SETC";
                                ppsBase.Catalogo = "SETC";
                                ppsBase.AffiliationType = "8655759";
                                ppsBase.Adquirente = "GETNET";
                            }

                            else
                            {
                                ppsBase.ShippingDeliveryDesc = "MG";
                                ppsBase.Catalogo = "MG";
                                ppsBase.AffiliationType = "1045441";
                                ppsBase.Adquirente = "EVO Payment";
                            }
                            #endregion

                            #region Lealtad
                            ppsBase.CustomerLoyaltyRedeemPoints = row["CustomerLoyaltyRedeemPoints"].ToString(); //punto aplicados
                            ppsBase.CustomerLoyaltyRedeemMoney = row["CustomerLoyaltyRedeemMoney"].ToString();  //efectivo disponble /dinero en efectivo                              
                            ppsBase.CustomerLoyaltyCardId = row["CustomerLoyaltyCardId"].ToString();
                            #endregion

                            #region ApprovalCode
                            var ApprovalCode = BL_GetApprovalCode(row["OrderReferenceNumber"].ToString());
                            ppsBase.TransactionAuthorizationId = ApprovalCode;
                            #endregion

                            #region OUE
                            var oue = BL_Ordenes_APP(row["OrderReferenceNumber"].ToString());

                            if (oue.Id_Num_Apl == "22" || oue.Id_Num_Apl == "")
                            {
                                if (row["OrderSaleChannel"].ToString() == "1")
                                    ppsBase.OrderSaleChannel = "SFWEB";
                                else
                                    ppsBase.OrderSaleChannel = row["OrderSaleChannel"].ToString();                  //Canal Compra
                            }
                            else
                            {
                                ppsBase.OrderSaleChannel = "APP";                 //Canal Compra
                                ppsBase.TipoMobile = oue.CreatedBy;
                            }

                            ppsBase.DeliveryType = oue.DeliveryType;
                            #endregion

                            #region GetTrace
                            var DatosExtra = GetTracePayment(ppsBase.OrderReferenceNumber, ppsBase.Catalogo);
                            if (DatosExtra != null)
                            {
                                if (DatosExtra.orderReferenceNumber != null)
                                {
                                    if (oue.Id_Num_Apl == "22")
                                    {
                                        FechaOrden = DatosExtra.orderDateTime.Substring(0, 10);
                                        HoraOrden = DatosExtra.orderDateTime.Substring(11, 5);
                                    }
                                    else
                                    {
                                        FechaOrden = DatosExtra.orderDateTime.Substring(0, 8);
                                        HoraOrden = DatosExtra.orderDateTime.Substring(9, 5);
                                    }

                                    ppsBase.OrderDate = FechaOrden;
                                    ppsBase.OrderHour = HoraOrden;
                                    ppsBase.orderAmount = DatosExtra.orderAmount;
                                    ppsBase.TransactionAuthorizationId = DatosExtra.TransactionAuthorizationId;
                                    ppsBase.MerchandiseType = DatosExtra.MerchandiseType;
                                    ppsBase.clientEmail = DatosExtra.customerEmail;
                                    ppsBase.TransactionStatus = DatosExtra.TransactionStatus;

                                    if (DatosExtra.paymentType == "WALLET")
                                    {
                                        ppsBase.paymentTypeJson = "PAYPAL";
                                    }
                                    else
                                    {
                                        bool flagOmonel = DatosExtra.paymentToken.Contains("OMONEL");

                                        if (DatosExtra.paymentToken.Contains("OMONEL"))
                                        {
                                            ppsBase.paymentTypeJson = "OMONEL";
                                            ppsBase.Adquirente = "OMONEL";
                                        }

                                        else
                                        {
                                            ppsBase.paymentTypeJson = DatosExtra.paymentType;
                                            ppsBase.Adquirente = "EVO PAYMENT";
                                        }

                                    }

                                    if (DatosExtra.shipments.Count > 0)
                                    {
                                        ppsBase.shippingDeliveryDesc = DatosExtra.shipments[0].shippingDeliveryDesc;
                                        ppsBase.shippingPaymentImport = DatosExtra.shipments[0].shippingPaymentImport;
                                        ppsBase.shippingPaymentInstallments = DatosExtra.shipments[0].shippingPaymentInstallments;
                                        ppsBase.shippingItemCategory = DatosExtra.shipments[0].Items[0].shippingItemCategory;
                                        ppsBase.shippingItemId = DatosExtra.shipments[0].Items[0].shippingItemId;
                                        ppsBase.shippingItemName = DatosExtra.shipments[0].Items[0].shippingItemName;
                                        ppsBase.ShippingItemTotal = DatosExtra.shipments[0].Items[0].ShippingItemTotal;

                                        ppsBase.ShippingFirstName = DatosExtra.shipments[0].shippingFirstName;
                                        ppsBase.ShippingLastName = DatosExtra.shipments[0].shippingLastName;

                                        #region Datos Reverson
                                        ppsBase.FechaReversoAutorizacion = "";
                                        ppsBase.HoraReversoAutorizacion = "";
                                        ppsBase.MontoReverso = "";
                                        ppsBase.IDTransaccionReverso = "";
                                        #endregion
                                    }
                                }
                            }

                            #endregion

                            #region Reverso
                            var reverso = GetDatosRefound(row["OrderReferenceNumber"].ToString());

                            ppsBase.FechaReversoAutorizacion = reverso.FechaReverso;
                            ppsBase.HoraReversoAutorizacion = reverso.HoraReverso;
                            ppsBase.MontoReverso = reverso.MontoRverso;
                            ppsBase.IDTransaccionReverso = reverso.IDReverso;
                            #endregion

                            #region Estatus Shipment
                            var estatusShipment = GetEstatusShipment(row["OrderReferenceNumber"].ToString());

                            ppsBase.TipoAlmacen = estatusShipment.CarrierName;
                            ppsBase.EstatusEnvio = estatusShipment.status;
                            #endregion
                            #endregion

                            if (DatosExtra != null)
                                LstppsBase.Add(ppsBase);
                        }
                    }
                }
                else
                {
                    var LstOmonel = GetProcesadorPagosBase_Omonel(OrderReferenceNumber, "Reverso");

                    foreach (var omonel in LstOmonel)
                    {
                        LstppsBase.Add(omonel);
                    }
                }

                return LstppsBase;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProcesadorPagosBase> GetCreditos(string OrderReferenceNumber)
        {
            DataSet ds = new DataSet();
            List<ProcesadorPagosBase> LstppsBase = new List<ProcesadorPagosBase>();

            try
            {
                ds = GetConcentradoDatosBase(OrderReferenceNumber);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            ProcesadorPagosBase ppsBase = new ProcesadorPagosBase();

                            #region Mapping

                            #region Datos Orden
                            ppsBase.OrderReferenceNumber = row["OrderReferenceNumber"].ToString();          //Orden ID
                            ppsBase.PaymentTransactionID = row["PaymentTransactionID"].ToString();          //Transacción
                            //ppsBase.OrderSaleChannel = row["OrderSaleChannel"].ToString();                  //Canal Compra
                            #endregion

                            #region Tokenizacion
                            ppsBase.Bank = row["Bank"].ToString();                                          //Banco
                            ppsBase.BinCode = row["BinCode"].ToString();
                            ppsBase.MaskCard = row["MaskCard"].ToString().Substring(15);                    // Sufijo hay q recortar
                            ppsBase.TypeOfCard = row["TypeOfCard"].ToString();                              //Tipo Tarjeta
                            ppsBase.PaymentMethod = row["PaymentMethod"].ToString();                        //Marca
                            ppsBase.CustomerFirstName = row["CustomerFirstName"].ToString();
                            ppsBase.CustomerLastName = row["CustomerLastName"].ToString();
                            #endregion

                            #region shipping
                            ppsBase.ShippingStoreId = row["shippingStoreId"].ToString();                    //Tipo ALmacen
                            ppsBase.ShippingReferenceNumber = row["ShippingReferenceNumber"].ToString();    //Consignación ID

                            if (row["ShippingReferenceNumber"].ToString() == "001-1")
                            {
                                ppsBase.ShippingDeliveryDesc = "SETC";
                                ppsBase.Catalogo = "SETC";
                                ppsBase.AffiliationType = "8655759";
                                ppsBase.Adquirente = "GETNET";
                            }

                            else
                            {
                                ppsBase.ShippingDeliveryDesc = "MG";
                                ppsBase.Catalogo = "MG";
                                ppsBase.AffiliationType = "1045441";
                                ppsBase.Adquirente = "EVO Payment";
                            }
                            #endregion

                            #region Lealtad
                            ppsBase.CustomerLoyaltyRedeemPoints = row["CustomerLoyaltyRedeemPoints"].ToString(); //punto aplicados
                            ppsBase.CustomerLoyaltyRedeemMoney = row["CustomerLoyaltyRedeemMoney"].ToString();  //efectivo disponble /dinero en efectivo                              
                            ppsBase.CustomerLoyaltyCardId = row["CustomerLoyaltyCardId"].ToString();
                            #endregion

                            #region GetTrace
                            var DatosExtra = GetTracePayment(ppsBase.OrderReferenceNumber, ppsBase.Catalogo);

                            if (DatosExtra != null)
                            {
                                if (DatosExtra.orderReferenceNumber != null)
                                {
                                    string FechaOrden = DatosExtra.orderDateTime;

                                    ppsBase.OrderDate = FechaOrden;
                                    ppsBase.orderAmount = DatosExtra.orderAmount;
                                    ppsBase.TransactionReferenceID = DatosExtra.TransactionReferenceID;
                                    ppsBase.Apply3DS = DatosExtra.Apply3DS;
                                    ppsBase.MerchandiseType = DatosExtra.MerchandiseType;
                                    ppsBase.TransactionStatus = DatosExtra.TransactionStatus;
                                    ppsBase.clientEmail = DatosExtra.customerEmail;

                                    if (DatosExtra.paymentType == "WALLET")
                                    {
                                        ppsBase.paymentTypeJson = "PAYPAL";
                                    }
                                    else
                                    {
                                        bool flagOmonel = DatosExtra.paymentToken.Contains("OMONEL");

                                        if (DatosExtra.paymentToken.Contains("OMONEL"))
                                        {
                                            ppsBase.paymentTypeJson = "OMONEL";
                                            ppsBase.Adquirente = "OMONEL";
                                        }
                                        else
                                        {
                                            ppsBase.paymentTypeJson = DatosExtra.paymentType;
                                            ppsBase.Adquirente = "EVO PAYMENT";
                                        }
                                    }

                                    if (DatosExtra.shipments.Count > 0)
                                    {
                                        ppsBase.shippingDeliveryDesc = DatosExtra.shipments[0].shippingDeliveryDesc;
                                        ppsBase.shippingPaymentImport = DatosExtra.shipments[0].shippingPaymentImport;
                                        ppsBase.shippingPaymentInstallments = DatosExtra.shipments[0].shippingPaymentInstallments;
                                        //ppsBase.shippingItemCategory = DatosExtra.shipments[0].Items[0].shippingItemCategory;
                                        //ppsBase.shippingItemId = DatosExtra.shipments[0].Items[0].shippingItemId;
                                        //ppsBase.shippingItemName = DatosExtra.shipments[0].Items[0].shippingItemName;
                                        ppsBase.ShippingItemTotal = DatosExtra.shipments[0].Items[0].ShippingItemTotal;
                                        ppsBase.ShippingFirstName = DatosExtra.shipments[0].shippingFirstName;
                                        ppsBase.ShippingLastName = DatosExtra.shipments[0].shippingLastName;
                                    }
                                }
                            }
                            #endregion

                            #region Cancellation
                            decimal TotalPrice = 0;
                            decimal TotalPiezas = 0;
                            string Consignacion = string.Empty;
                            string productName = string.Empty;

                            var Cancelacion = GetCancelDevolucion(ppsBase.OrderReferenceNumber);

                            if (Cancelacion.Cancelacion.OrderId != "" || Cancelacion.Devolucion.OrderId != "")
                            {
                                var Productos = GetDatosArticuloCancelDev(ppsBase.OrderReferenceNumber);

                                foreach (var prod in Productos)
                                {
                                    TotalPrice = decimal.Parse(prod.Price) + TotalPrice;
                                    Consignacion = prod.ProductId + ", " + Consignacion;
                                    TotalPiezas = TotalPiezas + 1;
                                    productName = productName + ", " + prod.ProductName;
                                }

                                ppsBase.NombreCancelacion = ppsBase.ShippingFirstName + " " + ppsBase.CustomerLastName;
                                ppsBase.Motivo = Cancelacion.Cancelacion.cancellationReason.Trim();

                                if (Cancelacion.Cancelacion.fec_movto == "")
                                {
                                    ppsBase.FechaCancel = "";
                                    ppsBase.HoraCancel = "";
                                }
                                else
                                {
                                    var fechaCancel = Cancelacion.Cancelacion.fec_movto;
                                    DateTime FecCancel = DateTime.Parse(fechaCancel);
                                    ppsBase.FechaCancel = FecCancel.ToString("MMMM");
                                    ppsBase.HoraCancel = fechaCancel.Substring(10);
                                }

                                ppsBase.MontoCancel = TotalPrice.ToString();
                                ppsBase.ConsignacionIDCancelada = Consignacion;
                                ppsBase.NoPiezasConsignacionCancelacion = TotalPiezas.ToString();
                                ppsBase.FechaINgresoRMA = Cancelacion.Cancelacion.fec_movto;
                                ppsBase.MontoConsignacionIDCancelada = TotalPrice.ToString();

                                ppsBase.ConsignaciónIDDevolucin = Consignacion;
                                ppsBase.DetalleConsignacionIngresada = productName;
                                ppsBase.NoPzasConsignacionDevolucion = TotalPiezas.ToString();

                                if (Cancelacion.Devolucion.fec_movto == "")
                                {
                                    ppsBase.FechaDevolucion = "";
                                    ppsBase.HoraDevolucion = "";
                                }
                                else
                                {
                                    var fechaDevolucion = Cancelacion.Devolucion.fec_movto;
                                    DateTime FecDev = DateTime.Parse(fechaDevolucion);

                                    ppsBase.FechaDevolucion = FecDev.ToString("MMMM");
                                    ppsBase.HoraDevolucion = fechaDevolucion.Substring(10);
                                }

                                ppsBase.FechaDevolucion = Cancelacion.Devolucion.fec_movto;
                                ppsBase.MontoDevolucionConsignacion = TotalPrice.ToString();

                                ppsBase.FechaReembolso = Cancelacion.Devolucion.fec_movto;
                                ppsBase.HoraReembolso = "";
                                ppsBase.FormaPagoRembolso = "";
                                ppsBase.Bin_Reembolso = row["BinCode"].ToString();
                                ppsBase.SufijoReembolso = row["MaskCard"].ToString().Substring(15);
                                ppsBase.ReembolsoAutomatico = "True";
                                ppsBase.ReembolsoManual = "";
                                ppsBase.IDTransaccionReembolso = DatosExtra.TransactionReferenceID; ;
                            }
                            #endregion

                            #region ApprovalCode
                            var ApprovalCode = BL_GetApprovalCode(row["OrderReferenceNumber"].ToString());
                            ppsBase.TransactionAuthorizationId = ApprovalCode;
                            #endregion

                            #region OUE
                            var oue = BL_Ordenes_APP(row["OrderReferenceNumber"].ToString());

                            if (oue.Id_Num_Apl == "22" || oue.Id_Num_Apl == "")
                            {
                                if (row["OrderSaleChannel"].ToString() == "1")
                                    ppsBase.OrderSaleChannel = "SFWEB";
                                else
                                    ppsBase.OrderSaleChannel = row["OrderSaleChannel"].ToString();                  //Canal Compra
                            }
                            else
                            {
                                ppsBase.OrderSaleChannel = "APP";                 //Canal Compra
                                ppsBase.TipoMobile = oue.CreatedBy;
                            }


                            ppsBase.DeliveryType = oue.DeliveryType;
                            #endregion

                            #region Estatus Shipment
                            var estatusShipment = GetEstatusShipment(row["OrderReferenceNumber"].ToString());

                            ppsBase.TipoAlmacen = estatusShipment.CarrierName;
                            ppsBase.EstatusEnvio = estatusShipment.status;
                            #endregion
                            #endregion

                            LstppsBase.Add(ppsBase);
                        }
                    }
                }
                else
                {
                    var LstOmonel = GetProcesadorPagosBase_Omonel(OrderReferenceNumber, "Creditos");

                    foreach (var omonel in LstOmonel)
                    {
                        LstppsBase.Add(omonel);
                    }
                }

                return LstppsBase;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private List<ProcesadorPagosBase> GetProcesadorPagosBase_Omonel(string OrderReferenceNumber, string Method)
        {
            List<ProcesadorPagosBase> LstppsBase = new List<ProcesadorPagosBase>();
            DataSet ds = new DataSet();

            string FechaOrden = string.Empty;
            string HoraOrden = string.Empty;

            string spName = string.Empty;

            var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_PDP"].ToString();
            //var _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioPDPProdDB;Persist Security Info=False;User ID=ProcesadorPago;Password=W3rcur10PDP!#$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            spName = "up_PPS_sel_PaymentTransactionOmonelRpt_byOrder";

            try
            {
                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderReferenceNumber", SqlDbType.VarChar);

                        param.Value = OrderReferenceNumber;

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);
                    }
                }

                #region Mapeo
                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ProcesadorPagosBase ppsBase = new ProcesadorPagosBase();

                        #region Datos Orden
                        ppsBase.PaymentToken = row["PaymentToken"].ToString();
                        ppsBase.OrderReferenceNumber = row["OrderReferenceNumber"].ToString();          //Orden ID
                        #endregion

                        #region Tokenizacion
                        ppsBase.Bank = row["Bank"].ToString();                                          //Banco
                        ppsBase.BinCode = row["BinCode"].ToString();
                        ppsBase.MaskCard = row["MaskCard"].ToString().Substring(15);                    // Sufijo hay q recortar
                        ppsBase.TypeOfCard = row["TypeOfCard"].ToString();                              //Tipo Tarjeta
                        ppsBase.PaymentMethod = row["PaymentMethod"].ToString();                        //Marca
                        ppsBase.CustomerFirstName = row["CustomerFirstName"].ToString();
                        ppsBase.CustomerLastName = row["CustomerLastName"].ToString();
                        #endregion

                        #region Lealtad
                        ppsBase.CustomerLoyaltyRedeemPoints = row["CustomerLoyaltyRedeemPoints"].ToString(); //punto aplicados
                        ppsBase.CustomerLoyaltyRedeemMoney = row["CustomerLoyaltyRedeemMoney"].ToString();  //efectivo disponble /dinero en efectivo                              
                        ppsBase.CustomerLoyaltyCardId = row["CustomerLoyaltyCardId"].ToString();
                        //ppsBase.MethodPaymentShipment = row["MethodPaymentShipment"].ToString();
                        #endregion

                        #region Cancellation
                        if (Method == "Creditos")
                        {
                            decimal TotalPrice = 0;
                            decimal TotalPiezas = 0;
                            string Consignacion = string.Empty;

                            var Cancelacion = GetCancelDevolucion(ppsBase.OrderReferenceNumber);
                            var Productos = GetDatosArticuloCancelDev(ppsBase.OrderReferenceNumber);

                            foreach (var prod in Productos)
                            {
                                TotalPrice = decimal.Parse(prod.Price) + TotalPrice;
                                Consignacion = prod.ProductId + ", " + Consignacion;
                                TotalPiezas = TotalPiezas + 1;
                            }

                            ppsBase.NombreCancelacion = ppsBase.ShippingFirstName + " " + ppsBase.CustomerLastName;
                            ppsBase.Motivo = Cancelacion.Cancelacion.cancellationReason.Trim();

                            if (Cancelacion.Cancelacion.fec_movto == "")
                            {
                                ppsBase.FechaCancel = "";
                                ppsBase.HoraCancel = "";
                            }
                            else
                            {
                                var fechaCancel = Cancelacion.Cancelacion.fec_movto;
                                DateTime FecCancel = DateTime.Parse(fechaCancel);
                                ppsBase.FechaCancel = FecCancel.ToString("MMMM");
                                ppsBase.HoraCancel = fechaCancel.Substring(10);
                            }

                            ppsBase.MontoCancel = TotalPrice.ToString();
                            ppsBase.ConsignacionIDCancelada = Consignacion;
                            ppsBase.NoPiezasConsignacionCancelacion = TotalPiezas.ToString();
                            ppsBase.FechaINgresoRMA = Cancelacion.Cancelacion.fec_movto;

                            ppsBase.ConsignaciónIDDevolucin = Consignacion;
                            ppsBase.DetalleConsignacionIngresada = "";
                            ppsBase.NoPzasConsignacionDevolucion = TotalPiezas.ToString();

                            if (Cancelacion.Devolucion.fec_movto == "")
                            {
                                ppsBase.FechaDevolucion = "";
                                ppsBase.HoraDevolucion = "";
                            }
                            else
                            {
                                var fechaDevolucion = Cancelacion.Devolucion.fec_movto;
                                DateTime FecDev = DateTime.Parse(fechaDevolucion);

                                ppsBase.FechaDevolucion = FecDev.ToString("MMMM");
                                ppsBase.HoraDevolucion = fechaDevolucion.Substring(10);
                            }


                            ppsBase.FechaDevolucion = Cancelacion.Devolucion.fec_movto;

                            ppsBase.MontoDevolucionConsignacion = TotalPrice.ToString();

                            ppsBase.FechaReembolso = "";
                            ppsBase.HoraReembolso = "";
                            ppsBase.FormaPagoRembolso = "";
                            ppsBase.Bin_Reembolso = row["BinCode"].ToString();
                            ppsBase.SufijoReembolso = row["MaskCard"].ToString().Substring(15);
                            ppsBase.ReembolsoAutomatico = "";
                            ppsBase.ReembolsoManual = "";
                            ppsBase.IDTransaccionReembolso = "";
                        }
                        #endregion

                        #region Emisor
                        if (Method == "AuthBancaria")
                        {
                            var EmisorResponse = GetDatosEmisor(ppsBase.OrderReferenceNumber);

                            ppsBase.DecisionEmisor = EmisorResponse.DecisionEmisor;
                            ppsBase.CveReespuestaEmisor = EmisorResponse.CveReespuestaEmisor;
                            ppsBase.DescReespuestaEmisor = EmisorResponse.DescReespuestaEmisor;

                            if (EmisorResponse.DecisionEmisor == "AUTHORIZED")
                                ppsBase.DescReespuestaEmisor = "AUTHORIZED";
                        }
                        #endregion

                        #region Reverso
                        var reverso = GetDatosRefound(row["OrderReferenceNumber"].ToString());

                        ppsBase.FechaReversoAutorizacion = reverso.FechaReverso;
                        ppsBase.HoraReversoAutorizacion = reverso.HoraReverso;
                        ppsBase.MontoReverso = reverso.MontoRverso;
                        ppsBase.IDTransaccionReverso = reverso.IDReverso;
                        #endregion

                        #region ApprovalCode
                        var ApprovalCode = BL_GetApprovalCode(row["OrderReferenceNumber"].ToString());
                        ppsBase.TransactionAuthorizationId = ApprovalCode;
                        #endregion

                        #region OUE
                        var oue = BL_Ordenes_APP(row["OrderReferenceNumber"].ToString());

                        if (oue.Id_Num_Apl == "22" || oue.Id_Num_Apl == "")
                        {
                            if (row["OrderSaleChannel"].ToString() == "1")
                                ppsBase.OrderSaleChannel = "SFWEB";
                            else
                                ppsBase.OrderSaleChannel = row["OrderSaleChannel"].ToString();                  //Canal Compra
                        }
                        else
                        {
                            ppsBase.OrderSaleChannel = "APP";                 //Canal Compra
                            ppsBase.TipoMobile = oue.CreatedBy;
                        }

                        ppsBase.DeliveryType = oue.DeliveryType;
                        #endregion

                        #region Omonel Transaction
                        var OmonelResponse = Get_Omonel_Aut(row["OrderReferenceNumber"].ToString());

                        ppsBase.TransactionAuthorizationId = OmonelResponse.Cve_Autz;

                        if (OmonelResponse.Cve_Autz == "000000")
                        {
                            ppsBase.TransactionStatus = "DECLINED";
                        }
                        else
                        {
                            ppsBase.TransactionStatus = "AUTHORIZED";
                        }
                        #endregion

                        #region GetTrace Omonel
                        var datosExtra = GetTracePaymentOmonel(ppsBase.OrderReferenceNumber);

                        if (datosExtra.orderReferenceNumber != null)
                        {
                            if (Method == "Creditos")
                            {
                                ppsBase.OrderDate = FechaOrden;
                                ppsBase.OrderHour = HoraOrden;
                            }
                            else
                            {
                                if (oue.Id_Num_Apl == "22")
                                {
                                    FechaOrden = datosExtra.orderDateTime.Substring(0, 10);
                                    HoraOrden = datosExtra.orderDateTime.Substring(11, 5);

                                    ppsBase.OrderDate = FechaOrden;
                                    ppsBase.OrderHour = HoraOrden;
                                }
                                else
                                {
                                    FechaOrden = datosExtra.orderDateTime.Substring(0, 8);
                                    HoraOrden = datosExtra.orderDateTime.Substring(9, 5);

                                    ppsBase.OrderDate = FechaOrden;
                                    ppsBase.OrderHour = HoraOrden;
                                }
                            }

                            ppsBase.TransactionReferenceID = datosExtra.TransactionReferenceID;
                            ppsBase.AffiliationType = datosExtra.AffiliationType;
                            ppsBase.IsAuthenticated = datosExtra.IsAuthenticated;
                            ppsBase.IsAuthorized = datosExtra.IsAuthorized;
                            ppsBase.Apply3DS = datosExtra.Apply3DS;
                            ppsBase.MerchandiseType = datosExtra.MerchandiseType;
                            //ppsBase.TransactionAuthorizationId = datosExtra.TransactionAuthorizationId;
                            ppsBase.clientEmail = datosExtra.customerEmail;
                            ppsBase.PaymentTransactionService = datosExtra.TransactionStatus;


                            if (datosExtra.paymentType == "WALLET")
                            {
                                ppsBase.paymentTypeJson = "PAYPAL";
                            }
                            else
                            {
                                bool flagOmonel = datosExtra.paymentToken.Contains("OMONEL");

                                if (datosExtra.paymentToken.Contains("OMONEL"))
                                {
                                    ppsBase.paymentTypeJson = "OMONEL";
                                    ppsBase.Adquirente = "OMONEL";
                                }

                                else
                                {
                                    ppsBase.Adquirente = "EVO PAYMENT";
                                    ppsBase.paymentTypeJson = datosExtra.paymentType;
                                }
                            }

                            if (datosExtra.shipments.Count > 0)
                            {
                                foreach (var ship in datosExtra.shipments)
                                {
                                    ppsBase.orderAmount = ship.shippingPaymentImport;
                                    ppsBase.shippingDeliveryDesc = ship.shippingDeliveryDesc;
                                    ppsBase.shippingPaymentImport = ship.shippingPaymentImport;
                                    ppsBase.ShippingFirstName = ship.shippingFirstName;
                                    ppsBase.ShippingLastName = ship.shippingLastName;
                                    ppsBase.shippingPaymentInstallments = ship.shippingPaymentInstallments;

                                    #region Datos Liquidacion
                                    ppsBase.FechaLiquidacion = FechaOrden;
                                    ppsBase.HoraLiquidacion = HoraOrden;
                                    ppsBase.MontoLiquidacion = datosExtra.shipments[0].shippingPaymentImport;
                                    ppsBase.LiquidacionManual = "";
                                    ppsBase.LiquidacionAutomatica = "True";
                                    ppsBase.IDTransaccionLiquidacion = datosExtra.TransactionReferenceID; ;
                                    #endregion

                                    foreach (var items in ship.Items)
                                    {
                                        ppsBase.shippingItemCategory = items.shippingItemCategory;
                                        ppsBase.shippingItemId = items.shippingItemId;
                                        ppsBase.shippingItemName = items.shippingItemName;
                                        ppsBase.ShippingItemTotal = items.ShippingItemTotal;
                                    }

                                    if (ship.shippingReferenceNumber == "001-1")
                                    {
                                        ppsBase.ShippingDeliveryDesc = "SETC";
                                        ppsBase.Catalogo = "SETC";
                                        ppsBase.AffiliationType = "8655759";
                                        ppsBase.Adquirente = "GETNET";

                                        LstppsBase.Add(ppsBase);
                                    }
                                    else
                                    {
                                        ppsBase.ShippingDeliveryDesc = "MG";
                                        ppsBase.Catalogo = "MG";
                                        ppsBase.AffiliationType = "1045441";
                                        ppsBase.Adquirente = "EVO Payment";

                                        LstppsBase.Add(ppsBase);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Estatus Shipment
                        var estatusShipment = GetEstatusShipment(row["OrderReferenceNumber"].ToString());

                        ppsBase.TipoAlmacen = estatusShipment.CarrierName;
                        ppsBase.EstatusEnvio = estatusShipment.status;
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return LstppsBase;
        }
        #endregion

        #region Metodos Obtener Datos     
        private DataSet GetConcentradoDatosBase(string OrderReferenceNumber)
        {
            try
            {
                DataSet ds = new DataSet();

                string spName = string.Empty;

                var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_PDP"].ToString(); //Environment.GetEnvironmentVariable("ConnectionStrings:MercurioDB");
                //var _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioPDPProdDB;Persist Security Info=False;User ID=ProcesadorPago;Password=W3rcur10PDP!#$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


                spName = "up_PPS_sel_PaymentTransactionRpt_byOrder";

                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderReferenceNumber", SqlDbType.VarChar);
                        param.Value = OrderReferenceNumber;

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);
                    }
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private JsonRespoonseModel GetTracePayment(string OrderReferenceNumber, string Catalogo)
        {
            string ceros = "";
            DataSet ds = new DataSet();
            string JsonRequest = string.Empty;
            string JsonRequest_SETC = string.Empty;
            string JsonReques_MG = string.Empty;
            JsonRespoonseModel Response = new JsonRespoonseModel();

            var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_PDP"].ToString(); //Environment.GetEnvironmentVariable("ConnectionStrings:MercurioDB");
            //var _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioPDPProdDB;Persist Security Info=False;User ID=ProcesadorPago;Password=W3rcur10PDP!#$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Application Name=PaymentOrderProcess;Min Pool Size=0;Max Pool Size=5;Pooling=true;";

            try
            {
                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("[up_PPS_sel_Trace_payments_Prov]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderReferenceNumber", SqlDbType.VarChar);
                        param.Value = OrderReferenceNumber;

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);
                    }
                }

                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        JsonRequest = row["RequestJson"].ToString();
                        Response = JsonConvert.DeserializeObject<JsonRespoonseModel>(JsonRequest);

                        foreach (var sh in Response.shipments)
                        {
                            if (sh.shippingReferenceNumber == "001-1")
                            {
                                JsonRequest_SETC = JsonRequest;
                            }
                            else
                            {
                                JsonReques_MG = JsonRequest;
                            }
                        }
                        JsonRequest = string.Empty;
                    }
                }

                if (JsonRequest_SETC != "" || JsonReques_MG != "")
                {
                    if (Catalogo == "SETC")
                        Response = JsonConvert.DeserializeObject<JsonRespoonseModel>(JsonRequest_SETC);
                    else if (Catalogo == "MG")
                        Response = JsonConvert.DeserializeObject<JsonRespoonseModel>(JsonReques_MG);

                    shipments shipment = new shipments();
                    items item = new items();
                    List<items> lstItems = new List<items>();

                    if (Response != null)
                    {
                        foreach (var row in Response.shipments)
                        {
                            shipment.shippingDeliveryDesc = row.shippingDeliveryDesc;
                            shipment.shippingPaymentImport = row.shippingPaymentImport;
                            shipment.shippingPaymentInstallments = row.shippingPaymentInstallments;
                            shipment.shippingFirstName = row.shippingFirstName;
                            shipment.shippingLastName = row.shippingLastName;
                            foreach (var row2 in row.Items)
                            {
                                item.shippingItemCategory = row2.shippingItemCategory;
                                if (row2.shippingItemCategory == "Costo de envio")
                                {
                                    item.shippingItemId = row2.shippingItemId;
                                    item.shippingItemName = row2.shippingItemName;
                                    item.ShippingItemTotal = row2.ShippingItemTotal;

                                    lstItems.Add(item);

                                    shipment.Items = lstItems;
                                }
                                else
                                {
                                    item.shippingItemId = "";
                                    item.shippingItemName = "";
                                    item.ShippingItemTotal = "";

                                    lstItems.Add(item);

                                    shipment.Items = lstItems;
                                }
                            }
                        }
                        Response.shipments.Clear();
                        Response.shipments.Add(shipment);
                    }

                }

                return Response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<PaymentStoreModelResponse> GetPaymentStore()
        {
            DataSet ds = new DataSet();
            List<ProcesadorPagosBase> LstppsBase = new List<ProcesadorPagosBase>();
            List<PaymentStoreModel> lstPaymentStore = new List<PaymentStoreModel>();
            List<PaymentStoreModelResponse> lstPaymentStoreResponse = new List<PaymentStoreModelResponse>();

            string spName = string.Empty;
            string JsonResponse = string.Empty;
            string paymentTypeJson = string.Empty;
            string ShippingDeliveryDesc = string.Empty;
            string Catalogo = string.Empty;
            string AffiliationType = string.Empty;
            string CostoEnvtio = string.Empty;
            string Banco = string.Empty;
            string BIN = string.Empty;
            string Sufijo = string.Empty;
            string TipoTarjeta = string.Empty;
            string Marca = string.Empty;
            string paymentToken = string.Empty;

            //var _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioPDPProdDB;Persist Security Info=False;User ID=ProcesadorPago;Password=W3rcur10PDP!#$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_PDP"].ToString(); //Environment.GetEnvironmentVariable("ConnectionStrings:MercurioDB");


            spName = "up_PPS_sel_PaymentStoreRpt";

            try
            {
                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);

                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                PaymentStoreModel payment = new PaymentStoreModel();

                                payment.OrderReferenceNumber = row["OrderReferenceNumber"].ToString();
                                payment.OrderAmount = row["OrderAmount"].ToString();
                                payment.LineaCaptura = row["LineaCaptura"].ToString();
                                payment.Estatus = row["Estatus"].ToString();
                                payment.CreatedDate = row["CreatedDate"].ToString();

                                lstPaymentStore.Add(payment);
                            }
                        }
                    }
                }

                foreach (var payment in lstPaymentStore)
                {
                    ds = new DataSet();

                    using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                    {
                        using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("up_PPS_sel_PaymentStorePagadaRpt", cnn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter param;
                            param = cmd.Parameters.Add("@barcode", SqlDbType.VarChar);
                            param.Value = payment.LineaCaptura;

                            using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                                dataAdapter.Fill(ds);
                        }
                    }

                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            JsonResponse = row["RequestJson"].ToString();

                            if (JsonResponse != "")
                            {
                                var RequestPaymentStore = JsonConvert.DeserializeObject<JsonRespoonseModel>(JsonResponse);

                                if (RequestPaymentStore.paymentType == "INSTORE")
                                {
                                    #region Mapeo
                                    PaymentStoreModelResponse PaymentStore = new PaymentStoreModelResponse();
                                    paymentToken = RequestPaymentStore.paymentToken;
                                    string FechaOrden = RequestPaymentStore.orderDateTime.Substring(0, 10);
                                    string HoraOrden = RequestPaymentStore.orderDateTime.Substring(11, 5);

                                    if (RequestPaymentStore.paymentType == "WALLET")
                                    {
                                        paymentTypeJson = "PAYPAL";
                                    }
                                    else
                                    {
                                        bool flagOmonel = RequestPaymentStore.paymentToken.Contains("OMONEL");

                                        if (RequestPaymentStore.paymentToken.Contains("OMONEL"))
                                            paymentTypeJson = "OMONEL";
                                        else
                                            paymentTypeJson = RequestPaymentStore.paymentType;
                                    }

                                    if (RequestPaymentStore.shipments[0].shippingReferenceNumber == "001-1")
                                    {
                                        ShippingDeliveryDesc = "SETC";
                                        Catalogo = "SETC";
                                        AffiliationType = "8655759";
                                    }

                                    else
                                    {
                                        ShippingDeliveryDesc = "MG";
                                        Catalogo = "MG";
                                        AffiliationType = "";
                                    }


                                    DataSet ds1 = new DataSet();

                                    if (paymentToken != "")
                                    {
                                        using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                                        {
                                            using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                                            {
                                                spName = "up_PPS_sel_CardDetailRpt";

                                                cmd.CommandType = CommandType.StoredProcedure;

                                                System.Data.SqlClient.SqlParameter param;
                                                param = cmd.Parameters.Add("@TypeCard", SqlDbType.VarChar);
                                                param.Value = paymentTypeJson;

                                                System.Data.SqlClient.SqlParameter param2;
                                                param2 = cmd.Parameters.Add("@ClientToken", SqlDbType.VarChar);
                                                param2.Value = paymentToken;

                                                using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                                                    dataAdapter.Fill(ds1);
                                            }
                                        }

                                        foreach (DataTable dt1 in ds1.Tables)
                                        {
                                            foreach (DataRow row1 in dt.Rows)
                                            {
                                                Banco = row["Bank"].ToString();
                                                BIN = row["BinCode"].ToString();
                                                Sufijo = row["MaskCard"].ToString();
                                                TipoTarjeta = row["TypeOfCard"].ToString();
                                                Marca = row["PaymentMethod"].ToString();
                                            }
                                        }
                                    }

                                    #region Mapeo
                                    PaymentStore.OrdenID = payment.OrderReferenceNumber;
                                    PaymentStore.IDtransaccion = RequestPaymentStore.TransactionReferenceID;
                                    PaymentStore.FechaCreacion = FechaOrden;
                                    PaymentStore.HoraCreacion = HoraOrden;
                                    PaymentStore.NoAfiliacion = AffiliationType;
                                    PaymentStore.Adquirente = "";
                                    PaymentStore.Catalogo = Catalogo;
                                    PaymentStore.TipoEntrega = ShippingDeliveryDesc;
                                    PaymentStore.CanalCompra = RequestPaymentStore.orderSaleChannel;
                                    //PaymentStore.FormaPago = paymentTypeJson;
                                    PaymentStore.NoTienda = RequestPaymentStore.shipments[0].shippingStoreId;
                                    PaymentStore.NombreTienda = "";
                                    PaymentStore.noCajero = "";

                                    PaymentStore.montoPagado = payment.OrderAmount;
                                    PaymentStore.precioTotalOrden = RequestPaymentStore.orderAmount;
                                    PaymentStore.fechaPago = payment.CreatedDate.Substring(0, 10); ;
                                    //PaymentStore.formaPago = "";
                                    PaymentStore.Banco = Banco;
                                    PaymentStore.NoAutorizacion = RequestPaymentStore.TransactionAuthorizationId;
                                    PaymentStore.BIN = BIN;
                                    PaymentStore.Sufijo = Sufijo;
                                    PaymentStore.TipoTarjeta = TipoTarjeta;
                                    PaymentStore.Marca = Marca;

                                    foreach (var sh in RequestPaymentStore.shipments)
                                    {
                                        //shipment.shippingDeliveryDesc = row.shippingDeliveryDesc;
                                        //shipment.shippingPaymentImport = row.shippingPaymentImport;
                                        //shipment.shippingPaymentInstallments = row.shippingPaymentInstallments;
                                        //shipment.shippingFirstName = row.shippingFirstName;
                                        //shipment.shippingLastName = row.shippingLastName;
                                        foreach (var row2 in sh.Items)
                                        {
                                            //item.shippingItemCategory = row2.shippingItemCategory;
                                            if (row2.shippingItemCategory == "Costo de envio")
                                            {
                                                //item.shippingItemId = row2.shippingItemId;
                                                //item.shippingItemName = row2.shippingItemName;
                                                PaymentStore.CostoEnvio = row2.ShippingItemTotal;
                                                break;
                                            }
                                            else
                                            {
                                                PaymentStore.CostoEnvio = row2.ShippingItemTotal = "";
                                            }
                                        }

                                    }

                                    PaymentStore.formato = "";
                                    PaymentStore.ciudadEstatusPago = RequestPaymentStore.customerCity;
                                    PaymentStore.MSI = RequestPaymentStore.shipments[0].shippingPaymentInstallments;
                                    PaymentStore.PuntosAplicados = RequestPaymentStore.customerLoyaltyRedeemPoints;
                                    PaymentStore.PromocionesAplicadas = "";
                                    PaymentStore.NombrePersonaRegistrada = RequestPaymentStore.shipments[0].shippingFirstName;
                                    PaymentStore.Apellido_P = RequestPaymentStore.shipments[0].shippingLastName; ;
                                    PaymentStore.Apellido_M = "";
                                    PaymentStore.NoTarjetalealtad = RequestPaymentStore.customerLoyaltyCardId;
                                    PaymentStore.Correo = RequestPaymentStore.customerEmail;
                                    PaymentStore.EstatusOrden = payment.Estatus;
                                    PaymentStore.EstatusEnvío = "";
                                    PaymentStore.almacenSurtio = "";
                                    PaymentStore.loyalty = RequestPaymentStore.customerLoyaltyCardId;

                                    PaymentStore.CreteOrderStore = payment.CreatedDate.Substring(0, 10);
                                    PaymentStore.HoraOrderStore = payment.CreatedDate.Substring(11, 5);

                                    if (RequestPaymentStore.paymentType == "WALLET")
                                    {
                                        PaymentStore.FormaPago = "PAYPAL";
                                    }
                                    else
                                    {
                                        bool flagOmonel = RequestPaymentStore.paymentToken.Contains("OMONEL");

                                        if (RequestPaymentStore.paymentToken.Contains("OMONEL"))
                                        {
                                            PaymentStore.FormaPago = "OMONEL";
                                            PaymentStore.Adquirente = "OMONEL";
                                        }
                                        else
                                        {
                                            PaymentStore.FormaPago = RequestPaymentStore.paymentType;
                                            PaymentStore.Adquirente = "EVO PAYMENT";
                                        }

                                    }
                                    #endregion

                                    lstPaymentStoreResponse.Add(PaymentStore);
                                    #endregion
                                }
                            }
                        }
                    }
                }

                return lstPaymentStoreResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<ParticipacionFormasPagoModel> GetParticipacionFormasPago(string FecIni, string FecFin)
        {
            ParticipacionFormasPagoModel Response = new ParticipacionFormasPagoModel();
            List<ParticipacionFormasPagoModel> lstFormasPago = new List<ParticipacionFormasPagoModel>();
            DataSet ds = new DataSet();
            string spName = string.Empty;

            var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_DEV"].ToString();
            //var _ConnectionString = "Server=tcp:srvsqlmercurioqa.database.windows.net,1433;Initial Catalog=MercurioQaDB;Persist Security Info=False;User ID=t_eliseogr;Password=W3rcur10!QA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            spName = "up_PPS_Sel_Totales_MetodosPago";

            try
            {
                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        if (FecIni != "" && FecFin != "")
                        {
                            System.Data.SqlClient.SqlParameter param;
                            param = cmd.Parameters.Add("@fechaini", SqlDbType.VarChar);
                            param.Value = FecIni;

                            System.Data.SqlClient.SqlParameter param2;
                            param2 = cmd.Parameters.Add("@fechafin", SqlDbType.VarChar);
                            param2.Value = FecFin;
                        }

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);

                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                ParticipacionFormasPagoModel FormaPago = new ParticipacionFormasPagoModel
                                {
                                    metodoPago = row["MethodPayment"].ToString(),
                                    monto = row["Total"].ToString(),
                                    Catalogo = row["UeType"].ToString(),
                                    clientesCantidad = "",
                                    ordenesCantidad = row["CantOrder"].ToString(),
                                    fecha = "",
                                    mes = ""
                                };

                                lstFormasPago.Add(FormaPago);
                            }
                        }
                    }
                }

                return lstFormasPago;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<AprobacionesMarcas> GetAprobacionesMarcas()
        {
            #region Definiciones
            DataSet ds = new DataSet();
            List<Aprobaciones> LstAprobaciones = new List<Aprobaciones>();
            List<AprobacionesMarcas> LstaprobacionesMarcas = new List<AprobacionesMarcas>();
            string spName = string.Empty;
            int contWebCard = 0;
            decimal MontoWebCard = 0;
            int contWebPOD = 0;
            decimal MontoWebPOD = 0;
            int contWebPIS = 0;
            decimal MontoWebPIS = 0;
            int contWebFa = 0;
            decimal MontoWebFa = 0;

            int contAppCard = 0;
            decimal MontoAppCard = 0;
            int contAppPOD = 0;
            decimal MontoAppPOD = 0;
            int contAppPIS = 0;
            decimal MontoAppPIS = 0;
            int contAppFa = 0;
            decimal MontoAppFa = 0;
            #endregion

            var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_DEV"].ToString();
            //var _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioDB;Persist Security Info=False;User ID=sorianaprod_mrcr;Password=!#W3rCuR10;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            spName = "up_PPS_Sel_AprobacionesMarcas";

            try
            {
                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);
                    }
                }

                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Aprobaciones aprobaciones = new Aprobaciones();
                        aprobaciones.id_num_apl = row["id_num_apl"].ToString();
                        aprobaciones.id_num_formapago = row["id_num_formapago"].ToString();
                        aprobaciones.Id_Num_Orden = row["Id_Num_Orden"].ToString();
                        aprobaciones.nom_pagOrig = row["nom_pagOrig"].ToString();
                        aprobaciones.tipoOrden = row["tipoOrden"].ToString();
                        aprobaciones.imp_preciounit = row["imp_preciounit"].ToString();

                        LstAprobaciones.Add(aprobaciones);
                    }
                }

                //Mapeo Resultado
                foreach (var row in LstAprobaciones)
                {
                    if (row.id_num_apl == "22")
                    {
                        #region web
                        if (row.id_num_formapago == "1")
                        {
                            contWebCard = contWebCard + 1;
                            MontoWebCard = decimal.Parse(row.imp_preciounit) + MontoWebCard;
                        }

                        if (row.id_num_formapago == "26")
                        {
                            contWebPOD = contWebPOD + 1;
                            MontoWebPOD = decimal.Parse(row.imp_preciounit) + MontoWebPOD;
                        }

                        if (row.id_num_formapago == "4")
                        {
                            contWebPIS = contWebPIS + 1;
                            MontoWebPIS = decimal.Parse(row.imp_preciounit) + MontoWebPIS;
                        }

                        if (row.id_num_formapago == "25")
                        {
                            contWebFa = contWebFa + 1;
                            MontoWebFa = decimal.Parse(row.imp_preciounit) + MontoWebFa;
                        }
                        #endregion
                    }
                    else if ((row.id_num_apl == "23") || (row.id_num_apl == "24"))
                    {
                        #region Movil
                        if (row.id_num_formapago == "1")
                        {
                            contAppCard = contAppCard + 1;
                            MontoAppCard = decimal.Parse(row.imp_preciounit) + MontoAppCard;
                        }

                        if (row.id_num_formapago == "26")
                        {
                            contAppPOD = contAppPOD + 1;
                            MontoAppPOD = decimal.Parse(row.imp_preciounit) + MontoAppPOD;
                        }

                        if (row.id_num_formapago == "4")
                        {
                            contAppPIS = contAppPIS + 1;
                            MontoAppPIS = decimal.Parse(row.imp_preciounit) + MontoAppPIS;
                        }

                        if (row.id_num_formapago == "25")
                        {
                            contAppFa = contAppFa + 1;
                            MontoAppFa = decimal.Parse(row.imp_preciounit) + MontoAppFa;
                        }
                        #endregion
                    }
                }


                AprobacionesMarcas aprobacionesCard = new AprobacionesMarcas
                {
                    canalCompra = "WEB",
                    marca = "CARD",
                    totalOrdenes = contWebCard.ToString(),
                    monto = MontoWebCard.ToString()
                };
                AprobacionesMarcas aprobacionesPOD = new AprobacionesMarcas
                {
                    canalCompra = "WEB",
                    marca = "POD",
                    totalOrdenes = contWebPOD.ToString(),
                    monto = MontoWebPOD.ToString()
                };
                AprobacionesMarcas aprobacionesPIS = new AprobacionesMarcas
                {
                    canalCompra = "WEB",
                    marca = "PIS",
                    totalOrdenes = contWebPIS.ToString(),
                    monto = MontoWebPIS.ToString()
                };
                AprobacionesMarcas aprobacionesFa = new AprobacionesMarcas
                {
                    canalCompra = "WEB",
                    marca = "FALABELLA",
                    totalOrdenes = contWebFa.ToString(),
                    monto = MontoWebFa.ToString()
                };

                AprobacionesMarcas aprobacionesCard_app = new AprobacionesMarcas
                {
                    canalCompra = "APP",
                    marca = "CARD",
                    totalOrdenes = contAppCard.ToString(),
                    monto = MontoAppCard.ToString()
                };
                AprobacionesMarcas aprobacionesPOD_app = new AprobacionesMarcas
                {
                    canalCompra = "APP",
                    marca = "POD",
                    totalOrdenes = contAppPOD.ToString(),
                    monto = MontoAppPOD.ToString()
                };
                AprobacionesMarcas aprobacionesPIS_app = new AprobacionesMarcas
                {
                    canalCompra = "APP",
                    marca = "PIS",
                    totalOrdenes = contAppPIS.ToString(),
                    monto = MontoAppPIS.ToString()
                };
                AprobacionesMarcas aprobacionesFa_app = new AprobacionesMarcas
                {
                    canalCompra = "APP",
                    marca = "FALABELLA",
                    totalOrdenes = contAppFa.ToString(),
                    monto = MontoAppFa.ToString()
                };

                LstaprobacionesMarcas.Add(aprobacionesCard);
                LstaprobacionesMarcas.Add(aprobacionesPOD);
                LstaprobacionesMarcas.Add(aprobacionesPIS);
                LstaprobacionesMarcas.Add(aprobacionesFa);

                LstaprobacionesMarcas.Add(aprobacionesCard_app);
                LstaprobacionesMarcas.Add(aprobacionesPOD_app);
                LstaprobacionesMarcas.Add(aprobacionesPIS_app);
                LstaprobacionesMarcas.Add(aprobacionesFa_app);

                return LstaprobacionesMarcas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Complementos
        private JsonRespoonseModel GetTracePaymentOmonel(string OrderReferenceNumber)
        {
            string ceros = "";
            DataSet ds = new DataSet();
            string JsonRequest = string.Empty;
            string JsonRequest_SETC = string.Empty;
            string JsonReques_MG = string.Empty;
            JsonRespoonseModel Response = new JsonRespoonseModel();
            List<JsonRespoonseModel> LstResponse = new List<JsonRespoonseModel>();

            var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_PDP"].ToString();
            //var _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioPDPProdDB;Persist Security Info=False;User ID=ProcesadorPago;Password=W3rcur10PDP!#$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Application Name=PaymentOrderProcess;Min Pool Size=0;Max Pool Size=5;Pooling=true;";

            try
            {
                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("up_PPS_sel_Trace_payments_Prov_Omonel", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderReferenceNumber", SqlDbType.VarChar);
                        param.Value = OrderReferenceNumber;//OrderReferenceNumber;

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);

                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                JsonRequest = row["RequestJson"].ToString();
                                Response = JsonConvert.DeserializeObject<JsonRespoonseModel>(JsonRequest);
                            }
                        }
                    }
                }

                return Response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ResponseEmisor GetDatosEmisor(string OrderReferenceNumber)
        {
            DataSet ds = new DataSet();
            ResponseEmisor responseEmisor = new ResponseEmisor();
            string spName = string.Empty;

            var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_PDP"].ToString();
            //var _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioPDPProdDB;Persist Security Info=False;User ID=ProcesadorPago;Password=W3rcur10PDP!#$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Application Name=PaymentOrderProcess;Min Pool Size=0;Max Pool Size=5;Pooling=true;";

            spName = "up_PPS_Sel_DecisionEmisor";

            try
            {
                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderReferenceNumber", SqlDbType.VarChar);
                        param.Value = OrderReferenceNumber;

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);
                    }
                }

                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        responseEmisor.DecisionEmisor = row["StatusEmisor"].ToString();
                        responseEmisor.DescReespuestaEmisor = row["ReasonCodes"].ToString();
                    }
                }

                return responseEmisor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BL_GetApprovalCode(string OrderReferenceNumber)
        {
            try
            {
                string dsS = string.Empty;
                string ApprovalCode = string.Empty;

                DataSet ds = new DataSet();

                var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_PDP"].ToString();
                //var _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioPDPProdDB;Persist Security Info=False;User ID=ProcesadorPago;Password=W3rcur10PDP!#$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Application Name=PaymentOrderProcess;Min Pool Size=0;Max Pool Size=5;Pooling=true;";

                string spName = "up_PPS_Sel_ApprovalCode";

                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderReferenceNumber", SqlDbType.Int);
                        param.Value = OrderReferenceNumber;

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);
                    }
                }

                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        dsS = row["ResponseJson"].ToString();
                    }
                }

                if (dsS != "")
                {
                    if (dsS.Contains("DECLINED"))
                        ApprovalCode = "";
                    else
                    {
                        var approval = JsonConvert.DeserializeObject<ApprovalCodeModel>(dsS);

                        if (approval.ResponseObject.processorInformation.approvalCode != "")
                            ApprovalCode = approval.ResponseObject.processorInformation.approvalCode;
                    }
                }

                return ApprovalCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ReverseModel GetDatosRefound(string OrderReferenceNumber)
        {
            DataSet ds = new DataSet();
            RefoundRev rev = new RefoundRev();
            ReverseModel Response = new ReverseModel();
            string spName = string.Empty;

            var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_PDP"].ToString();
            //var _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioPDPProdDB;Persist Security Info=False;User ID=ProcesadorPago;Password=W3rcur10PDP!#$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Application Name=PaymentOrderProcess;Min Pool Size=0;Max Pool Size=5;Pooling=true;";

            spName = "up_PPS_Sel_Refound_Rev";

            try
            {
                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderReferenceNumber", SqlDbType.VarChar);
                        param.Value = OrderReferenceNumber;

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);
                    }
                }

                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        rev.ResponseJson = row["ResponseJson"].ToString();
                        rev.idPayment = row["idPayment"].ToString();
                        rev.fec_movto = row["fec_movto"].ToString();
                    }
                }

                if (rev.ResponseJson != null)
                {
                    RefoundJson refound = JsonConvert.DeserializeObject<RefoundJson>(rev.ResponseJson.ToString());

                    Response.FechaReverso = rev.fec_movto.Substring(0, 10);
                    Response.HoraReverso = rev.fec_movto.Substring(11, 5);
                    Response.IDReverso = rev.idPayment;
                    Response.MontoRverso = refound.refundAmountDetails.refundAmount;
                }

                return Response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ShipmentDataEstatus GetEstatusShipment(string OrderReferenceNumber)
        {
            ApprovalCodeModel aproval = new ApprovalCodeModel();
            string spName = string.Empty;
            DataSet ds = new DataSet();
            ShipmentDataEstatus Estatus = new ShipmentDataEstatus();

            var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_DEV"].ToString();
            //var _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioDB;Persist Security Info=False;User ID=sorianaprod_mrcr;Password=!#W3rCuR10;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            spName = "up_PPS_sel_EstatusShipment";

            try
            {
                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderReferenceNumber", SqlDbType.VarChar);
                        param.Value = OrderReferenceNumber;

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);
                    }
                }

                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Estatus.CarrierName = row["Tipo-Almacen"].ToString();
                        //Estatus.OrderId = row["OrderId"].ToString(); ;
                        //Estatus.shipmentAlias = row["shipmentAlias"].ToString(); ;
                        Estatus.status = row["estatus"].ToString();
                        Estatus.DeliveryType = row["DeliveryType"].ToString();
                    }
                }

                return Estatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private CancelDevolucionModel GetCancelDevolucion(string OrderReferenceNumber)
        {
            CancelDevolucionModel Response = new CancelDevolucionModel();
            DataSet ds = new DataSet();
            CancellationModel Cancelacion = new CancellationModel();
            DevolucionModel devolucion = new DevolucionModel();

            string spName = string.Empty;

            var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_DEV"].ToString();
            //var _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioDB;Persist Security Info=False;User ID=sorianaprod_mrcr;Password=!#W3rCuR10;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            spName = "up_PPS_Sel_Cancel_Devolucion";

            try
            {
                #region Cancelacion
                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderReferenceNumber", SqlDbType.VarChar);
                        param.Value = OrderReferenceNumber;

                        param = cmd.Parameters.Add("@accion", SqlDbType.VarChar);
                        param.Value = "cancelar";


                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);

                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                Cancelacion.OrderId = row["OrderId"].ToString();
                                Cancelacion.clientEmail = row["clientEmail"].ToString();
                                Cancelacion.accion = row["accion"].ToString();
                                Cancelacion.fec_movto = row["fec_movto"].ToString();
                                Cancelacion.estatusRma = row["estatusRma"].ToString();
                                Cancelacion.ProcesoAut = row["ProcesoAut"].ToString();
                                Cancelacion.idProceso = row["idProceso"].ToString();
                                Cancelacion.cancellationReason = row["cancellationReason"].ToString();
                                break;
                            }
                        }
                    }
                }
                #endregion

                #region Devolucion
                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderReferenceNumber", SqlDbType.VarChar);
                        param.Value = OrderReferenceNumber;

                        param = cmd.Parameters.Add("@accion", SqlDbType.VarChar);
                        param.Value = "retorno";


                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);

                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                devolucion.OrderId = row["OrderId"].ToString();
                                devolucion.clientEmail = row["clientEmail"].ToString();
                                devolucion.accion = row["accion"].ToString();
                                devolucion.fec_movto = row["fec_movto"].ToString();
                                devolucion.estatusRma = row["estatusRma"].ToString();
                                devolucion.ProcesoAut = row["ProcesoAut"].ToString();
                                devolucion.idProceso = row["idProceso"].ToString();
                                devolucion.cancellationReason = row["cancellationReason"].ToString();

                                break;
                            }
                        }
                    }
                }
                #endregion

                Response.Cancelacion = Cancelacion;
                Response.Devolucion = devolucion;

                return Response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<DetalleProducto> GetDatosArticuloCancelDev(string OrderReferenceNumber)
        {
            AprobacionesMarcas Response = new AprobacionesMarcas();
            DataSet ds = new DataSet();
            List<ProcesadorPagosBase> LstppsBase = new List<ProcesadorPagosBase>();
            List<DetalleProducto> lstDetalleProd = new List<DetalleProducto>();
            string spName = string.Empty;
            string spName2 = string.Empty;
            string OrderNo = OrderReferenceNumber;

            var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_DEV"].ToString();
            //var _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioDB;Persist Security Info=False;User ID=sorianaprod_mrcr;Password=!#W3rCuR10;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            spName = "spDatosSplitOrder_rpt_sUP";
            spName2 = "spDatosArticulosbyOrderId_rpt_sUP";

            try
            {
                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderNo", SqlDbType.Int);
                        param.Value = int.Parse(OrderNo);

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);

                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                OrderNo = row["OrderNo"].ToString();
                            }
                        }
                    }
                }
                ds = new DataSet();

                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName2, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderId", SqlDbType.Int);
                        param.Value = int.Parse(OrderNo);

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);

                        foreach (DataTable dt in ds.Tables)
                        {

                            if (dt.TableName == "Table1")
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    DetalleProducto prod = new DetalleProducto
                                    {
                                        CodeBarra = row["CodeBarra"].ToString(),
                                        ProductName = row["ProductName"].ToString(),
                                        Quantity = row["Quantity"].ToString(),
                                        Price = row["Price"].ToString(),
                                        ProductId = row["ProductId"].ToString()
                                    };

                                    lstDetalleProd.Add(prod);
                                }
                            }



                        }
                    }
                }

                return lstDetalleProd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private OrdersAppModel BL_Ordenes_APP(string OrderReferenceNumber)
        {
            try
            {
                string OrderNo = string.Empty;
                OrdersAppModel ordersApp = new OrdersAppModel();
                DataSet ds = SliptOrder(OrderReferenceNumber);

                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        OrderNo = row["OrderNo"].ToString();
                    }
                }

                if (OrderNo != "")
                {
                    DataSet ds2 = OrdenesAPP(OrderNo);

                    foreach (DataTable dt in ds2.Tables)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            ordersApp.OrderNo = row["OrderNo"].ToString();
                            ordersApp.StatusUe = row["StatusUe"].ToString();
                            ordersApp.CreatedBy = row["CreatedBy"].ToString();
                            ordersApp.Id_Num_Apl = row["Id_Num_Apl"].ToString();
                            ordersApp.MethodPayment = row["MethodPayment"].ToString();
                            ordersApp.DeliveryType = row["DeliveryType"].ToString();
                        }
                    }
                }

                return ordersApp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataSet SliptOrder(string OrderReferenceNumber)
        {
            DataSet ds = new DataSet();

            try
            {
                var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_DEV"].ToString();
                //var _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioDB;Persist Security Info=False;User ID=sorianaprod_mrcr;Password=!#W3rCuR10;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                string spName = "spDatosSplitOrder_rpt_sUP";  //DatabaseSchemaConstants.PROCEDURE_NAME_GET_APPROVAL_CODE;

                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderNo", SqlDbType.NVarChar);
                        param.Value = int.Parse(OrderReferenceNumber);

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);
                    }
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet OrdenesAPP(string OrderReferenceNumber)
        {
            DataSet ds = new DataSet();

            try
            {
                var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_DEV"].ToString();
                //var _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioDB;Persist Security Info=False;User ID=sorianaprod_mrcr;Password=!#W3rCuR10;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                string spName = "up_PPS_Sel_OrdersApp";

                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderReferenceNumber", SqlDbType.NVarChar);
                        param.Value = OrderReferenceNumber;

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);
                    }
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ProcesadorPagosBase GetOrderAPP(string OrderReferenceNumber, string Method)
        {
            try
            {
                DataSet ds = new DataSet();
                ProcesadorPagosBase ppsBase = new ProcesadorPagosBase();
                int strOrderLenght = OrderReferenceNumber.Length;
                OrderReferenceNumber = OrderReferenceNumber.Substring(0, strOrderLenght - 2);

                var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_DEV"].ToString();
                //var _ConnectionString = "Server=tcp:srvsqlmercurioqa.database.windows.net,1433;Initial Catalog=MercurioQaDB;Persist Security Info=False;User ID=t_eliseogr;Password=W3rcur10!QA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Application Name=SplitOrdenFunction;Min Pool Size=0;Max Pool Size=10;Pooling=true;";
                //var _ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioDB;Persist Security Info=False;User ID=sorianaprod_mrcr;Password=!#W3rCuR10;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                string spName = "up_PPS_sel_PaymentTransactionRpt_byOrder";

                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderReferenceNumber", SqlDbType.VarChar);
                        param.Value = OrderReferenceNumber;

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);
                    }
                }

                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        #region Mapping
                        #region Order
                        ppsBase.OrderReferenceNumber = row["OrderReferenceNumber"].ToString();          //Orden ID
                        ppsBase.PaymentTransactionID = row["PaymentTransactionID"].ToString();          //Transacción
                        ppsBase.OrderAmount = row["OrderAmount"].ToString();                            //Monto Total Orden
                        #endregion

                        #region Tokenizacion
                        ppsBase.Bank = row["Bank"].ToString();                                          //Banco
                        ppsBase.BinCode = row["BinCode"].ToString();
                        ppsBase.MaskCard = row["MaskCard"].ToString().Substring(15);                    // Sufijo hay q recortar
                        ppsBase.TypeOfCard = row["TypeOfCard"].ToString();                              //Tipo Tarjeta
                        ppsBase.PaymentMethod = row["PaymentMethod"].ToString();                        //Marca
                        #endregion

                        #region shipping
                        ppsBase.ShippingStoreId = row["shippingStoreId"].ToString();                    //Tipo ALmacen
                        ppsBase.ShippingReferenceNumber = row["ShippingReferenceNumber"].ToString();    //Consignación ID

                        if (row["ShippingReferenceNumber"].ToString() == "001-1")
                        {
                            ppsBase.ShippingDeliveryDesc = "SETC";
                            ppsBase.Catalogo = "SETC";
                            ppsBase.AffiliationType = "8655759";
                            ppsBase.Adquirente = "GETNET";
                        }

                        else
                        {
                            ppsBase.ShippingDeliveryDesc = "MG";
                            ppsBase.Catalogo = "MG";
                            ppsBase.AffiliationType = "1045441";
                            ppsBase.Adquirente = "EVO Payment";
                        }
                        #endregion

                        #region Lealtad
                        ppsBase.CustomerLoyaltyRedeemPoints = row["CustomerLoyaltyRedeemPoints"].ToString(); //punto aplicados
                        ppsBase.CustomerLoyaltyRedeemMoney = row["CustomerLoyaltyRedeemMoney"].ToString();  //efectivo disponble /dinero en efectivo                              
                        ppsBase.CustomerLoyaltyCardId = row["CustomerLoyaltyCardId"].ToString();
                        #endregion

                        #region Cancellation                        
                        if (Method == "Creditos")
                        {
                            decimal TotalPrice = 0;
                            decimal TotalPiezas = 0;
                            string Consignacion = string.Empty;

                            var Cancelacion = GetCancelDevolucion(ppsBase.OrderReferenceNumber);
                            var Productos = GetDatosArticuloCancelDev(ppsBase.OrderReferenceNumber);

                            foreach (var prod in Productos)
                            {
                                TotalPrice = decimal.Parse(prod.Price) + TotalPrice;
                                Consignacion = prod.ProductId + ", " + Consignacion;
                                TotalPiezas = TotalPiezas + 1;
                            }

                            ppsBase.NombreCancelacion = ppsBase.ShippingFirstName + " " + ppsBase.CustomerLastName;
                            ppsBase.Motivo = Cancelacion.Cancelacion.cancellationReason.Trim();

                            if (Cancelacion.Cancelacion.fec_movto == "")
                            {
                                ppsBase.FechaCancel = "";
                                ppsBase.HoraCancel = "";
                            }
                            else
                            {
                                var fechaCancel = Cancelacion.Cancelacion.fec_movto;
                                DateTime FecCancel = DateTime.Parse(fechaCancel);
                                ppsBase.FechaCancel = FecCancel.ToString("MMMM");
                                ppsBase.HoraCancel = fechaCancel.Substring(10);
                            }

                            ppsBase.MontoCancel = TotalPrice.ToString();
                            ppsBase.ConsignacionIDCancelada = Consignacion;
                            ppsBase.NoPiezasConsignacionCancelacion = TotalPiezas.ToString();
                            ppsBase.FechaINgresoRMA = Cancelacion.Cancelacion.fec_movto;

                            ppsBase.ConsignaciónIDDevolucin = Consignacion;
                            ppsBase.DetalleConsignacionIngresada = "";
                            ppsBase.NoPzasConsignacionDevolucion = TotalPiezas.ToString();

                            if (Cancelacion.Devolucion.fec_movto == "")
                            {
                                ppsBase.FechaDevolucion = "";
                                ppsBase.HoraDevolucion = "";
                            }
                            else
                            {
                                var fechaDevolucion = Cancelacion.Devolucion.fec_movto;
                                DateTime FecDev = DateTime.Parse(fechaDevolucion);

                                ppsBase.FechaDevolucion = FecDev.ToString("MMMM");
                                ppsBase.HoraDevolucion = fechaDevolucion.Substring(10);
                            }


                            ppsBase.FechaDevolucion = Cancelacion.Devolucion.fec_movto;
                            ppsBase.MontoDevolucionConsignacion = TotalPrice.ToString();
                            ppsBase.FechaReembolso = "";
                            ppsBase.HoraReembolso = "";
                            ppsBase.FormaPagoRembolso = "";
                            ppsBase.Bin_Reembolso = row["BinCode"].ToString();
                            ppsBase.SufijoReembolso = row["MaskCard"].ToString().Substring(15);
                            ppsBase.ReembolsoAutomatico = "";
                            ppsBase.ReembolsoManual = "";
                            ppsBase.IDTransaccionReembolso = "";
                        }
                        #endregion

                        #region Reverso
                        if (Method == "Reverso")
                        {
                            var reverso = GetDatosRefound(row["OrderReferenceNumber"].ToString());

                            ppsBase.FechaReversoAutorizacion = reverso.FechaReverso;
                            ppsBase.HoraReversoAutorizacion = reverso.HoraReverso;
                            ppsBase.MontoReverso = reverso.MontoRverso;
                            ppsBase.IDTransaccionReverso = reverso.IDReverso;
                        }
                        #endregion

                        #region ApprovalCode
                        var ApprovalCode = BL_GetApprovalCode(row["OrderReferenceNumber"].ToString());
                        ppsBase.TransactionAuthorizationId = ApprovalCode;
                        #endregion

                        #region Emisor
                        if (Method == "AutBancaria")
                        {
                            var EmisorResponse = GetDatosEmisor(ppsBase.OrderReferenceNumber);

                            ppsBase.DecisionEmisor = EmisorResponse.DecisionEmisor;
                            ppsBase.CveReespuestaEmisor = ApprovalCode;
                            ppsBase.DescReespuestaEmisor = EmisorResponse.DescReespuestaEmisor;
                        }
                        #endregion

                        #region GetTrace
                        var DatosExtra = GetTracePayment(ppsBase.OrderReferenceNumber, ppsBase.Catalogo);

                        if (DatosExtra != null)
                        {
                            if (DatosExtra.orderReferenceNumber != null)
                            {
                                //string FechaOrden = DatosExtra.orderDateTime.Substring(0, 10);
                                //string HoraOrden = DatosExtra.orderDateTime.Substring(11, 5);

                                //ppsBase.OrderDate = FechaOrden;
                                //ppsBase.OrderHour = HoraOrden;

                                ppsBase.orderAmount = DatosExtra.orderAmount;
                                ppsBase.TransactionReferenceID = DatosExtra.TransactionReferenceID;
                                ppsBase.IsAuthenticated = DatosExtra.IsAuthenticated;
                                ppsBase.IsAuthorized = DatosExtra.IsAuthorized;
                                ppsBase.Apply3DS = DatosExtra.Apply3DS;
                                ppsBase.MerchandiseType = DatosExtra.MerchandiseType;
                                ppsBase.clientEmail = DatosExtra.customerEmail;
                                ppsBase.TransactionStatus = DatosExtra.TransactionStatus;

                                #region Datos Liquidacion
                                if (DatosExtra.TransactionStatus == "AUTHORIZED")
                                {
                                    //ppsBase.FechaLiquidacion = FechaOrden;
                                    //ppsBase.HoraLiquidacion = HoraOrden;
                                    ppsBase.MontoLiquidacion = DatosExtra.shipments[0].shippingPaymentImport;
                                    ppsBase.LiquidacionManual = "";
                                    ppsBase.LiquidacionAutomatica = "True";
                                    ppsBase.IDTransaccionLiquidacion = DatosExtra.TransactionReferenceID;
                                }
                                #endregion

                                if (DatosExtra.paymentType == "WALLET")
                                {
                                    ppsBase.paymentTypeJson = "PAYPAL";
                                }
                                else
                                {
                                    bool flagOmonel = DatosExtra.paymentToken.Contains("OMONEL");

                                    if (DatosExtra.paymentToken.Contains("OMONEL"))
                                        ppsBase.paymentTypeJson = "OMONEL";
                                    else
                                        ppsBase.paymentTypeJson = DatosExtra.paymentType;
                                }

                                if (DatosExtra.shipments.Count > 0)
                                {
                                    ppsBase.shippingDeliveryDesc = DatosExtra.shipments[0].shippingDeliveryDesc;
                                    ppsBase.shippingPaymentImport = DatosExtra.shipments[0].shippingPaymentImport;
                                    ppsBase.shippingPaymentInstallments = DatosExtra.shipments[0].shippingPaymentInstallments;
                                    ppsBase.ShippingFirstName = DatosExtra.shipments[0].shippingFirstName;
                                    ppsBase.ShippingItemTotal = DatosExtra.shipments[0].Items[0].ShippingItemTotal;
                                }
                            }
                        }
                        #endregion

                        #region OUE
                        var oue = BL_Ordenes_APP(row["OrderReferenceNumber"].ToString());

                        if (oue.Id_Num_Apl == "22" || oue.Id_Num_Apl == "")
                            ppsBase.OrderSaleChannel = row["OrderSaleChannel"].ToString();                  //Canal Compra
                        else
                        {
                            ppsBase.OrderSaleChannel = "APP";                 //Canal Compra
                            ppsBase.TipoMobile = oue.CreatedBy;
                        }

                        //var Delivery = Regex.Replace(oue.DeliveryType, @"[^a-zA-z0-9 ]+", "");
                        //Delivery = Delivery.Replace("Envo", "Envio");

                        ppsBase.DeliveryType = oue.DeliveryType;
                        #endregion

                        #region Estatus Shipment
                        var estatusShipment = GetEstatusShipment(row["OrderReferenceNumber"].ToString());

                        ppsBase.TipoAlmacen = estatusShipment.CarrierName;
                        ppsBase.EstatusEnvio = estatusShipment.status;
                        #endregion
                        #endregion
                    }
                }

                return ppsBase;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Omonel_Auth Get_Omonel_Aut(string OrderReferenceNumber)
        {
            try
            {
                DataSet ds = new DataSet();
                Omonel_Auth Response = new Omonel_Auth();

                var _ConnectionString = ConfigurationManager.ConnectionStrings["Connection_PDP"].ToString();
                string spName = "up_PPS_sel_OmonelAuth";

                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        System.Data.SqlClient.SqlParameter param;
                        param = cmd.Parameters.Add("@OrderReferenceNumber", SqlDbType.VarChar);
                        param.Value = OrderReferenceNumber;

                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);
                    }
                }

                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Response.Cve_Autz = row["Cve_Autz"].ToString();
                        Response.ShippingReferenceNumber = row["ShippingReferenceNumber"].ToString();
                    }
                }

                return Response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Ftp
        public FileContentResult ProcesaArchivos(string NombreArchivo, string Fecha)
        {
            string host = ConfigurationManager.AppSettings["server"];
            string username = ConfigurationManager.AppSettings["userName"];
            string password = ConfigurationManager.AppSettings["password"];
            string remoteDirectory = ConfigurationManager.AppSettings["pathUpload"];
            int port = int.Parse(ConfigurationManager.AppSettings["puerto"].ToString());

            string year = Fecha.Substring(0, 4);
            string month = Fecha.Substring(5, 2);
            string day = Fecha.Substring(8, 2);

            string strFecha = day + month + year;
            NombreArchivo = NombreArchivo + strFecha;

            PasswordAuthenticationMethod authMethod = new PasswordAuthenticationMethod(username, password);
            Renci.SshNet.ConnectionInfo connectionInfo = new Renci.SshNet.ConnectionInfo(host, port, username, authMethod);

            using (SftpClient sftp = new SftpClient(connectionInfo))
            {
                try
                {
                    sftp.Connect();

                    List<Renci.SshNet.Sftp.SftpFile> files = sftp.ListDirectory(remoteDirectory).Where(x => x.Name.Contains(".xls")).ToList();

                    foreach (Renci.SshNet.Sftp.SftpFile file in files)
                    {
                        string filePath = file.FullName;

                        var cont = filePath.Contains(NombreArchivo + ".xls");

                        if (cont == true)
                        {
                            using (Renci.SshNet.Sftp.SftpFileStream remoteFileStream = sftp.OpenRead(filePath))
                            {
                                using (StreamReader reader = new StreamReader(remoteFileStream))
                                {
                                    var bytes = default(byte[]);
                                    using (var memstream = new MemoryStream())
                                    {
                                        reader.BaseStream.CopyTo(memstream);
                                        bytes = memstream.ToArray();
                                    }

                                    return new FileContentResult(bytes, "application/vnd.ms-excel") { FileDownloadName = NombreArchivo + ".xls" };
                                }
                            }
                        }
                        //else
                        //{
                        //    byte[] bytes = { 0, 0, 0, 25 };
                        //    NombreArchivo = "Error Descarga";

                        //    return new FileContentResult(bytes, "application/vnd.txt") { FileDownloadName = NombreArchivo + ".txt" };
                        //}
                    }

                    sftp.Disconnect();

                    return new FileContentResult(null, "application/vnd.ms-excel") { FileDownloadName = "Filename.xls" };
                }
                catch (Exception e)
                {
                    byte[] bytes = { 0, 0, 0, 25 };
                    NombreArchivo = "Error Descarga";

                    return new FileContentResult(bytes, "application/vnd.txt") { FileDownloadName = NombreArchivo + ".txt" };
                }
            }
        }

        public FileContentResult ProcesaArchivosCVV(string NombreArchivo, string Fecha)
        {
            string host = ConfigurationManager.AppSettings["server"];
            string username = ConfigurationManager.AppSettings["userName"];
            string password = ConfigurationManager.AppSettings["password"];
            string remoteDirectory = ConfigurationManager.AppSettings["pathUpload"];
            int port = int.Parse(ConfigurationManager.AppSettings["puerto"].ToString());

            string year = Fecha.Substring(0, 4);
            string month = Fecha.Substring(5, 2);
            string day = Fecha.Substring(8, 2);

            string strFecha = day + month + year;
            NombreArchivo = NombreArchivo + strFecha;

            PasswordAuthenticationMethod authMethod = new PasswordAuthenticationMethod(username, password);
            Renci.SshNet.ConnectionInfo connectionInfo = new Renci.SshNet.ConnectionInfo(host, port, username, authMethod);

            using (SftpClient sftp = new SftpClient(connectionInfo))
            {
                try
                {
                    sftp.Connect();

                    List<Renci.SshNet.Sftp.SftpFile> files = sftp.ListDirectory(remoteDirectory).Where(x => x.Name.Contains(".csv")).ToList();

                    foreach (Renci.SshNet.Sftp.SftpFile file in files)
                    {
                        string filePath = file.FullName;

                        var cont = filePath.Contains(NombreArchivo);

                        if (cont == true)
                        {
                            using (Renci.SshNet.Sftp.SftpFileStream remoteFileStream = sftp.OpenRead(filePath))
                            {
                                using (StreamReader reader = new StreamReader(remoteFileStream))
                                {
                                    var bytes = default(byte[]);
                                    using (var memstream = new MemoryStream())
                                    {
                                        reader.BaseStream.CopyTo(memstream);
                                        bytes = memstream.ToArray();
                                    }

                                    return new FileContentResult(bytes, "application/vnd.ms-excel") { FileDownloadName = NombreArchivo + ".xls" };
                                }
                            }
                        }
                        //else
                        //{
                        //    byte[] bytes = { 0, 0, 0, 25 };
                        //    NombreArchivo = "Error Descarga";

                        //    return new FileContentResult(bytes, "application/vnd.txt") { FileDownloadName = NombreArchivo + ".txt" };
                        //}
                    }

                    sftp.Disconnect();

                    return new FileContentResult(null, "application/vnd.ms-excel") { FileDownloadName = "Filename.xls" };
                }
                catch (Exception e)
                {
                    byte[] bytes = { 0, 0, 0, 25 };
                    NombreArchivo = "Error Descarga";

                    return new FileContentResult(bytes, "application/vnd.txt") { FileDownloadName = NombreArchivo + ".txt" };
                }
            }
        }
        #endregion
    }
}