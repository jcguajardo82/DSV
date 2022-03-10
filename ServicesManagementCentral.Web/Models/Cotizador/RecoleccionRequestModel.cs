using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Cotizador
{
    public class RecoleccionRequestModel
    {
        public AddressCotzModel origin { get; set; }
        public ShipmentLTLModel shipment { get; set; }
        public SettingsRecoleccionModel settings { get; set; }
    }
}