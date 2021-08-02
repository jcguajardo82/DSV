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
        public string idRMA { get; set; }
        public decimal BarCode { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public string IdTrackingService { get; set; }
        public string PackageCondition { get; set; }
        public string Cause_Desc { get; set; }
        public string ReturnedComment { get; set; }
        public string TrackingType { get; set; }
        public int ItemReturned { get; set; }
    }

    public class upCorpOms_Cns_UeCondCauses
    {
        public int IdCause { get; set; }
        public string Cause_Desc { get; set; }
        public int idOwner { get; set; }
        public int Bit_deleted { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }

    public class upCorpOms_Cns_UeDevolCauses
    {
        public int IdCause { get; set; }
        public string Cause_Desc { get; set; }
        public int idOwner { get; set; }
        public int Bit_deleted { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }

}