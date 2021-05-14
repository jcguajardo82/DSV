using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServicesManagementCentral.Web.Models.SubirArchivos
{
    public class CargaManuales
    {
        [Required]
        [DisplayName("Examinar")]public HttpPostedFileBase Examinar { get; set; }

  
        [DisplayName("Cadena")]
        public string Cadena { get; set; }

        public int idOwner { get; set; }
        public string ownerName { get; set; }
    }
}