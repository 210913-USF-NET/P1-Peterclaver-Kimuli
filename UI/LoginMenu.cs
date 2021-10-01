using System;
using Models;
using DL;
using BL;
using System.Collections.Generic;
using Serilog;

namespace UI
{
    public class LoginMenu : IMenu
    {
        private IBL _bl;

        public LoginMenu(IBL bl){
            _bl = bl;
        }

        string phonenumber;
        
        public void Start()
        {
            login:
            Log.Information("Logging in...");
            Console.WriteLine("\nLogin Please...");

            Console.WriteLine("Phonenumber: "); 
            phonenumber = Console.ReadLine();

            Console.WriteLine("Password: "); 
            string password = Console.ReadLine();

            List<Customer> allCustomers =_bl.GetLoggedInCustomer(phonenumber, password);

            List<Manager> allManagers =_bl.GetManagers(phonenumber, password); 

            //check if user is in the customers' DB
            if(allCustomers.Count > 0 && allCustomers != null)
            {
                Log.Information("\nSuccessfully Logged in!");
                MenuFactory.GetMenu("customerinterface", allCustomers[0]).Start();
            }

            //if not in customers' DB then check if user is in the managers' DB
            else if(allManagers.Count > 0 && allManagers != null)
            {
                Log.Information("\nSuccessfully Logged in!");
                MenuFactory.GetMenu("managerinterface", allManagers[0]).Start();
            }

            //Failed login attempt
            else
            {
                Log.Information("Failed login attempt!");
                Console.WriteLine("The information does not match our records! Please try again.");
                goto login;
            }
        }
    }
}