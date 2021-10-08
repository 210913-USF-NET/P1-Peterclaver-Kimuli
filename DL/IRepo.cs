using Models;
using System.Collections.Generic;

namespace DL
{
    public interface IRepo
    {
        Customer AddCustomer(Customer cust);
        Customer GetLoggedInCustomer(string phonenumber, string password);
        Customer GetOneCustomer(int id);
        Manager GetManager(string phonenumber, string password);
        Manager GetOneManager(int id);
        Store AddStore(Store store);
        List<Store> GetManagerStores(string managerNumber);
        List<Store> GetCustomerStores();
        Product AddProduct(Product product);
        //void AddToStoreProduct(string storeNumber, int productID);
        List<Product> GetProducts(string storeNumber);
        Product GetOneProduct(int id);
        List<Customer> GetCustomerSearch(string name);
        Order AddOrder(Order order);
        List<LineItem> AddLineItems(List<LineItem> items);
        List <Order> GetCustomerOrders(string customerNumber);
        List <Order> GetCustomerOrdersByCost(string customerNumber);
        List<Order> GetStoreOrders(string storeNumber);
        List<Order> GetStoreOrdersByCost(string storeNumber);
        void UpdateProduct(Product product);
    }
}