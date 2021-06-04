using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ReemvioMcia
{
    public class DetalleReenvioMciaCEDIS
    {
        public string RMA { get; set; }
        public string MotivoRMA { get; set; }
        public string ID { get; set; }
        public string Descripcion { get; set; }
        public string NumeroGuiaDevolucion { get; set; }
        public string CondicionEmpaque { get; set; }
        public string Guiareenvio { get; set; }
        public string Estatusguiareenvio { get; set; }
        public string Fechaentrega { get; set; }
        public string Nombrequienrecibe { get; set; }

    }
}