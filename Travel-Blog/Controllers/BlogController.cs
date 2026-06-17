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
        public static string SeoUrlCreate(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";

            text = text.ToLower();
            text = text.Replace("ş", "s").Replace("ç", "c").Replace("ı", "i")
                       .Replace("ğ", "g").Replace("ö", "o").Replace("ü", "u");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"[^a-z0-9]", "-");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"-+", "-");
            return text.Trim('-');
        }
        [Route("{seourl}")]
        public ActionResult Index(string categoryName, string searchString, int? page)
        {
            data.AdminList = db.TBLADMIN.Where(x=>x.STATUS == true).ToList();
            data.CategoryList = db.TBLCATEGORY.Where(x=>x.STATUS == true).ToList();
            var query = db.TBLBLOGS.Where(x => x.STATUS == true).AsQueryable();
            if (!string.IsNullOrEmpty(categoryName))
            {
                var selected = data.CategoryList.FirstOrDefault(x => SeoUrlCreate(x.CATEGORYNAME) == categoryName);

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
            data.PagedBlogsList = query.OrderByDescending(x => x.DATE).ToPagedList(pageNumber, pageSize);
            return View(data);
        }

        public ActionResult BlogDetail(string seourl)
        {
            if (string.IsNullOrEmpty(seourl))
            {
                return RedirectToAction("Index");
            }
            var blogList = db.TBLBLOGS.Where(x => x.STATUS == true).ToList();
            var blogDetail = blogList.FirstOrDefault(x => SeoUrlCreate(x.TITLE) == seourl);
            if (blogDetail == null)
            {
                return View("Error", "NotFound");
            }
            return View(blogDetail);
        }

        [ChildActionOnly]
        public PartialViewResult GetRecentBlogs()
        {
            var randomBlogs = db.TBLBLOGS.Where(x => x.STATUS == true).OrderBy(x => Guid.NewGuid()).Take(5).ToList();
            return PartialView(randomBlogs);
        }

        [ChildActionOnly]
        public PartialViewResult GetCategories()
        {
            var categories = db.TBLCATEGORY.Where(x => x.STATUS == true).ToList();
            return PartialView(categories);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LeaveComment(TBLCOMMENT newComment)
        {
            try
            {
                ModelState.Clear();
                if (newComment != null)
                {
                    newComment.DATE = DateTime.Now;
                    newComment.STATUS = false;

                    db.TBLCOMMENT.Add(newComment);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Yorumunuz başarıyla gönderildi, onaylandıktan sonra yayınlanacaktır.";
                }
                if (newComment != null && newComment.BLOGID > 0)
                {
                    var blog = db.TBLBLOGS.Find(newComment.BLOGID);

                    if (blog != null)
                    {
                        string seoLink = SeoUrlCreate(blog.TITLE);
                        return RedirectToAction("BlogDetail", "Blog", new { seourl = seoLink });
                    }
                }

                return RedirectToAction("Index", "Blog");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kayıt Hatası: " + ex.Message;
                return RedirectToAction("Index", "Blog");
            }
        }
    }
}