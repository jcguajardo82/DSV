using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class Gastos
    {
        public int IdGasto { get; set; }
        public string Desc_Gasto { get; set; }
        public string Activo { get; set; }
        public string CreateDate { get; set; }
        public string CreateTime { get; set; }
        public string Created_User { get; set; }
        public string FecMovto { get; set; }
        public string TimeMovto { get; set; }
        public string Modified_User
        {
            get; set;
        }
    }
}