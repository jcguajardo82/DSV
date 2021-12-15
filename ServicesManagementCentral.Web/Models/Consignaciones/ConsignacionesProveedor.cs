using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Consignaciones
{
    public class ConsignacionesProveedor
    {

        public string Consignacion { get; set; }
        public string OrdenCompra { get; set; }
        public string TotalConsignacion { get; set; }
        public string TipoAlmacen { get; set; }
        public string FechaCreacion { get; set; }
        public string HoraCreacion { get; set; }
        public string NombreCliente { get; set; }
        public string EstatusEnvio { get; set; }
        public string EstatusConsignacionAlmacen { get; set; }
        public string EstatusConsignacionEntrega { get; set; }
        public string MotivoNoRecoleccion { get; set; }
        public string NroOrdenCompra { get; set; }
        public string EstatusOrdenCompra { get; set; }
        public string NroProveedor { get; set; }
        public string NombreProveedor { get; set; }

        public string ConsignacionReenvio { get; set; }
        public string GuiaDevolNro { get; set; }
        public string GuiaDevolStatus { get; set; }
        public string GuiaReenvioNro { get; set; }
        public string GuiaReenvioStatus { get; set; }
        
        public string GuiaEnvio { get; set; }
        public string GuiaVig { get; set; }
        public string FechaPago { get; set; }
        public string FechaLimite { get; set; }

        public string TipoGuia { get; set; }
        public string Paqueteria { get; set; }
        public string FechaEntrega { get; set; }
        public string HoraEntrega { get; set; }
        public string QuienRecibio { get; set; }

        public string FechaProg { get; set; }
        public string HoraProg { get; set; }

        public string GuiaStatus { get; set; }
        public string NroAlmacen { get; set; }
        public string NombreAlmacen { get; set; }
        public string ProductId { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string CostoConsignacion { get; set; }
        public string Categoria { get; set; }

        public string FechaSolicitudGuia { get; set; }
        public string PuntoDeVenta { get; set; }
        ////[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        //public string FechaStock { get; set; }
        //public int NumCodigos { get; set; }


    }
}