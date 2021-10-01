using System;
using System.Collections.Generic;

#nullable disable

namespace DL.Entities
{
    public partial class Manager
    {
        public Manager()
        {
            Stores = new HashSet<Store>();
        }

        public string Phonenumber { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Password1 { get; set; }

        public virtual ICollection<Store> Stores { get; set; }
    }
}
