using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Cotizador
{
    public class CotizadorResponseModel
    {
        public string meta { get; set; }
        public List<DataResponseModel> data { get; set; }
    }
}