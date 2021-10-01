using System;
using System.Collections.Generic;

#nullable disable

namespace DL.Entities
{
    public partial class Lineitem
    {
        public int Id { get; set; }
        public int Orderid { get; set; }
        public int Productid { get; set; }
        public string Productname { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }

        public virtual Customerorder Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
