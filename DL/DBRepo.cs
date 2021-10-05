using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DL
{
    /// <summary>
    /// This is the class that contains methods that interact with the DBContext. It implements the IRepo interface.
    /// </summary>
    public class DBRepo : IRepo
    {
        private ShoppingDBContext _context;
        /// <summary>
        /// This is a constructor of the DBRepo class.
        /// </summary>
        /// <param name="context"></param>
        public DBRepo(ShoppingDBContext context)
        {
            _context = context;
        }

        //This method adds a new customer to the DB
        /// <summary>
        /// This is the method that sign ups the customer
        /// </summary>
        /// <param name="cust">Customer Object to be added in the DB</param>
        /// <returns>Returns an object of a customer that has been successfully added to the DB.</returns>
        public Customer AddCustomer(Customer cust){
            Customer returnedCust = _context.Customers.FirstOrDefault(c => c.Phonenumber == cust.Phonenumber);

            if(returnedCust == null)
            {
                Customer custToAdd = new Customer()
                {
                    Phonenumber = cust.Phonenumber,
                    Name = cust.Name,
                    Password = cust.Password,
                    Password2 = cust.Password2
                };

                //Adding the custToAdd obj to change tracker
                custToAdd = _context.Add(custToAdd).Entity;

                //the "changes" are not executed until I call the SaveChanges method
                _context.SaveChanges();

                //the below line clears the changetracker so it returns to a clean slate
                _context.ChangeTracker.Clear();

                return new Customer()
                {
                    Phonenumber = custToAdd.Phonenumber,
                    Name = custToAdd.Name,
                    Password = custToAdd.Password,
                    Password2 = custToAdd.Password2
                };
            }
            else
            {
                return null;
            }
        }

        //Add a product to the DB
        /// <summary>
        /// This is the method that adds a product to the DB
        /// </summary>
        /// <param name="product">Product object to be added in the DB</param>
        /// <returns>Returns an object of a product that has been successfully added to the DB.</returns>
        public Product AddProduct(Product product)
        {
            Product newProduct = new Product()
            {
                Name = product.Name,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice,
                StoreID = product.StoreID
            };

            newProduct = _context.Add(newProduct).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();

            return new Product()
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Quantity = newProduct.Quantity,
                UnitPrice = newProduct.UnitPrice
            };
        }

        /// <summary>
        /// This is the method used when creating a new store.
        /// </summary>
        /// <param name="store">Store object to be added in the DB</param>
        /// <returns>Returns an object of a store that has been successfully added to the DB.</returns>
        public Store AddStore(Store store)
        {
            Store storeToAdd = new Store()
            {
                Number = store.Number,
                Location = store.Location,
                Zipcode = store.Zipcode,
                ManagerPhone = store.ManagerPhone
            };

            storeToAdd = _context.Add(storeToAdd).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();

            return new Store()
            {
                Number = storeToAdd.Number,
                Location = storeToAdd.Location,
                Zipcode = storeToAdd.Zipcode,
                ManagerPhone = storeToAdd.ManagerPhone
            };
        }

        /*/// <summary>
        /// This method is not relevant as of now. Will recheck it later on.
        /// </summary>
        /// <param name="storeNumber">Store ID</param>
        /// <param name="productID">Product ID</param>
        public void AddToStoreProduct(string storeNumber, int productID)
        {
            Storeproduct storeProduct = new Storeproduct()
            {
                Storeid = storeNumber,
                Productid = productID
            };

            storeProduct = _context.Add(storeProduct).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }*/

        /// <summary>
        /// This method is used when a manager is searching for a customer
        /// </summary>
        /// <param name="name">Customer name</param>
        /// <returns>Returns an object of a customer if they exist in the DB.</returns>
        public List<Customer> GetCustomerSearch(string name)
        {
            return _context.Customers.Where(custName => custName.Name.Contains(name)).Select(
                c => new Customer(){
                    Phonenumber = c.Phonenumber,
                    Name = c.Name
                }
            ).ToList();
        }

        /// <summary>
        /// This method is used when a customer is logging in the DB.
        /// </summary>
        /// <param name="phonenumber">Customer phone number</param>
        /// <param name="password">Customer password</param>
        /// <returns>Returns an object of a customer if they exist in the DB.</returns>
        public Customer GetLoggedInCustomer(string phonenumber, string password){
          Customer returnedCust = _context.Customers.FirstOrDefault(c => c.Phonenumber == phonenumber && c.Password == password);

            if (returnedCust != null)
            {
                return new Customer()
                {
                    Id = returnedCust.Id,
                    Phonenumber = returnedCust.Phonenumber,
                    Name = returnedCust.Name,
                    Password = returnedCust.Password,
                    Password2 = returnedCust.Password2
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a customer whose ID matches the id parameter
        /// </summary>
        /// <param name="id">ID that is to be searched for in the DB.</param>
        /// <returns></returns>
        public Customer GetOneCustomer(int id)
        {
            Customer returnedCust = _context.Customers.FirstOrDefault(c => c.Id == id);

            if (returnedCust != null)
            {
                return new Customer()
                {
                    Phonenumber = returnedCust.Phonenumber,
                    Name = returnedCust.Name,
                    Password = returnedCust.Password,
                    Password2 = returnedCust.Password2
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// This method is used when a manager is logging in
        /// </summary>
        /// <param name="phonenumber">Manager phone number</param>
        /// <param name="password">Manager password</param>
        /// <returns>Returns an object of a manager if they exist in the DB</returns>
        public Manager GetManager(string phonenumber, string password)
        {
            Manager manager = _context.Managers.FirstOrDefault(c => c.Phonenumber == phonenumber && c.Password == password);

            if (manager != null)
            {
                return new Manager()
                {
                    Id = manager.Id,
                    Phonenumber = manager.Phonenumber,
                    Name = manager.Name,
                    Password = manager.Password,
                    Password2 = manager.Password2
                };
            }
            else
            {
                return null;
            }
        }

        public Manager GetOneManager(int id)
        {
            Manager manager = _context.Managers.FirstOrDefault(c => c.Id == id);

            if (manager != null)
            {
                return new Manager()
                {
                    Phonenumber = manager.Phonenumber,
                    Name = manager.Name,
                    Password = manager.Password,
                    Password2 = manager.Password2
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// This method returns the products stored in the DB
        /// </summary>
        /// <param name="storeNumber">The store ID number</param>
        /// <returns>Returns a list of products if they exist in the DB</returns>
        public List<Product> GetProducts(string storeNumber)
        {
            /* List <Entity.Storeproduct> products = _context.Storeproducts.
            Include(p => p.Product).Where(p => p.Storeid == storeNumber).ToList(); */
            
            return _context.Products.Where(p => p.StoreID == storeNumber).Select(
                sp => new Product(){
                    Id = sp.Id,
                    Name = sp.Name,
                    Quantity = sp.Quantity,
                    UnitPrice = sp.UnitPrice,
                    StoreID = sp.StoreID
                }
            ).ToList();
            
        }
        /// <summary>
        /// This method returns only the stores that were created by a particular manager
        /// </summary>
        /// <param name="managerNumber">The manager's phone number</param>
        /// <returns>Returns a list of stores if they exist in the DB</returns>
        public List<Store> GetManagerStores(string managerNumber)
        {
            return _context.Stores.Where(managerPhone => managerPhone.ManagerPhone.Contains(managerNumber))
            .Select(
                s => new Store(){
                    Number = s.Number,
                    Location = s.Location,
                    Zipcode = s.Zipcode
                }
            ).ToList();
        }
        /// <summary>
        /// This methods returns from the DB customers that have ordered from a particular
        /// </summary>
        /// <returns>Returns a list of customer that ordered from a store if they exist in the DB</returns>
        public List<Store> GetCustomerStores()
        {
            return _context.Stores.Select(
                s => new Store(){
                    Number = s.Number,
                    Location = s.Location,
                    Zipcode = s.Zipcode
                }
            ).ToList();
        }
        /// <summary>
        /// This method adds a customer order to the DB.
        /// </summary>
        /// <param name="order">Order object to be added to the DB</param>
        /// <returns>The order object added to the DB</returns>
        public Order AddOrder(Order order)
        {
            Order newOrder = new Order()
            {
                Total = order.Total,
                CustomerPhone = order.CustomerPhone,
                StoreID = order.StoreID,
                OrderDate = order.OrderDate
            };

            newOrder = _context.Add(newOrder).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();

            return new Order(){
                Id = newOrder.Id,
                Total = newOrder.Total,
                CustomerPhone = newOrder.CustomerPhone,
                StoreID = newOrder.StoreID,
                OrderDate = newOrder.OrderDate
            };
        }

        /// <summary>
        /// This method adds the order items to the DB
        /// </summary>
        /// <param name="items">Items list to add to the DB</param>
        /// <returns>Returns a list of items successfully added to the DB</returns>
        public List<LineItem> AddLineItems(List<LineItem> items)
        {
            List<LineItem> addedItems = new List<LineItem>();

            for(int i = 0; i < items.Count; i++)
            {
                LineItem newItem = new LineItem()
                {
                    Id = items[i].Id,
                    OrderId = items[i].OrderId,
                    ProductId = items[i].ProductId,
                    ProductName = items[i].ProductName,
                    Quantity = items[i].Quantity,
                    Cost = items[i].Cost
                };

                newItem = _context.Add(newItem).Entity;
                _context.SaveChanges();
                _context.ChangeTracker.Clear();

                addedItems.Add(new LineItem(){
                    Id = newItem.Id,
                    OrderId = newItem.OrderId,
                    ProductId = newItem.ProductId,
                    ProductName = newItem.ProductName,
                    Quantity = newItem.Quantity,
                    Cost = newItem.Cost
                });
            }

            return addedItems;
        }

        /// <summary>
        /// This method returns the orders that were made by a particular customer sorted by Date.
        /// </summary>
        /// <param name="customerNumber">Customer phone number</param>
        /// <returns>A list of orders that were made by a customer sorted by Date.</returns>
        public List<Order> GetCustomerOrders(string customerNumber)
        {
            return _context.Orders.OrderBy(o =>o.OrderDate).Include(cn => cn.Items).
            Where(cn => cn.CustomerPhone == customerNumber).Select(
                o => new Order()
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Total = o.Total,
                    StoreID = o.StoreID,
                    Items = o.Items.Select(i => new LineItem()
                    {
                        ProductName = i.ProductName,
                        Quantity = i.Quantity,
                        Cost = i.Cost,
                    }).ToList()
                }
            ).ToList();
        }

        /// <summary>
        /// This method returns the orders that were made by a particular customer sorted by Total cost.
        /// </summary>
        /// <param name="customerNumber">Customer phone number</param>
        /// <returns>A list of orders that were made by a customer sorted by Total cost.</returns>
        public List<Order> GetCustomerOrdersByCost(string customerNumber)
        {
            return _context.Orders.OrderBy(o =>o.Total).Include(cn => cn.Items).
            Where(cn => cn.CustomerPhone == customerNumber).Select(
                o => new Order(){
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Total = o.Total,
                    StoreID = o.StoreID,
                    Items = o.Items.Select(i => new LineItem(){
                        ProductName = i.ProductName,
                        Quantity = i.Quantity,
                        Cost = i.Cost,
                    }).ToList()
                }
            ).ToList();
        }

        /// <summary>
        /// This method returns the orders that were made to a particular store sorted by Date.
        /// </summary>
        /// <param name="storeNumber">Store ID</param>
        /// <returns>A list of orders that were made to a particular store sorted by Date.</returns>
        public List<Order> GetStoreOrders(string storeNumber)
        {
            return _context.Orders.OrderBy(o =>o.OrderDate).Include(cn => cn.Items).
            Where(cn => cn.StoreID == storeNumber).Select(
                o => new Order(){
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Total = o.Total,
                    StoreID = o.StoreID,
                    CustomerName = o.CustomerName,
                    Items = o.Items.Select(i => new LineItem(){
                        ProductName = i.ProductName,
                        Quantity = i.Quantity,
                        Cost = i.Cost,
                    }).ToList()
                }
            ).ToList();
        }
        /// <summary>
        /// This method returns the orders that were made to a particular store sorted by Total cost.
        /// </summary>
        /// <param name="storeNumber">Store ID</param>
        /// <returns>A list of orders that were made to a particular store sorted by Total cost.</returns>
        public List<Order> GetStoreOrdersByCost(string storeNumber)
        {
            return _context.Orders.OrderBy(o => o.Total).Include(cn => cn.Items).
            Where(cn => cn.StoreID == storeNumber).Select(
                o => new Order()
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Total = o.Total,
                    StoreID = o.StoreID,
                    CustomerName = o.CustomerName,
                    Items = o.Items.Select(i => new LineItem()
                    {
                        ProductName = i.ProductName,
                        Quantity = i.Quantity,
                        Cost = i.Cost,
                    }).ToList()
                }
            ).ToList();
        }

        /// <summary>
        /// This method is used when a manager is changing the quantity number of a product.
        /// </summary>
        /// <param name="product">The product object to be changed in the DB</param>
        public void UpdateProduct(Product product)
        {
            Product updateProduct = new Product()
            {
                Id = product.Id,
                Name = product.Name,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice,
                StoreID = product.StoreID
            };

            updateProduct = _context.Products.Update(updateProduct).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }
    }
}