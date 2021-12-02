using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.NivelExistencia
{
    public class JsonData
    {
        public int IdTienda { get; set; }
        public int IdOwner { get; set; }
        public string Nombre { get; set; }
        public string NombreProveedor { get; set; }
    }
}