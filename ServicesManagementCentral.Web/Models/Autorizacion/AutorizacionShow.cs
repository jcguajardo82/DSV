using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Autorizacion
{
    public class AutorizacionShow
    {

        public int Id_cancelacion { get; set; }
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }
        public string JsonRequest { get; set; }
        public bool Bit_Envio { get; set; }
        public string Fec_movto { get; set; }
        public string Accion { get; set; }
        public string EstatusRma { get; set; }
        
        public string Desc_Est { get; set; }
        public string FecOrder { get; set; }
        public string Origen { get; set; }
        public string ProcesoAut { get; set; }
        public string WarehouseType { get; set; }
        public string DescAut { get; set; }



       

    }
}