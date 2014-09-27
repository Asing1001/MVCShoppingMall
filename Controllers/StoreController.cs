using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WecareMVC.Models;
using WecareMVC.ViewModels;
using PagedList;
using PagedList.Mvc;


namespace WecareMVC.Controllers
{
    public class StoreController : Controller
    {
        //
        MusicStoreEntities storeDB = new MusicStoreEntities();
        // GET: /Store/
        public ActionResult Index()
        {
            var genres = storeDB.Genres.ToList();            
            return View(genres);
            //var genres = new List<Genre> { 
            //    new Genre { Name = "Disco" }, 
            //    new Genre { Name = "Jazz" }, 
            //    new Genre { Name = "Rock" } 
            //};
            //return View(genres);  
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

        //
        // url: "store/{genre}"
        public ActionResult Browse(int? page, string genre = "Hot")
        {
           // Retrieve Genre and its Associated Albums from database
            var genreModel = storeDB.Genres.Include("Albums")
                .Single(g => g.Name == genre).Albums;


            return View(genreModel.ToList().ToPagedList(page?? 1, 6));
        }

        // GET: /Store/Details/5
        public ActionResult Details(int id)
        {
            var album = storeDB.Albums.Find(id);

            return View(album);
        }

        [ChildActionOnly]
        public ActionResult GenreMenu()   //左側分類Menu
        {                        
            //var genres = from p in storeDB.Genres
            //             orderby p.Name ascending
            //             select new GenreViewModel
            //             {  AlbumsCount = p.Albums.Count, GenreName =p.Name };
            var genres = storeDB.Genres.Include("Albums").OrderBy(g => g.Name);
            var model = genres.ToList();         
            return PartialView("_GenreMenu", model);
        }
    }
}