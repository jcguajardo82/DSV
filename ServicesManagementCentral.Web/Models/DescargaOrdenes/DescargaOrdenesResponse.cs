using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.DescargaOrdenes
{
    public class DescargaOrdenesResponse
    {
        public int statusCode { get; set; }
        public string statusDescription { get; set; }
        public string url { get; set; }
    }
}