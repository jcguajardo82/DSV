using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class GastosVehiculoModel
    {
        public int IdConsecutivo { get; set; }
        public int IdGasto { get; set; }
        public int Id_Vehiculo { get; set; }
        public string FecGasto { get; set; }
        public string Kilometraje { get; set; }
        public string CantidadGasto { get; set; }
        public bool Estatus { get; set; }
        public string CreateDate { get; set; }
        public string CreateTime { get; set; }
        public string activo { get; set; }
        public string FecMovto { get; set; }
        public string TimeMovto { get; set; }
        public string created_user { get; set; }
        public string modified_user { get; set; }
    }
}