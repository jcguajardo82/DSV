﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagementCentral.Web.Controllers.Api;
using ServicesManagement.Web.Models.ProcesoReciboDevoluciones;
using ServicesManagement.Web.DAL.ProcesoReciboDevoluciones;
using ServicesManagement.Web.Helpers;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class ProcesoReciboDevolucionesController : Controller
    {
        // GET: ProcesoReciboDevoluciones
        public ActionResult ProcesoReciboDevoluciones()
        {
            var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
            int idOwner = 1;

            if (ds.Tables[0].Rows.Count == 0)
            {
                idOwner = 2;
            }
            else
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    UserRolModel rol = new UserRolModel();
                    rol.idRol = item["rol"].ToString();
                    rol.nombreRol = item["nombreRol"].ToString();
                    Session["UserRol"] = rol;
                    idOwner = 4;
                }
            }
            
            Session["ListaConsignaciones"] = DALProcesoReciboDevoluciones.upCorpOMS_Cns_UeNoDevolProcess(idOwner);
            return View();
        }

        //Combo Condiciones
        public ActionResult ListCondiciones()
        {
            try
            {
                var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
                int idOwner = 1;

                if (ds.Tables[0].Rows.Count == 0)
                {
                    idOwner = 2;
                }
                else
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        UserRolModel rol = new UserRolModel();
                        rol.idRol = item["rol"].ToString();
                        rol.nombreRol = item["nombreRol"].ToString();
                        Session["UserRol"] = rol;
                        idOwner = 4;
                    }
                }

                var list = DataTableToModel.ConvertTo<upCorpOms_Cns_UeCondCauses>(DALProcesoReciboDevoluciones.upCorpOms_Cns_UeCondCauses(idOwner).Tables[0]);

                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        //Combo Devoluciones
        public ActionResult ListDevoluciones()
        {
            try
            {
                var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
                int idOwner = 1;

                if (ds.Tables[0].Rows.Count == 0)
                {
                    idOwner = 2;
                }
                else
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        UserRolModel rol = new UserRolModel();
                        rol.idRol = item["rol"].ToString();
                        rol.nombreRol = item["nombreRol"].ToString();
                        Session["UserRol"] = rol;
                        idOwner = 4;
                    }
                }
                var list = DataTableToModel.ConvertTo<upCorpOms_Cns_UeDevolCauses>(DALProcesoReciboDevoluciones.upCorpOms_Cns_UeDevolCauses(idOwner).Tables[0]);

                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        //listar productos de  consignación
        public ActionResult LstProdPaquete(string UeNo)
        {
            try
            {
                var ds = DALConfig.Autenticar_sUP(User.Identity.Name);
                int idOwner = 1;

                if (ds.Tables[0].Rows.Count == 0)
                {
                    idOwner = 2;
                }
                else
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        UserRolModel rol = new UserRolModel();
                        rol.idRol = item["rol"].ToString();
                        rol.nombreRol = item["nombreRol"].ToString();
                        Session["UserRol"] = rol;
                        idOwner = 3;
                    }
                }

                var listaProd = DataTableToModel.ConvertTo<upCorpOMS_Cns_UeNoDevolDetail>(DALProcesoReciboDevoluciones.upCorpOMS_Cns_UeNoDevolDetail(UeNo).Tables[0]);
                var listCond = DataTableToModel.ConvertTo<upCorpOms_Cns_UeCondCauses>(DALProcesoReciboDevoluciones.upCorpOms_Cns_UeCondCauses(idOwner).Tables[0]);
                var listDev = DataTableToModel.ConvertTo<upCorpOms_Cns_UeDevolCauses>(DALProcesoReciboDevoluciones.upCorpOms_Cns_UeDevolCauses(idOwner).Tables[0]);
                var result = new { Success = true, resp = listaProd, lstCond = listCond, lstDev = listDev };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        //Aceptar Devolución
        public ActionResult AceptarDevolucion(string UeNo, int OrderNo, string IdTrackingService, string TrackingType, decimal Barcode, string CreationId,
            string PackageCondition, string MotivoDevol)
        {
            try
            {
                var aceptadaDevolucion = DALProcesoReciboDevoluciones.upCorpOMS_Ins_UeNoDevolProcess(UeNo, OrderNo, IdTrackingService, TrackingType, Barcode, User.Identity.Name,
                    PackageCondition, MotivoDevol).Tables[0].Rows[0][0];
                var result = new { Success = true, resp = aceptadaDevolucion };
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