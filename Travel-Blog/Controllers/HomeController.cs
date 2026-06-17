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
        TableList data = new TableList();
        // GET: Home
        public ActionResult Index()
        {
            var sliderBlogs = db.TBLBLOGS.Where(x => x.STATUS == true).OrderByDescending(x => x.ID).Take(5).ToList();
            var randomBlogs = db.TBLBLOGS.Where(x => x.STATUS == true).OrderBy(x => Guid.NewGuid()).Take(12).ToList();
            var lifestyleBlogs = db.TBLBLOGS.Where(x => x.CATEGORYID == 1 && x.STATUS == true).OrderByDescending(x => x.ID).Take(15).ToList();
            var sportBlogs = db.TBLBLOGS .Where(x => x.CATEGORYID == 4 && x.STATUS == true).OrderByDescending(x => x.ID).Take(10).ToList();

            var viewModel = new TableList
            {
                SliderBlogs = sliderBlogs,

                TrendingMainBlog = randomBlogs.FirstOrDefault(),
                TrendingCol1Blogs = randomBlogs.Skip(1).Take(3).ToList(),
                TrendingCol2Blogs = randomBlogs.Skip(4).Take(3).ToList(),
                TrendingListBlogs = randomBlogs.Skip(7).Take(5).ToList(),

                LifestyleMainBlog = lifestyleBlogs.FirstOrDefault(),
                LifestyleLeftSmallBlogs = lifestyleBlogs.Skip(1).Take(2).ToList(),
                LifestyleCol1Blogs = lifestyleBlogs.Skip(3).Take(3).ToList(),
                LifestyleCol2Blogs = lifestyleBlogs.Skip(6).Take(3).ToList(),
                LifestyleRightListBlogs = lifestyleBlogs.Skip(9).Take(6).ToList(),

                SportMainBlog = sportBlogs.FirstOrDefault(),
                SportBottomLeftBlogs = sportBlogs.Skip(1).Take(2).ToList(),
                SportBottomRightBlog = sportBlogs.Skip(3).FirstOrDefault(),
                SportSidebarBlogs = sportBlogs.Skip(4).Take(6).ToList(),
            };
            return View(viewModel);
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