using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class ShipmentPackingModel
    {
        public int idCnscPacking { get; set; }
        public string IdPackingCode { get; set; }
        public string IdPackingType { get; set; }
        public decimal PackageLength { get; set; }
        public decimal PackageWidth { get; set; }
        public decimal PackageHeight { get; set; }
        public decimal PackageWeight { get; set; }
        public bool BitActivo { get; set; }
    }
}