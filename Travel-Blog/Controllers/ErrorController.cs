using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel_Blog.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        // Genel hatalar için
        public ActionResult Index()
        {
            return View();
        }

        // 404 Hataları için
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }
    }
}