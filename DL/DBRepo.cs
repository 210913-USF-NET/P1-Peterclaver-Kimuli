using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model = Models;
using Entity = DL.Entities;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DL
{
    /// <summary>
    /// This is the class that contains methods that interact with the DBContext. It implements the IRepo interface.
    /// </summary>
    public class DBRepo : IRepo
    {
        private Entity.ShoppingAppDBContext _context;
        /// <summary>
        /// This is a constructor of the DBRepo class.
        /// </summary>
        /// <param name="context"></param>
        public DBRepo(Entity.ShoppingAppDBContext context)
        {
            _context = context;
        }

        //This method adds a new customer to the DB
        /// <summary>
        /// This is the method that sign ups the customer
        /// </summary>
        /// <param name="cust">Customer Object to be added in the DB</param>
        /// <returns>Returns an object of a customer that has been successfully added to the DB.</returns>
        public Model.Customer AddCustomer(Model.Customer cust){
            Entity.Customer custToAdd = new Entity.Customer(){
                Phonenumber = cust.Phonenumber,
                Name = cust.Name,
                Password = cust.Password,
                Password1 = cust.Password2
            };

            //Adding the custToAdd obj to change tracker
            custToAdd = _context.Add(custToAdd).Entity;

            //the "changes" are not executed until I call the SaveChanges method
            _context.SaveChanges();

            //the below line clears the changetracker so it returns to a clean slate
            _context.ChangeTracker.Clear();

            return new Model.Customer()
            {
                Phonenumber = custToAdd.Phonenumber,
                Name = custToAdd.Name,
                Password = custToAdd.Password,
                Password2 = custToAdd.Password1
            };
        }

        //Add a product to the DB
        /// <summary>
        /// This is the method that adds a product to the DB
        /// </summary>
        /// <param name="product">Product object to be added in the DB</param>
        /// <returns>Returns an object of a product that has been successfully added to the DB.</returns>
        public Product AddProduct(Product product)
        {
            Entity.Product newProduct = new Entity.Product()
            {
                Name = product.Name,
                Stock = product.Quantity,
                Unitprice = product.UnitPrice,
                Storeid = product.StoreID
            };

            newProduct = _context.Add(newProduct).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();

            return new Model.Product()
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Quantity = newProduct.Stock,
                UnitPrice = newProduct.Unitprice
            };
        }

        /// <summary>
        /// This is the method used when creating a new store.
        /// </summary>
        /// <param name="store">Store object to be added in the DB</param>
        /// <returns>Returns an object of a store that has been successfully added to the DB.</returns>
        public Store AddStore(Store store)
        {
            Entity.Store storeToAdd = new Entity.Store()
            {
                Number = store.Number,
                Location = store.Location,
                Zipcode = store.Zipcode,
                Managerphone = store.ManagerPhone
            };

            storeToAdd = _context.Add(storeToAdd).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();

            return new Model.Store()
            {
                Number = storeToAdd.Number,
                Location = storeToAdd.Location,
                Zipcode = storeToAdd.Zipcode,
                ManagerPhone = storeToAdd.Managerphone
            };
        }

        /// <summary>
        /// This method is not relevant as of now. Will recheck it later on.
        /// </summary>
        /// <param name="storeNumber">Store ID</param>
        /// <param name="productID">Product ID</param>
        public void AddToStoreProduct(string storeNumber, int productID)
        {
            Entity.Storeproduct storeProduct = new Entity.Storeproduct()
            {
                Storeid = storeNumber,
                Productid = productID
            };

            storeProduct = _context.Add(storeProduct).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        /// <summary>
        /// This method is used when a manager is searching for a customer
        /// </summary>
        /// <param name="name">Customer name</param>
        /// <returns>Returns an object of a customer if they exist in the DB.</returns>
        public List<Customer> GetCustomerSearch(string name)
        {
            return _context.Customers.Where(custName => custName.Name.Contains(name)).Select(
                c => new Model.Customer(){
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
        public List<Model.Customer> GetLoggedInCustomer(string phonenumber, string password){
            List<Model.Customer> loggedInCust = new List<Model.Customer>();

            loggedInCust = _context.Customers.Where(
                cust => cust.Phonenumber == phonenumber && cust.Password == password
            ).Select(
                c => new Model.Customer(){
                    Phonenumber = c.Phonenumber,
                    Name = c.Name,
                    Password = c.Password
                }
            ).ToList();

            return loggedInCust;
        }

        /// <summary>
        /// This method is used when a manager is logging in
        /// </summary>
        /// <param name="phonenumber">Manager phone number</param>
        /// <param name="password">Manager password</param>
        /// <returns>Returns an object of a manager if they exist in the DB</returns>
        public List<Model.Manager> GetManagers(string phonenumber, string password)
        {
            return _context.Managers.Where(manager => manager.Phonenumber == phonenumber
            && manager.Password == password).Select(
                m => new Model.Manager(){
                    Phonenumber = m.Phonenumber,
                    Name = m.Name,
                    Password = m.Password
                }
            ).ToList();
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
            
            return _context.Products.Where(p => p.Storeid == storeNumber).Select(
                sp => new Model.Product(){
                    Id = sp.Id,
                    Name = sp.Name,
                    Quantity = sp.Stock,
                    UnitPrice = sp.Unitprice,
                    StoreID = sp.Storeid
                }
            ).ToList();
            
        }
        /// <summary>
        /// This method returns only the stores that were created by a particular manager
        /// </summary>
        /// <param name="managerNumber">The manager's phone number</param>
        /// <returns>Returns a list of stores if they exist in the DB</returns>
        public List<Model.Store> GetManagerStores(string managerNumber)
        {
            return _context.Stores.Where(managerPhone => managerPhone.Managerphone.Contains(managerNumber))
            .Select(
                s => new Model.Store(){
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
        public List<Model.Store> GetCustomerStores()
        {
            return _context.Stores.Select(
                s => new Model.Store(){
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
            Entity.Customerorder newOrder = new Entity.Customerorder()
            {
                Total = order.Total,
                Customerphone = order.CustomerPhone,
                Storeid = order.StoreID,
                Orderdate = order.OrderDate
            };

            newOrder = _context.Add(newOrder).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();

            return new Model.Order(){
                Id = newOrder.Id,
                Total = newOrder.Total,
                CustomerPhone = newOrder.Customerphone,
                StoreID = newOrder.Storeid,
                OrderDate = newOrder.Orderdate
            };
        }

        /// <summary>
        /// This method adds the order items to the DB
        /// </summary>
        /// <param name="items">Items list to add to the DB</param>
        /// <returns>Returns a list of items successfully added to the DB</returns>
        public List<LineItem> AddLineItems(List<LineItem> items)
        {
            List<Model.LineItem> addedItems = new List<LineItem>();

            for(int i = 0; i < items.Count; i++)
            {
                Entity.Lineitem newItem = new Entity.Lineitem()
                {
                    Id = items[i].Id,
                    Orderid = items[i].OrderId,
                    Productid = items[i].ProductId,
                    Productname = items[i].ProductName,
                    Quantity = items[i].Quantity,
                    Cost = items[i].Cost
                };

                newItem = _context.Add(newItem).Entity;
                _context.SaveChanges();
                _context.ChangeTracker.Clear();

                addedItems.Add(new Model.LineItem(){
                    Id = newItem.Id,
                    OrderId = newItem.Orderid,
                    ProductId = newItem.Productid,
                    ProductName = newItem.Productname,
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
            return _context.Customerorders.OrderBy(o =>o.Orderdate).Include(cn => cn.Lineitems).
            Where(cn => cn.Customerphone == customerNumber).Select(
                o => new Model.Order(){
                    Id = o.Id,
                    OrderDate = o.Orderdate,
                    Total = o.Total,
                    StoreID = o.Storeid,
                    Items = o.Lineitems.Select(i => new Model.LineItem(){
                        ProductName = i.Productname,
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
            return _context.Customerorders.OrderBy(o =>o.Total).Include(cn => cn.Lineitems).
            Where(cn => cn.Customerphone == customerNumber).Select(
                o => new Model.Order(){
                    Id = o.Id,
                    OrderDate = o.Orderdate,
                    Total = o.Total,
                    StoreID = o.Storeid,
                    Items = o.Lineitems.Select(i => new Model.LineItem(){
                        ProductName = i.Productname,
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
            return _context.Customerorders.OrderBy(o =>o.Orderdate).Include(cn => cn.Lineitems).
            Where(cn => cn.Storeid == storeNumber).Select(
                o => new Model.Order(){
                    Id = o.Id,
                    OrderDate = o.Orderdate,
                    Total = o.Total,
                    StoreID = o.Storeid,
                    CustomerName = o.CustomerphoneNavigation.Name,
                    Items = o.Lineitems.Select(i => new Model.LineItem(){
                        ProductName = i.Productname,
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
            return _context.Customerorders.OrderBy(o =>o.Total).Include(cn => cn.Lineitems).
            Where(cn => cn.Storeid == storeNumber).Select(
                o => new Model.Order(){
                    Id = o.Id,
                    OrderDate = o.Orderdate,
                    Total = o.Total,
                    StoreID = o.Storeid,
                    CustomerName = o.CustomerphoneNavigation.Name,
                    Items = o.Lineitems.Select(i => new Model.LineItem(){
                        ProductName = i.Productname,
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
            Entity.Product updateProduct = new Entity.Product()
            {
                Id = product.Id,
                Name = product.Name,
                Stock = product.Quantity,
                Unitprice = product.UnitPrice,
                Storeid = product.StoreID
            };

            updateProduct = _context.Products.Update(updateProduct).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }
    }
}