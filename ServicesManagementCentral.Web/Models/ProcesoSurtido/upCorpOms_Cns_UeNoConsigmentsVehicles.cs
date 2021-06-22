using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ProcesoSurtido
{
    public class upCorpOms_Cns_UeNoConsigmentsVehicles
    {
        public string UeNo { get; set; }
        public int OrderNo { get; set; }
        public string CnscOrder { get; set; }
        public string idOwner { get; set; }
        public string idSupplierWH { get; set; }
        public string idSupplierWHCode { get; set; }
        public string SerialId { get; set; }
        public string BateryId { get; set; }

        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string PetitionId { get; set; }
        public string MotorId { get; set; }
        public string RepuveId { get; set; }
        public string Year { get; set; }
        public string ColorId { get; set; }
        public string CylinderCapacityId { get; set; }
        public string BrandId { get; set; }
        public string CreatedId { get; set; }
        public string ModelId { get; set; }
     
    }
}