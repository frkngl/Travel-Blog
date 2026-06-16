using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Travel_Blog.Models;

namespace Travel_Blog.Controllers
{
    public class BlogController : Controller
    {
        TravelBlogEntities db = new TravelBlogEntities();
        TableList data = new TableList();
        // GET: Blog
        public ActionResult Index(string categoryName, string searchString, int? page)
        {
            // data tanımlamaların ve listelerin...
            data.AdminList = db.TBLADMIN.ToList();
            data.CategoryList = db.TBLCATEGORY.ToList();

            var query = db.TBLBLOGS.AsQueryable();

            // -- MEVCUT KATEGORİ FİLTRELEMESİ --
            if (!string.IsNullOrEmpty(categoryName))
            {
                var selected = data.CategoryList.FirstOrDefault(x =>
                        x.CATEGORYNAME.ToLower()
                        .Replace("ş", "s").Replace("ç", "c").Replace("ı", "i")
                        .Replace("ğ", "g").Replace("ö", "o").Replace("ü", "u")
                        .Replace(" ", "-") == categoryName);

                if (selected != null)
                {
                    query = query.Where(x => x.CATEGORYID == selected.ID);
                    data.ActiveCategoryName = selected.CATEGORYNAME;
                }
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.TITLE.Contains(searchString) || x.DESCRIPTION.Contains(searchString));
            }

            ViewBag.CurrentSearch = searchString;

            int pageSize = 6;
            int pageNumber = (page ?? 1);

            data.BlogsList = query.OrderByDescending(x => x.DATE).ToPagedList(pageNumber, pageSize);

            return View(data);
        }
    }
}