using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Consignaciones
{
    public class DetalleConsignaciones
    {

            public string EAN { get; set; }
            public string ID { get; set; }
            public string Descripcion { get; set; }
            public string Piezas { get; set; }
            public string CostoUnitario { get; set; }
            public string PesoVolumetrico { get; set; }
            public string DiasDesdeCreacion { get; set; }
            public string DiasDesdeAutoriza { get; set; }
            public string FechaEnvio { get; set; }
            public string HoraEnvio { get; set; }
            public string IdRMA { get; set; }
            public string StatusRMA { get; set; }

        


    }
}