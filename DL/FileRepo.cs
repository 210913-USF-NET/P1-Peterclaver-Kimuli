/* using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Models;
using System.Collections.Generic;


namespace DL
{
    public class FileRepo : IRepo
    {
        private const string filePath = "../DL/Customers.json";

        private const string filePath1 = "../DL/Managers.json";

        private string jsonString;

        public Customer AddCustomer(Customer cust){
            List<Customer> allCust = GetCustomers();

            allCust.Add(cust);

            jsonString = JsonSerializer.Serialize(allCust);

            File.WriteAllText(filePath, jsonString);

            return cust;
        }

        public List<Customer> GetCustomers(){
            jsonString = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<List<Customer>>(jsonString);
        }

        public List<Manager> GetManagers(){
            jsonString = File.ReadAllText(filePath1);

            return JsonSerializer.Deserialize<List<Manager>>(jsonString);
        }

    }
} */