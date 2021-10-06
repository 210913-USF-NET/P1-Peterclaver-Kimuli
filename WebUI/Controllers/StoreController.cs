using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using BL;
using Serilog;

namespace WebUI.Controllers
{
    public class StoreController : Controller
    {
        private IBL _bl;

        public StoreController(IBL bl)
        {
            _bl = bl;
        }

        // GET: StoreController
        public ActionResult Index(string message)
        {
            if (HttpContext.Session.GetString("name") == null)
            {
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                ViewBag.Name = HttpContext.Session.GetString("name");
                ViewBag.Success = message;

                List<Store> stores = _bl.GetCustomerStores();
                return View(stores);
            }
        }

        // GET: CustomerController/Index
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("name");
            HttpContext.Session.Remove("phonenumber");

            Log.Information("Logged out...");

            return RedirectToAction("Index", "Customer");
        }

        // GET: StoreController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StoreController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StoreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: StoreController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StoreController/Edit/5
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

        // GET: StoreController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StoreController/Delete/5
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
