using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class ProductDimensionsModel
    {
        public decimal ANCHO { get; set; }
        public decimal LARGO { get; set; }
        public decimal ALTO { get; set; }
        public decimal PESO { get; set; }
        public string UNIDAD_DIM { get; set; }
        public string UNIDAD_PES { get; set; }
    }
}