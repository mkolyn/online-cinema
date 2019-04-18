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
    public class HomeController : Controller
    {
        private UserContext userDb = new UserContext();
        private CinemaHallMovieContext cinemaHallMovieDb = new CinemaHallMovieContext();

        public ActionResult Index()
        {
            DateTime date = DateTime.Now;
            ViewBag.date = date;

            List<CinemaHallScheduleMovie> movies = cinemaHallMovieDb.GetList(date.Year, date.Month, date.Day);
            ViewBag.movies = movies;

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
    }
}