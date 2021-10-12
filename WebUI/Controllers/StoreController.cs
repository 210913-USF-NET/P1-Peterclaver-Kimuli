using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using BL;
using Serilog;
using WebUI.Models;

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
            HttpContext.Session.Remove("productadded");

            Log.Information("Logged out...");

            return RedirectToAction("Index", "Customer");
        }

        // GET: StoreController/Details/5
        public ActionResult Details(string name)
        {
            if(name == null)
            {
                name = HttpContext.Session.GetString("storename");
            }
            else
            {

                HttpContext.Session.SetString("storename", name);
            }

            ViewBag.Name = name;

            List<Product> returnedProd = _bl.GetProducts(name);
            List<Product> p = new List<Product>();

            foreach(Product item in returnedProd)
            {
                if(item.Quantity != 0)
                {
                    p.Add(item);
                }
            }

            return View(p);
        }

        // POST: StoreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(Product product)
        {
            List<Product> returnedProd = _bl.GetProducts(HttpContext.Session.GetString("storename"));

            try
            {
                if (ModelState.IsValid)
                {
                    List<LineItem> items = new List<LineItem>();

                        LineItem item = new LineItem();
                        item.ProductId = product.Id;
                        item.ProductName = product.Name;
                        item.Quantity = product.Quantity;
                        item.Cost = product.Quantity * product.UnitPrice;

                        items.Add(item);

                        List<LineItem> items1 = HttpContext.Session.GetComplexData<List<LineItem>>("productadded");

                        if (items1 == null)
                        {
                            HttpContext.Session.SetComplexData("productadded", items);
                        }
                        else
                        {
                            items1.Add(item);
                            HttpContext.Session.SetComplexData("productadded", items1);
                        }

                        return RedirectToAction(nameof(Details), new { name = HttpContext.Session.GetString("storename") });
                   
                    
                }
                return RedirectToAction(nameof(Details), new { name = HttpContext.Session.GetString("storename") });
            }
            catch
            {
                return View(returnedProd);
            }
        }

        // GET: StoreController/Cart
        public ActionResult Cart()
        {
            List<LineItem> cart = HttpContext.Session.GetComplexData<List<LineItem>>("productadded");
            if (cart != null)
            {
                ViewBag.Check = true;

                decimal total = 0.0M;
                for (int i = 0; i < cart.Count; i++)
                {
                    total += cart[i].Cost;
                }

                ViewBag.total = total;
            }

            return View(cart);
        }

        // POST: StoreController/Cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cart(List<LineItem> items)
        {
            try
            {
                items = HttpContext.Session.GetComplexData<List<LineItem>>("productadded");

                Order order = new Order();
                order.Total = 0;
                for (int i = 0; i < items.Count; i++)
                {
                    order.Total += items[i].Cost;
                }
                order.CustomerPhone = HttpContext.Session.GetString("phonenumber"); 
                order.StoreID = HttpContext.Session.GetString("storename"); 
                order.CustomerName = HttpContext.Session.GetString("name");
                order.OrderDate = DateTime.Today;
                Order addedOrder = _bl.AddOrder(order);

                Log.Information("Order successfully created");

                for (int i = 0; i < items.Count; i++)
                {
                    items[i].OrderId = addedOrder.Id;
                }

                List<LineItem> addedItems = _bl.AddLineItems(items);

                foreach(LineItem l in items)
                {
                    Product p = _bl.GetOneProduct(l.ProductId);
                    int newQuantity = p.Quantity - l.Quantity;
                    p.Quantity = newQuantity;

                    _bl.UpdateProduct(p);
                }
                HttpContext.Session.Remove("productadded");

                return RedirectToAction(nameof(Cart));
            }
            catch
            {
                Log.Error("Order Failed to create");
                return View();
            }
        }

        // GET: StoreController/Details/5
        public ActionResult Orders(string sort)
        {
            if (sort == null)
            {
                List<Order> orders = _bl.GetCustomerOrders(HttpContext.Session.GetString("phonenumber"));
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
                List<Order> orders = _bl.GetCustomerOrdersByCost(HttpContext.Session.GetString("phonenumber"));
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

        // GET: StoreController/Edit/5
        public ActionResult EditItem(int id)
        {
            LineItem item = new LineItem();

            List<LineItem> items = HttpContext.Session.GetComplexData<List<LineItem>>("productadded");

            foreach(LineItem item1 in items)
            {
                if(item1.ProductId == id)
                {
                    item = item1;
                }
            }

            return View(item);
        }

        // POST: StoreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditItem(int id, LineItem item)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    List<LineItem> items = HttpContext.Session.GetComplexData<List<LineItem>>("productadded");
                    HttpContext.Session.Remove("productadded");

                    LineItem t = new LineItem();

                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i].ProductId == item.ProductId)
                        {
                            t = items[i];
                            items.Remove(items[i]);
                            decimal unitPrice = t.Cost / t.Quantity;
                            t.Quantity = item.Quantity;
                            t.Cost = t.Quantity * unitPrice;
                            if(t.Quantity > 0)
                            {
                                items.Insert(i, t);
                            }
                        }
                    }

                    HttpContext.Session.SetComplexData("productadded", items);

                    Log.Information("Product successfully updated");

                    return RedirectToAction(nameof(Cart));
                
                }

                return View(item);
            }
            catch
            {
                return View(item);
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
