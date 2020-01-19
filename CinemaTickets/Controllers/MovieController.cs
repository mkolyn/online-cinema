using System;
using System.Collections.Generic;
using System.Data;
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

            ViewBag.dates = Core.GetNextDates(DateTime.Now);
            ViewBag.monthList = Core.GetMonthList();
            ViewBag.schedule = cinemaHallMovieDb.GetMovieSchedule(id);
            ViewBag.dayList = Core.GetDayList();
            ViewBag.currentDate = DateTime.Now;

            return View(movie);
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
