using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ProcesoSurtido
{
    public class UenoTracking
    {
        public string IdTrackingService { get; set; }
        public string IdTracking { get; set; }
        public decimal PackageHeight { get; set; }
        public decimal PackageLength { get; set; }
        public decimal PackageWidth { get; set; }
        public decimal PackageWeight { get; set; }
        public string declaredValue { get; set; }
        public string ShippingMethod { get; set; }
        public string Quantity { get; set; }
        public decimal? PesoReal { get; set; }
    }
}