using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ProcesadorPagosSoriana
{
    public class ParticipacionFormasPagoModel
    {
        public string metodoPago { get; set; } = "";
        public string fecha { get; set; } = "";
        public string mes { get; set; } = "";
        public string ordenesCantidad { get; set; } = "";
        public string monto { get; set; } = "";
        public string clientesCantidad { get; set; } = "";
        public string Catalogo { get; set; } = "";

    }
}