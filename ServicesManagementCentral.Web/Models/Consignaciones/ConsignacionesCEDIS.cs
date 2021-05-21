using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Consignaciones
{
    public class ConsignacionesCEDIS
    {


            public string Consignacion { get; set; }
            public string TotalConsignacion { get; set; }
            public string TipoAlmacen { get; set; }
            public string FechaCreacion { get; set; }
            public string HoraCreacion { get; set; }
            public string NombreCliente { get; set; }
            public string EstatusEnvio { get; set; }
            public string EstatusConsignacionAlmacen { get; set; }
            public string EstatusConsignacionEntrega { get; set; }
            public string NroOrdenCompra { get; set; }
            public string EstatusOrdenCompra { get; set; }



            ////[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
            //public string FechaStock { get; set; }
            //public int NumCodigos { get; set; }

      

    }
}