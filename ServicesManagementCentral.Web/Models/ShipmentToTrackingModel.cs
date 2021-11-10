using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class ShipmentToTrackingModel
    {
        public string ueNo { get; set; }
        public int orderNo { get; set; }
        public string tipoEmpaque { get; set; }
        public decimal peso { get; set; }
        public decimal largo { get; set; }
        public decimal ancho { get; set; }
        public decimal alto { get; set; }
        public long barcode { get; set; }
        public int productId { get; set; }
        public string ucc { get; set; }
        public int piezas { get; set; }

    }
}