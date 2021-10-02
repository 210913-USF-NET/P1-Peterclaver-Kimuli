// This is the customer model
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Serilog;

namespace Models
{
    public class Customer
    {
        public Customer(){}

        public Customer(string name, string phonenumber, string password, string password2){
            this.Name = name;
            this.Phonenumber = phonenumber;
            this.Password = password;
            this.Password2 = password2;
        }

        public int Id { get; set; }
        public List<Order> Orders{get; set;}
        //creating name validation
        private string _name;

        //Setting properties
        public string Name
        { get
        {
        return _name;
        } 
        set
        {
            Regex pattern = new Regex("^[a-zA-Z ]+$");

            if(value.Length == 0){
                InputInvalidException e = new InputInvalidException("Name cannot be empty");
                Log.Warning(e.Message);
                throw e;
            }
            else if(!pattern.IsMatch(value)){
                InputInvalidException e = new InputInvalidException("Name can only have alphabetical characters!");
                Log.Warning(e.Message);
                throw e;
            }
            else{
                _name = value;
            }

        } }
        
        //creating phone validation
        private string _phonenumber;
        public string Phonenumber 
        { get
        {
            return _phonenumber;
        } 
        set
        {
            Regex pattern = new Regex("^[0-9]+$");
            if (value.Length == 0)
            {
                InputInvalidException e = new InputInvalidException("Phonenumber cannot be empty.");
                Log.Warning(e.Message);
                throw e;
            }
            else if(!pattern.IsMatch(value)){
                InputInvalidException e = new InputInvalidException("Phonenumber can only have numbers!");
                Log.Warning(e.Message);
                throw e;
            }
            else if(value.Length != 10){
                throw new InputInvalidException("Phonenumber is not complete!");
            }
            else{
                _phonenumber = value;
            }

        } }
        
        //creating password validation
        private string _password;
        public string Password
        { get
        {
            return _password;
        } 
        set
        {
            if (value.Length == 0)
            {
                InputInvalidException e = new InputInvalidException("Password cannot be empty.");
                Log.Warning(e.Message);
                throw e;
            }
            else if(value.Length < 4){
                InputInvalidException e = new InputInvalidException("Password should have 4 or more characters.");
                Log.Warning(e.Message);
                throw e;
            }
            else{
                _password = value;
            }

        } }

        private string _password2;
        public string Password2
        { get
        {
            return _password2;
        } 
        set
        {
            if (value != _password)
            {
                InputInvalidException e = new InputInvalidException("Passwords are not matching");
                Log.Warning(e.Message);
                throw e;
            }
            else{
                _password2 = value;
            }

        }}

        public override string ToString(){
            return $"Name: {this.Name}, Phone Number:{this.Phonenumber}\n";
        }

        public bool Equals(Customer customer)
        {
            return this.Phonenumber == customer.Phonenumber && this.Password == customer.Password;
        }
    }
}