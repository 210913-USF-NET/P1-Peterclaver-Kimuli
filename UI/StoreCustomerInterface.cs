using System;
using Models;
using BL;
using System.Collections.Generic;
using Serilog;

namespace UI
{
    public class StoreCustomerInterface : IMenu
    {
        private CBL _bl;
        private Store _store;
        private Customer _cust;
        public StoreCustomerInterface(CBL bl, Store store, Customer cust)
        {
            _bl = bl;
            _store = store;
            _cust = cust;
        }
        public void Start()
        {
            Console.WriteLine($"        Store: {_store.Number}");
            
            bool exit = false;

            do
            {
                Console.WriteLine("Navigate the store using menu below;");
                Console.WriteLine("[1] Type 1 to place an order.");
                Console.WriteLine("[2] Type x to exit.");

                switch(Console.ReadLine())
                {
                    case "1":
                        PlaceOrder();
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

        public void PlaceOrder(){
            //menu:
            List<Product> products = _bl.GetProducts(_store.Number);
            Console.WriteLine("\nStore items");
            if(products.Count == 0 || products == null)
            {
                Console.WriteLine("There are currently no items in this store\n");
                return;
            }
            for(int i=0; i<products.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {products[i].Name}, In-stock: {products[i].Quantity}, Unit Price: {products[i].UnitPrice}");
            }

            List<LineItem> items = new List<LineItem>();
            Order order = new Order();
            
            bool exit = false;
            
            do
            {
                item:
                LineItem addedItem = new LineItem();
                Console.WriteLine("Type the list number of the product to select it:");
                string itemNumber = Console.ReadLine();
                int actualInput = 0;
                int parsedInput;
                bool parseSuccess = Int32.TryParse(itemNumber, out parsedInput);
                if (parseSuccess && parsedInput > 0 && parsedInput <= products.Count)
                {
                    actualInput = parsedInput - 1;
                    addedItem.ProductId = products[actualInput].Id;
                    addedItem.ProductName = products[actualInput].Name;
                }
                else
                {
                    Console.WriteLine("Invalid input...Please type a number from the items list.");
                    goto item;
                }

                quantity:
                Console.WriteLine("Type the numbers of items to purchase:");
                string itemQuantity = Console.ReadLine();
                int newQuantity = 0;
                int parsedQuantity;
                bool parseSuccess1 = Int32.TryParse(itemQuantity, out parsedQuantity);
                if (parseSuccess && parsedQuantity > 0 && parsedQuantity <= products[actualInput].Quantity)
                {
                    addedItem.Quantity = parsedQuantity;
                    newQuantity = products[actualInput].Quantity - parsedQuantity;
                    products[actualInput].Quantity = newQuantity;
                    _bl.UpdateProduct(products[actualInput]);
                }
                else
                {
                    Console.WriteLine("Invalid input...Please type a quantity less than or equal to the item quantity.");
                    goto quantity;
                }

                addedItem.Cost = products[actualInput].UnitPrice * parsedQuantity;

                items.Add(addedItem);

                Console.WriteLine("Type x to submit your order or any other key to add other items.");
                if(Console.ReadLine() == "x")
                {
                    exit = true;
                }
                else{
                    goto item;
                }
            }while(!exit);

            order.Total = 0;
            for(int i = 0; i < items.Count; i++){
                order.Total += items[i].Cost;
            }
            order.CustomerPhone = _cust.Phonenumber;
            order.StoreID = _store.Number;
            order.OrderDate = DateTime.Today;
            Order addedOrder = _bl.AddOrder(order);

            for(int i = 0; i < items.Count; i++){
                items[i].OrderId = addedOrder.Id;
            }

            List <LineItem> addedItems = _bl.AddLineItems(items);
            Log.Information("Order added Successfully!");

            Console.WriteLine("Your order below has been added Successfully:");
            for(int i = 0; i < addedItems.Count; i++){
                Console.WriteLine($"Item: {addedItems[i].ProductName} Quantity: {addedItems[i].Quantity} Cost: ${addedItems[i].Cost}");
            }
            Console.WriteLine($"Total cost: {addedOrder.Total}");
        }
    }
}