using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class StoreProducts
    {
        public int Id { get; set; }
        public string StoreID{get; set;}
        public int ProductID{get; set;}
        public Product Product { get; set; }
        public Store Store { get; set; }
    }
}