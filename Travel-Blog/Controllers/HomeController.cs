using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel_Blog.Models;
using System.Web.Mvc;

namespace Travel_Blog.Controllers
{
    public class HomeController : Controller
    {
       TravelBlogEntities db = new TravelBlogEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var hakkimizda = db.TBLABOUT.FirstOrDefault();
            var admin = db.TBLADMIN.ToList();
            var models = Tuple.Create(hakkimizda, admin);
            return View(models);
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}