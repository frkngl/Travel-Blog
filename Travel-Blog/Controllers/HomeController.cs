using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Description;
using Travel_Blog.Models;

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

        [HttpGet]
        public ActionResult Contact()
        {
            var degerler = db.TBLCONTACT.FirstOrDefault();
            return View(degerler);
        }

        [HttpPost]
        public ActionResult SendMail(string Name_Surname, string Email, string Subject, string Message)
        {
            try
            {
                MailMessage mail = new MailMessage();

                mail.ReplyToList.Add(new MailAddress(Email, Name_Surname));

                mail.From = new MailAddress("furkangul.dev@gmail.com", "GülBlog İletişim");

                mail.To.Add("furkangul.dev@gmail.com");

                // Başlık ve İçerik
                mail.Subject = "GülBlog İletişim Formu: " + Subject;
                mail.IsBodyHtml = true;
                mail.Body = $@"<h3>Yeni Mesaj</h3>
                       <p><b>Gönderen:</b> {Name_Surname}</p>
                       <p><b>Ziyaretçi E-posta:</b> {Email}</p>
                       <p><b>Konu:</b> {Subject}</p>
                       <hr/>
                       <p><b>Mesaj:</b></p>
                       <p>{Message.Replace("\r\n", "<br />")}</p>";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;

                smtp.Credentials = new NetworkCredential("furkangul.dev@gmail.com", "nzpzsjfichzzpuyq");

                smtp.Send(mail);

                TempData["ContactSuccess"] = "Mesajınız başarıyla e-posta olarak gönderildi!";
            }
            catch (Exception ex)
            {
                TempData["ContactError"] = "Mail gönderilirken bir hata oluştu: " + ex.Message;
            }
            return RedirectToAction("Contact");
        }
    }
}