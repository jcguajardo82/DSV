using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Almacenes
{
    public class AltaProveedor
    {
        public int idSupplierWH { get; set; }
        public string supplierName { get; set; }
        public int idSupplierWHCode { get; set; }
        public int idOwner { get; set; }
        public string SupplierWHName { get; set; }
        public string addressStreet { get; set; }
        public string addressNumberExt { get; set; }
        public string addressNumberInt { get; set; }
        public string addressCity { get; set; }
        public string addressPostalCode { get; set; }
        public string addressState { get; set; }
        public string addressReference1 { get; set; }
        public string addressReference2 { get; set; }
        public string commInfoName { get; set; }
        public string operInfoName { get; set; }
        public string  operInfoPhone { get; set; }
        public string operInfoEmail { get; set; }
        public string commInfoPhone { get; set; }
        public string commInfoEmail { get; set; }
        public string creationId { get; set; }
        public int idOperType { get; set; }
        public int idShipType { get; set; }
        public bool bitVehicles { get; set; }

    }

    public class Owners
    {
        public int idOwner { get; set; }
        public string ownerName { get; set; }
    }

    public class Consecutivo
    {
        public int idSupplierWHCode { get; set; }
        public string supplierName { get; set; }
    }
}