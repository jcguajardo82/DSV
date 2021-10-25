using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.CallCenter
{
    public class HistorialOrden
    {

        public string CreatedBy { get; set; }
        public string OrderDate { get; set; }
        public string OrderDeliveryDate { get; set; }
        public string DeliveryType { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Total { get; set; }
        public string MethodPayment { get; set; }
        public string StatusDescription { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryDate { get; set; }
        public string OrderNo { get; set; }
        public string UeNo { get; set; }

        public List<HistorialOrdenArt> ArtNoSurtidos { get; set; }
        public List<HistorialOrdenArt> ArtSurtidos { get; set; }

        public HistorialTotales Totales { get; set; }

    }

    public class HistorialOrdenArt
    {

        public int CnscOrder { get; set; }
        public string ProductId { get; set; }

        public string Barcode { get; set; }
        public string ProductName { get; set; }

        public string pe_Quantity { get; set; }
        public string e_Quantity { get; set; }
        public string c_Quantity { get; set; }
        public string UnitMeasure { get; set; }
        public string pe_price { get; set; }
        public string e_price { get; set; }
        public string c_price { get; set; }
        public string pe_tot { get; set; }
        public string e_tot { get; set; }
        public string c_tot { get; set; }

    }

    public class HistorialTotales
    {

        public string TotNormal { get; set; }
        public string TotOferta { get; set; }
        public string TotNormalSurtido { get; set; }
        public string TotOfertaSurtido { get; set; }
        public string TotPOS { get; set; }
        public string TotPagado { get; set; }
        public string Ahorro { get; set; }
        public string PuntosGanados { get; set; }
        public string PuntosAcumulados { get; set; }
        public string MyProperty { get; set; }

    }
}