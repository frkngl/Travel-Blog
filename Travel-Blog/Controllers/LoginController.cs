using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Travel_Blog.Models;

namespace Travel_Blog.Controllers
{
    public class LoginController : Controller
    {
        new TravelBlogEntities db = new TravelBlogEntities();
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string Username, string Password)
        {
            var admin = db.TBLADMIN.FirstOrDefault(x => x.USERNAME == Username && x.PASSWORD == Password);

            if (admin != null)
            {
                FormsAuthentication.SetAuthCookie(admin.USERNAME, false);
                Session["NameAndSurname"] = admin.NAME_AND_SURNAME;
                return Redirect("/Admin/Index");
            }
            else
            {
                ViewBag.ErrorMessage = "Kullanıcı adı veya şifre hatalı!";
                return View();
            }
        }
    }
}