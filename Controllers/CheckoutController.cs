using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WecareMVC.Models;

namespace WecareMVC.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();
        const string PromoCode = "FD1FBN2FMNJT";   //讓user可以使用序號達到免費、打折等等

        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        // Post: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order {   };
            
            TryUpdateModel(order);

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode, StringComparison.OrdinalIgnoreCase) == true)
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;                    

                    //Save Order
                    storeDB.Orders.Add(order);
                    storeDB.SaveChanges();
                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order, 0.8m);

                    return RedirectToAction("OrderDetails",
                        new { id = order.OrderId});

                    //return RedirectToAction("OrderDetail", order);

                    //return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    //Save Order
                    storeDB.Orders.Add(order);
                    storeDB.SaveChanges();
                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order,1);

                    return RedirectToAction("OrderDetails", new { id = order.OrderId});
                }
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }

        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }

        // GET: /Checkout/OrderDetails
        public ActionResult OrderDetails(int id)
        {
            ViewBag.total = storeDB.Orders.Single(o=>o.OrderId==id).Total;

            bool isValid = storeDB.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                var oderWithDetails = storeDB.Orders.Include("OrderDetails").Single(o => o.OrderId == id).OrderDetails.ToList();

                return View(oderWithDetails);
            }
            else
            {
                return View("Error");
            }
           
        }
    }
}