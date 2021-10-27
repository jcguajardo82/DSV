using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.CallCenter
{
    public class ArtCar_d
    {

        //public string Id_Num_Car { get; set; }
        public int Id_Num_Sku { get; set; }
        //public string Id_Num_ArtCar_Tipo { get; set; }
        //public string Num_SkuRef { get; set; }
        public decimal Cant_Unidades { get; set; }
        //public string Cant_Surtida { get; set; }
        public decimal Precio_VtaNormal { get; set; }
        public decimal Precio_VtaOferta { get; set; }
        public decimal Dcto { get; set; }
        public string obs { get; set; }
        public string Desc_art { get; set; }
        public string Cve_UnVta { get; set; }

        public decimal Num_CodBarra { get; set; }

        public string Sustituto { get; set; }
        //public string Impto { get; set; }
        //public string Bit_SkuObligadoComp { get; set; }
        //public string Bit_SkuObligadoSurt { get; set; }
        //public string Fec_Movto { get; set; }

    }


}