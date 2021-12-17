using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ProcesadorPagosSoriana
{
    public class CancelDevolucionModel
    {
		public CancellationModel Cancelacion { get; set; }
		public DevolucionModel Devolucion { get; set; }
	}
	
	public class CancellationModel
    {
		public string OrderId { get; set; } = "";
		public string clientEmail { get; set; } = "";
		public string accion { get; set; } = "";
		public string fec_movto { get; set; } = "";
		public string estatusRma { get; set; } = "";
		public string ProcesoAut { get; set; } = "";
		public string idProceso { get; set; } = "";
		public string cancellationReason { get; set; } = "";
	}

	public class DevolucionModel
    {
		public string OrderId { get; set; } = "";
		public string clientEmail { get; set; } = "";
		public string accion { get; set; } = "";
		public string fec_movto { get; set; } = "";
		public string estatusRma { get; set; } = "";
		public string ProcesoAut { get; set; } = "";
		public string idProceso { get; set; } = "";
		public string cancellationReason { get; set; } = "";
	}

	public class DetalleProducto
    {
		public string ProductName { get; set; } = "";
		public string Quantity { get; set; } = "";
		public string Price { get; set; } = "";
		public string CodeBarra { get; set; } = "";
		public string ProductId { get; set; } = "";
	}

	public class ParticipacionFormaPago
    {
		public string MethodPayment { get; set; } = "";
		public string UeType { get; set; } = "";
		public string Total { get; set; } = "";
	}
}