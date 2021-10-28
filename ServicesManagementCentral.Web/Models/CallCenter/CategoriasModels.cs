using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.CallCenter
{
    public class CategoriasModels
    {
        public string IdCategoria { get; set; }// ": "2-en-1",
        public string IdCategoriaPadre { get; set; }// ": "equipos-de-computo",
        public string NombreCategoria { get; set; }// ": "2 en 1",
        public string urlImage { get; set; }// ": ""
    }
}