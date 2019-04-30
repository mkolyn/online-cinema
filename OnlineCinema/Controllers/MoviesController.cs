using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineCinema.Models;
using PagedList;

namespace OnlineCinema.Controllers
{
    public class MoviesController : AdminController
    {
        private MovieContext db = new MovieContext();
        private CinemaMovieContext cinemaMovieDb = new CinemaMovieContext();
        private GenreContext genreDb = new GenreContext();

        // GET: Movies
        public ActionResult Index(int? page)
        {
            LoginIfNotAuthorized();
            int pageNumber = page ?? 1;
            var cinemaMovies = cinemaMovieDb.GetList(Core.GetCinemaId());
            ViewBag.cinemaId = Core.GetCinemaId();
            return View(cinemaMovies.ToPagedList(pageNumber, Core.PAGE_SIZE));
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
        public ActionResult Create([Bind(Include = "ID,GenreID,Name,Duration,Description")] Movie movie, int price = 0, HttpPostedFileBase image = null)
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

                if (cinemaId > 0)
                {
                    CinemaMovie cinemaMovie = cinemaMovieDb.Get(cinemaId, movieId);

                    if (cinemaMovie == null)
                    {
                        cinemaMovie = new CinemaMovie()
                        {
                            CinemaID = cinemaId,
                            MovieID = movieId,
                            Price = price,
                        };

                        cinemaMovieDb.CinemaMovies.Add(cinemaMovie);
                        cinemaMovieDb.SaveChanges();
                    }
                    else
                    {
                        cinemaMovie.Price = price;
                    }

                    string imageFileName = Core.UploadImage(image, Server.MapPath("~/Images"), cinemaMovie.ID.ToString());
                    if (imageFileName != "")
                    {
                        cinemaMovie.Image = imageFileName;
                    }

                    cinemaMovieDb.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int id)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            CinemaMovie cinemaMovie = cinemaMovieDb.Get(Core.GetCinemaId(), id);
            if (cinemaMovie == null)
            {
                return HttpNotFound();
            }

            ViewBag.GenreID = genreDb.GetSelectList(movie.GenreID);
            ViewBag.Image = cinemaMovie.Image;
            ViewBag.price = cinemaMovie.Price;

            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HttpPostedFileBase image, int price)
        {
            if (ModelState.IsValid)
            {
                CinemaMovie cinemaMovie = cinemaMovieDb.Get(Core.GetCinemaId(), id);

                if (cinemaMovie != null)
                {
                    string imageFileName = Core.UploadImage(image, Server.MapPath("~/Images"), cinemaMovie.ID.ToString());
                    if (imageFileName != "")
                    {
                        Core.RemoveImage(Server.MapPath("~/Images"), cinemaMovie.Image);
                        cinemaMovie.Image = imageFileName;
                    }
                    cinemaMovie.Price = price;
                    cinemaMovieDb.SaveChanges();
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
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            CinemaMovie cinemaMovie = cinemaMovieDb.Get(Core.GetCinemaId(), id);

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
            var movies = cinemaMovieDb.GetList(Core.GetCinemaId(), term);

            List<MovieAutocomplete> movieItems = new List<MovieAutocomplete>();

            foreach (var movie in movies)
            {
                MovieAutocomplete movieItem = new MovieAutocomplete();
                movieItem.label = movie.Name;
                movieItem.value = movie.Name;
                movieItem.id = movie.ID;
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
