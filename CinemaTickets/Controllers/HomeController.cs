using CinemaTickets.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTickets.Controllers
{
    public class HomeController : CoreController
    {
        private UserContext userDb = new UserContext();
        private CinemaHallMovieContext cinemaHallMovieDb = new CinemaHallMovieContext();
        private CinemaContext cinemaDb = new CinemaContext();
        private GenreContext genreDb = new GenreContext();

        public ActionResult Index(
            int year = 0,
            int month = 0,
            int day = 0,
            int cinemaId = 0,
            int genreId = 0,
            string searchString = ""
        )
        {
            ViewBag.Scripts.Add("home");

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

            List<CinemaHallScheduleMovie> movies = cinemaHallMovieDb.GetList(date.Year, date.Month, date.Day, cinemaId, genreId, searchString);

            ViewBag.movies = movies;
            ViewBag.dates = Core.GetNextDates(DateTime.Now);
            ViewBag.cityId = Core.GetCityId();
            ViewBag.CinemaID = cinemaDb.GetSelectList(cinemaId, Core.GetCityId());
            ViewBag.GenreID = genreDb.GetSelectList(genreId);

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