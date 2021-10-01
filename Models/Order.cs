using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Order
    {
        public Order(){}
        public Order(decimal total, string customerPhone, string storeID)
        {
            this.Total = total;
            this.CustomerPhone = customerPhone;
            this.StoreID = storeID;
        }

        public int Id { get; set; }
        public decimal Total{get; set;}
        public string CustomerPhone{get; set;}
        public string StoreID{get; set;}
        public DateTime OrderDate{get; set;}
        public List<LineItem> Items{get; set;}
        public string CustomerName { get; set; }
    }
}