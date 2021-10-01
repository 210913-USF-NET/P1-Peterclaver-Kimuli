using System;
using Models;
using BL;
using System.Collections.Generic;
using Serilog;


namespace UI
{
    public class SignupMenu : IMenu
    {
         private IBL _bl;

        /// <summary>
        /// Repo instance
        /// </summary>
        /// <param name="bl">Repo instance</param>
        public SignupMenu(IBL bl)
        {
            _bl = bl;
        }

        public void Start() {
            //bool exit = false;
            
            Console.WriteLine("\nType A to create an Account... Type x to return to the Main menu.");
            

            switch (Console.ReadLine())
            {
                case "A":
                    CreateCustomer();
                    break;
                // case "1":
                //     ViewAllCustomers();
                //     break;
                /* case "x":
                    exit = true;
                    break; */
                default:
                    Console.WriteLine("Please type the correct input\n");
                    break;
            }

        }

        //Creating customer account

        private void CreateCustomer()
        {
            Log.Information("Creating Account");
            Console.WriteLine("\nPlease fill in the required information");

            Customer newCust = new Customer();
            name:
            Console.WriteLine("Name: ");
            string name = Console.ReadLine();
            try{
                newCust.Name = name;
            }
            catch(InputInvalidException e){
                Console.WriteLine(e.Message);
                goto name;
            }

            phonenumber:
            Console.WriteLine("Phone number: ");
            string phonenumber = Console.ReadLine();
            try{
                newCust.Phonenumber = phonenumber;
            }
            catch(InputInvalidException e){
                Console.WriteLine(e.Message);
                goto phonenumber;
            }
            
            password:
            Console.WriteLine("Password: ");
            string password = Console.ReadLine();
            try{
                newCust.Password = password;
            }
            catch(InputInvalidException e){
                Console.WriteLine(e.Message);
                goto password;
            }

            password2:
            Console.WriteLine("Please confirm your Password: ");
            string password2 = Console.ReadLine();
            try{
                newCust.Password2 = password2;
            }
            catch(InputInvalidException e){
                Console.WriteLine(e.Message);
                goto password2;
            }

            //Customer newCust = new Customer(name, email, password, phonenumber, zipcode);
            _bl.AddCustomer(newCust);
            Console.WriteLine($"Congratulations! Your account is created: {newCust.Name}");
            Log.Information("Account successfully created!"); 

            MenuFactory.GetMenu("customerinterface", newCust).Start();
        }

        // private void ViewAllCustomers()
        // {
        //     List<Customer> allCust = _bl.GetCustomers();

        //     foreach (Customer cust in allCust)
        //     {
        //         Console.WriteLine(cust.ToString());
        //     }
        // }
    }
}