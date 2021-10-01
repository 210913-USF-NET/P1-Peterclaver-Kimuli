// This is the store Model
using System;
using System.Text.RegularExpressions;
using Serilog;

namespace Models
{
    public class Store
    {
        public Store(){}

        public Store(string number, string location, string zipcode){
            this.Number = number;
            this.Location = location;
            this.Zipcode = zipcode;
        }

        public string ManagerPhone{get; set;}

        //Validating the store number
        private string _number;
        public string Number 
        { 
            get
            {
                return _number;
            } 
            set
            {
                if(value.Length == 0){
                    InputInvalidException e = new InputInvalidException("Please fill in a store number.");
                    Log.Warning(e.Message);
                    throw e;
                }
                else{
                    _number = value;
                }
            } 
        }
        
        //Validating location
        private string _location;
        public string Location 
        {
            get
            {
                return _location;
            }
            set
            {
                Regex pattern = new Regex("^[a-zA-Z0-9 ,!&/']+$");
                if(value.Length == 0)
                {
                    InputInvalidException e = new InputInvalidException("Please fill in a store location.");
                    Log.Warning(e.Message);
                    throw e;
                }
                if(!pattern.IsMatch(value))
                {
                    InputInvalidException e = new InputInvalidException("Location should only contain alphanumerical characters, -, !, &, / and ' characters");
                    Log.Warning(e.Message);
                    throw e;
                }
                else{
                    _location = value;
                }

            } 
        }
        
        //Validating the zipcode
        private string _zipcode;
        public string Zipcode 
        {
            get
            {
                return _zipcode;
            }
            set
            {
                Regex zipPattern = new Regex("^[0-9]+$");

                if(value.Length == 0)
                {
                    InputInvalidException e = new InputInvalidException("Zipcode cannot be empty");
                    Log.Warning(e.Message);
                    throw e;
                }
                else if(!zipPattern.IsMatch(value))
                {
                    InputInvalidException e = new InputInvalidException("Zipcode can only be numbers");
                    Log.Warning(e.Message);
                    throw e;
                }
                else if(value.Length != 5)
                {
                    InputInvalidException e = new InputInvalidException("Zipcode should have a length of 5 numbers");
                    Log.Warning(e.Message);
                    throw e;
                }
                else{
                    _zipcode = value;
                }

            } 
        }

        public override string ToString()
        {
            return $"Store number:{this.Number}\nLocation:{this.Location}";
        }
    }
}