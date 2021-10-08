using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class ProductVM
    {
        public ProductVM() { }

        public ProductVM(Product product)
        {
            this.Id = product.Id;
            this.Name = product.Name;
            this.Quantity = product.Quantity;
            this.UnitPrice = product.UnitPrice;
            this.StoreID = product.StoreID;
        }
        public int Id { get; set; }
        public string StoreID { get; set; }

        //Setting properties
        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Quantity can only have numbers!")]
        public int Quantity { get; set; }

        //creating password validation
        [Required]
        [RegularExpression("^[0-9 .]+$", ErrorMessage = "Price can only have numbers!")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// This method is used to map CustomerVM model in Web.Models to Customer Model in Models
        /// </summary>
        /// <returns>Returns a customer object</returns>
        public Product ToModel()
        {
            Product product;
            try
            {
                product = new Product
                {
                    Id = this.Id,
                    Name = this.Name,
                    Quantity = this.Quantity,
                    UnitPrice = this.UnitPrice,
                    StoreID = this.StoreID
                };
            }
            catch
            {
                throw;
            }

            return product;
        }
    }
}
