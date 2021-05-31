using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ProcesoSurtido
{
    public class upCorpOms_Cns_UeNoTracking
    {
        public class Item
        {
            public string UeNo { get; set; }
            public int OrderNo { get; set; }
            public int IdTracking { get; set; }
            public int ProductId { get; set; }
            public string Barcode { get; set; }
            public string ProductName { get; set; }
            public bool ConGuia { get; set; }


        }

        public class Guia
        {
            public string UeNo { get; set; }
            public int OrderNo { get; set; }
            public int CnscOrder { get; set; }
            public int IdOwner { get; set; }
            public int IdSupplierWH { get; set; }
            public int IdSupplierWHCode { get; set; }
            public int IdTracking { get; set; }
            public string TrackingType { get; set; }
            public string PackageType { get; set; }
            public decimal PackageLength { get; set; }
            public decimal PackageWidth { get; set; }
            public decimal PackageHeight { get; set; }
            public decimal PackageWeight { get; set; }
            public int IdTrackingService { get; set; }
            public string TrackingServiceName { get; set; }
            public string TrackingDate { get; set; }
            public string TrackingTIME { get; set; }
            public string TrackingExpDate { get; set; }
            public string TrackingExpTIME { get; set; }
            public decimal ShippingCost { get; set; }



            //,[]	-- nombre de la paqueteria
            //,[]			-- fecha, creacion de la guia de paqueteria
            //,[]			-- hora, creacion de la guia de paqueteria
            //,[]		-- fecha, expiracion de la guia de paqueteria
            //,[]		-- hora, expiracion de la guia de paqueteria
            //,[]			-- costo del envio
        }
    }


}