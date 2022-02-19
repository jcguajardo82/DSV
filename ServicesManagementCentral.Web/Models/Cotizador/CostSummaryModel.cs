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
        public decimal extendedFare { get; set; }
        public decimal insurance { get; set; }
        public decimal additionalServices { get; set; }
        public decimal totalPrice { get; set; }
        public string currency { get; set; }
        public bool customKey { get; set; }
        public decimal cashOnDeliveryCommission { get; set; }
        public decimal cashOnDeliveryAmount { get; set; }
        public decimal smsCommission { get; set; }
        public decimal importFree { get; set; }
        public decimal taxes { get; set; }
        public decimal whatsappCommission { get; set; }
        public string folio { get; set; }
    }
}