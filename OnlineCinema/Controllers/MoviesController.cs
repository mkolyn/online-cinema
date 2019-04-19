using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using OnlineCinema.Models;

namespace OnlineCinema.Controllers
{
    public class MoviesController : RunBeforeController
    {
        private MovieContext db = new MovieContext();
        private CinemaMovieContext cinemaMovieDb = new CinemaMovieContext();
        private GenreContext genreDb = new GenreContext();

        // GET: Movies
        public ActionResult Index()
        {
            LoginIfNotAuthorized();
            return View(cinemaMovieDb.GetList(Core.GetCinemaId()));
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            ViewBag.GenreID = genreDb.GetSelectList();
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,GenreID,Name,Duration,Description")] Movie movie, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                int cinemaId = Core.GetCinemaId();
                int movieId = 0;

                var existedMovie = db.Movies.ToList().Where(m => m.Name == movie.Name).FirstOrDefault();
                if (existedMovie == null)
                {
                    db.Movies.Add(movie);
                    db.SaveChanges();
                    existedMovie = db.Movies.ToList().Where(m => m.Name == movie.Name).FirstOrDefault();
                    movieId = existedMovie.ID;
                }
                else
                {
                    movieId = existedMovie.ID;
                }

                CinemaMovie cinemaMovie = cinemaMovieDb.CinemaMovies.ToList()
                    .Where(m => m.CinemaID == cinemaId)
                    .Where(m => m.MovieID == movieId)
                    .FirstOrDefault();

                if (cinemaMovie == null)
                {
                    cinemaMovie = new CinemaMovie()
                    {
                        CinemaID = cinemaId,
                        MovieID = movieId,
                    };

                    cinemaMovieDb.CinemaMovies.Add(cinemaMovie);
                    cinemaMovieDb.SaveChanges();
                }

                string imageFileName = Core.UploadImage(image, Server.MapPath("~/Images"), cinemaMovie.ID.ToString());
                if (imageFileName != "")
                {
                    cinemaMovie.Image = imageFileName;
                    cinemaMovieDb.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenreID = genreDb.GetSelectList(movie.GenreID);

            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                CinemaMovie cinemaMovie = cinemaMovieDb.CinemaMovies.ToList()
                    .Where(m => m.CinemaID == Core.GetCinemaId())
                    .Where(m => m.MovieID == id)
                    .FirstOrDefault();

                if (cinemaMovie != null)
                {
                    string imageFileName = Core.UploadImage(image, Server.MapPath("~/Images"), cinemaMovie.ID.ToString());
                    if (imageFileName != "")
                    {
                        Core.RemoveImage(Server.MapPath("~/Images"), cinemaMovie.Image);
                        cinemaMovie.Image = imageFileName;
                        cinemaMovieDb.SaveChanges();
                    }
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Int32.TryParse(Session["CinemaID"].ToString(), out int cinemaId);

            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            CinemaMovie cinemaMovie = cinemaMovieDb.CinemaMovies.ToList()
                    .Where(m => m.CinemaID == cinemaId)
                    .Where(m => m.MovieID == id)
                    .FirstOrDefault();

            if (cinemaMovie != null)
            {
                cinemaMovieDb.CinemaMovies.Remove(cinemaMovie);
                cinemaMovieDb.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // POST: Movies/Find
        [HttpGet]
        public ActionResult Find(string term)
        {
            Int32.TryParse(Session["CinemaID"].ToString(), out int cinemaId);

            var movies = cinemaMovieDb.GetList(cinemaId, term);

            List<MovieAutocomplete> movieItems = new List<MovieAutocomplete>();

            foreach (var movie in movies)
            {
                MovieAutocomplete movieItem = new MovieAutocomplete();
                movieItem.label = movie.Name;
                movieItem.value = movie.ID.ToString();
                movieItems.Add(movieItem);
            }

            return Json(movieItems, JsonRequestBehavior.AllowGet);
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
