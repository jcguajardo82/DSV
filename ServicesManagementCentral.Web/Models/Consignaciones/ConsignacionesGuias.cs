using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Consignaciones
{
    public class ConsignacionesGuias
    {
        public string Id_Num_Orden { get; set; }
        public string UeNo { get; set; }
        public int IdGuias { get; set; }
        public string IdGuiasPaq { get; set; }
        public string GuiaPaqStatus { get; set; }
        public string GuiaPaqStatusDesc { get; set; }
    }
}