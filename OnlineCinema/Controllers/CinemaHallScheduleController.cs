using System;
using System.Collections.Generic;
using System.Web.Mvc;
using OnlineCinema.Models;
using System.IO;

namespace OnlineCinema.Controllers
{
    public class CinemaHallScheduleController : AdminController
    {
        private CinemaHallPlaceContext db = new CinemaHallPlaceContext();
        private MovieContext movieDb = new MovieContext();
        private CinemaHallMovieContext cinemaHallMovieDb = new CinemaHallMovieContext();
        private CinemaHallContext cinemaHallDb = new CinemaHallContext();
        private CinemaMovieContext cinemaMovieDb = new CinemaMovieContext();
        private CinemaPlaceGroupContext cinemaPlaceGroupDb = new CinemaPlaceGroupContext();

        // GET: CinemaHallSchedule/5
        public ActionResult Index(int id)
        {
            CinemaHall cinemaHall = cinemaHallDb.CinemaHalls.Find(id);
            if (cinemaHall == null)
            {
                return HttpNotFound();
            }
            CheckCinemaRights(cinemaHall.CinemaID);

            ViewBag.CinemaHallID = id;
            ViewBag.CinemaHallName = cinemaHall.Name;

            DateTime date = DateTime.Now;
            DateTime prevDate = date.AddDays(-1).Date;
            DateTime nextDate = date.AddDays(1).Date;

            ViewBag.Year = date.Year;
            ViewBag.Month = date.Month;
            ViewBag.Day = date.Day;

            ViewBag.PrevDay = date.AddDays(-1).Date;
            ViewBag.NextDay = date.AddDays(1).Date;
            ViewBag.PrevWeek = date.AddDays(-7).Date;
            ViewBag.NextWeek = date.AddDays(7).Date;

            List<CinemaHallScheduleMovie> cinemaHallMovies = cinemaHallMovieDb.GetListByCinemaHallId(id, date.Year, date.Month, date.Day);

            string cinemaHallMoviesHtml = "";
            foreach (CinemaHallScheduleMovie cinemaHallMovie in cinemaHallMovies)
            {
                cinemaHallMoviesHtml += GetScheduleMovieItemHtml(cinemaHallMovie);
            }
            ViewBag.CinemaHallMoviesHtml = cinemaHallMoviesHtml;

            ViewBag.prevMovieEndHour = null;
            ViewBag.prevMovieEndMinute = null;

            CinemaHallScheduleMovie cinemaHallLastMovie = cinemaHallMovieDb.GetLastByCinemaHallId(id, prevDate.Year, prevDate.Month, prevDate.Day);
            if (cinemaHallLastMovie != null)
            {
                DateTime lastMovieEndDate = cinemaHallLastMovie.Date.AddMinutes(cinemaHallLastMovie.Duration);
                if (lastMovieEndDate.Year == date.Year && lastMovieEndDate.Month == date.Month && lastMovieEndDate.Day == date.Day)
                {
                    ViewBag.prevMovieEndHour = lastMovieEndDate.Hour;
                    ViewBag.prevMovieEndMinute = lastMovieEndDate.Minute;
                }
            }

            return View();
        }

        // POST: CinemaHallSchedule/Save
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Save(int id, int year, int month, int day, List<CinemaHallScheduleMovie> movies)
        {
            CinemaHall cinemaHall = cinemaHallDb.CinemaHalls.Find(id);
            if (cinemaHall == null)
            {
                return HttpNotFound();
            }
            CheckCinemaRights(cinemaHall.CinemaID);

            foreach (CinemaHallScheduleMovie scheduleMovie in movies)
            {
                Movie movie = movieDb.Movies.Find(scheduleMovie.MovieID);
                int hour = scheduleMovie.StartMinute / 60;
                int minute = scheduleMovie.StartMinute - hour * 60;
                DateTime date = new DateTime(year, month, day, hour, minute, 0);

                cinemaHallMovieDb.SaveMovie(scheduleMovie.CinemaHallMovieID, id, movie.ID, scheduleMovie.IsRemoved, date);
            }

            return Json("ok");
        }

        public ActionResult GetMovieItemHtml(int id)
        {
            Movie movie = movieDb.Movies.Find(id);
            CinemaMovie cinemaMovie = cinemaMovieDb.Get(Core.GetCinemaId(), id);

            ViewData["CinemaHallMovieID"] = 0;
            ViewData["MovieID"] = movie.ID;
            ViewData["MovieName"] = movie.Name;
            ViewData["Duration"] = movie.Duration;
            ViewData["ItemHeight"] = movie.Duration / 60 * 25;
            ViewData["Price"] = cinemaMovie.Price;

            return PartialView("Movie");
        }

        public string GetScheduleMovieItemHtml(CinemaHallScheduleMovie cinemaHallMovie)
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
                    { "Price", cinemaHallMovie.Price },
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

            DateTime prevDay = date.AddDays(-1);
            DateTime nextDay = date.AddDays(1);
            DateTime prevWeek = date.AddDays(-7);
            DateTime nextWeek = date.AddDays(7);

            List<CinemaHallScheduleMovie> cinemaHallMovies = cinemaHallMovieDb.GetListByCinemaHallId(cinemaHallId, date.Year, date.Month, date.Day);

            string html = "";
            foreach (CinemaHallScheduleMovie cinemaHallMovie in cinemaHallMovies)
            {
                html += GetScheduleMovieItemHtml(cinemaHallMovie);
            }

            return Json(new {
                year = date.Year,
                month = date.Month,
                day = date.Day,
                date = date.Day + "." + date.Month + "." + date.Year,
                prevDay = prevDay.Day + "." + prevDay.Month + "." + prevDay.Year,
                nextDay = nextDay.Day + "." + nextDay.Month + "." + nextDay.Year,
                prevWeek = prevWeek.Day + "." + prevWeek.Month + "." + prevWeek.Year,
                nextWeek = nextWeek.Day + "." + nextWeek.Month + "." + nextWeek.Year,
                html,
            });
        }

        public ActionResult LoadSetPlacesGroupPopupHtml(int cinemaHallPlaceId)
        {
            ViewDataDictionary viewData = new ViewDataDictionary()
            {
                { "CinemaHallPlaceID", cinemaHallPlaceId },
                { "CinemaPlaceGroupID", cinemaPlaceGroupDb.GetSelectList(cinemaPlaceGroupDb.GetCinemaPlaceGroupId(cinemaHallPlaceId)) },
            };

            string cinemaHallPlaceGroupHtml = Core.GetHtmlString("PlaceGroup", viewData, ControllerContext);
            string html = Core.GetHtmlString("Popup", new ViewDataDictionary() {
                { "html", cinemaHallPlaceGroupHtml },
            }, ControllerContext);

            return Json(new
            {
                html,
            });
        }

        public ActionResult SetPlacesGroup(int cinemaHallPlaceId, int cinemaPlaceGroupId)
        {
            CinemaHallPlaceGroup cinemaHallPlaceGroup = cinemaPlaceGroupDb.Get(cinemaHallPlaceId);
            if (cinemaHallPlaceGroup != null)
            {
                cinemaHallPlaceGroup.CinemaPlaceGroupID = cinemaPlaceGroupId;
            }
            else
            {
                cinemaHallPlaceGroup = new CinemaHallPlaceGroup()
                {
                    CinemaHallPlaceID = cinemaHallPlaceId,
                    CinemaPlaceGroupID = cinemaPlaceGroupId,
                };
                cinemaPlaceGroupDb.CinemaHallPlaceGroups.Add(cinemaHallPlaceGroup);
            }
            cinemaPlaceGroupDb.SaveChanges();

            return Json(new
            {
                success = true,
            });
        }

        [HttpPost]
        public ActionResult Copy(int id, int year, int month, int day)
        {
            CinemaHall cinemaHall = cinemaHallDb.CinemaHalls.Find(id);
            if (cinemaHall == null)
            {
                return HttpNotFound();
            }
            CheckCinemaRights(cinemaHall.CinemaID);

            List<CinemaHallScheduleMovie> cinemaHallScheduleMovies = cinemaHallMovieDb.GetListByCinemaHallId(cinemaHall.ID, year, month, day);
            foreach (CinemaHallScheduleMovie cinemaHallScheduleMovie in cinemaHallScheduleMovies)
            {
                CinemaHallMovie cinemaHallMovie = cinemaHallMovieDb.CinemaHallMovies.Find(cinemaHallScheduleMovie.CinemaHallMovieID);
                cinemaHallMovieDb.CinemaHallMovies.Remove(cinemaHallMovie);
            }
            cinemaHallMovieDb.SaveChanges();

            DateTime date = new DateTime(year, month, day).AddDays(-1);
            cinemaHallScheduleMovies = cinemaHallMovieDb.GetListByCinemaHallId(cinemaHall.ID, date.Year, date.Month, date.Day);
            foreach (CinemaHallScheduleMovie cinemaHallScheduleMovie in cinemaHallScheduleMovies)
            {
                date = new DateTime(year, month, day, cinemaHallScheduleMovie.Date.Hour, cinemaHallScheduleMovie.Date.Minute, 0);
                cinemaHallMovieDb.SaveMovie(0, cinemaHall.ID, cinemaHallScheduleMovie.MovieID, false, date);
            }

            return Json("ok");
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
