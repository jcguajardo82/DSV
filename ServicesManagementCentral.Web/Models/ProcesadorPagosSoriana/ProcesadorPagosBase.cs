using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ProcesadorPagosSoriana
{
    public class ProcesadorPagosBase
    {
        public string PaymentTransactionID { get; set; } = "";
        public string OrderReferenceNumber { get; set; } = "";
        public string OrderDate { get; set; } = "";
        public string OrderHour { get; set; } = "";
        public string OrderMonth { get; set; } = "";
        public string OrderSaleChannel { get; set; } = "";
        public string OrderAmount { get; set; } = "";
        public string CustomerFirstName { get; set; } = "";
        public string CustomerLastName { get; set; } = "";
        public string CustomerContact { get; set; } = "";
        public string PaymentToken { get; set; } = "";
        public string CustomerAddress { get; set; } = "";
        public string CustomerCity { get; set; } = "";
        public string CustomerZipCode { get; set; } = "";
        public string CustomerLoyaltyRedeemMoney { get; set; } = "";
        public string CustomerLoyaltyRedeemPoints { get; set; } = "";
        public string CustomerLoyaltyCardId { get; set; } = "";
        public string ShippingReferenceNumber { get; set; } = "";
        public string ShippingStoreId { get; set; } = "";
        public string ShippingDeliveryDesc { get; set; } = "";
        public string ShippingPaymentImport { get; set; } = "";
        public string ShippingFirstName { get; set; } = "";
        public string ShippingLastName { get; set; } = "";
        public string ShippingAddress { get; set; } = "";
        public string ShippingCity { get; set; } = "";
        public string ClientID { get; set; } = "";
        public string BinCode { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public string Bank { get; set; } = "";
        public string MaskCard { get; set; } = "";
        public string PersistToken { get; set; } = "";
        public string TypeOfCard { get; set; } = "";
        public string Id_cancelacion { get; set; } = "";
        public string clientEmail { get; set; } = "";
        public string fec_movto { get; set; } = "";
        public string Motivo { get; set; } = "";
        public string Products { get; set; } = "";
        public string PaymentTransactionService { get; set; } = "";
        public string TransactionStatus { get; set; } = "";
        public string MethodPayment { get; set; } = "";
        public string Bin_Reembolso { get; set; } = "";                   //BIN Tarjeta Reembolso
        public string SufijoReembolso { get; set; } = "";
        public string orderAmount { get; set; } = "";
        public string paymentType { get; set; } = "";
        public string paymentProcessor { get; set; } = "";
        public string TransactionAuthorizationId { get; set; } = "";
        public string TransactionReferenceID { get; set; } = "";
        public string AffiliationType { get; set; } = "";
        public string IsAuthenticated { get; set; } = "";
        public string IsAuthorized { get; set; } = "";
        public string Apply3DS { get; set; } = "";
        public string MerchandiseType { get; set; } = "";
        public string shippingDeliveryDesc { get; set; } = "";
        public string shippingPaymentImport { get; set; } = "";
        public string shippingItemCategory { get; set; } = "";
        public string shippingItemId { get; set; } = "";
        public string shippingItemName { get; set; } = "";
        public string ShippingItemTotal { get; set; } = "";
        public string shippingPaymentInstallments { get; set; } = "";
        public string Adquirente { get; set; } = "";
        public string MethodPaymentShipment { get; set; } = "";
        public string uSC_StatusDescription { get; set; } = "";
        public string paymentTypeJson { get; set; } = "";


        public string TipoAlmacen { get; set; }
        public string EstatusEnvio { get; set; }

        public string DecisionEmisor { get; set; } = "";
        public string CveReespuestaEmisor { get; set; } = "";
        public string DescReespuestaEmisor { get; set; } = "";
        public string Catalogo { get; set; } = "";
        public string DeliveryType { get; set; } = "";

        #region Cancelacion
        public string NombreCancelacion { get; set; } = "";
        public string FechaCancel { get; set; } = "";
        public string HoraCancel { get; set; } = "";
        public string MontoCancel { get; set; } = "";
        public string ConsignacionIDCancelada { get; set; } = "";
        public string MontoConsignacionIDCancelada { get; set; } = "";
        public string NoPiezasConsignacionCancelacion { get; set; } = "";
        public string FechaINgresoRMA { get; set; } = "";
        public string ConsignaciónIDDevolucin { get; set; } = "";
        public string DetalleConsignacionIngresada { get; set; } = "";
        public string NoPzasConsignacionDevolucion { get; set; } = "";
        public string FechaDevolucion { get; set; } = "";
        public string HoraDevolucion { get; set; } = "";
        public string MontoDevolucionConsignacion { get; set; } = "";
        public string FechaReembolso { get; set; } = "";
        public string HoraReembolso { get; set; } = "";
        public string FormaPagoRembolso { get; set; } = "";
        public string ReembolsoManual { get; set; } = "";
        public string ReembolsoAutomatico { get; set; } = "";
        public string IDTransaccionReembolso { get; set; } = "";
        #endregion


        #region Liquidacion 
        public string FechaLiquidacion { get; set; } = "";
        public string HoraLiquidacion { get; set; } = "";
        public string MontoLiquidacion { get; set; } = "";
        public string LiquidacionManual { get; set; } = "";
        public string LiquidacionAutomatica { get; set; } = "";
        public string IDTransaccionLiquidacion { get; set; } = "";
        #endregion

        #region Reverso
        public string FechaReversoAutorizacion { get; set; } = "";
        public string HoraReversoAutorizacion { get; set; } = "";
        public string MontoReverso { get; set; } = "";
        public string IDTransaccionReverso { get; set; } = "";
        #endregion


        /*public string ofUE_StatusUE { get; set; } = "";

        public string ofUE_DeliveryType { get; set; } = "";

        public string ofUE_UEType { get; set; } = "";

        public string ofUE_IsPickingManual { get; set; } = "";

        public string ofUE_CustomerNo { get; set; } = "";

        public string ofUE_CustomerName { get; set; } = "";

        public string ofUE_Phone { get; set; } = "";

        public string ofUE_Address1 { get; set; } = "";

        public string ofUE_Address2 { get; set; } = "";

        public string ofUE_City { get; set; } = "";

        public string ofUE_StateCode { get; set; } = "";

        public string ofUE_PostalCode { get; set; } = "";

        public string ofUE_Total { get; set; } = "";

        public string ofUE_MethodPayment { get; set; } = "";

        public string ofUE_OrderRMA { get; set; } = "";*/




    }
}