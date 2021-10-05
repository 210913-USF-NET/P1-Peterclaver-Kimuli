using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models;

namespace WebUI.Models
{
    public class CustomerVM
    {
        public CustomerVM() { }

        public CustomerVM(Customer cust)
        {
            this.Id = cust.Id;
            this.Name = cust.Name;
            this.Phonenumber = cust.Phonenumber;
            this.Password = cust.Password;
            this.Password2 = cust.Password2;
        }
        public int Id { get; set; }
        public List<Order> Orders { get; set; }
       
        //Setting properties
        [Required]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name can only have alphabetical characters!")]
        public string Name{ get; set; }

        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Phonenumber can only have numbers!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage ="Phone number should be 10 figures")]
        public string Phonenumber{ get; set; }

        //creating password validation
        [Required]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Password should have more than 4 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage ="This field is required")]
        [Compare("Password", ErrorMessage ="The passwords must match")]
        public string Password2 { get; set; }
        
        /// <summary>
        /// This method is used to map CustomerVM model in Web.Models to Customer Model in Models
        /// </summary>
        /// <returns>Returns a customer object</returns>
        public Customer ToModel()
        {
            Customer cust;
            try
            {
                cust = new Customer
                {
                    Id = this.Id,
                    Name = this.Name,
                    Phonenumber = this.Phonenumber,
                    Password = this.Password,
                    Password2 = this.Password2
                };
            }
            catch
            {
                throw;
            }

            return cust;
        }
    }
}
