using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Autorizacion
{
    public class RequestDevolucionModel
    {
        public string No_order { get; set; }
        public string No_order_UE { get; set; }
        public List<ItemModels> items { get; set; }
    }
    public class ItemModels
    {
        public string codigo { get; set; }
        public string cantidad { get; set; }
        public string precio { get; set; }
        public string itemTotal { get; set; }
    }
}