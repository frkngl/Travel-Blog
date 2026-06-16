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

            routes.MapRoute(
                name: "Anasayfa",
                url: "anasayfa",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "Hakkimizda",
                url: "hakkimizda",
                defaults: new { controller = "Home", action = "About" }
            );

            routes.MapRoute(
                name: "Iletisim",
                url: "iletisim",
                defaults: new { controller = "Home", action = "Contact" }
            );

            routes.MapRoute(
                name: "PageNotFound",
                url: "sayfa-bulunamadi",
                defaults: new { controller = "Error", action = "NotFound" }
            );

            routes.MapRoute(
                name: "CategoryRoute",
                url: "{categoryName}",
                defaults: new { controller = "Blog", action = "Index", categoryName = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "BlogDetailRoute",
                url: "blog/{seourl}",
                defaults: new { controller = "Blog", action = "BlogDetail" }
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
