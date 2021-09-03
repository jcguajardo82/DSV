using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class ShipmentRequestsFromWMSModel
    {
        public int Cuenta { get; set; }
        public long idShipmentWMS { get; set; }
		public string UeNo { get; set; }
		public int OrderNo { get; set; }
		public long CnscOrder { get; set; }
        public string ProductId { get; set; }
		public string Barcode { get; set; }
		public int idOwner { get; set; }
		public long idSupplierWH { get; set; }
		public long idSupplierWHCode { get; set; }
		public int cantidad { get; set; }
		public string cita { get; set; }
		public string codigoPostal { get; set; }
		public string colonia { get; set; }
		public string contacto { get; set; }
		public string direccion { get; set; }
		public string fechaLimite { get; set; }
		public string montoAsegurado { get; set; }
		public string poblacion { get; set; }
		public string razonSocial { get; set; }
		public string referencia2 { get; set; }
		public string referencia3 { get; set; }
		public string referencia4 { get; set; }
		public string referencia5 { get; set; }
		public string referencia6 { get; set; }
		public string referencia7 { get; set; }
		public string servicio { get; set; }
		public string telefono { get; set; }
		public string siglasCliente { get; set; }
		public DateTime createdDate { get; set; }
		public string createdUser { get; set; }
		public bool bitProcesado { get; set; }
		public string ucc { get; set; }

	}
	public class upCorpOms_Sel_ShipmentProcessedFromWMSModel
	{
		public int Cuenta { get; set; }
		public string FechaConsignacion { get; set; }
		public long idShipmentWMS { get; set; }
		public string UeNo { get; set; }
		public int OrderNo { get; set; }
		public long CnscOrder { get; set; }
		public int idOwner { get; set; }
		public long idSupplierWH { get; set; }
		public long idSupplierWHCode { get; set; }
		public int cantidad { get; set; }
		public string cita { get; set; }
		public string codigoPostal { get; set; }
		public string colonia { get; set; }
		public string contacto { get; set; }
		public string direccion { get; set; }
		public string fechaLimite { get; set; }
		public string montoAsegurado { get; set; }
		public string poblacion { get; set; }
		public string razonSocial { get; set; }
		public string referencia2 { get; set; }
		public string referencia3 { get; set; }
		public string referencia4 { get; set; }
		public string referencia5 { get; set; }
		public string referencia6 { get; set; }
		public string referencia7 { get; set; }
		public string servicio { get; set; }
		public string telefono { get; set; }
		public string siglasCliente { get; set; }
		public DateTime createdDate { get; set; }
		public string createdUser { get; set; }
		public bool bitProcesado { get; set; }
		public string IdTrackingService { get; set; }
		public string TrackingServiceName { get; set; }
		public string ucc { get; set; }

	}
}