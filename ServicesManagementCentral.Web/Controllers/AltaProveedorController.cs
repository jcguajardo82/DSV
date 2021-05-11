using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagement.Web.DAL.Almacenes;

namespace ServicesManagement.Web.Controllers
{
    public class AltaProveedorController : Controller
    {
        // GET: ManualesOperativos
        public ActionResult AltaProveedor()
        {
            return View();
        }

        public ActionResult GrabarProveedor(int idSupplierWH, string supplierName, int idSupplierWHCode, int idOwner, string addressStreet, string addressNumberExt, string addressNumberInt,
            string addressCity, string addressPostalCode, string addressState, string addressReference1, string addressReference2, string commInfoName, string operInfoName, string operInfoPhone,
            string operInfoEmail, string commInfoPhone, string commInfoEmail)
        {
            try
            {
                var list = DALAltaProveedor.SuppliersWH_iUP(idSupplierWH, supplierName, idSupplierWHCode, idOwner, addressStreet, addressNumberExt, 
                    addressNumberInt, addressCity, addressPostalCode, addressState, addressReference1, addressReference2, commInfoName, operInfoName, 
                    operInfoPhone, operInfoEmail, commInfoPhone, commInfoEmail);

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