using ServicesManagement.Web.DAL.Autorizacion;
using ServicesManagement.Web.DAL.CallCenter;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models;
using ServicesManagement.Web.Models.Autorizacion;
using ServicesManagement.Web.Models.CallCenter;
using ServicesManagement.Web.Controllers;
using ServicesManagement.Web.Correos;
using ServicesManagement.Web.DAL.Correos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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
           , string IdAccion, int Id_cancelacion)
        {
            try
            {
                //Valida si la Solicitud Rma Requiere fotogrifias por parte del usuario,y que este ya las haya subido
                

                var val = Convert.ToBoolean(DALAutorizacion.valida_foto_sUP(Id_cancelacion).Tables[0].Rows[0][0].ToString());
                if (!val)
                {
                    if (IdAccion.Equals("1"))
                    {
                        throw new Exception(string.Format("El folio RMA {0} no puede ser autorizado ya que el cliente no ha subido evidencias fotográficas", Id_cancelacion));
                    }
                    else {
                        throw new Exception(string.Format("El folio RMA {0} no puede ser cancelado ya que el cliente no ha subido evidencias fotográficas", Id_cancelacion));
                    }
                }
                else
                {
                    switch (IdProceso)
                    {
                        case 0:
                            // Solicitud de Cancelación
                            //Correos.Correos.Correo8A(Id_cancelacion, 2);
                            break;
                        case 2:
                            // DSV, DST, CEDIS Duda: 9A y 9B
                            Correos.Correos.Correo9(Id_cancelacion);
                            break;
                        case 4:
                            // Administrador Reembolso Duda; 10B
                            Correos.Correos.Correo10B(Id_cancelacion);
                            break;
                    }
                }

                DALAutorizacion.BitacoraAutRma_iUp_v2(IdProceso, IdAccion, Comentario, Id_cancelacion, User.Identity.Name);

                if (IdAccion.Equals("1"))
                {
                  var ds=  DALCallCenter.PaymentsGetCancelacion_sUp(Id_cancelacion);
                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["isCancelacion"])) {

                      
                        string apiUrl = System.Configuration.ConfigurationManager.AppSettings["Rma_PaymentsGetCancelacion"];

                        apiUrl = string.Format("{0}?order={1}&amount={2}", apiUrl, ds.Tables[0].Rows[0]["OrderSF"].ToString(), ds.Tables[0].Rows[0]["TotalAmount"].ToString());

                        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, apiUrl, "");

                        var motivo = ds.Tables[0].Rows[0]["IdTmovimiento"].ToString();

                        switch (IdProceso)
                        {

                            case 2:
                                // Se autoriza Cancelación
                                //Correos.Correos.Correo8A(Id_cancelacion, 2);
                                // Se aprueba por Supervisor
                                Correos.Correos.Correo9A(Id_cancelacion);
                                break;
                            case 3:
                                // DSV, DST, CEDIS Duda: Da entrada Almacen
                                switch (motivo)
                                {
                                    case "2":
                                    case "3":
                                        Correos.Correos.Correo10A(Id_cancelacion);
                                        break;
                                }
                                
                                break;
                            case 4:
                                // Administrador  Autoriza Gerencia
   
                                switch (motivo)
                                {
                                    case "2":
                                    case "3":
                                        Correos.Correos.Correo10A(Id_cancelacion);
                                        break;
                                }
                                break;
                        }

                        if (r.code != "00")
                        {
                            throw new Exception("Error al ejecutar el api PaymentsGetCancelacion "+r.message);
                        }


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
    }
}