using System;
using Models;
using DL;
using System.Collections.Generic;

namespace BL
{
    public class CBL : IBL
    {
        private IRepo _repo;

        public CBL(IRepo repo){
            _repo = repo;
        }

        public Customer GetLoggedInCustomer(string phonenumber, string password){
            return _repo.GetLoggedInCustomer(phonenumber, password);
        }

        public Customer GetOneCustomer(int id)
        {
            return _repo.GetOneCustomer(id);
        }

        public Customer AddCustomer(Customer cust){
            return _repo.AddCustomer(cust);
        }

        public Manager GetManager(string phonenumber, string password)
        {
            return _repo.GetManager(phonenumber, password);
        }

        public Manager GetOneManager(int id)
        {
            return _repo.GetOneManager(id);
        }

        public Store AddStore(Store store)
        {
            return _repo.AddStore(store);
        }

        public List<Store> GetManagerStores(string managerNumber)
        {
            return _repo.GetManagerStores(managerNumber);
        }

        public Product AddProduct(Product product)
        {
            return _repo.AddProduct(product);
        }

        /*public void AddToStoreProduct(string storeNumber, int productID)
        {
            _repo.AddToStoreProduct(storeNumber, productID);
        }*/

        public List<Product> GetProducts(string storeNumber)
        {
            return _repo.GetProducts(storeNumber);
        }

        public List<Customer> GetCustomerSearch(string name)
        {
            return _repo.GetCustomerSearch(name);
        }

        public List<Store> GetCustomerStores()
        {
            return _repo.GetCustomerStores();
        }

        public Order AddOrder(Order order)
        {
            return _repo.AddOrder(order);
        }

        public List<LineItem> AddLineItems(List<LineItem> items)
        {
            return _repo.AddLineItems(items);
        }

        public List<Order> GetCustomerOrders(string customerNumber)
        {
            return _repo.GetCustomerOrders(customerNumber);
        }
        public List<Order> GetCustomerOrdersByCost(string customerNumber)
        {
            return _repo.GetCustomerOrdersByCost(customerNumber);
        }

        public void UpdateProduct(Product product)
        {
            _repo.UpdateProduct(product);
        }

        public List<Order> GetStoreOrders(string storeNumber)
        {
            return _repo.GetStoreOrders(storeNumber);
        }
        public List<Order> GetStoreOrdersByCost(string storeNumber)
        {
            return _repo.GetStoreOrdersByCost(storeNumber);
        }

    }
}