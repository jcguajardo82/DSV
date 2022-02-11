using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ProcesadorPagosSoriana
{
	public class JsonRespoonseModel
	{
		public string orderReferenceNumber { get; set; }
		public string orderAmount { get; set; }
		public string orderDateTime { get; set; }
		public string orderSaleChannel { get; set; }
		public string paymentType { get; set; }
		public string paymentProcessor { get; set; }
		public string paymentToken { get; set; }
		public string customerEmail { get; set; }
		public string customerCity { get; set; }
		public string customerState { get; set; }
		public string customerLoyaltyCardId { get; set; }
		public string customerLoyaltyRedeemElectronicMoney { get; set; }
		public string customerLoyaltyRedeemPoints { get; set; }
		public string customerLoyaltyRedeemMoney { get; set; }
		public string TransactionAuthorizationId { get; set; }
		public string TransactionStatus { get; set; }
		public string TransactionReferenceID { get; set; }
		public string AffiliationType { get; set; }
		public string IsAuthenticated { get; set; }
		public string IsAuthorized { get; set; }
		public string Apply3DS { get; set; }
		public string MerchandiseType { get; set; }
		public List<shipments> shipments { get; set; }

	}

	public class shipments
	{
		public string shippingStoreId { get; set; }
		public string shippingDeliveryDesc { get; set; }
		public string shippingPaymentImport { get; set; }
		public string shippingPaymentInstallments { get; set; }
		public string shippingReferenceNumber { get; set; }
		public string shippingFirstName { get; set; }
		public string shippingLastName { get; set; }
		public List<items> Items { get; set; }
	}

	public class items
	{
		public string shippingItemId { get; set; } = "";
		public string shippingItemName { get; set; } = "";
		public string shippingItemCategory { get; set; } = "";
		public string ShippingItemTotal { get; set; } = "";
	}
}