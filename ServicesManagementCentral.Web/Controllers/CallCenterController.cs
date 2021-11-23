using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using ServicesManagement.Web.DAL.Autorizacion;
using ServicesManagement.Web.DAL.CallCenter;
using ServicesManagement.Web.DAL.DALHistorialRMA;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.Autorizacion;
using ServicesManagement.Web.Models.CallCenter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using static ServicesManagement.Web.Controllers.OrderFacts_UEModel;


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
        public decimal Price { get; set; }// 86.9, cambio a decimal
        public decimal PromotionalPrice { get; set; }// 0.0, cambio a decimal
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
        public List<CategoryModel> categories { get; set; }// {

        public BrandModel Brand { get; set; }
        public List<PromotionsModels> Promotions { get; set; }
        public ConvertionRuleModel ConversionRule { get; set; }// null


    }

    public class ConvertionRuleModel
    {
        public string baseUnit { get; set; }
        public string equivalentUnit { get; set; }
        public decimal factorConversion { get; set; }
        public bool permiteFraccion { get; set; }

    }
    public class Combo
    {
        public string value { get; set; }
        public string text { get; set; }
    }
    public class AutoComplete
    {
        public string Barcode { get; set; }
        public string Description { get; set; }
        public string UrlImg { get; set; }
    }
    public class CategoryModel
    {
        public string categoryId { get; set; }// public string cuidado-de-los-piespublic string ,
        public string categoryDescription { get; set; }// public string Cuidado de los piespublic string
                                                       // 
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
        public int OrderNo { get; set; }
        public long CnscOrder { get; set; }
        public int StoreNum { get; set; }
        public string UeNo { get; set; }
        public byte StatusUe { get; set; }
        public DateTime OrderDate { get; set; }
        public TimeSpan OrderTime { get; set; }
        public DateTime OrderDeliveryDate { get; set; }
        public TimeSpan OrderDeliveryTime { get; set; }
        public string CreatedBy { get; set; }
        public string DeliveryType { get; set; }
        public string UeType { get; set; }
        public bool IsPickingManual { get; set; }
        public int CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string PostalCode { get; set; }
        public string Reference { get; set; }
        public string NameReceives { get; set; }
        public decimal Total { get; set; }
        public int NumPoints { get; set; }
        public int NumCashier { get; set; }
        public int NumPos { get; set; }
        public string TransactionId { get; set; }
        public string MethodPayment { get; set; }
        public string CardNumber { get; set; }
        public string ShipperName { get; set; }
        public DateTime ShippingDate { get; set; }
        public string TransactionNo { get; set; }
        public string TrackingNo { get; set; }
        public int NumBags { get; set; }
        public int NumCoolers { get; set; }
        public int NumContainers { get; set; }
        public string Terminal { get; set; }
        public DateTime DeliveryDate { get; set; }
        public TimeSpan DeliveryTime { get; set; }
        public string IdReceive { get; set; }
        public string NameReceive { get; set; }
        public string Comments { get; set; }
        public int Id_Supplier { get; set; }
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
        public string CancelCause { get; set; }
        public int idOwner { get; set; }
        public bool BitRMA { get; set; }
        public int OrderRMA { get; set; }
        public string UeNoRMA { get; set; }
        public bool BitBigT { get; set; }
        public short SplitMethod { get; set; }
        public int OriginType { get; set; }
        public long idSupplierWH { get; set; }
        public long idSupplierWHCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierWHName { get; set; }
        public int CarrierIDCotize { get; set; }
        public decimal CarrierCostCotize { get; set; }
        public int CarrierIDFinal { get; set; }
        public decimal CarrierCostFinal { get; set; }
        public DateTime WHPickedDate { get; set; }
        public TimeSpan WHPickedTime { get; set; }
        public DateTime ShippingEstDate { get; set; }
        public TimeSpan ShippingEstTime { get; set; }
        public string ConsignmentType { get; set; }
        //NVS
        public string SupplyStarted { get; set; }
        public string SupplyCompleted { get; set; }
        public string PaymentStart { get; set; }
        public string PaymentCompleted { get; set; }
        public string DeliveryMethod { get; set; }
        public string ShippingType { get; set; }
        public string StoreDescription { get; set; }
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

        public class LoyaltyModel
    {
        public string Cve_RespCode { get; set; }
        public string Desc_MensajeError { get; set; }
        public string cuenta { get; set; }
        public List<Balance> balances { get; set; }
        public string clientName { get; set; }
        public string cardType { get; set; }
        public string cardName { get; set; }
    }
    public class Balance
    {
        public string accountType { get; set; }
        public string balance { get; set; }
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

            string _ConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings:MercurioDB");
            _ConnectionString = "Server=tcp:srvsqlmercurio.database.windows.net,1433;Initial Catalog=MercurioDesaDB;Persist Security Info=False;User ID=t_eliseogr;Password=El1530%.*314;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            _ConnectionString = "Server=tcp:srvsqlmercurioqa.database.windows.net,1433;Initial Catalog=MercurioQaDB;Persist Security Info=False;User ID=t_eliseogr;Password=W3rcur10!QA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection(_ConnectionString))
            {
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("up_CorpPP_sel_ProcessPaymentWeb", cnn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;


                    using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                    {
                        dataAdapter.Fill(ds);
                    }

                    Session["listaServicios"] = ds;

                }
            }

        }

        public JsonResult TiendasCP(string cp)
        {
            try
            {
                System.Data.DataSet ds = new DataSet();

                string _ConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings:MercurioDB");
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
                        {
                            dataAdapter.Fill(ds);
                        }

                        Session["listaServicios"] = ds;

                    }
                }

                List<object> devolver = new List<object>
                {
                    new { id = 0, nombre = "Todas..." }
                };

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



        public ActionResult AutoComplete(string product, string tienda = "", string categoria = "")
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string urlApi = System.Configuration.ConfigurationManager.AppSettings["api_BuscadorCarrito"];
                string urlImg = System.Configuration.ConfigurationManager.AppSettings["api_ImgBuscadorCarrito"];
                string exteImg = System.Configuration.ConfigurationManager.AppSettings["api_ExtensionImgBuscadorCarrito"];

                string url = string.Format("{0}{1}&tienda={2}", urlApi, product.Trim(), tienda);
                if (categoria != "0")
                {
                    url = string.Format("{0}{1}&tienda={2}&filtro={3}", urlApi, product.Trim(), tienda, categoria);
                }


                RestClient client = new RestClient(url)
                {


                    //https://sorianacallcenterbuscadorqa.azurewebsites.net/api/Buscador_Producto?tienda=24&productId=coca

                    Timeout = -1
                };
                RestRequest request = new RestRequest(Method.POST);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                List<ResponseBuscadorModel> response1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResponseBuscadorModel>>(response.Content);

                List<AutoComplete> auto = new List<AutoComplete>();
                if (response1 != null)
                {
                    foreach (ResponseBuscadorModel item in response1)
                    {
                        auto.Add(new AutoComplete
                        {
                            Barcode = item.Barcode
                            ,
                            Description = item.Description
                            ,
                            UrlImg = string.Format("{0}{1}{2}", urlImg, item.Barcode, exteImg)
                        });
                    }
                }

                var result = new { data = auto, Success = true };
                //return Json(result, JsonRequestBehavior.AllowGet);
                //return Json("", JsonRequestBehavior.AllowGet);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message, urlRedirect = "" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetProduct(string product, string tienda, string categoria, string marca = "")
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string urlApi = System.Configuration.ConfigurationManager.AppSettings["api_BuscadorCarrito"];
                string urlImg = System.Configuration.ConfigurationManager.AppSettings["api_ImgBuscadorCarrito"];
                string exteImg = System.Configuration.ConfigurationManager.AppSettings["api_ExtensionImgBuscadorCarrito"];

                string url = string.Format("{0}{1}&tienda={2}", urlApi, product.Trim(), tienda);
                //if (categoria != "0")
                //{
                //    url = string.Format("{0}{1}&tienda={2}&filtro={3}", urlApi, product.Trim(), tienda, categoria);
                //}


                RestClient client = new RestClient(url)
                {


                    //https://sorianacallcenterbuscadorqa.azurewebsites.net/api/Buscador_Producto?tienda=24&productId=coca

                    Timeout = -1
                };
                RestRequest request = new RestRequest(Method.POST);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                List<ResponseBuscadorModel> response1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResponseBuscadorModel>>(response.Content);

                List<string> marcas = new List<string>();
                List<string> categorias = new List<string>();

                if (response1 != null)
                {
                    response1.All(x => { x.UrlImage = string.Format("{0}{1}{2}", urlImg, x.Barcode, exteImg); return true; });


                    if (!string.IsNullOrEmpty(categoria) && categoria != "0")
                    {
                        List<ResponseBuscadorModel> r = new List<ResponseBuscadorModel>();
                        foreach (ResponseBuscadorModel item in response1)
                        {
                            if (item.categories.Count(x => x.categoryDescription == categoria) > 0)
                            {
                                r.Add(item);
                            }
                        }
                        response1 = r;

                    }

                    if (!string.IsNullOrEmpty(marca) && marca != "0")
                    {
                        response1 = response1.Where(x => x.Brand.BrandDescription == marca).ToList();
                    }



                    foreach (ResponseBuscadorModel item in response1)
                    {
                        if (!marcas.Contains(item.Brand.BrandDescription) && item.Brand.BrandDescription != null)
                        {
                            marcas.Add(item.Brand.BrandDescription);
                        }
                    }
                    foreach (ResponseBuscadorModel item in response1)
                    {
                        foreach (CategoryModel cat in item.categories)
                        {
                            if (!categorias.Contains(cat.categoryDescription) && cat.categoryDescription != null)
                            {
                                categorias.Add(cat.categoryDescription);
                            }
                        }


                    }
                }

                string view = RenderRazorViewToString(ControllerContext, "tblBuscador", response1);

                var result = new
                {

                    view = view,
                    Success = true,
                    marcas = marcas,
                    categ = categorias

                };


                //var result = new { marcas=marcas,categ=categorias};
                return Json(result, JsonRequestBehavior.AllowGet);
                ////return Json("", JsonRequestBehavior.AllowGet);
                //return PartialView("tblBuscador", response1);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message, urlRedirect = "" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public static string RenderRazorViewToString(ControllerContext controllerContext, string viewName, object model)
        {
            controllerContext.Controller.ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult ViewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                ViewContext ViewContext = new ViewContext(controllerContext, ViewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                ViewResult.View.Render(ViewContext, sw);
                ViewResult.ViewEngine.ReleaseView(controllerContext, ViewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public List<OrderFacts_UEModel> getRams()
        {

            //var client = new RestClient("http://localhost:7071/api/GetRAM");
            RestClient client = new RestClient("https://sorianacallcentergetramqa.azurewebsites.net/api/GetRAM")
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.GET);
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

        #endregion<

        //NVS


        #region Historial RMA
        public ActionResult GethistorialRMA(DateTime FecIni, DateTime FecFin, int? OrderId)
        {
            try
            {
                Session.Remove("CheckListProd");
                List<upCorpOms_Cns_OrdersByDates> list = DataTableToModel.ConvertTo<upCorpOms_Cns_OrdersByDates>(
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
                string urlbase = ConfigurationManager.AppSettings["call_center_cliente"].ToString();
                List<upCorpOms_Cns_OrdersByHistorical> list = DataTableToModel.ConvertTo<upCorpOms_Cns_OrdersByHistorical>(
                    DALCallCenter.upCorpOms_Cns_OrdersByHistorical(OrderId).Tables[0]);

                decimal tot = list.Sum(x => x.SubTotal);

                string cliente = string.Format("{0}/?order={1}", urlbase, OrderId);

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
            string cliente = string.Empty;
            try
            {
                string urlbase = ConfigurationManager.AppSettings["call_center_cliente"].ToString();
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

                DataSet ds = DALCallCenter.up_Corp_cns_OrderInfo(OrderId);

                Order orden = DataTableToModel.ConvertTo<Order>(ds.Tables[0]).First();
                List<OrderDetail> detalle = DataTableToModel.ConvertTo<OrderDetail>(ds.Tables[1]);

                if (string.IsNullOrEmpty(orden.Clientphone))
                { orden.Clientphone = string.Empty; }

                AutorizacionShow id = DataTableToModel.ConvertTo<AutorizacionShow>(
                    DALCallCenter.up_Corp_ins_tbl_OrdenCancelada(
                        orden.Orderid, accion, "Call Center", orden.Clientid, orden.Clientemail, orden.Clientphone
                        , EstatusRma, ProcesoAut, IdTSolicitud, IdTmovimiento).Tables[0]).FirstOrDefault();
                if (id.foto)
                {
                    cliente = string.Format("{0}/?order={1}", urlbase, id.Id_cancelacion);
                }
                else
                {
                    cliente = string.Format("Se ha generado con éxito el folio RMA numero : {0}", id.Id_cancelacion);
                }
                foreach (OrderDetail item in detalle)
                {


                    if (Operacion != 5)
                    {
                        int quantity = Convert.ToInt32(item.Quantity);
                        //var i = Products.Select(x => x.ProductId == item.ProductId).ToList().FirstOrDefault();
                        foreach (OrderDetailCap i in Products)
                        {
                            if (i.ProductId == item.ProductId)
                            {
                                quantity = Convert.ToInt32(i.NewQuantity);
                                DALCallCenter.up_Corp_ins_tbl_OrdenRetorno_Detalle(id.Id_cancelacion, orden.Orderid, item.ShipmentId, item.Position, quantity, item.ProductId, Desc);
                            }


                        }

                    }
                    else
                    {
                        decimal quantity = item.Quantity;
                        //var i = Products.Select(x => x.ProductId == item.ProductId).ToList().FirstOrDefault();
                        foreach (OrderDetailCap i in Products)
                        {
                            if (i.ProductId == item.ProductId)
                            {
                                quantity = i.NewQuantity;
                                DALCallCenter.up_Corp_ins_tbl_OrdenCancelada_Detalle(id.Id_cancelacion, orden.Orderid, item.ShipmentId
                                , item.Position, quantity, item.ProductId, Desc);
                            }
                        }
                        //if (UeType.ToUpper().Equals("SETC"))
                        //    Cancelacion(int.Parse(orden.Orderid));
                    }
                }

                if (Operacion == 5)
                {
                    string ShipmentId = detalle[0].ShipmentId;
                    DALAutorizacion.upCorpOms_Del_UeNoSupplyProcess(OrderId, Desc, 1, ShipmentId);
                    if (UeType.ToUpper().Equals("SETC"))
                    {
                        if (DALCallCenter.up_PPS_Sel_PaymenTransactionOrderCancellation(orden.Orderid).Tables[0].Rows.Count > 0)
                        { Cancelacion(int.Parse(orden.Orderid)); }
                    }
                }

                if (Session["CheckListProd"] != null)
                {
                    List<ProdCheckList> lst = (List<ProdCheckList>)Session["CheckListProd"];
                    foreach (ProdCheckList item in lst)
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

                string json2 = JsonConvert.SerializeObject(new { OrderID = OrderId.ToString() });

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
                    List<ProdCheckList> lst = (List<ProdCheckList>)Session["CheckListProd"];

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
                    List<ProdCheckList> lst = (List<ProdCheckList>)Session["CheckListProd"];
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
                List<MotivosRMAById> list = DataTableToModel.ConvertTo<MotivosRMAById>(
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
                List<upCorpOms_Cns_OrdersByDates> list = DataTableToModel.ConvertTo<upCorpOms_Cns_OrdersByDates>(
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

                List<upCorpOms_Cns_OrdersByHistorical> list = DataTableToModel.ConvertTo<upCorpOms_Cns_OrdersByHistorical>(
                    DALCallCenter.upCorpOms_Cns_OrdersByHistoricalSF(idOrden, IdCancelacion).Tables[0]);

                decimal tot = list.Sum(x => x.SubTotal);


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
            string cliente = string.Empty;
            try
            {
                string urlbase = ConfigurationManager.AppSettings["call_center_cliente"].ToString();


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

                List<AltaRMAModel> list = DataTableToModel.ConvertTo<AltaRMAModel>(DALHistorialRMA.MotivosRMA_sUp().Tables[0]);



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

                List<GetClient> list = DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetClientByName(Criterio).Tables[0]);

                if (list != null && list.Count > 0)
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

        [HttpPost]
        public ActionResult BuscarCliente2(string Criterio = "")
        {
            //logistica datatable
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault().ToLower();

            bool existe = false;

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;



            List<GetClient> query = new List<GetClient>() ;

            foreach (DataRow item in DALCallCenter.GetClientByPhoneEmail(Criterio).Tables[0].Rows)
            {
                query.Add(
                    new GetClient
                    {
                        Nom_Cte=item["NomCompleto"].ToString(),
                        Id_Email = item["Id_Email"].ToString(),
                        Direccion = item["Direccion"].ToString(),
                        Telefono = item["Telefono"].ToString(),
                        Id_Cnsc_DirCTe= item["Id_Cnsc_DirCTe"].ToString()
                    }
                    );
            }



            if (query.Count == 0)
            {
                foreach (DataRow item in DALCallCenter.GetClientByName(Criterio).Tables[0].Rows)
                {
                    query.Add(
                        new GetClient
                        {
                            Nom_Cte = item["NomCompleto"].ToString(),
                            Id_Email = item["Id_Email"].ToString(),
                            Direccion = item["Direccion"].ToString(),
                            Telefono = item["Telefono"].ToString(),
                            Id_Cnsc_DirCTe = item["Id_Cnsc_DirCTe"].ToString()
                        }
                        );
                }
            }


            if (searchValue != "")
                query = query.Where(d => d.Nom_Cte.ToLower().Contains(searchValue)
                || d.Id_Email.ToString().ToLower().Contains(searchValue)
                || d.Direccion.ToString().ToLower().Contains(searchValue)
                || d.Telefono.ToString().ToLower().Contains(searchValue)

                ).ToList();


            //{ data: "Nom_Cte" },
            //{ data: "Id_Email" },
            //{ data: "Direccion" },
            //{ data: "Telefono" },
            //{ data: "Telefono" },


            recordsTotal = query.Count();

            if (recordsTotal > 0) { existe = true; }

            //Sorting    
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    query = query.OrderBy(sortColumn + " " + sortColumnDir).ToList();

            //}




            List<GetClient> data = query.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data,
                Success = true
                    ,
                existe = existe
            },
                JsonRequestBehavior.AllowGet);
        }
        public ActionResult VerificaCliente(string Criterio = "", string Id_Cnsc_DirCTe = "")
        {
            try

            {

                bool exist = false;
                int total = 0;

                GetClient list = new GetClient();



                if (string.IsNullOrEmpty(Id_Cnsc_DirCTe))
                {

                    List<GetClient> res = DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetClientByPhoneEmail(Criterio).Tables[0]);
                    total = res.Count();
                    list = res.FirstOrDefault();
                }
                else
                {
                    list = DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetClientByPhoneEmail(Criterio).Tables[0])
                            .Where(x => x.Id_Cnsc_DirCTe == Id_Cnsc_DirCTe).FirstOrDefault();

                    //foreach (var item in DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetClientByPhoneEmail(Criterio).Tables[0]))
                    //{
                    //    if (item.Id_Cnsc_DirCTe == Id_Cnsc_DirCTe)
                    //    {
                    //        list = item;
                    //        break;
                    //    }
                    //}

                }
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
                    ,
                    total = total


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
            string Telefono, string Id_Email, string TarjetaLealtad, int Ids_Num_Edo, int Id_Cnsc_DirCTe, int Id_Num_Cte = 0) //Lealtad
        {
            try

            {

                GetClient list = new GetClient();

                list = DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetClientByPhoneEmail(Telefono).Tables[0]).FirstOrDefault();

                if (list == null && Id_Cnsc_DirCTe == 0 && Id_Num_Cte == 0)
                //if ( Id_Cnsc_DirCTe == 0 && Id_Num_Cte == 0)
                {


                    Id_Num_Cte = int.Parse(DALCallCenter.Cte_iUp(Nom_Cte, Ap_Paterno, Ap_Materno).Tables[0].Rows[0]["Id_Num_Cte"].ToString());

                    DALCallCenter.Email_iUp(Id_Num_Cte, Id_Email, TarjetaLealtad); //Lealtad
                    DALCallCenter.DirCte_iUp(Id_Num_Cte, 1, Ids_Num_Edo, Calle, Nom_DirCTe, Num_Ext, Num_Int, Ciudad, Cod_Postal, Colonia, Telefono);
                    list = DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetClientByPhoneEmail(Telefono).Tables[0]).FirstOrDefault();




                }
                else
                {
                    if (Id_Cnsc_DirCTe != 0 && Id_Num_Cte != 0)
                    {

                        DALCallCenter.Cte_uUp(Id_Num_Cte, Nom_Cte, Ap_Paterno, Ap_Materno);
                        DALCallCenter.Email_uUp(Id_Num_Cte, Id_Email, TarjetaLealtad); //Lealtad
                        DALCallCenter.DirCte_uUp(Id_Cnsc_DirCTe, Id_Num_Cte, 1, Ids_Num_Edo, Calle, Nom_DirCTe, Num_Ext, Num_Int, Ciudad, Cod_Postal, Colonia, Telefono);
                        list = DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetClientByPhoneEmail(Telefono).Tables[0])
                            .Where(x => x.Id_Cnsc_DirCTe == Id_Cnsc_DirCTe.ToString()).FirstOrDefault();

                    }
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

        public ActionResult GetDias(string fechaOriginal = "")
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


                string fechaActual = DateTime.Now.ToString("yyyy/MM/dd");
                if (string.IsNullOrEmpty(fechaOriginal))
                {
                    fechaOriginal = DateTime.Now.ToString("yyyy/MM/dd");
                }


                List<Dias> horas = HoraEntrga(fechaOriginal, fechaActual);
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



            //if (fechaSelec.ToUpper().Equals("HOY"))
            if (fechaOriginal == fechaSelec)
            {
                //var fec = Convert.ToDateTime(fechaOriginal).ToString("dd/MM/yyyy") + " " + DateTime.Now.AddHours(-5).Hour.ToString() + ":00";
               // string fec = DateTime.Now.AddHours(-5).ToString();
                string fec = DateTime.Now.AddHours(-5).ToString();
                hora = Convert.ToDateTime(fec).AddHours(4).Hour;

            }

            if (hora > 8 & hora < 23)
            {
                for (int i = hora; i < 22; i++)
                {
                    Dias dia = new Dias
                    {
                        fecha = string.Format("{0}:01:00", i),
                        NombeDia = string.Format("{0}:01:00 - {1}:00:00", i, i + 1)
                    };

                    dias.Add(dia);
                }
            }
            return dias;
        }

        public ActionResult GetHorasEntrega(string fechaSelec)
        {

            try
            {
                var fecActual = DateTime.Now.AddHours(-5).ToString("yyyy/MM/dd");


                List<Dias> dias = HoraEntrga(fecActual, fechaSelec);
                var result = new { Success = true, json = dias, fecAct= fecActual,fechaSelec=fechaSelec };
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



                List<GetClient> list = DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetDirCteIdNumCte(Id_Num_Cte).Tables[0]);



                var result = new
                {
                    Success = true
                    ,
                    resp = list
                    ,
                    total = list.Count

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
        public ActionResult GetDirDirCteIdDirCTe(int Id_Cnsc_DirCTe, int Id_Num_Cte)
        {
            try

            {



                GetClient list = DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetDirDirCteIdDirCTe(Id_Cnsc_DirCTe, Id_Num_Cte).Tables[0]).FirstOrDefault();



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



                List<Edos> list = DataTableToModel.ConvertTo<Edos>(DALCallCenter.Estado_d_sUp(1).Tables[0]);



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
            , int metodoEnt, string metodoPago, decimal efectivo, decimal vales, int tda
            , string Calle, string Nom_DirCTe,
            string Num_Ext, string Num_Int, string Ciudad, string Cod_Postal, string Colonia,
            string Telefono, int Ids_Num_Edo, int idDirPrinCte)
        {
            try

            {

                decimal TotOrden = 0;

                //foreach (var item in arts)
                //{

                //    TotOrden += item.Cant_Unidades * item.Precio_VtaNormal;
                //}

                if (metodoPago == "efectivo")
                {
                    if ((efectivo + vales) == 0)
                    {
                        throw new Exception("La cantidad de efectivo y vales no pude ser 0");

                    }
                }


                int id_Num_SrvEntrega = 0;
                //CREACION DEL CARRITO
                int Id_Num_Car = int.Parse(DALCallCenter.Car_iUp(tda).Tables[0].Rows[0][0].ToString());


                //DETALLE DEL CARRITO
                foreach (ArtCar_d item in arts)
                {
                    //ARTICULOS
                    string[] descripcion = item.Desc_art.Split('<');

                    decimal descuento = item.Precio_VtaNormal - item.Precio_VtaOferta;

                    decimal Cant_Piezas = 0;
                    if (item.Factor > 0)
                    {
                        if (item.Cve_UnVta == "pza")
                        {
                            Cant_Piezas = item.Cant_Unidades;

                            item.Cant_Unidades = (item.Cant_Unidades / item.Factor); ;
                            item.Cve_UnVta = "Kg";
                        }

                    }


                    DALCallCenter.ArtCar_d_iUp(Id_Num_Car, item.Id_Num_Sku, item.Cant_Unidades, item.Precio_VtaNormal
                        , item.Precio_VtaOferta, descuento, descripcion[0], item.Cve_UnVta, item.Num_CodBarra);


                    //SE AGREGA QUE EL ARTICULO SE PIDIO EN PIEZAS PARA QUE VIAJE EN OMS
                    if (Cant_Piezas > 0)
                    {
                        DALCallCenter.ArtCar_o_iUp(Id_Num_Car, item.Id_Num_Sku, Cant_Piezas, item.Factor);
                    }

                    if (item.Precio_VtaOferta < item.Precio_VtaNormal)
                    {
                        TotOrden += item.Cant_Unidades * item.Precio_VtaOferta;
                    }
                    else
                    {
                        TotOrden += item.Cant_Unidades * item.Precio_VtaNormal;
                    }


                    //COMENTARIOS POR ARTICULO
                    if (item.obs != null)
                    { DALCallCenter.ArtCar_Obser_iUp(Id_Num_Car, item.Id_Num_Sku, item.obs); }
                }

                //OBSERVACIONES NIVEL CARRITO
                if (carObs != null)
                {
                    foreach (CarObs item in carObs)
                    {
                        DALCallCenter.CarObs_iUp(Id_Num_Car, item.Desc_CarObs);
                    }
                }
                else
                {
                    DALCallCenter.CarObs_iUp(Id_Num_Car, "");
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

                //SE CREA LA ORDEN
                int Id_Num_Orden = int.Parse(DALCallCenter.Orden_iUp(Id_Num_Car, idCliente, id_Num_SrvEntrega).Tables[0].Rows[0][0].ToString());

                //SE REGRISTRA LA FECHA DE ENTREGA
                DateTime Fec_Entrega = Convert.ToDateTime(string.Format("{0} {1}", diaEnt, horaEnt));

                //SE GUARDA LA FECCHA DE ENTREGA
                DALCallCenter.CalEntrega_iUp(id_Num_SrvEntrega, tda, Id_Num_Orden, Fec_Entrega.ToString("yyyy-MM-dd HH:mm"));

                //REGISTRAMOS/CONSULTAMOS LA DIRECCION DE ENTREGA EN LA TABLA DIRCTE
                if (metodoEnt == 2)
                {
                    int Id_Cnsc_DirCTe = int.Parse(DALCallCenter.DirCteEnt_iUp(idCliente, 1, Ids_Num_Edo, Calle, Nom_DirCTe, Num_Ext, Num_Int
                        , Ciudad, Cod_Postal, Colonia, Telefono, "").Tables[0].Rows[0][0].ToString());

                    //REGISTRAMOS LA DIRECCION ENTREGA
                    DALCallCenter.DirEnt_iUp(Id_Num_Orden, idCliente, Id_Cnsc_DirCTe);
                }
                else
                {
                    //entrega en tienda
                    string list = DataTableToModel.ConvertTo<GetClient>(DALCallCenter.GetDirCteIdNumCte(idCliente).Tables[0]).Min(y => y.Id_Cnsc_DirCTe);
                    //REGISTRAMOS LA DIRECCION ENTREGA
                    DALCallCenter.DirEnt_iUp(Id_Num_Orden, idCliente, int.Parse(list));

                }

                int Id_Cnsc_FormaPagoCte_d = 0;
                switch (metodoPago.ToLower())
                {
                    case "efectivo":
                        //5 Efectivo
                        //6 Vales de despensa

                        if (efectivo > 0)
                        {
                            Id_Cnsc_FormaPagoCte_d = int.Parse(DALCallCenter.FormaPagoCte_d_iUp(idCliente, 5, DateTime.Now).Tables[0].Rows[0][0].ToString());
                            DALCallCenter.OrdenPago_iUp(idCliente, Id_Cnsc_FormaPagoCte_d, Id_Num_Orden, DateTime.Now, efectivo);
                        }
                        if (vales > 0)
                        {
                            Id_Cnsc_FormaPagoCte_d = int.Parse(DALCallCenter.FormaPagoCte_d_iUp(idCliente, 6, DateTime.Now).Tables[0].Rows[0][0].ToString());
                            DALCallCenter.OrdenPago_iUp(idCliente, Id_Cnsc_FormaPagoCte_d, Id_Num_Orden, DateTime.Now, vales);
                        }



                        break;

                    case "debito": //22  Tarjeta de débito contra entrega
                        Id_Cnsc_FormaPagoCte_d = int.Parse(DALCallCenter.FormaPagoCte_d_iUp(idCliente, 22, DateTime.Now).Tables[0].Rows[0][0].ToString());
                        DALCallCenter.OrdenPago_iUp(idCliente, Id_Cnsc_FormaPagoCte_d, Id_Num_Orden, DateTime.Now, TotOrden);
                        break;
                    case "credito":  //21  Tarjeta de crédito contra entrega


                        Id_Cnsc_FormaPagoCte_d = int.Parse(DALCallCenter.FormaPagoCte_d_iUp(idCliente, 21, DateTime.Now).Tables[0].Rows[0][0].ToString());
                        DALCallCenter.OrdenPago_iUp(idCliente, Id_Cnsc_FormaPagoCte_d, Id_Num_Orden, DateTime.Now, TotOrden);
                        break;
                }

                #region Llamado al APi
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["api_AltaCarrito"];
                DataSet ds = DALCallCenter.sp_OMSGetOrderDetails(Id_Num_Orden, Nom_DirCTe, idDirPrinCte);


                OrdersToXML obj = new OrdersToXML();
                string x = obj.CreateXMLDocument(ds, Id_Num_Orden.ToString()).ToString().Replace("\"", "'");
                OrderJson OrderToMicroService = new OrderJson
                {
                    xmlOrden = x
                };

                // Serializar el mensaje en formato Json
                string jsonString = JsonConvert.SerializeObject(OrderToMicroService);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, apiUrl, "", jsonString);

                if (r.code != "00")
                {
                    throw new Exception(r.message);
                }
                else
                {
                    JObject json = JObject.Parse(r.message);
                    if (json.TryGetValue("status-description-detail", out JToken v1))
                    {
                        if (JObject.Parse(v1.ToString()).TryGetValue("numeroOrden", out JToken v2))
                        {
                            Id_Num_Orden = int.Parse(v2.ToString().Replace("{", "").Replace("}", ""));
                        }
                    }
                }
                #endregion

                var result = new
                {
                    Success = true
                    ,
                    Message = string.Format("La Orden No. {0} fue generada con éxito", Id_Num_Orden)
                };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }

        //public ActionResult XML(int orderid)
        //{
        //    try

        //    {

        //        int Id_Num_Orden = orderid;
        //        var ds = DALCallCenter.sp_OMSGetOrderDetails(Id_Num_Orden,"",0);


        //        OrdersToXML obj = new OrdersToXML();
        //        string x = obj.CreateXMLDocument(ds, Id_Num_Orden.ToString()).ToString().Replace("\"", "'");
        //        var OrderToMicroService = new OrderJson
        //        {
        //            xmlOrden = x
        //        };

        //        // Serializar el mensaje en formato Json
        //        string jsonString = JsonConvert.SerializeObject(OrderToMicroService);


        //        var result = new
        //        {
        //            Success = true



        //        };

        //        return Json(result, JsonRequestBehavior.AllowGet);

        //    }

        //    catch (Exception x)

        //    {

        //        var result = new { Success = false, Message = x.Message };
        //        return Json(result, JsonRequestBehavior.AllowGet);

        //    }
        //}

        public ActionResult GetLoyaltyPoints(string Cliente)
        {
            try
            {
                string Accion = "9";
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["call_center_LoyaltyAccount"];
                var req = new { Action = Accion, ClientId = Cliente.ToString() };
                string json2 = JsonConvert.SerializeObject(new { Action = Accion, ClientId = Cliente.ToString() });


                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, apiUrl, "", json2);

                string json3 = JsonConvert.SerializeObject(r.message);
                LoyaltyModel LoyaltyT = JsonConvert.DeserializeObject<LoyaltyModel>(r.message);

                var result = new
                {
                    Success = true,
                    resp = LoyaltyT
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public bool TieneCobertura(int tienda, string codigopostal)
        {
            bool puede = DALCallCenter.upCorpOms_Cns_CoberturaTienda(tienda, codigopostal);
            return puede;
        }

        public ActionResult GetCategorias()
        {
            try
            {
                string urlApi = System.Configuration.ConfigurationManager.AppSettings["call_center_busqueda_categorias"];
                //"https://sorianacallcenterbuscadorqa.azurewebsites.net/api/Buscador_Categorias"
                RestClient client = new RestClient(urlApi)
                {
                    Timeout = -1
                };
                RestRequest request = new RestRequest(Method.POST);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                List<CategoriasModels> ListP = JsonConvert.DeserializeObject<List<CategoriasModels>>(response.Content);

                var result = new
                {
                    Success = true
                      ,
                    resp = ListP
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetFlete(int StoreNum, string CodigoPostal = "")
        {
            try
            {
                if (TieneCobertura(StoreNum, CodigoPostal))
                {
                    CostoFlete ListP = DataTableToModel.ConvertTo<CostoFlete>(DALCallCenter.Selecciona_CostoFlete_sUp(StoreNum).Tables[0]).FirstOrDefault();

                    var result = new
                    {
                        Success = true
                          ,
                        resp = ListP
                    };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    var result = new
                    {
                        Success = false
                          ,
                        Message = "Código Postal fuera de la cobertura de la tienda"
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

        public ActionResult GetHistorial(int Id_Num_Cte)
        {
            try
            {

                List<Historial> ListP = DataTableToModel.ConvertTo<Historial>(DALCallCenter.HistorialOrdenes_sUP(Id_Num_Cte).Tables[0]);

                foreach (Historial item in ListP)
                {
                    HistorialTotales tot = DataTableToModel.ConvertTo<HistorialTotales>(DALServicesM.GetOrdersByOrderNo(item.UeNo).Tables[6]).FirstOrDefault();
                    //try
                    //{
                    //    item.Imp_Pago = decimal.Parse(tot.TotPagado);
                    //}
                    //catch (Exception ex )
                    //{

                    //    throw ex;
                    //}
                    if (tot.TotPagado == null)
                    {
                        item.Imp_Pago = 0;
                    }
                    else
                    { item.Imp_Pago = decimal.Parse(tot.TotPagado); }
                }


                var result = new
                {
                    Success = true
                      ,
                    resp = ListP
                };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult HistorialDetalle(string UeNo = "")
        {
            try
            {

                HistorialOrden orden = new HistorialOrden();

                if (!string.IsNullOrEmpty(UeNo.Trim()))
                {
                    DataSet ds = DALServicesM.GetOrdersByOrderNo(UeNo);



                    orden = DataTableToModel.ConvertTo<HistorialOrden>(ds.Tables[0]).FirstOrDefault();
                    //ds.Tables[5];
                    orden.ArtNoSurtidos = DataTableToModel.ConvertTo<HistorialOrdenArt>(ds.Tables[5]);
                    orden.ArtSurtidos = DataTableToModel.ConvertTo<HistorialOrdenArt>(ds.Tables[4]);
                    orden.Totales = DataTableToModel.ConvertTo<HistorialTotales>(ds.Tables[6]).FirstOrDefault();
                }

                var result = new
                {
                    Success = true
                      ,
                    orden = orden
                };

                //return View("HistorialDetalle", orden);
                return PartialView("HistorialDetalle", orden);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult EcharCarrito(string UeNo, string tienda)
        {
            try
            {

                List<ResponseBuscadorModel> productos = new List<ResponseBuscadorModel>();


                DataSet ds = DALServicesM.GetOrdersByOrderNo(UeNo);


                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string urlApi = System.Configuration.ConfigurationManager.AppSettings["api_BuscadorCarrito"];
                string urlImg = System.Configuration.ConfigurationManager.AppSettings["api_ImgBuscadorCarrito"];
                string exteImg = System.Configuration.ConfigurationManager.AppSettings["api_ExtensionImgBuscadorCarrito"];

                //orden.ArtNoSurtidos = DataTableToModel.ConvertTo<HistorialOrdenArt>(ds.Tables[5]);
                //orden.ArtSurtidos = DataTableToModel.ConvertTo<HistorialOrdenArt>(ds.Tables[4]);
                foreach (DataRow item in ds.Tables[5].Rows)
                {


                    string product = item["Barcode"].ToString();
                    string url = string.Format("{0}{1}&tienda={2}", urlApi, product.Trim(), tienda);



                    RestClient client = new RestClient(url)
                    {
                        Timeout = -1
                    };
                    RestRequest request = new RestRequest(Method.POST);
                    IRestResponse response = client.Execute(request);



                    List<ResponseBuscadorModel> response1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResponseBuscadorModel>>(response.Content);

                    if (response1 != null)
                    {
                        if (response1.Count > 0)
                        {
                            response1.All(x => { x.UrlImage = string.Format("{0}{1}{2}", urlImg, x.Barcode, exteImg); return true; });

                            productos.Add(response1.FirstOrDefault());
                        }
                    }


                }
                foreach (DataRow item in ds.Tables[4].Rows)
                {


                    string product = item["Barcode"].ToString();
                    string url = string.Format("{0}{1}&tienda={2}", urlApi, product.Trim(), tienda);



                    RestClient client = new RestClient(url)
                    {
                        Timeout = -1
                    };
                    RestRequest request = new RestRequest(Method.POST);
                    IRestResponse response = client.Execute(request);



                    List<ResponseBuscadorModel> response1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResponseBuscadorModel>>(response.Content);

                    if (response1 != null)
                    {
                        if (response1.Count > 0)
                        {
                            response1.All(x => { x.UrlImage = string.Format("{0}{1}{2}", urlImg, x.Barcode, exteImg); return true; });

                            productos.Add(response1.FirstOrDefault());
                        }
                    }


                }



                var result = new { Success = true, resp = productos };
                return Json(result, JsonRequestBehavior.AllowGet);


            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }


        public ActionResult DelDirCte(int Id_Cnsc_DirCTe, int Id_Num_Cte)
        {
            try
            {


                string msj = DALCallCenter.DirCte_dUp(Id_Num_Cte, Id_Cnsc_DirCTe).Tables[0].Rows[0][0].ToString();


                if (!msj.Equals("ok"))
                {
                    throw new Exception(msj);
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
        #endregion
    }
}