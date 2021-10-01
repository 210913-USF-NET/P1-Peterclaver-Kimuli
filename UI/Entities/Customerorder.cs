using System;
using System.Collections.Generic;

#nullable disable

namespace UI.Entities
{
    public partial class Customerorder
    {
        public Customerorder()
        {
            Lineitems = new HashSet<Lineitem>();
        }

        public int Id { get; set; }
        public decimal Total { get; set; }
        public string Customerphone { get; set; }
        public string Storeid { get; set; }
        public DateTime Orderdate { get; set; }

        public virtual Customer CustomerphoneNavigation { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<Lineitem> Lineitems { get; set; }
    }
}
