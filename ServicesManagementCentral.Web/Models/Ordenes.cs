using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class Ordenes
    {

    }

    public class UeNoTrackingDetail
    {
        public string UeNo { get; set; }
        public int OrderNo { get; set; }
        public int IdTracking { get; set; }
        public decimal Barcode { get; set; }
        public decimal ProductId { get; set; }
        public string ProductName { get; set; }
    }
}