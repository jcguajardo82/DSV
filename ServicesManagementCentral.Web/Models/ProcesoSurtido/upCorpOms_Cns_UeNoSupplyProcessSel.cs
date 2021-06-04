using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ProcesoSurtido
{
    public class upCorpOms_Cns_UeNoSupplyProcessSel
    {
        public int OrderNo { get; set; }
        public string UeNo { get; set; }
        public string SKU { get; set; }
        public string EAN { get; set; }
        public string Descripcion { get; set; }
        public double Piezas { get; set; }
        public int ConGuia { get; set; }
        public bool Suplido { get; set; }

    }
}