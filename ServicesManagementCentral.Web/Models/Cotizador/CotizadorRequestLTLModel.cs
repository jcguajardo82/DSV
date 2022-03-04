using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Cotizador
{
    public class CotizadorRequestLTLModel
    {
        public AddressCotzModel origin { get; set; }
        public AddressCotzModel destination { get; set; }
        public List<PackageModel> packages { get; set; }
        public ShipmentLTLModel shipment { get; set; }
        public SettingsModel settings { get; set; }
        public List<string> additionalServices { get; set; }
    }
}