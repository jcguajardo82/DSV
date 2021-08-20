using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Cotizador
{
    public class DeliveryDateModel
    {
        public string date { get; set; }
        public string time { get; set; }
        public int dateDifference { get; set; }
        public string timeUnit { get; set; }
    }
}