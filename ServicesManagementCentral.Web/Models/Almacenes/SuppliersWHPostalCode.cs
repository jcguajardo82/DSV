using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Almacenes
{
    public class SuppliersWHPostalCode
    {
        public long cnscPostalCode { get; set; }
        public string Country { get; set; }
        public string Region1Name { get; set; }
        public string Region2Name { get; set; }
        public string Locality { get; set; }
        public string SubLocality { get; set; }
    }
}