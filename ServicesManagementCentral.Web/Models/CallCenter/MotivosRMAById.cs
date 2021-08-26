using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.CallCenter
{
    public class MotivosRMAById
    {
        public int Id_Padre { get; set; }
        public int Id_Motivo { get; set; }
        public string Descripcion_Motivo { get; set; }
    }
}