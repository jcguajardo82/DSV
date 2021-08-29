using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class ShipmentLocksModel
    {
        public string IdPackingType { get; set; }
        public bool LockLength { get; set; }
        public bool LockWidth { get; set; }
        public bool LockHeight { get; set; }
        public bool LockWeight { get; set; }
    }
}