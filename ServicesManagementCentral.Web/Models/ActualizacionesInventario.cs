using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class ActualizacionesInventario
    {
    }

    public class OrderFacts_UE
    {
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public string tipoAlmacen { get; set; }
        public int TotalConsignaciones { get; set; }
    }

    public class OrderFacts_UE2
    {
        public string UeNo { get; set; }
        public string tipoAlmacen { get; set; }
        public string horas { get; set; }

    }

    public class OrderFacts_CreditNotes
    {
        public int NroProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string NombreAlmacen { get; set; }
        public decimal FolioOrdenCompra { get; set; }
        public decimal ImporteOrdenCompra { get; set; }
        public int FolioNotaCredito { get; set; }
        public decimal ImporteNotaCredito { get; set; }
        public string Motivo { get; set; }
    }

    public class OrderFacts_CreditNotes2
    {
        public int NroProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string Consignacion { get; set; }
        public int NivelServicio { get; set; }
        public int FolioNotaCredito { get; set; }
        public decimal ImporteNotaCredito { get; set; }
        public string Motivo { get; set; }
    }
}