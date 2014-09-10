using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WecareMVC.Models;
using WecareMVC.ViewModels;

namespace WecareMVC.Controllers
{
   
        // GET: ShoppingCart
        public class ShoppingCartController : Controller
        {
            MusicStoreEntities storeDB = new MusicStoreEntities();
            //
            // GET: /ShoppingCart/
            public ActionResult Index()
            {
                var cart = ShoppingCart.GetCart(this.HttpContext);

                // Set up our ViewModel
                var viewModel = new ShoppingCartViewModel
                {
                    CartItems = cart.GetCartItems(),
                    CartTotal = cart.GetTotal()
                };
                // Return the view
                return View(viewModel);
            }
            //
            // GET: /Store/AddToCart/5
            public ActionResult AddToCart(int id)
            {
                // Retrieve the album from the database
                var addedAlbum = storeDB.Albums
                    .Single(album => album.AlbumId == id);

                // Add it to the shopping cart
                var cart = ShoppingCart.GetCart(this.HttpContext);

                cart.AddToCart(addedAlbum);

                // Go back to the main store page for more shopping
                return RedirectToAction("Index");
            }
            //
            // AJAX: /ShoppingCart/RemoveFromCart/5
            [HttpPost]
            public ActionResult RemoveFromCart(int id)
            {
                // Remove the item from the cart
                var cart = ShoppingCart.GetCart(this.HttpContext);  
                //傳送HttpContext透過context.Session[CartSessionKey]取得某cartId的cart　

                // Get the name of the album to display confirmation
                string albumName = storeDB.Carts
                    .Single(item => item.RecordId == id).Album.Title;

                // Remove from cart
                int itemCount = cart.RemoveFromCart(id);

                // Display the confirmation message
                var results = new ShoppingCartRemoveViewModel
                {
                    Message = Server.HtmlEncode(albumName) +
                        "已從購物車中移除",
                    CartTotal = cart.GetTotal(),
                    CartCount = cart.GetCount(),
                    ItemCount = itemCount,
                    DeleteId = id
                };
                return Json(results);
            }
            //
            // GET: /ShoppingCart/CartSummary
            [ChildActionOnly]
            public ActionResult CartSummary()
            {
                var cart = ShoppingCart.GetCart(this.HttpContext);

                ViewData["CartCount"] = cart.GetCount();
                return PartialView("_CartSummary");
            }

            [HttpPost]
            public ActionResult UpdateCartCount(int id, int cartCount)
            {
                ShoppingCartRemoveViewModel results = null;
                try
                {
                    // Get the cart 
                    var cart = ShoppingCart.GetCart(this.HttpContext);

                    // Get the name of the album to display confirmation 
                    string albumName = storeDB.Carts
                        .Single(item => item.RecordId == id).Album.Title;

                    // Update the cart count 
                    int itemCount = cart.UpdateCartCount(id, cartCount);

                    //Prepare messages
                    string msg =  Server.HtmlEncode(albumName) +
                            " 的數量已更新！ ";
                    if (itemCount == 0) msg = Server.HtmlEncode(albumName) +
                            " 已移除！ ";
                    //
                    // Display the confirmation message 
                    results = new ShoppingCartRemoveViewModel
                    {
                        Message = msg,
                        CartTotal = cart.GetTotal(),
                        CartCount = cart.GetCount(),
                        ItemCount = itemCount,
                        DeleteId = id
                    };
                }
                catch
                {
                    results = new ShoppingCartRemoveViewModel
                    {
                        Message = "錯誤的輸入數量！",
                        CartTotal = -1,
                        CartCount = -1,
                        ItemCount = -1,
                        DeleteId = id
                    };
                }
                return Json(results);
            }
        }
    }
