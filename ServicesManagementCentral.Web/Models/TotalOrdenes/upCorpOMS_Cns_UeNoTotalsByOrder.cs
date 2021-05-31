using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.TotalOrdenes
{
    public class upCorpOMS_Cns_UeNoTotalsByOrder
    {
        public string consignacion { get; set; }
        public string FechaCreacion { get; set; }
        public string HoraCreacion { get; set; }
        public string Cliente { get; set; }
        public string EstatusPedido { get; set; }
        public string EstatusConsignacion { get; set; }
        public string NroGuia { get; set; }
        public string EstatusGuia { get; set; }
        public string OrdenCompra { get; set; }
        public string EstatusOrdenCompra { get; set; }
                                                         
    }
}