using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.DTO
{
    public class SupplierStockDto
    {
        public int idSupWH { get; set; }
        public int idSupWCode { get; set; }
        public decimal stockL { get; set; }
        public decimal stockCod { get; set; }
        public string StockDate { get; set; }
        public string StockTime { get; set; }

        public List<ProductStock> Stock { get; set; }

    }
}