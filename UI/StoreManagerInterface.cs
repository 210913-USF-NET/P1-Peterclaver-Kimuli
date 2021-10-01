using System;
using Models;
using BL;
using System.Collections.Generic;
using Serilog;
using System.Text.RegularExpressions;

namespace UI
{
    public class StoreManagerInterface : IMenu
    {
        private CBL _bl;
        private Store _store;

        public StoreManagerInterface(CBL bl, Store store)
        {
            _bl = bl;
            _store = store;
        }
        public void Start()
        {
            Console.WriteLine($"        Store: {_store.Number}");
            
            bool exit = false;

            do
            {
                Console.WriteLine("Navigate the store using menu below;");
                Console.WriteLine("[1] Type 0 to view Products.");
                Console.WriteLine("[2] Type 1 to add Products.");
                Console.WriteLine("[3] Type 2 to view orders.");
                Console.WriteLine("[4] Type 3 to search for a customer.");
                Console.WriteLine("[5] Type x to exit.");

                switch(Console.ReadLine())
                {
                    case "0":
                        ProductsReturned();
                        break;
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        StoreOrders();
                        break;
                    case "3":
                        CustomerSearch();
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

        private void AddProduct()
        {
            Console.WriteLine("\nCreate a product...");

            Product newProduct = new Product();
            name:
            Console.WriteLine("Product name: ");
            try{
                newProduct.Name = Console.ReadLine();
            }
            catch(InputInvalidException e){
                Console.WriteLine(e.Message);
                goto name;
            }
            
            quantity:
            Console.WriteLine("On-hand quantity: ");
            string quantity = Console.ReadLine();
            int parsedInt;
            bool parseSuccess = Int32.TryParse(quantity, out parsedInt);
            if(parseSuccess)
            {
                try
                {
                    newProduct.Quantity = parsedInt;
                }
                catch(InputInvalidException e)
                {
                    Console.WriteLine(e.Message);
                    goto quantity;
                }
            }
            else{
                System.Console.WriteLine("Please put the correct quantity.");
                goto quantity;
            }

            unitprice:
            Console.WriteLine("Unit price: ");
            string unitPrice = Console.ReadLine();
            decimal parsedDecimal;
            try 
            {
                parsedDecimal = System.Convert.ToDecimal(unitPrice);
            }
            catch (System.OverflowException)
            {
                System.Console.WriteLine(
                    "Please put a correct price.");
                goto unitprice;
            }
            catch (System.FormatException)
            {
                System.Console.WriteLine(
                    "The unit price should be a decimal number.");
                goto unitprice;
            }
            catch (System.ArgumentNullException) 
            {
                System.Console.WriteLine(
                    "Please fill in the unit price.");
                goto unitprice;
            }

            try 
            {
                newProduct.UnitPrice = parsedDecimal;
            }
            catch (InputInvalidException e)
            {
                Console.WriteLine(e.Message);
                goto unitprice;
            }

            newProduct.StoreID = _store.Number;

            Product addedProduct = _bl.AddProduct(newProduct);
            Log.Information("Product Successfully added");
            Console.WriteLine($"You have successfully added {addedProduct.Quantity} {addedProduct.Name} of unit price ${addedProduct.UnitPrice}");
        }

        private void ProductsReturned()
        {
            List<Product> products = _bl.GetProducts(_store.Number);
            item:
            Console.WriteLine("\nStore Items:");
            if(products.Count == 0 || products == null)
            {
                Console.WriteLine("There are currently no items in this store\n");
                return;
            }
            Log.Information("Displaying Products...");
            for(int i=0; i<products.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {products[i].Name}, In stock: {products[i].Quantity}, Unit Price: {products[i].UnitPrice}");
            }
            Console.WriteLine("Would you love to edit any product quantity?(y/n)");
            string input = Console.ReadLine();
            if(input == "y" || input == "Y")
            {
                Console.WriteLine("Type the list number of the product to select it:");
                string itemNumber = Console.ReadLine();
                int actualInput = 0;
                int parsedInput;
                bool parseSuccess = Int32.TryParse(itemNumber, out parsedInput);
                if (parseSuccess && parsedInput > 0 && parsedInput <= products.Count)
                {
                    actualInput = parsedInput - 1;
                    quantity:
                    Console.WriteLine("Type the new product quantity:");
                    string quantity = Console.ReadLine();
                    int parsedQuantity;
                    bool parseSuccess1 = Int32.TryParse(quantity, out parsedQuantity);
                    if (parseSuccess && parsedInput > 0)
                    {
                        products[actualInput].Quantity = parsedQuantity;
                        Console.WriteLine($"Please confirm the new quantity: {parsedQuantity} (y/n)");
                        if(Console.ReadLine() == "y" || Console.ReadLine() == "Y")
                        {
                            _bl.UpdateProduct(products[actualInput]);
                            Console.WriteLine("Product quanntity successfully edited!");
                            goto item;
                        }
                    }
                    else{
                        Console.WriteLine("Invalid Input! Please try again");
                        goto quantity;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input...Please type a number from the items list.");
                    goto item;
                }
            }
            else
            {
                return;
            }
        }

        private void CustomerSearch()
        {
            Console.WriteLine("\nPlease enter the name of the customer to search:");
            string customername = Console.ReadLine();

            List<Customer> returnedCust = _bl.GetCustomerSearch(customername);
            if(returnedCust.Count == 0 || returnedCust == null)
            {
                Console.WriteLine("There are currently no customers with that name\n");
                return;
            }
            
            Console.WriteLine("Search results:");

            for(int i=0; i<returnedCust.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {returnedCust[i].Name}, Phone number: {returnedCust[i].Phonenumber}");
            }
            Console.WriteLine("\n");
        }

        private void StoreOrders()
        {
            List<Order> orders = _bl.GetStoreOrders(_store.Number);
            if(orders.Count == 0 || orders == null)
            {
                Console.WriteLine("This store has no orders yet!\n");
                return;
            }
            Console.WriteLine("\nBelow are the Store orders:");
            Log.Information("Displaying orders...");
            for(int i=0; i<orders.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] Order number: {orders[i].Id} Customer:{orders[i].CustomerName}  Date: {orders[i].OrderDate.ToString("d")}");
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
                List<Order> orders_c = _bl.GetStoreOrdersByCost(_store.Number);
                Console.WriteLine("Below are your orders sorted by cost:");
                Console.WriteLine($"Order count: {orders_c.Count}");
                for(int i=0; i<orders_c.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}] Order number: {orders_c[i].Id} Customer:{orders_c[i].CustomerName}  Date: {orders_c[i].OrderDate.ToString("d")}");
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