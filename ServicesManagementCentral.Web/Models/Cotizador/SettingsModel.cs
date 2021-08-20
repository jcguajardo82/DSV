using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Cotizador
{
    public class SettingsModel
    {
        public string printFormat { get; set; }
        public string printSize { get; set; }
        public string currency { get; set; }
        public string cashOnDelivery { get; set; }
        public string comments { get; set; }
    }
}