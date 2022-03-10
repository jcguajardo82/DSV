using ServicesManagement.Web.Models.Cotizador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class CarrierRequest
    {
        public string Carrier { get; set; }
        public string[] requests { get; set; }
        public string msj { get; set; }
    }
}