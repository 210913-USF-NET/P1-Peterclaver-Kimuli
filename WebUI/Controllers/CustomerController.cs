using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using BL;

namespace WebUI.Controllers
{
    public class CustomerController : Controller
    {
        private IBL _bl;

        public CustomerController(IBL bl) => _bl = bl;
        public ActionResult Index()
        {
            return View();
        }

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
                        return View();
                    }
                    else
                    {
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

        // GET: CustomerController/Signup
        public ActionResult Signup()
        {
            return View();
        }

        // POST: CustomerController/Signup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Customer cust = _bl.AddCustomer(customer);
                    if (cust == null)
                    {
                        return View();
                    }
                    else
                    {
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

        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
