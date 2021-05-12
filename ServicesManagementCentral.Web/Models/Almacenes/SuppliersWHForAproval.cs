using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Almacenes
{
    public class SuppliersWHForAproval
    {
        public int noProveedor { get; set; }
        public string nombreProv { get; set; }
        public int noAlmacen { get; set; }
        public string TipoAlmacen { get; set; }
        public bool habilitado { get; set; }
        public string inventario { get; set; }
        public string diasTrans { get; set; }
        public string nombreAlmacen { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public string FechaStock { get; set; }
        public int NumCodigos { get; set; }
    }
}