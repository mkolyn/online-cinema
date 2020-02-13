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

            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            DateTime date = DateTime.Now;
            DateTime maxDate = cinemaHallMovieDb.GetMovieMaxDate(id);
            DateTime moviePeriodEndDate = date.AddDays(Core.SELECT_MOVIE_TIME_DAYS);
            moviePeriodEndDate = moviePeriodEndDate > maxDate ? maxDate : moviePeriodEndDate;

            ViewBag.dates = Core.GetNextDates(date);
            ViewBag.monthList = Core.GetMonthList();
            ViewBag.schedule = cinemaHallMovieDb.GetMovieSchedule(id);
            ViewBag.dayList = Core.GetDayList();
            ViewBag.currentDate = date;
            ViewBag.moviePeriodStartDate = date;
            ViewBag.moviePeriodEndDate = moviePeriodEndDate;

            return View(movie);
        }

        public string GetSelectMovieTimeHtml(int id, int day, int month, int year)
        {
            string html = "";
            DateTime maxDate = cinemaHallMovieDb.GetMovieMaxDate(id);
            DateTime moviePeriodStartDate = new DateTime(year, month, day).AddDays(1);
            DateTime moviePeriodEndDate = moviePeriodStartDate.AddDays(Core.SELECT_MOVIE_TIME_DAYS);
            moviePeriodEndDate = moviePeriodEndDate > maxDate ? maxDate : moviePeriodEndDate;

            ControllerContext context = ControllerContext;

            ViewEngineResult viewEngineResult = ViewEngines.Engines.FindPartialView(context, "Time");
            var view = viewEngineResult.View;
            ViewDataDictionary viewData = new ViewDataDictionary
                {
                    { "schedule", cinemaHallMovieDb.GetMovieSchedule(id) },
                    { "moviePeriodStartDate", moviePeriodStartDate },
                    { "moviePeriodEndDate", moviePeriodEndDate },
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
