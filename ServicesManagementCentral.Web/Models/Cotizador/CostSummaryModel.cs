using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Cotizador
{
    public class CostSummaryModel
    {
        public int quantity { get; set; }
        public decimal basedPrice { get; set; }
        public int extendedFare { get; set; }
        public int insurance { get; set; }
        public int additionalServices { get; set; }
        public decimal totalPrice { get; set; }
        public string currency { get; set; }
        public bool customKey { get; set; }
        public int cashOnDeliveryCommission { get; set; }
        public int cashOnDeliveryAmount { get; set; }
        public int smsCommission { get; set; }
        public int importFree { get; set; }
        public int taxes { get; set; }
        public int whatsappCommission { get; set; }
        public string folio { get; set; }
    }
}