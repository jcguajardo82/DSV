using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Consignaciones
{
    public class ConsignacionesAdm
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

        public string CostoEnvio { get; set; }
        public string PesoVol { get; set; }
        public string CpOrigen { get; set; }
        public string CpDestino { get; set; }
        public string EdoOrigen { get; set; }
        public string EdoDestino { get; set; }
        public string CiudadOrigen { get; set; }
        public string CiudadDestino { get; set; }
        
        public string NroAlmacen { get; set; }
        public string NombreAlmacen { get; set; }
        public string TipoEnvio { get; set; }
        public string Transportista { get; set; }
        public string ConsignacionReenvio { get; set; }
        public string GuiaDevolNro { get; set; }
        public string GuiaDevolStatus { get; set; }
        public string GuiaReenvioNro { get; set; }
        public string GuiaReenvioStatus { get; set; }

        public string GuiaEnvio { get; set; }
        public string GuiaVig { get; set; }
        public string FechaPago { get; set; }
        public string FechaLimite { get; set; }

        ////[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        //public string FechaStock { get; set; }
        //public int NumCodigos { get; set; }

    }
}