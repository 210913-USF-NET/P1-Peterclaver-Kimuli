/* using System;
using System.Linq;
using System.Collections.Generic;
using Models;

namespace DL
{
    public sealed class RAMRepo : IRepo
    {
        private static RAMRepo _instance;

        private RAMRepo()
        {
            _customers = new List<Customer>(){
                new Customer(){
                    Name = "Peter",
                    Phonenumber = "7979696",
                    Password = "password"
                }
            };
        }

        public static RAMRepo GetInstance(){
            if(_instance == null){
                _instance = new RAMRepo();
            }

            return _instance;
        }

        private static List<Customer> _customers;

        public Customer AddCustomer(Customer cust){
            _customers.Add(cust);
            return cust;
        }

        public List<Customer> GetCustomers(){
            return  _customers;
        }

        public List<Manager> GetManagers()
        {
            throw new NotImplementedException();
        }
    }
} */