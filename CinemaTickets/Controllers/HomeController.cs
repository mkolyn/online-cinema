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
    public class HomeController : BaseController
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
            ViewBag.Styles.Add("home");
            ViewBag.Styles.Add("calendar");
            ViewBag.Scripts.Add("home");
            ViewBag.ScriptTexts.Add("var IS_HOME_PAGE = true;");

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

            ViewBag.moviesHtml = GetMoviesHtml(date.Year, date.Month, date.Day, cinemaId, genreId, searchString);
            ViewBag.dates = Core.GetNextDates(DateTime.Now);
            ViewBag.cityId = Core.GetCityId();
            ViewBag.CinemaID = cinemaDb.GetSelectList(cinemaId, Core.GetCityId());
            ViewBag.GenreID = genreDb.GetSelectList(genreId);
            ViewBag.monthList = Core.GetMonthList();
            ViewBag.dayList = Core.GetDayList();
            ViewBag.currentDate = DateTime.Now;
            ViewBag.genres = genreDb.Genres.ToList();
            ViewBag.searchString = searchString;

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

        public ActionResult AllowShowThankyouPage()
        {
            Session["showThankyouPage"] = true;
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Thankyou()
        {
            if (Session["showThankyouPage"] == null)
            {
                return Redirect("/");
            }

            Session["showThankyouPage"] = null;
            return View();
        }

        public string GetMoviesHtml(int year, int month, int day, int cinemaId, int genreId, string searchString)
        {
            List<Movie> movies = cinemaHallMovieDb.GetList(year, month, day, cinemaId, genreId, searchString);

            ViewDataDictionary viewData = new ViewDataDictionary()
            {
                { "movies", movies },
            };

            return Core.GetHtmlString("Movies", viewData, ControllerContext);
        }
    }
}