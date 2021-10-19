using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ProcesoSurtido
{
    public class Guias_por_OrderFact
    {
        public string IdTrackingService { get; set; }
        public decimal PackageWeight { get; set; }
        public string PackageType { get; set; }
    }
}