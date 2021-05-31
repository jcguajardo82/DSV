using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.NivelExistencia
{
    public class upCorpOMS_Cns_UeNoTotalsByOrder
    {
        public string EAN { get; set; }
        public string SKU { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public string NroProveedor { get; set; }
        public string TipoAlmacen { get; set; }
        public string NombreProveedor { get; set; }
        public string NombreAlmacen { get; set; }
        public string NivelExistencia { get; set; }
        public string InvReservado { get; set; }
        public string InvVenta { get; set; }
        public string InvSeguridad { get; set; }
        public string TipoArticulo { get; set; }
        public string Largo { get; set; }
        public string Alto { get; set; }
        public string Ancho { get; set; }
        public string Peso { get; set; }
        public string PesoVol { get; set; }
        public string PesoReal { get; set; }
        public string EstatusProducto { get; set; }
        public string CostoMaterial { get; set; }
        public string FechaCreacion { get; set; }
        public string NroProvOrigen { get; set; }
        public string NomProvOrigen { get; set; }
    }
}