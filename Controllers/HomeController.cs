using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WecareMVC.Controllers
{
    [RequireHttps]  //增加安全性，需要憑證
    public class HomeController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
    }
}