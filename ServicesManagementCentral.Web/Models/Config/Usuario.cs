using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Config
{
    public class Usuario
    {
        public string idUsuario { get; set; }
        public string nombre { get; set; }       
        public bool activo { get; set; }
        public string usuario { get; set; }
        public string rol { get; set; }
        public string nombreRol { get; set; }
       
    }
}