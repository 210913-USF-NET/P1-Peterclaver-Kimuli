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
    public class ManagerController : Controller
    {
        private IBL _bl;

        public ManagerController(IBL bl)
        {
            _bl = bl;
        }

        // GET: ManagerController
        public ActionResult Index(string message)
        {
            if(HttpContext.Session.GetString("manager") == null)
            {
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                ViewBag.Name = HttpContext.Session.GetString("manager");

                ViewBag.Message = message;

                List<Store> stores = _bl.GetManagerStores(HttpContext.Session.GetString("phonenumber"));
                return View(stores);
            }
            
        }
        
        // GET: CustomerController/Index
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("manager");
            HttpContext.Session.Remove("phonenumber");

            Log.Information("Manager Logged out...");

            return RedirectToAction("Index", "Customer");
        }

        // GET: ManagerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ManagerController/Create/5
        public ActionResult Create(int id)
        {
            return View();
        }

        // POST: ManagerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult Create(StoreVM store)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    store.ManagerPhone = HttpContext.Session.GetString("phonenumber");
                    Store addedStore = _bl.AddStore(store.ToModel());

                    return RedirectToAction("Index", "Manager", new { message = "Store successfully created" });
                    Log.Information("Store created Successfully");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ManagerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManagerController/Edit/5
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

        // GET: ManagerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManagerController/Delete/5
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
