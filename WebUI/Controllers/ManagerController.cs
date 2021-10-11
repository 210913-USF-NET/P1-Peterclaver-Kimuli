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

                HttpContext.Session.Remove("storename");

                List<Store> stores = _bl.GetManagerStores(HttpContext.Session.GetString("phonenumber"));
                return View(stores);
            }
            
        }
        
        // GET: CustomerController/Index
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("manager");
            HttpContext.Session.Remove("phonenumber");
            HttpContext.Session.Remove("storename");

            Log.Information("Manager Logged out...");

            return RedirectToAction("Index", "Customer");
        }

        // GET: ManagerController/Details/5
        public ActionResult Details(string name, string message)
        {

            if(name != null)
            {
                if (HttpContext.Session.GetString("storename") != null)
                {
                    HttpContext.Session.Remove("storename");
                }

                HttpContext.Session.SetString("storename", name);
            }

            ViewBag.Name = HttpContext.Session.GetString("storename");

            ViewBag.Message = message;

            return View(_bl.GetProducts(ViewBag.Name));
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

                    Log.Information("Store created Successfully");

                    return RedirectToAction("Index", "Manager", new { message = "Store successfully created" });
                }
                return RedirectToAction(nameof(Create));
            }
            catch
            {
                return View();
            }
        }

        // GET: StoreController/Details/5
        public ActionResult Orders()
        {
            List<Order> orders = _bl.GetStoreOrders(HttpContext.Session.GetString("storename"));
            if (orders.Count == 0)
            {
                ViewBag.Check = true;
                return View();
            }
            else
            {
                Log.Information("Order History displayed");
                return View(orders);
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

        public ActionResult CreateProduct(string message)
        {

            ViewBag.Message = message;

            return View();
        }

        // POST: ManagerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(ProductVM product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    product.StoreID = HttpContext.Session.GetString("storename");
                    Product addedProduct = _bl.AddProduct(product.ToModel());

                    Log.Information("Product created Successfully");

                    return RedirectToAction(nameof(CreateProduct), new {message = "Product successfully created" });
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EditProduct(int id)
        {
            ProductVM product = new ProductVM(_bl.GetOneProduct(id));

            ViewBag.Name = product.Name;

            return View(product);
        }

        // POST: ManagerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(int id, ProductVM product) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    product.StoreID = HttpContext.Session.GetString("storename");
                    _bl.UpdateProduct(product.ToModel());

                    Log.Information("Product successfully updated");

                    return RedirectToAction(nameof(Details), new { message = "Product successfully updated" });
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
