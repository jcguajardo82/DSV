using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.CallCenter
{
    public class ProdCheckList
    {
        public int IdPregunta { get; set; }
        public string ProdId { get; set; }

        public int Resp { get; set; }
    }
}