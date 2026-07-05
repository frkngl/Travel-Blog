using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Travel_Blog.Models;

namespace Travel_Blog.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        TravelBlogEntities db = new TravelBlogEntities();
        TableList data = new TableList();
        // GET: Admin
        public async Task<ActionResult> Index()
        {
            if (Session["NameAndSurname"] == null)
            {
                return Redirect("/Login/Index");
            }
            ViewBag.BlogCount = await db.TBLBLOGS.CountAsync();
            ViewBag.BlogCountActive = await db.TBLBLOGS.Where(x=>x.STATUS == true).CountAsync();
            ViewBag.BlogCountPassive = await db.TBLBLOGS.Where(x => x.STATUS == false).CountAsync();
            ViewBag.AdminCount = await db.TBLADMIN.CountAsync();

            data.AdminList = await db.TBLADMIN.Where(x => x.STATUS == true).ToListAsync();
            data.BlogsList = await db.TBLBLOGS.Where(x => x.STATUS == true).ToListAsync();
            return View(data);
        }
    }
}