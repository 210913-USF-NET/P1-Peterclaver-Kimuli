using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class StoreVM
    {
        public StoreVM() { }

        public StoreVM(Store store)
        {
            this.Id = store.Id;
            this.ManagerPhone = store.ManagerPhone;
            this.Number = store.Number;
            this.Location = store.Location;
            this.Zipcode = store.Zipcode;
        }
        public int Id { get; set; }
        public string ManagerPhone { get; set; }

        //Setting properties
        [Required]
        public string Number { get; set; }

        [Required]
        public string Location { get; set; }

        //creating password validation
        [Required]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Zipcode should have five digits")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Please type the correct zipcode")]
        public string Zipcode { get; set; }

        /// <summary>
        /// This method is used to map CustomerVM model in Web.Models to Customer Model in Models
        /// </summary>
        /// <returns>Returns a customer object</returns>
        public Store ToModel()
        {
            Store store;
            try
            {
                store = new Store
                {
                   Id = this.Id,
                   ManagerPhone = this.ManagerPhone,
                   Number = this.Number,
                   Location = this.Location,
                   Zipcode = this.Zipcode
                };
            }
            catch
            {
                throw;
            }

            return store;
        }
    }
}
