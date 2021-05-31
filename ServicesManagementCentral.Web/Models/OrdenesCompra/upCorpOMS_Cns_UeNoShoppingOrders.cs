using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.OrdenesCompra
{
    public class upCorpOMS_Cns_UeNoShoppingOrders
    {
        public string consignacion { get; set; }
        public string TipoConsignacion { get; set; }
        public string EstatusPedido { get; set; }
        public string EstatusConsignacion { get; set; }
        public int NroProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public int NroAlmacen { get; set; }
        public string NombreAlmacen { get; set; }
        public int OrdenCompra { get; set; }
        public decimal Importe { get; set; }
        public string EstatusOrdenCompra { get; set; }
        public string PeriodoOrdenCompra { get; set; }
        public string FechaPedido { get; set; }
        public string HoraPedido { get; set; }
        public string FechaPedidoRecibo { get; set; }
        public string HoraPedidoRecibo { get; set; }
    }
}