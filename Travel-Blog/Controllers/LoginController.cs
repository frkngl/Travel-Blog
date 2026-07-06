using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        public JsonResult ResetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "E-posta alanı boş bırakılamaz." });
            }

            // Kendi DbContext adınızı buraya yazın (Örn: ProjectContext db = new ProjectContext())
            using (var db = new TravelBlogEntities())
            {
                // Kullanıcıyı e-posta adresine göre bul
                var user = db.TBLADMIN.FirstOrDefault(u => u.EMAIL == email);

                if (user == null)
                {
                    return Json(new { success = false, message = "Bu e-posta adresine ait kayıtlı bir hesap bulunamadı." });
                }

                // 1. Rastgele şifre oluştur (8 karakterli)
                string newPassword = GenerateRandomPassword(8);

                // 2. Şifreyi veritabanında güncelle
                // ÖNEMLİ NOT: Gerçek projelerde şifreyi MD5/SHA256 vb. ile Hash'leyerek kaydetmelisiniz!
                user.PASSWORD = newPassword;
                db.SaveChanges();

                // 3. E-posta gönderme işlemi
                bool isEmailSent = SendEmail(user.EMAIL, newPassword);

                if (isEmailSent)
                {
                    return Json(new { success = true, message = "Yeni şifreniz e-posta adresinize gönderildi." });
                }
                else
                {
                    return Json(new { success = false, message = "Şifreniz sıfırlandı ancak e-posta gönderilirken bir sunucu hatası oluştu." });
                }
            }
        }

        // Rastgele Şifre Üreten Metot
        private string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // SMTP E-posta Gönderme Metodu
        private bool SendEmail(string toEmail, string newPassword)
        {
            try
            {
                // GÖNDERİCİ BİLGİLERİ (Kendi şirket mailinizi ve şifrenizi girin)
                string senderEmail = "furkangul.dev@gmail.com";
                string senderPassword = "nzpzsjfichzzpuyq";

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(senderEmail, "Sistem Yönetimi");
                mail.To.Add(toEmail);
                mail.Subject = "Şifre Sıfırlama Talebi";
                mail.Body = $"Merhaba,\n\nŞifre sıfırlama talebiniz alınmıştır.\n\n<b>Yeni Şifreniz:</b> {newPassword}\n\nLütfen sisteme giriş yaptıktan sonra şifrenizi değiştiriniz.";
                mail.IsBodyHtml = true;

                // SMTP AYARLARI (Gmail, Outlook veya Kendi sunucunuza göre ayarlayın)
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com"; // Örn: smtp.gmail.com
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);

                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
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