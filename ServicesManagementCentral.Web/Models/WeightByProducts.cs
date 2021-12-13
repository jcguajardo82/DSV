﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class WeightByProducts
    {
        public decimal PesoVol { get; set; }
        public decimal Peso { get; set; }
        public long Product { get; set; }
        public decimal Width { get; set; }
        public decimal Lenght { get; set; }
        public decimal Height { get; set; }
    }
}