using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class VehiculoModel
    {
        public int Id_Vehiculo { get; set; }
        public string Descripcion { get; set; }
        public string Placas { get; set; }
        public string Motor { get; set; }
        public bool Estatus { get; set; }
        public string Fec_Movto { get; set; }
        public string created_user { get; set; }
        public string modified_user { get; set; }

        public int Id_TipoVehiculo { get; set; }
        public string TipoVehiculo { get; set; }
        public string Marca { get; set; }
        public string Anio { get; set; }

        // Modificar 

        public int IdGasto { get; set; }
        public string FecGasto { get; set; }
        public int Kilometraje { get; set; }
        public decimal CantidadGasto { get; set; }

        public string CreateDate { get; set; }
        public string activo { get; set; }


    }
}