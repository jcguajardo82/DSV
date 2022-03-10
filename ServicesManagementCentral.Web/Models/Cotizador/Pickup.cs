using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Cotizador
{
    public class Pickup
    {
        public string timeFrom { get; set; }
        public string timeTo { get; set; }
        public string date { get; set; }
        public string instructions { get; set; }
        public int totalPackages { get; set; }
        public decimal totalWeight { get; set; }
    }
}