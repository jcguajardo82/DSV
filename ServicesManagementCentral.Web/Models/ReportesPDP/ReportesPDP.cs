using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ReportesPDP
{
    public class ReportesPDP
    {
        public string NoOrden { get; set; }
        public string Fecha { get; set; }
        public string NoConsignacion { get; set; }
        public string Monto { get; set; }
        public string TipoPAgo { get; set; }
        public string Origen { get; set; }
        public string FecEntrega { get; set; }
 
    }
}