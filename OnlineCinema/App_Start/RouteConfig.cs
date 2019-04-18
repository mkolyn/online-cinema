using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineCinema
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            string[] namespaces = new string[1];
            namespaces[0] = "OnlineCinema";

            routes.MapRoute(
                name: "Administrator",
                url: "administrator/{controller}/{action}/{id}",
                defaults: new { controller = "Administrator", action = "Index", id = UrlParameter.Optional },
                namespaces: namespaces
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: namespaces
            );

            routes.MapRoute(
                name: "Date",
                url: "date/{year}/{month}/{day}",
                defaults: new { controller = "Home", action = "Index", year = DateTime.Now.Year, month = DateTime.Now.Month, day = DateTime.Now.Day },
                namespaces: namespaces
            );
        }
    }
}
