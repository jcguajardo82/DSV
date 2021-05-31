using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ReemvioMcia
{
    public class ReenvioMciaCEDIS
    {

        public string Consignacion { get; set; }
        public string ConsignacionReenvio { get; set; }
        public string FechaOrden { get; set; }
        public string HoraOrden { get; set; }
        public string Cliente { get; set; }
        public string OrdenReenvioStatus { get; set; }
        public string GuiaDevolNro { get; set; }
        public string GuiaDevolStatus { get; set; }
        public string GuiaReenvioNro { get; set; }
        public string GuiaReenvioStatus { get; set; }

    }
}