using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.EstatusGuiasEnvio
{
    public class EstatusGuiasEnvio
    {

        public string Consignacion { get; set; }
        public string ConsignacionOrigen { get; set; }
        public string TipoAlmacen { get; set; }
        public string FechaOrden { get; set; }
        public string HoraOrden { get; set; }
        public string TipoGuia { get; set; }
        public string Paqueteria { get; set; }
        public string GuiaNro { get; set; }
        public string GuiaStatus { get; set; }
        public string FechaEntrega { get; set; }
        public string HoraEntrega { get; set; }
        public string QuienRecibio { get; set; }
    }
}