using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.DTO
{
    public class ProductStock
    {
        public decimal barCode { get; set; }
        public decimal stockLvl { get; set; }
        public decimal qtyStkS { get; set; }
        public decimal qtyStkFSale { get; set; }
        public decimal qtyStkRsrvd { get; set; }
    }
}