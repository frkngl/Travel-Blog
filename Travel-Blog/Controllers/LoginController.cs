using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Travel_Blog.Models;

namespace Travel_Blog.Controllers
{
    public class LoginController : Controller
    {
        TravelBlogEntities db = new TravelBlogEntities();
        // GET: Login
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

        [HttpPost]
        public ActionResult Register(string Username, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                ViewBag.RegisterError = "Girdiğiniz şifreler birbiriyle uyuşmuyor!";
                return View("Index");
            }
            try
            {
                TBLADMIN newAdmin = new TBLADMIN();
                newAdmin.USERNAME = Username;
                newAdmin.NAME_AND_SURNAME = Username;
                newAdmin.PASSWORD = Password;
                newAdmin.ADMINROLEID = 3;
                newAdmin.CREATE_DATE = DateTime.Now;
                newAdmin.STATUS = true;
                newAdmin.DESCRIPTION = null;
                newAdmin.TWITTER = null;
                newAdmin.INSTAGRAM = null;
                newAdmin.LINKEDIN = null;
                newAdmin.IMAGE = null;
                newAdmin.JOB = null;
                newAdmin.EMAIL = null;

                db.TBLADMIN.Add(newAdmin);
                db.SaveChanges();

                ViewBag.RegisterSuccess = "Kayıt işleminiz başarıyla tamamlandı! Şimdi giriş yapabilirsiniz.";
                return View("Index");
            }
            catch (DbEntityValidationException ex)
            {
                string hataMesaji = "Veritabanı reddetti: ";
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        hataMesaji += string.Format("[{0} sütunu eksik veya hatalı!] ", validationError.PropertyName);
                    }
                }

                ViewBag.RegisterError = hataMesaji;
                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.RegisterError = "Kayıt Hata Detayı: " + ex.Message;
                return View("Index");
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return Redirect("/Login/Index");
        }
    }
}