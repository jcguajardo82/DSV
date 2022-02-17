using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Cotizador
{
    public class DataResponseModel
    {
        public string carrier { get; set; }
        public string service { get; set; }
        public string serviceDescription { get; set; }
        public string zone { get; set; }
        public string deliveryEstimate { get; set; }
        public DeliveryDateModel deliveryDate { get; set; }
        public int quantity { get; set; }
        public int basedPrice { get; set; }
        public int extendedFare { get; set; }
        public int insurance { get; set; }
        public int additionalServices { get; set; }
        public int totalPrice { get; set; }
        public string currency { get; set; }
        public bool customKey { get; set; }
        public int importFree { get; set; }
        public int taxes { get; set; }
        public int cashOnDeliveryCommission { get; set; }
        public int cashOnDeliveryAmount { get; set; }
        public int customKeyCost { get; set; }
        public int smsCost { get; set; }
        public int whatsappCost { get; set; }
        public List<CostSummaryModel> costSummary { get; set; }
    }
}