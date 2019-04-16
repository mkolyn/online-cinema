using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using OnlineCinema.Models;
using System.IO;

namespace OnlineCinema.Controllers
{
    public class CinemaHallScheduleController : Controller
    {
        private CinemaHallPlaceContext db = new CinemaHallPlaceContext();
        private MovieContext movieDb = new MovieContext();
        private CinemaHallMovieContext cinemaHallMovieDb = new CinemaHallMovieContext();

        public void LoginIfNotAuthorized()
        {
            if (Session["UserID"] == null || Session["UserID"].ToString() == "")
            {
                Response.Redirect("Home");
            }
        }

        // GET: CinemaHallSchedule/5
        public ActionResult Index(int? id)
        {
            LoginIfNotAuthorized();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CinemaHallContext cinemaHallDb = new CinemaHallContext();
            CinemaHall cinemaHall = cinemaHallDb.CinemaHalls.Find(id);
            if (cinemaHall == null)
            {
                return HttpNotFound();
            }

            ViewBag.CinemaHallID = id;
            ViewBag.CinemaHallName = cinemaHall.Name;

            DateTime date = DateTime.Now;
            DateTime prevDate = date.AddDays(-1).Date;
            DateTime nextDate = date.AddDays(1).Date;

            ViewBag.Year = date.Year;
            ViewBag.Month = date.Month;
            ViewBag.Day = date.Day;

            ViewBag.PrevYear = prevDate.Year;
            ViewBag.PrevMonth = prevDate.Month;
            ViewBag.PrevDay = prevDate.Day;

            ViewBag.NextYear = nextDate.Year;
            ViewBag.NextMonth = nextDate.Month;
            ViewBag.NextDay = nextDate.Day;

            var cinemaHallMovies = from chm in cinemaHallMovieDb.CinemaHallMovies
                                   join m in cinemaHallMovieDb.Movies on chm.MovieID equals m.ID
                                   orderby chm.Date
                                   select new CinemaHallScheduleMovie
                                   {
                                       CinemaHallMovieID = chm.ID,
                                       MovieID = m.ID,
                                       MovieName = m.Name,
                                       Duration = m.Duration,
                                       Date = chm.Date,
                                   };

            string cinemaHallMoviesHtml = "";
            foreach (CinemaHallScheduleMovie cinemaHallMovie in cinemaHallMovies)
            {
                cinemaHallMoviesHtml += GetSchedulteMovieItemHtml(cinemaHallMovie);
            }
            ViewBag.CinemaHallMoviesHtml = cinemaHallMoviesHtml;

            return View();
        }

        // POST: CinemaHallSchedule/Save
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Save(int id, int year, int month, int day, List<CinemaHallScheduleMovie> movies)
        {
            foreach (CinemaHallScheduleMovie scheduleMovie in movies)
            {
                var cinemaHallMovies = from chm in cinemaHallMovieDb.CinemaHallMovies
                                       select chm;

                cinemaHallMovies = cinemaHallMovies.Where(s => s.CinemaHallID == id);

                Movie movie = movieDb.Movies.Find(scheduleMovie.MovieID);
                int hour = scheduleMovie.StartMinute / 60;
                int minute = scheduleMovie.StartMinute - hour * 60;
                DateTime date = new DateTime(year, month, day, hour, minute, 0);

                if (scheduleMovie.CinemaHallMovieID > 0)
                {
                    CinemaHallMovie cinemaHallMovie = cinemaHallMovieDb.CinemaHallMovies.Find(scheduleMovie.CinemaHallMovieID);
                    if (scheduleMovie.IsRemoved)
                    {
                        cinemaHallMovieDb.CinemaHallMovies.Remove(cinemaHallMovie);
                    }
                    else
                    {
                        cinemaHallMovie.Date = date;
                    }
                    cinemaHallMovieDb.SaveChanges();
                }
                else
                {
                    cinemaHallMovies = cinemaHallMovies.Where(s => s.MovieID == movie.ID)
                        .Where(s => s.Date == date);

                    if (cinemaHallMovies.FirstOrDefault() == null)
                    {
                        CinemaHallMovie cinemaHallMovie = new CinemaHallMovie
                        {
                            CinemaHallID = id,
                            MovieID = movie.ID,
                            Date = date,
                        };

                        cinemaHallMovieDb.CinemaHallMovies.Add(cinemaHallMovie);
                        cinemaHallMovieDb.SaveChanges();
                    }
                }
            }

            return Json("ok");
        }

        public ActionResult GetMovieItemHtml(int id)
        {
            Movie movie = movieDb.Movies.Find(id);
            ViewData["CinemaHallMovieID"] = 0;
            ViewData["MovieID"] = movie.ID;
            ViewData["MovieName"] = movie.Name;
            ViewData["Duration"] = movie.Duration;
            ViewData["ItemHeight"] = movie.Duration / 60 * 25;
            return PartialView("Movie");
        }

        public string GetSchedulteMovieItemHtml(CinemaHallScheduleMovie cinemaHallMovie)
        {
            string html = "";
            DateTime cinemaHallMovieDate = cinemaHallMovie.Date;
            int startMinute = cinemaHallMovieDate.Hour * 60 + cinemaHallMovieDate.Minute;

            ControllerContext context = ControllerContext;

            ViewEngineResult viewEngineResult = ViewEngines.Engines.FindPartialView(context, "Movie");
            var view = viewEngineResult.View;
            ViewDataDictionary viewData = new ViewDataDictionary
                {
                    { "CinemaHallMovieID", cinemaHallMovie.CinemaHallMovieID },
                    { "MovieID", cinemaHallMovie.MovieID },
                    { "MovieName", cinemaHallMovie.MovieName },
                    { "StartMinute", startMinute },
                    { "Duration", cinemaHallMovie.Duration },
                    { "ItemHeight", cinemaHallMovie.Duration / 60 * 25 },
                };

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view, viewData, context.Controller.TempData, sw);
                view.Render(ctx, sw);
                html = sw.ToString();
            }

            return html;
        }

        public ActionResult ChangeDate(int cinemaHallId, int year, int month, int day, int days, int direction)
        {
            DateTime date = new DateTime(year, month, day);
            if (direction != 1)
            {
                days = -days;
            }
            date = date.AddDays(days);

            DateTime prevDate = date.AddDays(-1);
            DateTime nextDate = date.AddDays(1);

            var cinemaHallMovies = from chm in cinemaHallMovieDb.CinemaHallMovies
                                   join m in cinemaHallMovieDb.Movies on chm.MovieID equals m.ID
                                   orderby chm.Date
                                   select new CinemaHallScheduleMovie
                                   {
                                       CinemaHallMovieID = chm.ID,
                                       CinemaHallID = chm.CinemaHallID,
                                       MovieID = m.ID,
                                       MovieName = m.Name,
                                       Duration = m.Duration,
                                       Date = chm.Date
                                   };

            cinemaHallMovies = cinemaHallMovies.Where(s => s.CinemaHallID == cinemaHallId)
                .Where(s => s.Date.Year == date.Year)
                .Where(s => s.Date.Month == date.Month)
                .Where(s => s.Date.Day == date.Day);

            string html = "";
            foreach (CinemaHallScheduleMovie cinemaHallMovie in cinemaHallMovies)
            {
                html += GetSchedulteMovieItemHtml(cinemaHallMovie);
            }

            return Json(new {
                year = date.Year,
                month = date.Month,
                day = date.Day,
                date = date.Day + "." + date.Month + "." + date.Year,
                prevDate = prevDate.Day + "." + prevDate.Month + "." + prevDate.Year,
                nextDate = nextDate.Day + "." + nextDate.Month + "." + nextDate.Year,
                html,
            });
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
