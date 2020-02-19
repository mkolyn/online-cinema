using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CinemaTickets.Models;
using PagedList;

namespace CinemaTickets.Controllers
{
    public class MovieController : BaseController
    {
        private MovieContext db = new MovieContext();
        private CinemaMovieContext cinemaMovieDb = new CinemaMovieContext();
        private CinemaHallMovieContext cinemaHallMovieDb = new CinemaHallMovieContext();
        private GenreContext genreDb = new GenreContext();
        private CinemaPlaceGroupContext cinemaPlaceGroupDb = new CinemaPlaceGroupContext();
        private CinemaMovieGroupPriceContext cinemaMovieGroupPriceDb = new CinemaMovieGroupPriceContext();
        private List<CinemaPlaceGroup> cinemaPlaceGroups = new List<CinemaPlaceGroup>();

        // GET: Movie/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.Styles.Add("movie-detail");
            ViewBag.Styles.Add("calendar");
            ViewBag.Scripts.Add("movie");

            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            DateTime date = DateTime.Now;
            DateTime maxDate = cinemaHallMovieDb.GetMovieMaxDate(id);
            int days = Request.Browser.ScreenPixelsWidth > 480 ? Core.SELECT_MOVIE_TIME_DAYS : Core.SELECT_MOVIE_MOBILE_TIME_DAYS;
            DateTime moviePeriodEndDate = date.AddDays(days);
            moviePeriodEndDate = moviePeriodEndDate > maxDate ? maxDate : moviePeriodEndDate;

            ViewBag.movieId = id;
            ViewBag.dates = Core.GetNextDates(date);
            ViewBag.monthList = Core.GetMonthList();
            ViewBag.dayList = Core.GetDayList();
            ViewBag.currentDate = date;

            ViewBag.timeHtml = GetSelectMovieTimeHtml(id, date.Day, date.Month, date.Year, 0);

            return View(movie);
        }

        public string GetSelectMovieTimeHtml(int id, int day, int month, int year, int direction)
        {
            string html = "";
            DateTime maxDate = cinemaHallMovieDb.GetMovieMaxDate(id);
            DateTime date = new DateTime(year, month, day);
            DateTime moviePeriodStartDate;
            DateTime moviePeriodEndDate;

            switch (direction)
            {
                case 1:
                    moviePeriodStartDate = date.AddDays(1);
                    moviePeriodEndDate = moviePeriodStartDate.AddDays(Core.SELECT_MOVIE_TIME_DAYS - 1);
                    break;
                case -1:
                    moviePeriodEndDate = date.AddDays(-1);
                    moviePeriodStartDate = moviePeriodEndDate.AddDays(-Core.SELECT_MOVIE_TIME_DAYS + 1);
                    break;
                default:
                    moviePeriodStartDate = date;
                    moviePeriodEndDate = moviePeriodStartDate.AddDays(Core.SELECT_MOVIE_TIME_DAYS - 1);
                    break;
            }

            moviePeriodEndDate = moviePeriodEndDate > maxDate ? maxDate : moviePeriodEndDate;
            moviePeriodStartDate = new DateTime(moviePeriodStartDate.Year, moviePeriodStartDate.Month, moviePeriodStartDate.Day, 0, 0, 0);
            moviePeriodEndDate = new DateTime(moviePeriodEndDate.Year, moviePeriodEndDate.Month, moviePeriodEndDate.Day, 23, 23, 23);

            ControllerContext context = ControllerContext;

            ViewEngineResult viewEngineResult = ViewEngines.Engines.FindPartialView(context, "Time");
            var view = viewEngineResult.View;
            ViewDataDictionary viewData = new ViewDataDictionary
                {
                    { "schedule", cinemaHallMovieDb.GetMovieSchedule(id, moviePeriodStartDate, moviePeriodEndDate) },
                    { "moviePeriodStartFormattedDate", Core.GetFormatedDay(moviePeriodStartDate, false) },
                    { "moviePeriodEndFormattedDate", Core.GetFormatedDay(moviePeriodEndDate, false) },
                    { "moviePeriodStartDay", moviePeriodStartDate.Day },
                    { "moviePeriodStartMonth", moviePeriodStartDate.Month },
                    { "moviePeriodStartYear", moviePeriodStartDate.Year },
                    { "moviePeriodEndDay", moviePeriodEndDate.Day },
                    { "moviePeriodEndMonth", moviePeriodEndDate.Month },
                    { "moviePeriodEndYear", moviePeriodEndDate.Year },
                };

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view, viewData, context.Controller.TempData, sw);
                view.Render(ctx, sw);
                html = sw.ToString();
            }

            return html;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
