using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.CallCenter
{
    public class OrderJson
    {

        [JsonProperty("xml-orden", Order = 1)]
        public string xmlOrden { get; set; }
    }
}