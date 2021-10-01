using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class LineItem
    {
        public LineItem(){}
        public LineItem(int quantity, int cost)
        {
            this.Quantity = Quantity;
            this.Cost = cost;
        }
        
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
    }
}