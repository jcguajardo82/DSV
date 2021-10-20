﻿using ServicesManagement.Web.DAL.Autorizacion;
using ServicesManagement.Web.DAL.CallCenter;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models;
using ServicesManagement.Web.Models.Autorizacion;
using ServicesManagement.Web.Models.CallCenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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



                DALAutorizacion.BitacoraAutRma_iUp_v2(IdProceso, IdAccion, Comentario, Id_cancelacion, User.Identity.Name);

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