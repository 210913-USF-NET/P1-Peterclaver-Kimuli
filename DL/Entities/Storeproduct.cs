using System;
using System.Collections.Generic;

#nullable disable

namespace DL.Entities
{
    public partial class Storeproduct
    {
        public int Id { get; set; }
        public string Storeid { get; set; }
        public int Productid { get; set; }

        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
    }
}
