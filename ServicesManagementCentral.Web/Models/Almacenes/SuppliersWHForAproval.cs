﻿using System;
using System.Collections.Generic;
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
    }
}