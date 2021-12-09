﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServicesManagement.Web.Models.ProcesadorPagosSoriana;
using ServicesManagement.Web.Models.ReportesPDP;
using Soriana.ProcesadorPagosWeb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{

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

    public class responseObject
    {
        public ProcessorInformation processorInformation { get; set; }
    }

    public class ProcessorInformation
    {
        public string approvalCode { get; set; }
    }

    public class ApprovalCodeJsonResponse
    {
        public string ResponseJson { get; set; }
    }
    #endregion

    [Authorize]
    public class ReportesPDPController : Controller
    {
        // GET: ReportesPDP
        #region FormaPago
        public ActionResult FormaPago()
        {
            return View();
        }

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
            Session["listaGrid"] = Newtonsoft.Json.JsonConvert.SerializeObject(GetProcesadorPagosBase("", "", "AuthBancaria"));

            return View();
        }

        public ActionResult GetParamDates(string FecIni, string FecFin)
        {
            List<ProcesadorPagosBase> autoriazionBancaria = GetProcesadorPagosBase(FecIni, FecFin, "AuthBancaria");

            var result = new { Success = true, resp = autoriazionBancaria };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Canal de Compra
        public ActionResult Canaldecompra()
        {
            Session["listaGrid"] = Newtonsoft.Json.JsonConvert.SerializeObject(GetProcesadorPagosBase("", "", "CanalCompra"));

            return View();
        }

        public ActionResult GetParamDatesCanalCompra(string FecIni, string FecFin)
        {
            List<ProcesadorPagosBase> autoriazionBancaria = GetProcesadorPagosBase(FecIni, FecFin, "CanalCompra");

            var result = new { Success = true, resp = autoriazionBancaria };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        public ActionResult CanalPago()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

      
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

        public ActionResult Creditos()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Liquidaciones()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult ReservacionesSaldo()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult PagoTienda()
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

        public ActionResult AutRM()
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
            _ConnectionString = "Server=tcp:srvsqlmercurio.database.windows.net,1433;Initial Catalog=MercurioDesaDB;Persist Security Info=False;User ID=t_eliseogr;Password=El1530%.*314;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _ConnectionString = "Server=tcp:srvsqlmercurioqa.database.windows.net,1433;Initial Catalog=MercurioQaDB;Persist Security Info=False;User ID=t_eliseogr;Password=W3rcur10!QA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
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
            _ConnectionString = "Server=tcp:srvsqlmercurio.database.windows.net,1433;Initial Catalog=MercurioDesaDB;Persist Security Info=False;User ID=t_eliseogr;Password=El1530%.*314;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _ConnectionString = "Server=tcp:srvsqlmercurioqa.database.windows.net,1433;Initial Catalog=MercurioQaDB;Persist Security Info=False;User ID=t_eliseogr;Password=W3rcur10!QA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
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
                                                    //if (out_response["responseCode"].ToString().Equals("400")) { p.ErrorSpecified = true; break; }
                                                }
                                            }

                                            if (out_response["responseError"] != null && !out_response["responseCode"].ToString().Equals("400"))
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

                                                JObject error = JObject.Parse(orde3["errorInformation"].ToString());

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

                                                JObject error = JObject.Parse(orde4["errorInformation"].ToString());

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

                                                JObject error = JObject.Parse(orde2["errorInformation"].ToString());

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

                                                JObject error = JObject.Parse(orde5["errorInformation"].ToString());

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

        private List<ProcesadorPagosBase> GetProcesadorPagosBase(string FecIni, string FecFin, string Method)
        {
            DataSet ds = new DataSet();
            List<ProcesadorPagosBase> LstppsBase = new List<ProcesadorPagosBase>();
            string spName = string.Empty;

            var _ConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings:MercurioDB");
            _ConnectionString = "Server=tcp:srvsqlmercurioqa.database.windows.net,1433;Initial Catalog=MercurioQaDB;Persist Security Info=False;User ID=t_eliseogr;Password=W3rcur10!QA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            spName = "up_PPS_sel_PaymentTransactionRpt";

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
                                ProcesadorPagosBase ppsBase = new ProcesadorPagosBase();

                                #region Mapping
                                ppsBase.PaymentToken = row["PaymentToken"].ToString();
                                ppsBase.OrderReferenceNumber = row["OrderReferenceNumber"].ToString();          //Orden ID
                                ppsBase.PaymentTransactionID = row["PaymentTransactionID"].ToString();          //Transacción
                                var fechaCreacion = row["OrderDate"].ToString();
                                DateTime FecCreate = DateTime.Parse(fechaCreacion);
                                ppsBase.OrderMonth = FecCreate.ToString("MMMM");


                                ppsBase.OrderDate = row["OrderDate"].ToString().Substring(0, 10);                                //Fecha Creacion 
                                ppsBase.OrderHour = fechaCreacion.Substring(10);
                                ppsBase.OrderAmount = row["OrderAmount"].ToString();                            //Monto Total Orden
                                ppsBase.Products = row["Products"].ToString();                                  //SKU Catálogo
                                ppsBase.OrderSaleChannel = row["OrderSaleChannel"].ToString();                  //Canal Compra

                                #region Tokenizacion
                                ppsBase.Bank = row["Bank"].ToString();                                          //Banco
                                ppsBase.BinCode = row["BinCode"].ToString();
                                ppsBase.MaskCard = row["MaskCard"].ToString().Substring(15);                                  // Sufijo hay q recortar
                                ppsBase.TypeOfCard = row["TypeOfCard"].ToString();                              //Tipo Tarjeta
                                ppsBase.PaymentMethod = row["PaymentMethod"].ToString();                        //Marca
                                ppsBase.CustomerFirstName = row["CustomerFirstName"].ToString();
                                ppsBase.CustomerLastName = row["CustomerLastName"].ToString();
                                #endregion

                                #region shipping
                                ppsBase.ShippingStoreId = row["shippingStoreId"].ToString();                    //Tipo ALmacen
                                ppsBase.ShippingReferenceNumber = row["ShippingReferenceNumber"].ToString();    //Consignación ID
                                ppsBase.ShippingPaymentImport = row["ShippingPaymentImport"].ToString();        //Monto Consignación
                                ppsBase.ShippingFirstName = row["ShippingFirstName"].ToString();                //Nombre de Persona a Recibir
                                ppsBase.ShippingLastName = row["ShippingLastName"].ToString();                  //Apellido P 
                                ppsBase.CustomerContact = row["CustomerContact"].ToString();                    //Apellido M
                                ppsBase.CustomerContact = row["CustomerContact"].ToString();                    //Teléfono
                                ppsBase.CustomerAddress = row["ShippingAddress"].ToString();                    //Calle
                                ppsBase.CustomerCity = row["ShippingCity"].ToString();                          //Ciudad
                                ppsBase.CustomerZipCode = row["CustomerZipCode"].ToString();                     //CP
                                #endregion

                                ppsBase.PaymentTransactionService = row["PaymentTransactionService"].ToString();                         //Estatus Orden
                                ppsBase.TransactionStatus = row["TransactionStatus"].ToString();                //Estatus ENvio

                                if (row["ShippingDeliveryDesc"].ToString() == "Envío - 1 - 1")
                                {
                                    ppsBase.ShippingDeliveryDesc = "SETC";
                                    ppsBase.Catalogo = "SETC";
                                }

                                else
                                {
                                    ppsBase.ShippingDeliveryDesc = "MG";
                                    ppsBase.Catalogo = "MG";
                                }
                                    ppsBase.ShippingDeliveryDesc = "MG";

                                ppsBase.CustomerLoyaltyRedeemPoints = row["CustomerLoyaltyRedeemPoints"].ToString(); //punto aplicados
                                ppsBase.CustomerLoyaltyRedeemMoney = row["CustomerLoyaltyRedeemMoney"].ToString();  //efectivo disponble /dinero en efectivo                              
                                ppsBase.CustomerLoyaltyCardId = row["CustomerLoyaltyCardId"].ToString();
                                ppsBase.MethodPaymentShipment = row["MethodPaymentShipment"].ToString();

                                #region Cancellation
                                /*ppsBase.Id_cancelacion = row["Id_cancelacion"].ToString();                      //No. RMA
                                ppsBase.clientEmail = row["clientEmail"].ToString();                            //Nombre de quien Cancela
                                ppsBase.Motivo = row["Motivo"].ToString();                                      //Motivo Cancelacion
                                //ppsBase.newProductQuantity = Cancel.newProductQuantity;                         //No Piezas Consignación Cancelada
                                ppsBase.fec_movto = row["fec_movto"].ToString();                                //Fecha INgreso RMA
                                //ppsBase.productId = Cancel.productId;                                           //Detalle de la Consignación Ingresada (SKU)
                                ppsBase.fec_movto = row["fec_movto"].ToString();                                //Fecha Devolución
                                ppsBase.fec_movto = row["fec_movto"].ToString();                                //Fecha Reembolso
                                ppsBase.Bin_Reembolso = row["BinCode"].ToString();                              //BIN Tarjeta Reembolso
                                ppsBase.SufijoReembolso = row["MaskCard"].ToString();                           //Sufijo Tarjeta Reembolso                                                           
                                */
                                #endregion

                                #region rpt_v2
                                /*ppsBase.ofUE_StatusUE = row["StatusUE"].ToString();
                                ppsBase.ofUE_DeliveryType = row["DeliveryType"].ToString();
                                ppsBase.ofUE_UEType = row["UEType"].ToString();
                                ppsBase.ofUE_IsPickingManual = row["IsPickingManual"].ToString();
                                ppsBase.ofUE_CustomerNo = row["CustomerNo"].ToString();
                                ppsBase.ofUE_CustomerName = row["CustomerName"].ToString();
                                ppsBase.ofUE_Phone = row["Phone"].ToString();
                                ppsBase.ofUE_Address1 = row["Address1"].ToString();
                                ppsBase.ofUE_Address2 = row["Address2"].ToString();
                                ppsBase.ofUE_City = row["City"].ToString();
                                ppsBase.ofUE_StateCode = row["StateCode"].ToString();
                                ppsBase.ofUE_PostalCode = row["PostalCode"].ToString();
                                ppsBase.ofUE_Total = row["Total"].ToString();
                                ppsBase.ofUE_MethodPayment = row["MethodPayment"].ToString();
                                ppsBase.ofUE_OrderRMA = row["OrderRMA"].ToString();
                                ppsBase.uSC_StatusDescription = row["StatusDescription"].ToString();*/
                                #endregion

                                #region GetTrace
                                var DatosExtra = GetTracePayment(ppsBase.OrderReferenceNumber);

                                if (DatosExtra.orderReferenceNumber != null)
                                {
                                    ppsBase.orderAmount = DatosExtra.orderAmount;
                                    ppsBase.paymentTypeJson = DatosExtra.paymentType;
                                    ppsBase.TransactionAuthorizationId = DatosExtra.TransactionAuthorizationId;
                                    ppsBase.TransactionReferenceID = DatosExtra.TransactionReferenceID;
                                    ppsBase.AffiliationType = DatosExtra.AffiliationType;
                                    ppsBase.IsAuthenticated = DatosExtra.IsAuthenticated;
                                    ppsBase.IsAuthorized = DatosExtra.IsAuthorized;
                                    ppsBase.Apply3DS = DatosExtra.Apply3DS;
                                    ppsBase.MerchandiseType = DatosExtra.MerchandiseType;
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

                                if(Method == "AuthBancaria")
                                {
                                    #region Emisor
                                    var EmisorResponse = GetDatosEmisor(ppsBase.OrderReferenceNumber);

                                    ppsBase.DecisionEmisor = EmisorResponse.DecisionEmisor;
                                    ppsBase.CveReespuestaEmisor = EmisorResponse.CveReespuestaEmisor;
                                    ppsBase.DescReespuestaEmisor = EmisorResponse.DescReespuestaEmisor;
                                    #endregion
                                }
                                #endregion

                                LstppsBase.Add(ppsBase);
                            }
                        }
                    }
                }

                return LstppsBase;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private JsonRespoonseModel GetTracePayment(string OrderReferenceNumber)
        {
            string ceros = "";
            DataSet ds = new DataSet();
            string JsonRequest = string.Empty;
            JsonRespoonseModel Response = new JsonRespoonseModel();

            var count = OrderReferenceNumber.Length;

            int ceros2 = 8 - count;


            for (int x = 0; x < ceros2; x++)
            {
                ceros = "0" + ceros;
            }

            OrderReferenceNumber = ceros + OrderReferenceNumber;

            var _ConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings:MercurioDB");
            //_ConnectionString = "Server=tcp:srvsqlmercurio.database.windows.net,1433;Initial Catalog=MercurioDesaDB;Persist Security Info=False;User ID=t_eliseogr;Password=El1530%.*314;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _ConnectionString = "Server=tcp:srvsqlmercurioqa.database.windows.net,1433;Initial Catalog=MercurioQaDB;Persist Security Info=False;User ID=t_eliseogr;Password=W3rcur10!QA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //_ConnectionString = "Server=tcp:srvsqlmercurioprod.database.windows.net,1433;Initial Catalog=MercurioPDPProdDB;Persist Security Info=False;User ID=ProcesadorPago;Password=W3rcur10PDP!#$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Application Name=PaymentOrderProcess;Min Pool Size=0;Max Pool Size=5;Pooling=true;";

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

                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                JsonRequest = row["RequestJson"].ToString();

                            }
                        }

                        if (JsonRequest != "")
                        {
                            Response = JsonConvert.DeserializeObject<JsonRespoonseModel>(JsonRequest);
                            shipments shipment = new shipments();
                            items item = new items();
                            List<items> lstItems = new List<items>();

                            foreach (var row in Response.shipments)
                            {
                                shipment.shippingDeliveryDesc = row.shippingDeliveryDesc;
                                shipment.shippingPaymentImport = row.shippingPaymentImport;
                                shipment.shippingPaymentInstallments = row.shippingPaymentInstallments;
                                foreach (var row2 in row.Items)
                                {
                                    if (row2.shippingItemCategory == "Costo de envio")
                                    {
                                        item.shippingItemCategory = row2.shippingItemCategory;
                                        item.shippingItemId = row2.shippingItemId;
                                        item.shippingItemName = row2.shippingItemName;
                                        item.ShippingItemTotal = row2.ShippingItemTotal;

                                        lstItems.Add(item);

                                        shipment.Items = lstItems;
                                    }
                                }

                            }

                            Response.shipments.Clear();
                            Response.shipments.Add(shipment);
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
            List<ProcesadorPagosBase> LstppsBase = new List<ProcesadorPagosBase>();
            ResponseEmisor ResponseEmisor = new ResponseEmisor();
            Emisor EmisorRes = new Emisor();
            ApprovalCodeModel aproval = new ApprovalCodeModel();
            string spName = string.Empty;

            var _ConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings:MercurioDB");
            _ConnectionString = "Server=tcp:srvsqlmercurioqa.database.windows.net,1433;Initial Catalog=MercurioQaDB;Persist Security Info=False;User ID=t_eliseogr;Password=W3rcur10!QA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

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

                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                //OrderReferenceNumber ResponseJson    method
                                if (row["method"].ToString() == "Process_fin")
                                {
                                    if (row["ResponseJson"].ToString() != "")
                                    {
                                        aproval = JsonConvert.DeserializeObject<ApprovalCodeModel>((row["ResponseJson"].ToString()));
                                    }

                                    if (aproval.ResponseObject.processorInformation != null)
                                        ResponseEmisor.CveReespuestaEmisor = aproval.ResponseObject.processorInformation.approvalCode;

                                    else
                                        ResponseEmisor.CveReespuestaEmisor = "";

                                    ResponseEmisor.DecisionEmisor = aproval.Status;
                                    ResponseEmisor.DescReespuestaEmisor = aproval.ReasonCodes;

                                    break;
                                }
                                else if (row["method"].ToString() == "ValidateAuthentication")
                                {
                                    if(row["ResponseJson"].ToString() != "")
                                    {
                                        EmisorRes = JsonConvert.DeserializeObject<Emisor>(row["ResponseJson"].ToString());
                                    }

                                    ResponseEmisor.CveReespuestaEmisor = EmisorRes.id;
                                    ResponseEmisor.DecisionEmisor = EmisorRes.status;

                                    break;
                                }
                                else if (row["method"].ToString() == "EnrollmentPayment")
                                {
                                    if (row["ResponseJson"].ToString() != "")
                                    {
                                        EmisorRes = JsonConvert.DeserializeObject<Emisor>(row["ResponseJson"].ToString());
                                    }

                                    ResponseEmisor.CveReespuestaEmisor = EmisorRes.id;
                                    ResponseEmisor.DecisionEmisor = EmisorRes.status;

                                    break;
                                }
                                else if (row["method"].ToString() == "createDecision")
                                {
                                    if (row["ResponseJson"].ToString() != "")
                                    {
                                        EmisorRes = JsonConvert.DeserializeObject<Emisor>(row["ResponseJson"].ToString());
                                    }

                                    ResponseEmisor.CveReespuestaEmisor = EmisorRes.id;
                                    ResponseEmisor.DecisionEmisor = EmisorRes.status;

                                    break;
                                }
                            }
                        }
                    }
                }

                return ResponseEmisor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}