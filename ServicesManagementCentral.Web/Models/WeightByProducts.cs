using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class WeightByProducts
    {
        public decimal PesoVol { get; set; }
        public decimal Peso { get; set; }
        public long Product { get; set; }
        public decimal Width { get; set; }
        public decimal Lenght { get; set; }
        public decimal Height { get; set; }
        public string Cve_CategSAP { get; set; }
        public string Cve_GciaCategSAP { get; set; }
        public string Cve_GpoCategSAP { get; set; }
        public string Desc_CategSAP { get; set; }
        public string SAT_IdProdServ { get; set; }
        public string SAT_UniMedProd { get; set; }
        public string SAT_UniMedProdNombre { get; set; }
    }
}