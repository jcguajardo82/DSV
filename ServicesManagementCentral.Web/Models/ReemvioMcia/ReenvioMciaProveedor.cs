using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ReemvioMcia
{
    public class ReenvioMciaProveedor
    {
        public string Consignacion { get; set; }
        public string ConsignacionReenvio { get; set; }
        public string TipoAlmacen { get; set; }
        public string FechaOrden { get; set; }
        public string HoraOrden { get; set; }
        public string Cliente { get; set; }
        public string EstatusPedidoSurtido { get; set; }
        public string EstatusPedidoEnvio { get; set; }
        public string EstatusGuiaDevol { get; set; }
        public string GuiaReenvioNro { get; set; }
        public string GuiaReenvioEstatus { get; set; }
        public string GuiaVig { get; set; }       
        public string MotivoReenvio { get; set; }
        public string OrdenDSV { get; set; }
        public string OrdenDSVStatus { get; set; }

        public int OrderNo { get; set; }


        //nuevas columnas reenvio concluido

        public string TipoGuia { get; set; }
        public string Paqueteria { get; set; }
        public string GuiaStatus { get; set; }
        public string FechaEntrega { get; set; }
        public string HoraEntrega { get; set; }
        public string QuienRecibio { get; set; }
        public string NroAlmacen { get; set; }
        public string NombreAlmacen { get; set; }
        public string NroProveedor { get; set; }
        public string NombreProveedor { get; set; }

    }
}