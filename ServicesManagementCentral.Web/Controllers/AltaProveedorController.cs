using Soriana.FWK;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagement.Web.DAL.Almacenes;
using ServicesManagement.Web.Models.Almacenes;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.Config;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class AltaProveedorController : Controller
    {

        public ActionResult AltaProveedor()
        {
            ViewBag.Operations = DataTableToModel.ConvertTo<SuppliersWHOperations>(DALAltaProveedor.SuppliersWHOperations_sUP().Tables[0]);
            ViewBag.Shipments = DataTableToModel.ConvertTo<SuppliersWHShipments>(DALAltaProveedor.SuppliersWHShipments_sUP().Tables[0]);
            ViewBag.Owners = DataTableToModel.ConvertTo<Models.Almacenes.Owners>(DALAltaProveedor.spOwners_sUP().Tables[0]);

            var prov = new AltaProveedor();

            if (Request.QueryString["idSupplierWH"] != null && Request.QueryString["idSupplierWHCode"] != null)
            {
                int idSupplierWH = 0, idSupplierWHCode = 0;


  

                idSupplierWH = int.Parse(Request.QueryString["idSupplierWH"].ToString());
                idSupplierWHCode = int.Parse(Request.QueryString["idSupplierWHCode"].ToString());

                prov = DataTableToModel.ConvertTo<AltaProveedor>(DALAltaProveedor.SuppliersWHByCode_sUP(idSupplierWH, idSupplierWHCode).Tables[0]).FirstOrDefault();
                if (Request.QueryString["Nuevo"] != null && prov != null)
                {
                    prov.idSupplierWHCode = prov.idSupplierWHCode + 1;
                }
            }

            var usr = DataTableToModel.ConvertTo<Usuario>(DALConfig.Autenticar_sUP(User.Identity.Name).Tables[0]).FirstOrDefault();

            if (usr.rol.Equals("4")) { ViewBag.Dsv = true; }
            else
            { ViewBag.Dsv = false; }
            return View(prov);
        }
        public ActionResult IniciarComboTipoAlmacen()
        {
            try
            {
                var dropdownVD = DataTableToModel.ConvertTo<Models.Almacenes.Owners>(DALAltaProveedor.spOwners_sUP().Tables[0]);
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

        public ActionResult GrabarProveedor(int idSupplierWH, string supplierName, int idSupplierWHCode, int idOwner, string SupplierWHName, string addressStreet, string addressNumberExt, string addressNumberInt,
            string addressCity, string addressPostalCode, string addressState, string addressReference1, string addressReference2, string commInfoName, string operInfoName, string operInfoPhone,
            string operInfoEmail, string commInfoPhone, string commInfoEmail, string creationId, string idOperType, string idShipType, string bitVehicles, string addressCol,string reference)
        {
            try
            {
                var list = DALAltaProveedor.SuppliersWH_iUP(idSupplierWH, supplierName, idSupplierWHCode, idOwner, SupplierWHName, addressStreet, addressNumberExt,
                    addressNumberInt, addressCity, addressPostalCode, addressState, addressReference1, addressReference2, commInfoName, operInfoName,
                    operInfoPhone, operInfoEmail, commInfoPhone, commInfoEmail, User.Identity.Name, int.Parse(idOperType), int.Parse(idShipType), bool.Parse(bitVehicles), addressCol
                    , reference);

                var result = new { Success = true, Message = "OK" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ConsecutivoAlmacen(int idSupplierWH)
        {
            try
            {
                var ConsAlmacen = DataTableToModel.ConvertTo<Consecutivo>(DALAltaProveedor.SuppliersWHCode_sUP(idSupplierWH).Tables[0]);
                var result = new { Success = true, resp = ConsAlmacen };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetInfoByCP(string postalCode)
        {
            try
            {
                var InfoByCP = DataTableToModel.ConvertTo<SuppliersWHPostalCode>(DALAltaProveedor.SuppliersWHPostalCode_sUP(postalCode).Tables[0]);
                var result = new { Success = true, resp = InfoByCP };
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