using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.CallCenter
{
    public class upCorpOms_Cns_OrdersByHistorical
    {
        public string OrderNo { get; set; }
        public string UeNo { get; set; }
        public string StatusDescriptionUE { get; set; }
        public string PosBarcode { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Price { get; set; }
        public string ArticuloFoto { get; set; }


        public int StatusUe { get; set; }
        public string StatusDescriptionUE { get; set; }
        public string StatusDescription { get; set; }
        public byte StatusId { get; set; }

    }
}