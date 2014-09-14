using System.Collections.Generic;
using System.Web.Mvc;
using WecareMVC.Models;
using System.Linq;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        MusicStoreEntities storeDB = new MusicStoreEntities();
        public ActionResult Index()
        {
            // Get most popular albums
            var albums = GetTopSellingAlbums(5);

            if (HttpContext.Application["Theme"] == null)
                HttpContext.Application["Theme"] = LoadTheme();
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View(albums);
        }

        public string LoadTheme(string Theme = null)
        {
            string sLocation = "/Content/bootstrap_";
         
            if (Theme == null || Theme=="Default")
            {
                sLocation += "Default.css";
            }
            else 
            {
                sLocation +=  Theme+".css";
            }   


            HttpContext.Application["Theme"] = sLocation;
            return HttpContext.Application["Theme"] as string;
        }

        private List<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return storeDB.Albums
                .OrderByDescending(a => a.OrderDetails.Sum(o => o.Quantity))
                .Take(count)
                .ToList();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
