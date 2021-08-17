using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.CallCenter
{
    public class up_Corp_cns_OrderInfo
    {

    }
    public class Order
    {
        public string Orderid { get; set; }
        public int Clientid { get; set; }
        public string Clientname { get; set; }
        public string Clientemail { get; set; }
        public string Clientphone { get; set; }



    }

    public class OrderDetail
    {

        public string ShipmentId { get; set; }
        public int Position { get; set; }
        public decimal ProductId { get; set; }
        public decimal Quantity { get; set; }
        public int OrderNo { get; set; }


    }

    public class OrderDetailCap
    {
        public decimal ProductId { get; set; }
        public decimal NewQuantity { get; set; }
    }

}