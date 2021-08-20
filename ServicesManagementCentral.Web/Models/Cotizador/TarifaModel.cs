using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Cotizador
{
    public class TarifaModel
    {
        public decimal Price { get; set; }
        public string Carrier { get; set; }
        public string currency { get; set; }
    }
}