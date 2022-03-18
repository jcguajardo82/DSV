using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ProcesoSurtido
{
    public class OrderFacts_UE_CancelCauses
    {
        public int IdCause { get; set; }
        public string Cause_Desc { get; set; }
        public int idOwner { get; set; }
        public bool Bit_deleted { get; set; }
    }

    public class MotivosSolicitud
    {
        public int IdCause { get; set; }
        public string Cause_Desc { get; set; }
        public string FecMvo { get; set; }
        public bool Bit_deleted { get; set; }
    }
}