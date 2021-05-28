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
        public decimal Piezas { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioOrigen { get; set; }
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
}