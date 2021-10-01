using System;
using System.Collections.Generic;

#nullable disable

namespace UI.Entities
{
    public partial class Store
    {
        public Store()
        {
            Customerorders = new HashSet<Customerorder>();
        }

        public string Number { get; set; }
        public string Location { get; set; }
        public string Zipcode { get; set; }
        public string Managerphone { get; set; }

        public virtual Manager ManagerphoneNavigation { get; set; }
        public virtual ICollection<Customerorder> Customerorders { get; set; }
    }
}
