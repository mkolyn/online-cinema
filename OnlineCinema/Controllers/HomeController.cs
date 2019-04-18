using OnlineCinema.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCinema.Controllers
{
    public class HomeController : CoreController
    {
        private UserContext userDb = new UserContext();
        private CinemaHallMovieContext cinemaHallMovieDb = new CinemaHallMovieContext();

        public ActionResult Index(int year = 0, int month = 0, int day = 0)
        {
            DateTime date;
            if (year > 0 && month > 0 && day > 0)
            {
                date = new DateTime(year, month, day);
            }
            else
            {
                date = DateTime.Now;
            }
            ViewBag.date = date;

            List<CinemaHallScheduleMovie> movies = cinemaHallMovieDb.GetList(date.Year, date.Month, date.Day);

            ViewBag.movies = movies;
            ViewBag.dates = Core.GetNextDates(DateTime.Now);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult City(int id)
        {
            Session["CityID"] = id;
            return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
        }
    }
}