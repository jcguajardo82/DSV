using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Cotizador
{
    public class ShipmentLTLModel
    {
        public string carrier { get; set; }
        public string service { get; set; }
        public int type { get; set; }
        public Appointment appointment { get; set; }
        public Pickup pickup { get; set; }
    }
}