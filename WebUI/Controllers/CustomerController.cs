using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using BL;
using WebUI.Models;
using Serilog;

namespace WebUI.Controllers
{
    /// <summary>
    /// This is the class for the customer controller
    /// </summary>
    public class CustomerController : Controller
    {
        private IBL _bl;

        
        public CustomerController(IBL bl) => _bl = bl;

        /// <summary>
        /// Used when a customer is logging into their account
        /// </summary>
        /// <returns>Returns the Login page</returns>
        public ActionResult Index()
        {
            return View();
        }

        
        /// <summary>
        /// Used by a customer or manager during Login
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Redirects the customer to the store controller</returns>
        // POST: CustomerController/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Customer cust = _bl.GetLoggedInCustomer(customer.Phonenumber, customer.Password);
                    if (cust == null)
                    {
                        Manager manager = _bl.GetManager(customer.Phonenumber, customer.Password);
                        if (manager == null)
                        {
                            Log.Warning("Failed to Login");
                            ViewBag.Message = "Incorrect phonenumber or password. Please try again!";
                            return View(); 
                        }
                        else
                        {
                            Log.Information("Manager successfully Logged in.");

                            HttpContext.Session.SetString("manager", manager.Name);
                            HttpContext.Session.SetString("phonenumber", manager.Phonenumber);

                            return RedirectToAction("Index", "Manager");
                        }
                    }
                    else
                    {
                        Log.Information("Successfully Logged in.");
                        
                        HttpContext.Session.SetString("name", cust.Name);
                        HttpContext.Session.SetString("phonenumber", cust.Phonenumber);

                        return RedirectToAction("Index", "Store");
                    }

                }

                return View();

            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Used when a customer is creating a new account
        /// </summary>
        /// <returns>Returns the signup page</returns>
        // GET: CustomerController/Signup
        public ActionResult Signup()
        {
            return View();
        }

        /// <summary>
        /// Used when a customer is creating a new account
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Redirects the new customer to the store controller</returns>
        // POST: CustomerController/Signup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(CustomerVM customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Customer cust = _bl.AddCustomer(customer.ToModel());
                    if (cust == null)
                    {
                        Log.Warning("Failed to Signup");
                        ViewBag.Message = "Phone number already in use. Please try another!";
                        return View();
                    }
                    else
                    {

                        Log.Information("Account Successfully created.");

                        HttpContext.Session.SetString("name", cust.Name);
                        HttpContext.Session.SetString("phonenumber", cust.Phonenumber);

                        ViewBag.Success = "Account Created Successfully";

                        return RedirectToAction("Index", "Store", new { message = "Account successfully created!" });
                    }
                    
                }

                return View();

            }
            catch
            {
                return View();
            }
        }
    }
}
