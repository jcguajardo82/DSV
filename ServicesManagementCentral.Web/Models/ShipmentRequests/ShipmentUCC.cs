using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ShipmentRequests
{
    public class ShipmentUCC
    {
        public string IdShipmentWMS { get; set; }
        public string UeNo { get; set; }
        public string OrderNo { get; set; }
        public string CnscOrder { get; set; }
        public string Ucc { get; set; }
        public string CreatedDate { get; set; }

        public string createdUser { get; set; }

    }
}