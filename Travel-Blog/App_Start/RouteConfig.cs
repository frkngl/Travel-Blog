using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Travel_Blog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // 1. ANASAYFA ROTASI
            routes.MapRoute(
                name: "Anasayfa",
                url: "anasayfa", // Tarayıcıda siteadi.com/anasayfa şeklinde görünmesini sağlar
                defaults: new { controller = "Home", action = "Index" }
            );

            // 2. HAKKIMIZDA SAYFASI ROTASI
            routes.MapRoute(
                name: "Hakkimizda",
                url: "hakkimizda",
                defaults: new { controller = "Home", action = "About" }
            );

            // 3. İLETİŞİM SAYFASI ROTASI
            routes.MapRoute(
                name: "Iletisim",
                url: "iletisim",
                defaults: new { controller = "Home", action = "Contact" }
            );

            // 4. 404 SAYFASI ROTASI
            routes.MapRoute(
            name: "PageNotFound",
            url: "sayfa-bulunamadi",
            defaults: new { controller = "Error", action = "NotFound" }
            );

            // Varsayılan Rota (Buna dokunmuyoruz, site ilk açıldığında boş URL ile /Home/Index'e gitmeye devam eder)
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
