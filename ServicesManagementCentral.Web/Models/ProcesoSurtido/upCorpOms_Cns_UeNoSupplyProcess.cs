using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ProcesoSurtido
{
    public class upCorpOms_Cns_UeNoSupplyProcess
    {
        public int OrderNo { get; set; }
        public string UeNo { get; set; }
        public int SKU { get; set; }
        public string EAN { get; set; }
        public string Descripcion { get; set; }
        public double Piezas { get; set; }
        public double Precio { get; set; }
        public double PrecioOrigen { get; set; }
        public string UnidadMedida { get; set; }
        public string PesoAsignado { get; set; }
        public string Observaciones { get; set; }
        public string TipoEnvio { get; set; }
        public bool Suplido { get; set; }


    }

    public class msj
    {
        public string mensaje { get; set; }
    }

    public class Encabezado
    {
        public string Concatenado { get; set; }
        public string idSupplierWH { get; set; }
        public string SupplierName { get; set; }
    }

}