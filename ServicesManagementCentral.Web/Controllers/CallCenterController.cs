﻿using RestSharp;
using ServicesManagement.Web.DAL.Autorizacion;
using ServicesManagement.Web.DAL.CallCenter;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.Autorizacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Web.Mvc;
using ServicesManagement.Web.DAL.DALHistorialRMA;
using ServicesManagement.Web.Helpers;
using System.IO;
using ServicesManagement.Web.Models.CallCenter;
using System.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Configuration;
using static ServicesManagement.Web.Controllers.OrderFacts_UEModel;
using System.Threading.Tasks;
using System.Globalization;

namespace ServicesManagement.Web.Controllers
{
    public class ResponseBuscadorModel
    {

        //public string @search.score { get; set; }// 1.0,
        public string Barcode { get; set; }// public string 650240011351public string ,
        public string Sku { get; set; }// public string 1791346public string ,
        public string Description { get; set; }// public string Unesia Antimicotico Ung C/20 Grpublic string ,
        public string ExtendedDescription { get; set; }// public string public string ,
        public string SalesUnit { get; set; }// public string PCEpublic string ,
        public string UrlImage { get; set; }// public string https://edge.disstg.commercecloud.salesforce.com/dw/image/v2/BGBD_STG/on/demandware.static/-/Sites-soriana-grocery-master-catalog/default/dw4f7cd627/images/product/650240011351-A.jpgpublic string ,
        public string SellLimit { get; set; }// 100.0,
        public string Variants { get; set; }// public string public string ,
        public string AdditionalImages { get; set; }// public string public string ,
        public string Price { get; set; }// 86.9,
        public string PromotionalPrice { get; set; }// 0.0,
        public string PomotionalPriceCode { get; set; }// public string public string ,
        public string arrivals { get; set; }// public string public string ,
        public string beefCuts { get; set; }// public string public string ,
        public string capacity { get; set; }// public string public string ,
        public string CapacityInNumberOfPeople { get; set; }// null,
        public string Color { get; set; }// public string public string ,
        public string combo { get; set; }// public string Color 21public string ,
        public string Company { get; set; }// public string Company 6public string ,
        public string Conectivity { get; set; }// null,
        public string Country { get; set; }// public string public string ,
        public string FactorConversion { get; set; }// null,
        public string Finish { get; set; }// public string public string ,
        public string fit { get; set; }// public string public string ,
        public string fixDiscount { get; set; }// public string public string ,
        public string flavor { get; set; }// public string public string ,
        public string HealthApplication { get; set; }// public string public string ,
        public string HomeDelivery { get; set; }// public string public string ,
        public string InstorePickup { get; set; }// public string public string ,
        public string LimiteVenta { get; set; }// public string public string ,
        public string mattressSize { get; set; }// public string public string ,
        public string netContent { get; set; }// public string 16public string ,
        public string NumberOfCameras { get; set; }// public string public string ,
        public string numberOfHDMIConnections { get; set; }// public string public string ,
        public string onlineExclusive { get; set; }// public string public string ,
        public string operatingSystem { get; set; }// public string public string ,
        public string percentDiscount { get; set; }// public string public string ,
        public string PermiteFraccion { get; set; }// null,
        public string pieceCount { get; set; }// public string public string ,
        public string Platform { get; set; }// public string public string ,
        public string priceRange { get; set; }// public string public string ,
        public string processor { get; set; }// public string public string ,
        public string ProductForm { get; set; }// public string public string ,
        public string projectionDistance { get; set; }// public string public string ,
        public string RecomendedCapacity { get; set; }// public string public string ,
        public string recommendedSpecies { get; set; }// public string public string ,
        public string redeemPointsDiscount { get; set; }// public string public string ,
        public string RedeemPointsFree { get; set; }// public string public string ,
        public string region { get; set; }// public string Región de cultivo|Valor 6public string ,
        public string resolution { get; set; }// public string public string ,
        public string rin { get; set; }// public string public string ,
        public string Season { get; set; }// public string public string ,
        public string ShadeRange { get; set; }// public string public string ,
        public string Size { get; set; }// public string public string ,
        public string sizeNumber { get; set; }// public string 6public string ,
        public string sizesizeNumber { get; set; }// public string public string ,
        public string sleeves { get; set; }// public string public string ,
        public string speed { get; set; }// public string public string ,
        public string spice { get; set; }// public string public string ,
        public string Stage { get; set; }// public string public string ,
        public string stateOfReadiness { get; set; }// public string public string ,
        public string storageCapacity { get; set; }// public string public string ,
        public string SuggestedAge { get; set; }// public string public string ,
        public string SurtidoAlmacen { get; set; }// public string public string ,
        public string SurtidoTienda { get; set; }// public string public string ,
        public string technology { get; set; }// public string technology 6public string ,
        public string UnidadConversion { get; set; }// public string public string ,
        public string UnidadVenta { get; set; }// public string public string ,
        public string varietal { get; set; }// public string public string ,
        public string weight { get; set; }// public string public string ,
        public CategoryModel Category { get; set; }// {

        public BrandModel Brand { get; set; }
        public List<PromotionsModels> Promotions { get; set; }
        public string ConversionRule { get; set; }// null


    }

    public class CategoryModel
    {
        public string CategoryId { get; set; }// public string cuidado-de-los-piespublic string ,
        public string CategoryDescription { get; set; }// public string Cuidado de los piespublic string     
    }
    public class BrandModel
    {

        public string BrandId { get; set; }// public string G867public string ,
        public string BrandDescription { get; set; }// public string UNESIApublic string 
    }
    public class PromotionsModels
    {
        public string Id { get; set; }// public string 100657198public string ,
        public string Badged { get; set; }// public string u2:'dobles'|'Tarjetas Participantes'public string         
    }
    public class OrderFacts_UEModel
    {
        public Int32 OrderNo { get; set; }
        public Int64 CnscOrder { get; set; }
        public Int32 StoreNum { get; set; }
        public String UeNo { get; set; }
        public Byte StatusUe { get; set; }
        public DateTime OrderDate { get; set; }
        public TimeSpan OrderTime { get; set; }
        public DateTime OrderDeliveryDate { get; set; }
        public TimeSpan OrderDeliveryTime { get; set; }
        public String CreatedBy { get; set; }
        public String DeliveryType { get; set; }
        public String UeType { get; set; }
        public Boolean IsPickingManual { get; set; }
        public Int32 CustomerNo { get; set; }
        public String CustomerName { get; set; }
        public String Phone { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String City { get; set; }
        public String StateCode { get; set; }
        public String PostalCode { get; set; }
        public String Reference { get; set; }
        public String NameReceives { get; set; }
        public Decimal Total { get; set; }
        public Int32 NumPoints { get; set; }
        public Int32 NumCashier { get; set; }
        public Int32 NumPos { get; set; }
        public String TransactionId { get; set; }
        public String MethodPayment { get; set; }
        public String CardNumber { get; set; }
        public String ShipperName { get; set; }
        public DateTime ShippingDate { get; set; }
        public String TransactionNo { get; set; }
        public String TrackingNo { get; set; }
        public Int32 NumBags { get; set; }
        public Int32 NumCoolers { get; set; }
        public Int32 NumContainers { get; set; }
        public String Terminal { get; set; }
        public DateTime DeliveryDate { get; set; }
        public TimeSpan DeliveryTime { get; set; }
        public String IdReceive { get; set; }
        public String NameReceive { get; set; }
        public String Comments { get; set; }
        public Int32 Id_Supplier { get; set; }
        public DateTime PickingDateEnd { get; set; }
        public TimeSpan PickingTimeEnd { get; set; }
        public DateTime PaymentDateEnd { get; set; }
        public TimeSpan PaymentTimeEnd { get; set; }
        public DateTime PackingDate { get; set; }
        public TimeSpan PackingTime { get; set; }
        public DateTime CancelOrderDate { get; set; }
        public TimeSpan CancelOrderTime { get; set; }
        public DateTime CarrierDate { get; set; }
        public TimeSpan CarrierTime { get; set; }
        public String CancelCause { get; set; }
        public Int32 idOwner { get; set; }
        public Boolean BitRMA { get; set; }
        public Int32 OrderRMA { get; set; }
        public String UeNoRMA { get; set; }
        public Boolean BitBigT { get; set; }
        public Int16 SplitMethod { get; set; }
        public Int32 OriginType { get; set; }
        public Int64 idSupplierWH { get; set; }
        public Int64 idSupplierWHCode { get; set; }
        public String SupplierName { get; set; }
        public String SupplierWHName { get; set; }
        public Int32 CarrierIDCotize { get; set; }
        public Decimal CarrierCostCotize { get; set; }
        public Int32 CarrierIDFinal { get; set; }
        public Decimal CarrierCostFinal { get; set; }
        public DateTime WHPickedDate { get; set; }
        public TimeSpan WHPickedTime { get; set; }
        public DateTime ShippingEstDate { get; set; }
        public TimeSpan ShippingEstTime { get; set; }
        public String ConsignmentType { get; set; }
        //NVS
        public String SupplyStarted { get; set; }
        public String SupplyCompleted { get; set; }
        public String PaymentStart { get; set; }
        public String PaymentCompleted { get; set; }
        public String DeliveryMethod { get; set; }
        public String ShippingType { get; set; }
        public String StoreDescription { get; set; }
        //public String OrderDate { get; set; }
        //public String UeType { get; set; }



        public class AltaRMAModel
        {

            public int Id_Padre { get; set; }
            public int Id_Motivo { get; set; }
            public string Descripcion_Motivo { get; set; }
            //public string FecGasto { get; set; }
            //public string Kilometraje { get; set; }
            //public string CantidadGasto { get; set; }
            //public bool Estatus { get; set; }
            //public string CreateDate { get; set; }
            //public string CreateTime { get; set; }
            //public string activo { get; set; }
            //public string FecMovto { get; set; }
            //public string TimeMovto { get; set; }
            //public string created_user { get; set; }
            //public string modified_user { get; set; }

        }

    }
    public class CallCenterController : Controller
    {
        // GET: CallCenter
        public ActionResult CallCenter()
        {
            //GetServicios();

            ViewBag.tdas = DALServicesM.GetUN();

            return View();
        }

        public void GetServicios()
        {

            System.Data.DataSet ds = new DataSet();

            var _ConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings:MercurioDB");
            _ConnectionString = "Server=tcp:srvsqlmercurio.database.windows.net,1433;Initial Catalog=MercurioDesaDB;Persist Security Info=False;User ID=t_eliseogr;Password=El1530%.*314;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _ConnectionString = "Server=tcp:srvsqlmercurioqa.database.windows.net,1433;Initial Catalog=MercurioQaDB;Persist Security Info=False;User ID=t_eliseogr;Password=W3rcur10!QA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
            {
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("up_CorpPP_sel_ProcessPaymentWeb", cnn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;


                    using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                        dataAdapter.Fill(ds);


                    Session["listaServicios"] = ds;

                }
            }

        }

        public JsonResult TiendasCP(string cp)
        {
            try
            {
                System.Data.DataSet ds = new DataSet();

                var _ConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings:MercurioDB");
                _ConnectionString = "Server=tcp:srvsqlmercurio.database.windows.net,1433;Initial Catalog=MercurioDesaDB;Persist Security Info=False;User ID=t_eliseogr;Password=El1530%.*314;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                //_ConnectionString = "Server=tcp:srvsqlmercurioqa.database.windows.net,1433;Initial Catalog=MercurioQaDB;Persist Security Info=False;User ID=t_eliseogr;Password=W3rcur10!QA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("up_CorpCallCenter_sel_UNByCp", cnn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter param;

                        param = cmd.Parameters.Add("@Num_CP", SqlDbType.Int);
                        param.Value = cp;


                        using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);


                        Session["listaServicios"] = ds;

                    }
                }

                List<object> devolver = new List<object>();
                devolver.Add(new { id = 0, nombre = "Todas..." });

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow r in ds.Tables[0].Rows)
                        {

                            devolver.Add(new
                            {
                                id = r[0].ToString().Trim(),
                                nombre = r[1].ToString()
                            }
                                   );
                        }
                    }
                }


                return Json(devolver, JsonRequestBehavior.AllowGet);
                //return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message, urlRedirect = "" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult GetProduct(string product)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //var client = new RestClient("http://localhost:7071/api/Buscador_Producto?productId=" + product.Trim());
                var client = new RestClient("https://sorianacallcenterbuscadorqa.azurewebsites.net/api/Buscador_Producto?productId=" + product.Trim());

                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                List<ResponseBuscadorModel> response1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResponseBuscadorModel>>(response.Content);

                var result = new { Success = false, data = response1 };
                return Json(result, JsonRequestBehavior.AllowGet);
                //return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message, urlRedirect = "" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


        public List<OrderFacts_UEModel> getRams()
        {

            //var client = new RestClient("http://localhost:7071/api/GetRAM");
            var client = new RestClient("https://sorianacallcentergetramqa.azurewebsites.net/api/GetRAM");

            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            List<OrderFacts_UEModel> lista = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderFacts_UEModel>>(response.Content);

            return lista;
        }


        public ActionResult Historial()
        {
            //Session["listaRMAS"] = getRams();

            ViewBag.FecIni = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            ViewBag.FecFin = DateTime.Now.ToString("yyyy/MM/dd");

            return View();
        }

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

        public ActionResult RMA()
        {
            return View();
        }


        #region Estatus Rma
        public ActionResult EstatusRMA()
        {
            ViewBag.FecIni = DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd");
            ViewBag.FecFin = DateTime.Now.ToString("yyyy/MM/dd");
            return View();
        }

        public ActionResult GetEstatusRMA(DateTime FecIni, DateTime FecFin, int? OrderId)
        {
            try
            {

                var result = new
                {
                    Success = true,
                    resp = DataTableToModel.ConvertTo<AutorizacionShow>(DALCallCenter.up_Corp_cns_EstatusRma(FecIni, FecFin, OrderId).Tables[0])
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        //NVS


        #region Historial RMA
        public ActionResult GethistorialRMA(DateTime FecIni, DateTime FecFin, int? OrderId)
        {
            try
            {
                Session.Remove("CheckListProd");
                var list = DataTableToModel.ConvertTo<upCorpOms_Cns_OrdersByDates>(
                    DALHistorialRMA.upCorpOms_Cns_OrdersByDates(FecIni, FecFin, OrderId).Tables[0]);
                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetDetalleRma(int OrderId)
        {
            try
            {
                var urlbase = ConfigurationManager.AppSettings["call_center_cliente"].ToString();
                var list = DataTableToModel.ConvertTo<upCorpOms_Cns_OrdersByHistorical>(
                    DALCallCenter.upCorpOms_Cns_OrdersByHistorical(OrderId).Tables[0]);

                var tot = list.Sum(x => x.SubTotal);

                var cliente = string.Format("{0}/?order={1}", urlbase, OrderId);

                var result = new { Success = true, resp = list, total = tot, url = cliente };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FinalizarRma(int OrderId, int Operacion
            , string Desc, List<OrderDetailCap> Products, string UeType
            , int? IdTSolicitud, int? IdTmovimiento)
        {
            var cliente = string.Empty;
            try
            {
                var urlbase = ConfigurationManager.AppSettings["call_center_cliente"].ToString();
                string accion = "retorno";// Operacion == 5 ? "cancelar" : "retorno";

                int? EstatusRma = 1;
                int? ProcesoAut = 1;


                if (Operacion == 5)
                {
                    accion = "cancelar";
                    if (UeType.ToUpper().Equals("SETC"))
                    {
                        EstatusRma = 10;
                        ProcesoAut = 10;
                    }
                }

                var ds = DALCallCenter.up_Corp_cns_OrderInfo(OrderId);

                var orden = DataTableToModel.ConvertTo<Order>(ds.Tables[0]).First();
                var detalle = DataTableToModel.ConvertTo<OrderDetail>(ds.Tables[1]);

                if (string.IsNullOrEmpty(orden.Clientphone))
                { orden.Clientphone = string.Empty; }

                var id = DataTableToModel.ConvertTo<AutorizacionShow>(
                    DALCallCenter.up_Corp_ins_tbl_OrdenCancelada(
                        orden.Orderid, accion, "Call Center", orden.Clientid, orden.Clientemail, orden.Clientphone
                        , EstatusRma, ProcesoAut, IdTSolicitud, IdTmovimiento).Tables[0]).FirstOrDefault();

                cliente = string.Format("{0}/?order={1}", urlbase, id.Id_cancelacion);
                foreach (var item in detalle)
                {


                    if (Operacion != 5)
                    {
                        int quantity = Convert.ToInt32(item.Quantity);
                        //var i = Products.Select(x => x.ProductId == item.ProductId).ToList().FirstOrDefault();
                        foreach (var i in Products)
                        {
                            if (i.ProductId == item.ProductId)
                            { quantity = Convert.ToInt32(i.NewQuantity); }
                        }
                        DALCallCenter.up_Corp_ins_tbl_OrdenRetorno_Detalle(id.Id_cancelacion, orden.Orderid, item.ShipmentId, item.Position, quantity, item.ProductId, Desc);
                    }
                    else
                    {
                        decimal quantity = item.Quantity;
                        //var i = Products.Select(x => x.ProductId == item.ProductId).ToList().FirstOrDefault();
                        foreach (var i in Products)
                        {
                            if (i.ProductId == item.ProductId)
                            { quantity = i.NewQuantity; }
                        }
                        DALCallCenter.up_Corp_ins_tbl_OrdenCancelada_Detalle(id.Id_cancelacion, orden.Orderid, item.ShipmentId
                            , item.Position, quantity, item.ProductId, Desc);

                        if (UeType.ToUpper().Equals("SETC"))
                            Cancelacion(int.Parse(orden.Orderid));
                    }
                }


                if (Session["CheckListProd"] != null)
                {
                    List<ProdCheckList> lst = (List<ProdCheckList>)Session["CheckListProd"];
                    foreach (var item in lst)
                    {
                        DALCallCenter.Catalogo_Checklist_iUP(id.Id_cancelacion, item.IdPregunta, Convert.ToInt32(item.ProdId), Convert.ToBoolean(item.Resp));
                    }

                }





                var result = new { Success = true, url = cliente };
                return Json(result, JsonRequestBehavior.AllowGet);


            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message, url = cliente };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public void Cancelacion(int OrderId)
        {
            try
            {

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["api_Cancelacion"];



                var req = new { OrderID = OrderId.ToString() };
                //string json2 = string.Empty;
                //JavaScriptSerializer js = new JavaScriptSerializer();
                ////json2 = js.Serialize(o);
                //js = null;

                var json2 = JsonConvert.SerializeObject(new { OrderID = OrderId.ToString() });

                string PPSRequest = JsonConvert.SerializeObject(req);

                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "in_data: " + json2, false, null);

                Soriana.FWK.FmkTools.LoggerToFile.WriteToLogFile(Soriana.FWK.FmkTools.LogModes.LogError, Soriana.FWK.FmkTools.LogLevel.INFO, "Request: " + apiUrl, false, null);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, apiUrl, "", json2);

                if (r.code != "00")
                {
                    throw new Exception(r.message);
                }







            }
            catch (Exception x)
            {
                throw x;

            }
        }

        [HttpPost]
        public ActionResult AddCheckList(int[] values, string prodId)
        {
            try
            {


                if (Session["CheckListProd"] == null)
                {
                    List<ProdCheckList> lst = new List<ProdCheckList>();
                    for (int i = 0; i < values.Length; i++)
                    {
                        lst.Add(new ProdCheckList
                        {
                            IdPregunta = i + 1,
                            Resp = values[i],
                            ProdId = prodId
                        });
                    }
                    Session["CheckListProd"] = lst;
                }
                else
                {
                    var lst = (List<ProdCheckList>)Session["CheckListProd"];

                    lst.RemoveAll(x => x.ProdId == prodId);

                    for (int i = 0; i < values.Length; i++)
                    {
                        lst.Add(new ProdCheckList
                        {
                            IdPregunta = i,
                            Resp = values[i],
                            ProdId = prodId
                        });
                    }


                    Session["CheckListProd"] = lst;
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

        public ActionResult DelCheckList(string prodId)
        {
            try
            {


                if (Session["CheckListProd"] != null)
                {
                    var lst = (List<ProdCheckList>)Session["CheckListProd"];
                    lst.RemoveAll(x => x.ProdId == prodId);
                    Session["CheckListProd"] = lst;
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

        public ActionResult CheckList(int articulos)
        {
            try
            {
                int totListCh = 0;
                bool resp = false;


                if (Session["CheckListProd"] != null)
                {
                    List<ProdCheckList> lst = (List<ProdCheckList>)Session["CheckListProd"];
                    totListCh = lst.Select(x => x.ProdId).Distinct().Count();
                }

                if (totListCh == articulos) { resp = true; }


                var result = new { Success = true, resp = resp };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetMotivos(int Id_Padre)
        {
            try
            {
                var list = DataTableToModel.ConvertTo<MotivosRMAById>(
                  DALCallCenter.MotivosRMAById_sUp(Id_Padre).Tables[0]);


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

        #region Historial SF
        public ActionResult HistorialSF()
        {
            //Session["listaRMAS"] = getRams();

            ViewBag.FecIni = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            ViewBag.FecFin = DateTime.Now.ToString("yyyy/MM/dd");

            return View();
        }

        public ActionResult GethistorialSF(DateTime FecIni, DateTime FecFin, int? OrderId)
        {
            try
            {

                var list = DataTableToModel.ConvertTo<upCorpOms_Cns_OrdersByDates>(
                    DALCallCenter.upCorpOms_Cns_OrdersByDatesSF(FecIni, FecFin, OrderId).Tables[0]);
                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDetalleSF(int IdCancelacion, int idOrden)
        {
            try
            {

                var list = DataTableToModel.ConvertTo<upCorpOms_Cns_OrdersByHistorical>(
                    DALCallCenter.upCorpOms_Cns_OrdersByHistoricalSF(idOrden, IdCancelacion).Tables[0]);

                var tot = list.Sum(x => x.SubTotal);


                var result = new { Success = true, resp = list, total = tot };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FinalizarSF(int IdCancelacion
           , int IdTSolicitud, int IdTmovimiento)
        {
            var cliente = string.Empty;
            try
            {
                var urlbase = ConfigurationManager.AppSettings["call_center_cliente"].ToString();


                cliente = string.Format("{0}/?order={1}", urlbase, IdCancelacion);
                DALCallCenter.tbl_OrdenCancelada_uUp(IdCancelacion, IdTSolicitud, IdTmovimiento);

                var result = new { Success = true, url = cliente };
                return Json(result, JsonRequestBehavior.AllowGet);


            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message, url = cliente };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
        public static int diagonalDifference(List<List<int>> arr)
        {
            int resp = 0;


            return resp;
        }

        //NVS

        #region Alta RMA



        public ActionResult AltaRMA()
        {
            return View();
        }

        public ActionResult GetMotivosRMA()

        {

            try

            {

                var list = DataTableToModel.ConvertTo<AltaRMAModel>(DALHistorialRMA.MotivosRMA_sUp().Tables[0]);



                var result = new { Success = true, resp = list };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };

                return Json(result, JsonRequestBehavior.AllowGet);

            }



        }

        public ActionResult AddMotivos(int IdPadre, string descripcionmotivo)

        {

            try

            {

                string UserCreate = User.Identity.Name;



                DALHistorialRMA.MotivosRMA_iUP(IdPadre, descripcionmotivo, UserCreate);



                var result = new { Success = true };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };

                return Json(result, JsonRequestBehavior.AllowGet);

            }



        }

        public ActionResult EditMotivos(int IdPadre, string descripcionmotivo, int IdMotivo)

        {

            try

            {

                string UserCreate = User.Identity.Name;



                DALHistorialRMA.MotivosRMA_uUp(IdPadre, descripcionmotivo, IdMotivo, UserCreate);



                var result = new { Success = true };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };

                return Json(result, JsonRequestBehavior.AllowGet);

            }



        }

        public ActionResult DeleteMotivo(int IdMotivo)

        {

            try

            {
                string modified_user = User.Identity.Name;

                DALHistorialRMA.MotivosRMA_dUP(IdMotivo, modified_user);



                var result = new { Success = true };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };

                return Json(result, JsonRequestBehavior.AllowGet);

            }



        }



        #endregion


        #region Alta PEdido

        public ActionResult BuscarCliente(string Criterio = "")
        {
            try

            {

                bool exist = false;

                var list = DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetClientByName(Criterio).Tables[0]);

                if (list != null)
                {
                    exist = true;
                }

                var result = new
                {
                    Success = true
                    ,
                    resp = list
                    ,
                    existe = exist

                };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult VerificaCliente(string Criterio = "")
        {
            try

            {

                bool exist = false;

                var list = DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetClientByPhoneEmail(Criterio).Tables[0]).FirstOrDefault();

                if (list != null)
                {
                    exist = true;
                }

                var result = new
                {
                    Success = true
                    ,
                    resp = list
                    ,
                    existe = exist

                };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult AddClient(
            string Nom_Cte, string Ap_Materno, string Ap_Paterno, string Calle, string Nom_DirCTe,
            string Num_Ext, string Num_Int, string Ciudad, string Cod_Postal, string Colonia,
            string Telefono, string Id_Email, int Ids_Num_Edo
            )
        {
            try

            {

                GetClient list = new GetClient();

                list = DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetClientByPhoneEmail(Id_Email).Tables[0]).FirstOrDefault();

                if (list == null)
                {
                    int Id_Num_Cte = int.Parse(DALCallCenter.Cte_iUp(Nom_Cte, Ap_Paterno, Ap_Materno).Tables[0].Rows[0]["Id_Num_Cte"].ToString());

                    DALCallCenter.Email_iUp(Id_Num_Cte, Id_Email);
                    DALCallCenter.DirCte_iUp(Id_Num_Cte, 1, Ids_Num_Edo, Calle, Nom_DirCTe, Num_Ext, Num_Int, Ciudad, Cod_Postal, Colonia, Telefono);

                    list = DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetClientByPhoneEmail(Id_Email).Tables[0]).FirstOrDefault();
                }


                var result = new
                {
                    Success = true
                    ,
                    resp = list
                };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult GetDias(string fechaOriginal)
        {
            try
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
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
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

        public ActionResult GetHorasEntrega(string fechaOriginal, string fechaSelec)
        {

            try
            {


                var dias = HoraEntrga(fechaOriginal, fechaSelec);
                var result = new { Success = true, json = dias };
                return Json(result, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

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

        public ActionResult GetDirCteIdNumCte(int Id_Num_Cte)
        {
            try

            {



                var list = DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetDirCteIdNumCte(Id_Num_Cte).Tables[0]);



                var result = new
                {
                    Success = true
                    ,
                    resp = list


                };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }

        //Id_Cnsc_DirCTe
        public ActionResult GetDirDirCteIdDirCTe(int Id_Cnsc_DirCTe)
        {
            try

            {



                var list = DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetDirDirCteIdDirCTe(Id_Cnsc_DirCTe).Tables[0]).FirstOrDefault();



                var result = new
                {
                    Success = true
                    ,
                    resp = list


                };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult GetEstados()
        {
            try

            {



                var list = DataTableToModel.ConvertTo<Edos>(DALCallCenter.Estado_d_sUp(1).Tables[0]);



                var result = new
                {
                    Success = true
                    ,
                    resp = list


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
        public ActionResult FinalizarOrden(List<ArtCar_d> arts, List<CarObs> carObs, int idCliente, string diaEnt, string horaEnt
            , int metodoEnt, string metodoPago, string efectivo, string vales, int tda)
        {
            try

            {

                int id_Num_SrvEntrega = 0;
                //CREACION DEL CARRITO
                var Id_Num_Car = int.Parse(DALCallCenter.Car_iUp(tda).Tables[0].Rows[0][0].ToString());


                //DETALLE DEL CARRITO
                foreach (var item in arts)
                {
                    //ARTICULOS
                    DALCallCenter.ArtCar_d_iUp(Id_Num_Car, item.Id_Num_Sku, item.Cant_Unidades, item.Precio_VtaNormal, item.Precio_VtaOferta, item.Dcto);
                    //COMENTARIOS POR ARTICULO
                    DALCallCenter.ArtCar_Obser_iUp(Id_Num_Car, item.Id_Num_Sku, item.obs);
                }

                //OBSERVACIONES NIVEL CARRITO
                foreach (var item in carObs)
                {
                    DALCallCenter.CarObs_iUp(Id_Num_Car, item.Desc_CarObs);
                }

                switch (metodoEnt)
                {
                    //entrega en tienda
                    case 1:
                        id_Num_SrvEntrega = 3;
                        break;
                    //Domicilio
                    case 2:
                        id_Num_SrvEntrega = 2;
                        break;
                }

                var Id_Num_Orden = int.Parse(DALCallCenter.Orden_iUp(Id_Num_Car, idCliente, id_Num_SrvEntrega).Tables[0].Rows[0][0].ToString());

                DateTime Fec_Entrega = Convert.ToDateTime(string.Format("{0} {1}",diaEnt,horaEnt));

                DALCallCenter.CalEntrega_iUp(id_Num_SrvEntrega, tda, Id_Num_Orden, Fec_Entrega);


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
        #endregion
    }
}