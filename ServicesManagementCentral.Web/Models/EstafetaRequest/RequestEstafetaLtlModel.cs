using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.EstafetaRequest
{
    public class RequestEstafetaLtlModel
    {
        public Identification identification { get; set; }
        public SystemInformation systemInformation { get; set; }
        public LabelDefinitionLtl labelDefinition { get; set; }
    }
    public class Pallet
    {
        public string merchandise { get; set; }
        public string genericContent { get; set; }
        public string type { get; set; }
    }

    public class WayBillDocumentLtl
    {
        public string aditionalInfo { get; set; }
        public string content { get; set; }
        public string costCenter { get; set; }
        public string customerShipmentId { get; set; }
        public string referenceNumber { get; set; }
        public string groupShipmentId { get; set; }
        public Pallet pallet { get; set; }
    }

    public class LabelDefinitionLtl
    {
        public WayBillDocumentLtl wayBillDocument { get; set; }
        public ItemDescription itemDescription { get; set; }
        public ServiceConfiguration serviceConfiguration { get; set; }
        public Location location { get; set; }
    }
}