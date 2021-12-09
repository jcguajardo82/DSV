using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soriana.ProcesadorPagosWeb
{
    public class GetOrderPaymentModel
    {


        public string requestPaymentId { get; set; }

    }

    public class PaymentResponseModel
    {

        public string responseError { get; set; } = "0";
        public string responseMessage { get; set; } = "";
        public Guid requestPaymentId { get; set; }
        public string orderReferenceNumber { get; set; }
        public string redirectURL { get; set; }


    }

    public class PaymentModel
    {


        public string orderReferenceNumber { get; set; }
        public string orderDateTime { get; set; }
        public string orderSaleChannel { get; set; }
        public string orderAmount { get; set; }
        public string paymentType { get; set; }
        public string paymentProcessor { get; set; }
        public string paymentToken { get; set; }
        public string paymentCardCVV { get; set; }
        public string paymentCardNIP { get; set; }
        public string paymentSaveCard { get; set; }
        public string customerEmail { get; set; }
        public string customerFirstName { get; set; }
        public string customerLastName { get; set; }
        public string customerAddress { get; set; }
        public string customerCity { get; set; }
        public string customerState { get; set; }
        public string customerId { get; set; }
        public string customerDeviceFingerPrintId { get; set; }

        public string customerIPAddress { get; set; }
        public string customerZipCode { get; set; }
        public string customerCountry { get; set; }


        public string customerPurchaseQuantity { get; set; }
        public string customerContact { get; set; }
        public string customerLoyaltyCardId { get; set; }
        public string customerLoyaltyRedeemElectronicMoney { get; set; }
        public string customerLoyaltyRedeemPoints { get; set; }
        public string customerLoyaltyRedeemMoney { get; set; }
        public string customerRegisteredDays { get; set; }
        public string returnURL { get; set; }
        public List<ShipmentsModel> shipments { get; set; }

        public string AuthenticationTransactionId { get; set; } = "";
        public bool init_3ds { get; set; } = false;
        public bool aut_3ds { get; set; } = false;
        public bool not_3ds { get; set; } = false;
        public string ReferenceId { get; set; } = "";
    }

    public class ShipmentsModel
    {


        public string shippingReferenceNumber { get; set; }
        public string shippingStoreId { get; set; }
        public string shippingStoreName { get; set; }
        public string shippingDeliveryId { get; set; }
        public string shippingDeliveryDesc { get; set; }
        public string shippingPaymentInstallments { get; set; }
        public string shippingPaymentImport { get; set; }
        public string shippingFirstName { get; set; }
        public string shippingLastName { get; set; }
        public string shippingAddress { get; set; }
        public string shippingCity { get; set; }
        public List<ItemModels> items { get; set; }


    }


    public class ItemModels
    {

        public string shippingItemId { get; set; }
        public string shippingItemEAN { get; set; }
        public string shippingItemName { get; set; }
        public string shippingItemCategory { get; set; }
        public decimal shippingItemPrice { get; set; }
        public double shippingItemQuantity { get; set; }
        public decimal shippingItemTotal { get; set; }


    }


    public class ResponseModels
    {


        [JsonProperty("statusCode", Order = 1)]
        public int statusCode { get; set; } = 200;

        [JsonProperty("description", Order = 2)]
        public string description { get; set; } = "OK";

        [JsonProperty("descriptiondetail", Order = 3)]
        public string descriptiondetail { get; set; } = "startOrderPayment";

        [JsonProperty("object", Order = 4)]
        public objectsModels item { get; set; }
        //public Guid paymentProcessingId { get; set; }
        //public string responseMessage { get; set; }
        //public string responseCode { get; set; }
        //public string responseErrorText { get; set; }
        //public string responseError { get; set; }
        //public string RetunrURL { get; set; }
    }

    public class objectsModels
    {


        public string responseError { get; set; } = "0";
        public string responseMessage { get; set; } = "";
        public string requestPaymentId { get; set; }
        //public Guid requestPaymentId { get; set; }
        public string orderReferenceNumber { get; set; }
        public string redirectURL { get; set; }


    }


}
