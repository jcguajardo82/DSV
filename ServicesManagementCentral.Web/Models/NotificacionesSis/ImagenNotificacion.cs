using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.NotificacionesSis
{
    public class ImagenNotificacion
    {
        public int IdImagen { get; set; }
        public string Titulo { get; set; }
        public byte[] Imagen { get; set; }
        public string BitActivo { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public string CreatedDate { get; set; }
        public string CreationUser { get; set; }
        public string ModifDate { get; set; }
        public string ModifUser { get; set; }
        public string strImagen { get; set; }


                                           
    }
}