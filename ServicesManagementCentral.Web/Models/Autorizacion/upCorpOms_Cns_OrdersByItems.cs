using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Autorizacion
{
    public class upCorpOms_Cns_OrdersByItems
    {
        public string ProductDescription { get; set; }
        public string PriceNormalSale { get; set; }
        public string BarCode { get; set; }
        public string ProductID { get; set; }
        public string WarehouseType { get; set; }
        public string StoreDescription { get; set; }
        public string Quantity { get; set; }
        public string MyProperty { get; set; }
        public string ShipmentId { get; set; }
        public string Consignment { get; set; }

        public string Comment { get; set; }
        public string Reason { get; set; }

    }

    public class Header {
        public string Descripcion_Padre { get; set; }
        public string Descripcion_Motivo { get; set; }
        public string WarehouseType { get; set; }
        public string NombreAlmacen { get; set; }
    }
}