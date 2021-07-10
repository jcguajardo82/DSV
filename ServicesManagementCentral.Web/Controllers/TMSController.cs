using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net;
using ServicesManagement.Web.Models;
using ServicesManagement.Web.DAL.Vehiculos;
using ServicesManagement.Web.Helpers;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class TMSController : Controller
    {
        string UrlApi = ConfigurationManager.AppSettings["api_TMS"].ToString();
        // GET: TMS
        public ActionResult Cat_Vehiculo()
        {
            Session["lista"] = DALVehiculos.up_CorpTMS_Sel_TipoVehiculo(); //GetTipoVehiculos();
            Session["listaV"] = GetVehiculos_index();

            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetVehiculos()
        {
            try
            {
                string apiUrl = string.Format("{0}/GetVehiculos", UrlApi);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                Soriana.FWK.FmkTools.RestResponse responseApi1 = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.GET, apiUrl, null, "");

                if (responseApi1.code.Equals("00"))
                {
                    List<VehiculoModel> listC = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VehiculoModel>>(responseApi1.message);

                    var result = new { Success = true, json = listC };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var result1 = new { Success = true };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        public DataSet GetVehiculos_index()
        {



            DataSet ds = new DataSet();

            try
            {

                using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("up_TMS_sel_Vehiculo", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);
                    }
                }
                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

            return ds;


        }
        
        [HttpGet]
        public async Task<JsonResult> InsVehiculo(string descripcion, string motor, string placas, string idTipoVehiculo)
        {
            try
            {
                
                try
                {
                    string CreatedId = User.Identity.Name;

                    Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                    System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                    parametros.Add("@Descripcion", descripcion);
                    parametros.Add("@Placas", placas);
                    parametros.Add("@Motor", motor);
                    parametros.Add("@Id_TipoVehiculo", idTipoVehiculo);
                    parametros.Add("@user", CreatedId);
                    //parametros.Add("@Marca", marca);
                    //parametros.Add("@Anio", anio);


                    Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "tms.up_CorpTMS_ins_Vehiculo", false, parametros);


                    //return ds;
                }
                catch (SqlException ex)
                {

                    throw ex;
                }
                catch (System.Exception ex)
                {

                    throw ex;
                }


                var result1 = new { Success = true };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetVehiculoById(string Id)
        {
            try
            {
                //string apiUrl = string.Format("{0}/GetVehiculos", UrlApi) + "?Id_Vehiculo=" + Id;
                string apiUrl = string.Format("{0}/GetVehiculos", UrlApi);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //VehiculoModel v = new VehiculoModel { Descripcion = descripcion, Placas = placas, Motor = motor };

                Soriana.FWK.FmkTools.RestResponse responseApi1 = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.GET, apiUrl, null, "");

                if (responseApi1.code.Equals("00"))
                {
                    //VehiculoModel listC = Newtonsoft.Json.JsonConvert.DeserializeObject<VehiculoModel>(responseApi1.message);
                    List<VehiculoModel> listC = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VehiculoModel>>(responseApi1.message);



                    var result = new { Success = true, json = listC.First(c => c.Id_Vehiculo == Convert.ToInt32(Id)) };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { Success = false, Message = "Error al ejecutar la accion" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var result1 = new { Success = true };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> EditVehiculo(string Id, string descripcion, string motor, string placas, string estatus)
        {
            try
            {
                string apiUrl = string.Format("{0}/UpdVehiculo", UrlApi);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                VehiculoModel v = new VehiculoModel { Id_Vehiculo = Convert.ToInt32(Id), Descripcion = descripcion, Placas = placas, Motor = motor, Estatus = estatus.Equals("1") ? true : false };

                Soriana.FWK.FmkTools.RestResponse responseApi1 = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, apiUrl, null, Newtonsoft.Json.JsonConvert.SerializeObject(v));

                if (responseApi1.code.Equals("00"))
                {
                    List<VehiculoModel> listC = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VehiculoModel>>(responseApi1.message);

                    var result = new { Success = true, json = listC };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { Success = false, Message = "Error al ejecutar la accion" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var result1 = new { Success = true };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> DeleteVehiculo(string Id)
        {
            try
            {
                
                try
                {
                    Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                    System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                    parametros.Add("@Id_Vehiculo", Id);

                    Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "tms.up_CorpTMS_del_Vehiculo", false, parametros);


                    //return ds;
                }
                catch (SqlException ex)
                {

                    throw ex;
                }
                catch (System.Exception ex)
                {

                    throw ex;
                }

                var result1 = new { Success = true };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { Success = false, Message = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #region Gastos de vehiculo



        public ActionResult Gastosvehiculo()
        {
            return View();
        }



        public ActionResult ListGastos()

        {

            try

            {

                var list = DataTableToModel.ConvertTo<Gastos>(DALVehiculos.GastosV_sUp().Tables[0]);



                var result = new { Success = true, resp = list };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };

                return Json(result, JsonRequestBehavior.AllowGet);

            }



        }



        public ActionResult ListGastosVehiculo(int Id_Vehiculo)

        {

            try

            {

                var list = DataTableToModel.ConvertTo<GastosVehiculoModel>(DALVehiculos.GastoVehiculo_sUp(Id_Vehiculo).Tables[0]);



                var result = new { Success = true, resp = list };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };

                return Json(result, JsonRequestBehavior.AllowGet);

            }



        }


        public ActionResult ListGastosVehiculobyID(int IdConsecutivo)

        {

            try

            {

                var list = DataTableToModel.ConvertTo<GastosVehiculoModel>(DALVehiculos.GastoVehiculoById_sUP(IdConsecutivo).Tables[0]);



                var result = new { Success = true, resp = list };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };

                return Json(result, JsonRequestBehavior.AllowGet);

            }



        }


        public ActionResult AddGastoVehiculo(int IdGasto, int Id_Vehiculo, decimal CantidadGasto, int Kilometraje, string FecGasto)

        {

            try

            {

                string UserCreate = User.Identity.Name;



                DALVehiculos.GastoVehiculo_iUp(Id_Vehiculo, IdGasto, FecGasto, Kilometraje, CantidadGasto, UserCreate);



                var result = new { Success = true };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };

                return Json(result, JsonRequestBehavior.AllowGet);

            }



        }


        public ActionResult EditGastoVehiculo(int IdGasto, int Id_Vehiculo, decimal CantidadGasto, int Kilometraje, string FecGasto, int IdConsecutivo)

        {

            try

            {

                string UserCreate = User.Identity.Name;



                DALVehiculos.GastoVehiculo_uUp(Id_Vehiculo, IdGasto, IdConsecutivo, FecGasto, Kilometraje, CantidadGasto, UserCreate);



                var result = new { Success = true };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };

                return Json(result, JsonRequestBehavior.AllowGet);

            }



        }


        public ActionResult DeleteGastoVehiculo(decimal CantidadGasto, int Kilometraje, string FecGasto, int IdConsecutivo)

        {

            try

            {

                string UserCreate = User.Identity.Name;



                DALVehiculos.DeleteGastoVehiculo_uUp(IdConsecutivo, FecGasto, Kilometraje, CantidadGasto, UserCreate);



                var result = new { Success = true };

                return Json(result, JsonRequestBehavior.AllowGet);

            }

            catch (Exception x)

            {

                var result = new { Success = false, Message = x.Message };

                return Json(result, JsonRequestBehavior.AllowGet);

            }



        }

        public ActionResult DeleteGastoVehiculo2(int IdConsecutivo)

        {

            try

            {


                DALVehiculos.GastoVehiculo_dUp(IdConsecutivo, User.Identity.Name);



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