using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Config
{
    public class Rol
    {
        public int idRol { get; set; }
        public string nombreRol { get; set; }
        public string descripcion { get; set; }
        public bool activo { get; set; }

    }

    public class MenuRolConfig : Menu
    {
        public int seleccionado { get; set; }
    }
}