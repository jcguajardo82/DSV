using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Cotizador
{
    public class PackageModel
    {
        public string content { get; set; }
        public int amount { get; set; }
        public string type { get; set; }
        public int weight { get; set; }
        public int insurance { get; set; }
        public int declaredValue { get; set; }
        public string weightUnit { get; set; }
        public string lenghtUnit { get; set; }
        public DimensionsModel dimensions { get; set; }
    }
}