using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class EnviaResponseModel
    {
        public string meta { get; set; }
        public data[] data { get; set; }
    }
    public class data
    {
        public string carrier { get; set; }
        public string service { get; set; }
        public string trackingNumber { get; set; }
        public string trackUrl { get; set; }
        public string label { get; set; }
        public string[] additionalFiles { get; set; }
        public decimal totalPrice { get; set; }
        public decimal currentBalance { get; set; }
        public string currency { get; set; }
    }
}