using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travel_Blog.Models;

namespace Travel_Blog.Controllers
{
    public class BlogController : Controller
    {
        TravelBlogEntities db = new TravelBlogEntities();
        TableList data = new TableList();
        // GET: Blog
        public ActionResult Index(string categoryName)
        {
            TableList data = new TableList();
            data.AdminList = db.TBLADMIN.ToList();
            data.CategoryList = db.TBLCATEGORY.ToList();
                var selected = data.CategoryList.FirstOrDefault(x =>
                    x.CATEGORYNAME.ToLower()
                    .Replace("ş", "s").Replace("ç", "c").Replace("ı", "i")
                    .Replace("ğ", "g").Replace("ö", "o").Replace("ü", "u")
                    .Replace(" ", "-") == categoryName);
                data.BlogsList = db.TBLBLOGS.Where(x => x.CATEGORYID == selected.ID).ToList();
                data.ActiveCategoryName = selected.CATEGORYNAME;
            return View(data);
        }
    }
}