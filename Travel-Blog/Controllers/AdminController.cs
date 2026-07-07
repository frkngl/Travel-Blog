using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
        [HttpGet]
        public ActionResult UserSetting()
        {
            string currentUsername = User.Identity.Name;
            var user = db.TBLADMIN.FirstOrDefault(x => x.USERNAME == currentUsername);
            return View(user);
        }

        [HttpPost]
        public ActionResult UserSettingUpdate(TBLADMIN p, HttpPostedFileBase ImageFile)
        {
            var user = db.TBLADMIN.Find(p.ID);

            if (user != null)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/webimage"), fileName);
                    ImageFile.SaveAs(path);

                    user.IMAGE = fileName; 
                }

                string oldUsername = user.USERNAME;

                // Diğer Verileri Aktarma
                user.NAME_AND_SURNAME = p.NAME_AND_SURNAME;
                user.USERNAME = p.USERNAME;
                user.EMAIL = p.EMAIL;
                user.PASSWORD = p.PASSWORD;
                user.JOB = p.JOB;
                user.DESCRIPTION = p.DESCRIPTION;
                user.TWITTER = p.TWITTER;
                user.INSTAGRAM = p.INSTAGRAM;
                user.LINKEDIN = p.LINKEDIN;

                if (user.STATUS == false)
                {
                    user.STATUS = true;
                }
                else
                {
                    user.STATUS = false;
                }

                db.SaveChanges();

                if (oldUsername != p.USERNAME)
                {
                    FormsAuthentication.SetAuthCookie(p.USERNAME, false);
                }

                TempData["UpdateSuccess"] = "Profil bilgileriniz başarıyla güncellendi.";
                return RedirectToAction("UserSetting");
            }

            TempData["UpdateError"] = "Güncelleme sırasında bir hata oluştu.";
            return RedirectToAction("UserSetting");
        }

        public ActionResult AdminList()
        {
            var degerler = db.TBLADMIN.ToList();
            return View(degerler);
        }

        [HttpGet]
        public ActionResult ChangeStatus(int id)
        {
            var admin = db.TBLADMIN.Find(id);

            if (admin != null)
            {
                admin.STATUS = !admin.STATUS;
                db.SaveChanges();

                string durum = (admin.STATUS == true) ? "Aktif" : "Pasif";
                TempData["UpdateSuccess"] = "Kullanıcı durumu '" + durum + "' olarak güncellendi.";
            }
            else
            {
                TempData["UpdateError"] = "Kullanıcı bulunamadı!";
            }

            return RedirectToAction("AdminList");
        }
    }
}