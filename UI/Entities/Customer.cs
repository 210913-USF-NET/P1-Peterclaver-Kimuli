using System;
using System.Collections.Generic;

#nullable disable

namespace UI.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Customerorders = new HashSet<Customerorder>();
        }

        public string Phonenumber { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Password1 { get; set; }

        public virtual ICollection<Customerorder> Customerorders { get; set; }
    }
}
