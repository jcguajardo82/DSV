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
        public decimal basedPrice { get; set; }
        public decimal extendedFare { get; set; }
        public decimal insurance { get; set; }
        public decimal additionalServices { get; set; }
        public decimal totalPrice { get; set; }
        public string currency { get; set; }
        public bool customKey { get; set; }
        public decimal importFree { get; set; }
        public decimal taxes { get; set; }
        public decimal cashOnDeliveryCommission { get; set; }
        public decimal cashOnDeliveryAmount { get; set; }
        public decimal customKeyCost { get; set; }
        public decimal smsCost { get; set; }
        public decimal whatsappCost { get; set; }
        public List<CostSummaryModel> costSummary { get; set; }
    }
}