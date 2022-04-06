using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class Ordenes
    {

    }

    public class OrderFacts_UE_ShipmentPackingWMS
    {
        public int idCnscPacking { get; set; }
        public string IdPackingCode { get; set; }
        public string IdPackingType { get; set; }
        public decimal PackageLength { get; set; }
        public decimal PackageWidth { get; set; }
        public decimal PackageWeight { get; set; }
        public string createdDate { get; set; }
        public string createdUser { get; set; }
    }

    public class OrderFacts_UE_ShipmentLocksWMS
    {
        public int idCnscLock { get; set; }
        public string IdPackingCode { get; set; }
        public string IdPackingType { get; set; }
        public bool LockLength { get; set; }
        public bool LockWidth { get; set; }
        public bool LockHeight { get; set; }
        public bool LockWeight { get; set; }
        public string createdDate { get; set; }
        public string createdUser { get; set; }
    }

    public class UeNoTrackingDetail
    {
        public string UeNo { get; set; }
        public int OrderNo { get; set; }
        public int IdTracking { get; set; }
        public decimal Barcode { get; set; }
        public decimal ProductId { get; set; }
        public string ProductName { get; set; }
    }

    public class ItemsGuia
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public string UeNo { get; set; }
        public string OrderNo { get; set; }
    }

    public class NuevoProveedorReasignado
    {
        public string Proveedor { get; set; }
        public string CodigoPostal { get; set; }
        public decimal Distancia { get; set; }
        public List<productosProveedor>  Productos { get; set; }
        public decimal productosTot { get; set; }
        public decimal CostoTot { get; set; }
        public decimal PesoTot { get; set; }
        public decimal VolumenTot { get; set; }

    }

    public class productosProveedor
    {
        public string Producto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Costo { get; set; }
        public decimal Peso { get; set; }
        public decimal Volumen { get; set; }
    }
}