using System;
using System.Text.RegularExpressions;
using Serilog;
using System.Collections.Generic;

namespace Models
{
    public class Product
    {
        
        public Product(){}
        public Product(string name, int quantity, decimal unitPrice){
            this.Name = name;
            this.Quantity = quantity;
            this.UnitPrice = unitPrice;
        }

        public int Id { get; set; }

        public string StoreID{get; set;}

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
            if(value.Length == 0){
                InputInvalidException e = new InputInvalidException("Product name cannot be empty");
                Log.Warning(e.Message);
                throw e;
            }
            else{
                _name = value;
            }

        } }
        
        //creating quantity validation
        private int _quantity;
        public int Quantity 
        { get
        {
            return _quantity;
        } 
        set
        {
            if (value < 0)
            {
                InputInvalidException e = new InputInvalidException("Invalid value. Please try again");
                Log.Warning(e.Message);
                throw e;
            }
            else{
                _quantity = value;
            }

        } }
        
        //creating unitprice validation
        private decimal _unitPrice;
        public decimal UnitPrice
        { get
        {
            return _unitPrice;
        } 
        set
        {
            if (value < 0)
            {
                InputInvalidException e = new InputInvalidException("Invalid value");
                Log.Warning(e.Message);
                throw e;
            }
            else{
                _unitPrice = value;
            }

        } }

    }
}