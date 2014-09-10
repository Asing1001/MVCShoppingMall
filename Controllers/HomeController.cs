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

            return View(albums);
        }

        private List<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return storeDB.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
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
