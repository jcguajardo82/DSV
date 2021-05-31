using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ProcesoReciboDevoluciones
{
    public class ProcesoReciboDevoluciones
    {

    }

    public class upCorpOMS_Cns_UeNoDevolDetail
    {
        public int idRMA { get; set; }
        public decimal BarCode { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public int IdTrackingService { get; set; }
        public string PackageCondition { get; set; }
        public string MotivoDevol { get; set; }
        public int ItemReturned { get; set; }
    }
}