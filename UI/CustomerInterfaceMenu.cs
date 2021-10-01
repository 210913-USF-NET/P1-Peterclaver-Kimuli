using System;
using System.Collections.Generic;
using Models;
using BL;
using Serilog;

namespace UI
{
    public class CustomerInterfaceMenu : IMenu
    {
        private CBL _bl; 
        private Customer _cust;
        public CustomerInterfaceMenu(){}
        public CustomerInterfaceMenu(CBL bl, Customer cust){
            _bl = bl;
            this._cust = cust;
        }
        public void Start()
        {
            Console.WriteLine($"\nWelcome {_cust.Name}. Please use the menu below to navigate through the App.");

            bool exit = false;

            do{
                Console.WriteLine("[1] Type 1 to select a store");
                Console.WriteLine("[2] Type 2 to check your previous orders... type x to exit!");

                switch(Console.ReadLine())
                {
                    case "1":
                        SelectStore();
                        break;
                    case "2":
                        returnOrders();
                        break;
                    case "x":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Please type the correct input");
                        break;
                }
            }while(!exit);
            
        }

        private void SelectStore()
        {
            menu:
            List<Store> storesToSelect = _bl.GetCustomerStores();
            Console.WriteLine("\nPlease select from the stores below:");
            if(storesToSelect.Count == 0 || storesToSelect == null)
            {
                Console.WriteLine("There are no stores currently\n");
                return;
            }
            for(int i=0; i<storesToSelect.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {storesToSelect[i].Number}, {storesToSelect[i].Location} {storesToSelect[i].Zipcode}");
            }

            string input = Console.ReadLine();
            int parsedInput;
            bool parseSuccess = Int32.TryParse(input, out parsedInput);
            if (parseSuccess && parsedInput >= 0 && parsedInput <= storesToSelect.Count)
            {
                int actualInput = parsedInput - 1;
                Log.Information("Logging into the store customer menu");
                MenuFactory.GetMenu("storecustomermenu", storesToSelect[actualInput], _cust).Start();
            }
            else
            {
                Console.WriteLine("Invalid input.");
                goto menu;
            }
        }

        private void returnOrders(){
            List<Order> orders = _bl.GetCustomerOrders(_cust.Phonenumber);
            if(orders.Count == 0 || orders == null)
            {
                Console.WriteLine("You have not yet made any orders\n");
                return;
            }
            Console.WriteLine("\nBelow are your orders:");
            for(int i=0; i<orders.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] Store:{orders[i].StoreID} Order number: {orders[i].Id} Date: {orders[i].OrderDate.ToString("d")}");
                Console.WriteLine("Items:");
                for(int t = 0; t < orders[i].Items.Count; t++)
                {
                    Console.WriteLine($"{orders[i].Items[t].ProductName} ({orders[i].Items[t].Quantity}) Cost: {orders[i].Items[t].Cost}");
                }
                Console.WriteLine($"Total cost: {orders[i].Total}");
                Console.WriteLine("------------------------------------");
            }
            Console.WriteLine("Type c to have your orders sorted by Total cost... or any other key to exit.");
            string input = Console.ReadLine();
            if(input == "C" || input == "c")
            {
                List<Order> orders_c = _bl.GetCustomerOrdersByCost(_cust.Phonenumber);
                if(orders_c.Count == 0 || orders_c == null)
                {
                    Console.WriteLine("You have not yet made any orders\n");
                    return;
                }
                Console.WriteLine("\nBelow are your orders sorted by cost:");
                for(int i=0; i<orders_c.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}] Store:{orders_c[i].StoreID} Order number: {orders_c[i].Id} Date: {orders_c[i].OrderDate.ToString("d")}");
                    Console.WriteLine("Items:");
                    for(int t = 0; t < orders_c[i].Items.Count; t++)
                    {
                        Console.WriteLine($"{orders_c[i].Items[t].ProductName} ({orders_c[i].Items[t].Quantity}) Cost: {orders_c[i].Items[t].Cost}");
                    }
                    Console.WriteLine($"Total cost: {orders_c[i].Total}");
                    Console.WriteLine("------------------------------------\n");
                }
            }
            
        }
    }
}