using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace CinemaTickets
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            string[] namespaces = new string[1];
            namespaces[0] = "CinemaTickets";

            routes.MapRoute(
                name: "Administrator",
                url: "administrator/{controller}/{action}/{id}",
                defaults: new { controller = "Administrator", action = "Index", id = UrlParameter.Optional, isAdmin = "true" },
                namespaces: namespaces
            );

            routes.MapRoute(
                name: "Book",
                url: "book/{id}",
                defaults: new { controller = "Book", action = "Index" },
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

            routes.MapRoute(
                name: "DateAndCinema",
                url: "date/{year}/{month}/{day}/{cinemaId}",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: namespaces
            );

            routes.MapRoute(
                name: "DateAndCinemaAndGenre",
                url: "date/{year}/{month}/{day}/{cinemaId}/{genreId}",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: namespaces
            );
        }
    }
}
