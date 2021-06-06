using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ReemvioMcia
{
    public class DetalleReenvioMciaCEDIS
    {
        public string RMA { get; set; }
        public string MotivoRMA { get; set; }
        public string EAN { get; set; }
        public string ID { get; set; }
        public string Descripcion { get; set; }
        public string Piezas { get; set; }
        public string GuiaDevol { get; set; }
        public string PaqueteCondiciones { get; set; }
        public string GuiaReenvio { get; set; }
        public string GuiaStatus { get; set; }
        public string FechaEntrega { get; set; }
        public string HoraEntrega { get; set; }
        public string QuienRecibio { get; set; }

    }
}