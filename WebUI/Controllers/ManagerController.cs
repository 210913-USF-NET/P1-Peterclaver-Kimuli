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

        /// <summary>
        /// This function displays the stores that were created by the manager
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Returns the home page for the manager interface</returns>
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
        
        /// <summary>
        /// This method is used to log out the manager...
        /// </summary>
        /// <returns>Returns the login page.</returns>
        // GET: CustomerController/Index
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("manager");
            HttpContext.Session.Remove("phonenumber");
            HttpContext.Session.Remove("storename");

            Log.Information("Manager Logged out...");

            return RedirectToAction("Index", "Customer");
        }

        /// <summary>
        /// This method displays the products for a specific store
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        /// <returns>Returns the products for a particular store</returns>
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

        /// <summary>
        /// Used by the manage to create a new store
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the page for creation of a new store</returns>
        // GET: ManagerController/Create/5
        public ActionResult Create(int id)
        {
            return View();
        }

        /// <summary>
        /// Posts the newly created store to the DB
        /// </summary>
        /// <param name="store"></param>
        /// <returns>Returns the store creation page</returns>
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

        /// <summary>
        /// This method returns the page that displays the orders for a particular store
        /// </summary>
        /// <param name="sort"></param>
        /// <returns>Returns orders that were made to a particular store</returns>
        // GET: StoreController/Details/5
        public ActionResult Orders(string sort)
        {
            if (sort == null)
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
            else
            {
                List<Order> orders = _bl.GetStoreOrdersByCost(HttpContext.Session.GetString("storename"));
                if (orders.Count == 0)
                {
                    ViewBag.Check = true;
                    return View();
                }
                else
                {
                    Log.Information("Order sorted by cost");
                    return View(orders);
                }
            }

        }

        /// <summary>
        /// This method returns the page used by a manager to create a product.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Returns the page used by a manager to create a product.</returns>
        // GET: StoreController/Details/5
        public ActionResult CreateProduct(string message)
        {

            ViewBag.Message = message;

            return View();
        }

        /// <summary>
        /// This method posts the created product to the DB.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Returns the page used by a manager to create a product.</returns>
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

        /// <summary>
        /// This method returns the page used by a manager to edit a product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the page used by a manager to edit a product.</returns>
        // GET: StoreController/Details/5
        public ActionResult EditProduct(int id)
        {
            ProductVM product = new ProductVM(_bl.GetOneProduct(id));

            ViewBag.Name = product.Name;

            return View(product);
        }

        /// <summary>
        /// This method updates the edited product in the DB.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns>Returns the page used by a manager to create a product.</returns>
        // POST: ManagerController/Create
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
