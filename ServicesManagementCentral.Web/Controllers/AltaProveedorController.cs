﻿using Soriana.FWK;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagement.Web.DAL.Almacenes;
using ServicesManagement.Web.Models.Almacenes;
using ServicesManagement.Web.Helpers;

namespace ServicesManagement.Web.Controllers
{
    public class AltaProveedorController : Controller
    {
        // GET: ManualesOperativos
        public ActionResult AltaProveedor()
        {
            return View();
        }
        public ActionResult IniciarComboTipoAlmacen()
        {
            try
            {
                DataSet list = DALAltaProveedor.spOwners_sUP();
                var tabla = list.Tables[0];
                var dropdownVD = DataTableToModel.ConvertTo<Owners>(DALAltaProveedor.spOwners_sUP().Tables[0]);
                var dropdownLT = DataTableToModel.ConvertTo<Owners>(list.Tables[0]);
                var dropdownTA = DataTableToModel.ConvertTo<Owners>(tabla);
                //DataTableToModel.ConvertTo<ServicesManagement.Web.Models.Almacenes.SPOwners_sUP>(DALAltaProveedor.spOwners_sUP().Tables[0])
                var result = new { Success = true, resp = dropdownVD };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GrabarProveedor(int idSupplierWH, string supplierName, int idSupplierWHCode, int idOwner, string addressStreet, string addressNumberExt, string addressNumberInt,
            string addressCity, string addressPostalCode, string addressState, string addressReference1, string addressReference2, string commInfoName, string operInfoName, string operInfoPhone,
            string operInfoEmail, string commInfoPhone, string commInfoEmail, string creationId)
        {
            try
            {
                var list = DALAltaProveedor.SuppliersWH_iUP(idSupplierWH, supplierName, idSupplierWHCode, idOwner, addressStreet, addressNumberExt, 
                    addressNumberInt, addressCity, addressPostalCode, addressState, addressReference1, addressReference2, commInfoName, operInfoName, 
                    operInfoPhone, operInfoEmail, commInfoPhone, commInfoEmail, creationId);

                var result = new { Success = true, Message = "OK" };
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