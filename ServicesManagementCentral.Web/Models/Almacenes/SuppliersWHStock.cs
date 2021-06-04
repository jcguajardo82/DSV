using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Almacenes
{
    public class SuppliersWHStock
    {
        public string IdSupplierWH { get; set; }
        public string IdSupplierWHCode { get; set; }
        public string IdOwner { get; set; }
        public string ProductSKU { get; set; }
        public string BarCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryId { get; set; }
        public string StockLevel { get; set; }
        public string QtyStockSafety { get; set; }
        public string QtyStockForSale { get; set; }
        public string QtyStockReserved { get; set; }
        public string ProductType { get; set; }
        public string DimmensionLarge { get; set; }
        public string DimmensionHeight { get; set; }
        public string DimmensionWidth { get; set; }
        public string DimmensionWeight { get; set; }
        public string DimmensionWeightVol { get; set; }
        public string DimmensionWeightReal { get; set; }
        public string ProductStatus { get; set; }
        public string ProductMaterialCost { get; set; }
        public string ProductCreationDate { get; set; }
        public string IdSupplierOrigin { get; set; }
        public string IdSupplierOriginName { get; set; }
        public string OwnerName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierWHName { get; set; }


    }
}