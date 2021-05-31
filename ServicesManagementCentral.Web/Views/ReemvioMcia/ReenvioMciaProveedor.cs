using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ReemvioMcia
{
    public class ReenvioMciaProveedor
    {

        public string Consignacion { get; set; }
        public string ConsignacionReenvio { get; set; }
        public string TipoAlmacen { get; set; }
        public string FechaOrden { get; set; }
        public string HoraOrden { get; set; }
        public string Cliente { get; set; }
        public string EstatusAlmacen { get; set; }
        public string EstatusHomeD { get; set; }
        public string EstatusGuiaDevol { get; set; }
        public string GuiaReenvioNro { get; set; }
        public string GuiaReenvioEstatus { get; set; }
        public string OrdenDSV { get; set; }
        public string OredenDSVStatus { get; set; }

    }
}