using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.CallCenter
{
    public class Historial
    {
        public int Id_Num_Orden { get; set; }
        public int OrderNo { get; set; }
        public string OrderDate { get; set; }
        public decimal Imp_Pago { get; set; }
        public string Id_Num_Car { get; set; }
        public string UeNo { get; set; }
        public decimal TotalArt { get; set; }

    }
}