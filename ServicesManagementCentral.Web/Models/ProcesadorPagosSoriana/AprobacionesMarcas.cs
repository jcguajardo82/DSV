using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ProcesadorPagosSoriana
{
    public class AprobacionesMarcas
    {
        public string canalCompra { get; set; } = "";
        public string tipoTarjeta { get; set; } = "";
        public string marca { get; set; } = "";
        public string totalOrdenes { get; set; } = "";
        public string ordenesAprobadas { get; set; } = "";
        public string porcentajeAprobacion { get; set; } = "";
        public string ordenesRechazadas { get; set; } = "";
        public string porcentajeRechazo { get; set; } = "";
        public string monto { get; set; } = "";
    }

    public class Aprobaciones
    {
        public string Id_Num_Orden { get; set; }
        public string id_num_apl { get; set; }
        public string id_num_formapago { get; set; }
        public string nom_pagOrig { get; set; }
        public string tipoOrden { get; set; }
        public string imp_preciounit { get; set; }
    }
}