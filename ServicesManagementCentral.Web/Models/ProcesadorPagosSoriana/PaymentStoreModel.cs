using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.ProcesadorPagosSoriana
{
    public class PaymentStoreModel
    {
        public string OrderReferenceNumber { get; set; }
        public string OrderAmount { get; set; }
        public string LineaCaptura { get; set; }
        public string Estatus { get; set; }
        public string CreatedDate { get; set; }
    }

    public class PaymentStoreModelResponse
    {
        public string OrdenID { get; set; } = "";
        public string IDtransaccion { get; set; } = "";
        public string FechaCreacion { get; set; } = "";
        public string HoraCreacion { get; set; } = "";
        public string NoAfiliacion { get; set; } = "";
        public string Adquirente { get; set; } = "";
        public string Catalogo { get; set; } = "";
        public string TipoEntrega { get; set; } = "";
        public string CanalCompra { get; set; } = "";
        public string FormaPago { get; set; } = "";
        public string NoTienda { get; set; } = "";
        public string NombreTienda { get; set; } = "";
        public string noCajero { get; set; } = "";
        //fecha creacion de la orden
        //hora de creacion de la orden
        public string montoPagado { get; set; } = "";
        public string precioTotalOrden { get; set; } = "";
        public string fechaPago { get; set; } = "";
        public string formaPago { get; set; } = "";
        public string Banco { get; set; } = "";
        public string NoAutorizacion { get; set; } = "";
        public string BIN { get; set; } = "";
        public string Sufijo { get; set; } = "";
        public string TipoTarjeta { get; set; } = "";
        public string Marca { get; set; } = "";
        public string formato { get; set; } = "";
        public string ciudadEstatusPago { get; set; } = "";
        public string CostoEnvio { get; set; } = "";
        public string MSI { get; set; } = "";
        public string PuntosAplicados { get; set; } = "";
        public string PromocionesAplicadas { get; set; } = "";
        public string NombrePersonaRegistrada { get; set; } = "";
        public string Apellido_P { get; set; } = "";
        public string Apellido_M { get; set; } = "";
        public string NoTarjetalealtad { get; set; } = "";
        public string Correo { get; set; } = "";
        public string EstatusOrden { get; set; } = "";
        public string EstatusEnvío { get; set; } = "";
        public string almacenSurtio { get; set; } = "";
        public string loyalty { get; set; } = "";
        public string CreteOrderStore { get; set; } = "";
        public string HoraOrderStore { get; set; } = "";
    }

}