using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CinemaTickets.Models;
using PagedList;

namespace CinemaTickets.Controllers
{
    public class MoviesController : AdminController
    {
        private MovieContext db = new MovieContext();
        private CinemaMovieContext cinemaMovieDb = new CinemaMovieContext();
        private GenreContext genreDb = new GenreContext();
        private CinemaPlaceGroupContext cinemaPlaceGroupDb = new CinemaPlaceGroupContext();
        private CinemaMovieGroupPriceContext cinemaMovieGroupPriceDb = new CinemaMovieGroupPriceContext();
        private List<CinemaPlaceGroup> cinemaPlaceGroups = new List<CinemaPlaceGroup>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        // GET: Movies
        public ActionResult Index(int? page, string searchString = "")
        {
            //LoginIfNotAuthorized();

            int pageNumber = page ?? 1;
            var cinemaMovies = cinemaMovieDb.GetList(Core.GetCinemaId(), searchString);

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

        private void PopulateCreateMovieData(int movieID = 0, int genreID = 0)
        {
            if (Core.GetCinemaId() > 0)
            {
                cinemaPlaceGroups = cinemaPlaceGroupDb.GetList(Core.GetCinemaId());
            }

            ViewBag.GenreID = genreDb.GetSelectList(genreID);
            ViewBag.cinemaId = Core.GetCinemaId();
            ViewBag.cinemaPlaceGroups = cinemaPlaceGroups;
            ViewBag.movieID = movieID;
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            PopulateCreateMovieData();
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "GenreID,Name,Duration,Description,Country,Year,Director,Cast,Budget,YoutubeUrl,AgeLimit")] Movie movie = null,
            [Bind(Include = "Is3D")] bool is3D = false,
            [Bind(Include = "Price")] int price = 0,
            [Bind(Include = "Image")] HttpPostedFileBase image = null,
            [Bind(Include = "GroupPrices")] Dictionary<int, int> GroupPrices = null
            )
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

                string imageFileName = Core.UploadImage(image, Server.MapPath("~/Images"), movieId.ToString());
                if (imageFileName != "")
                {
                    existedMovie.Image = imageFileName;
                    db.SaveChanges();
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
                            Is3D = is3D,
                        };

                        cinemaMovieDb.CinemaMovies.Add(cinemaMovie);
                        cinemaMovieDb.SaveChanges();
                    }
                    else
                    {
                        cinemaMovie.Is3D = is3D;
                        cinemaMovie.Price = price;
                    }

                    cinemaMovieDb.SaveChanges();

                    if (GroupPrices != null)
                    {
                        UpdateCinemaMovieGroupPrices(cinemaMovie.ID, GroupPrices);
                    }
                }

                return RedirectToAction("Index");
            }
            else
            {
                AddModelStateErrors(ModelState.Values);
            }

            PopulateCreateMovieData();
            return View("Create", movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int id)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            CinemaMovie cinemaMovie = null;
            Dictionary<int, int> groupPrices = new Dictionary<int, int>();
            int cinemaId = Core.GetCinemaId();
            int moviePrice = 0;
            bool? is3D = false;

            if (cinemaId > 0)
            {
                cinemaMovie = cinemaMovieDb.Get(cinemaId, id);
                if (cinemaMovie == null)
                {
                    return HttpNotFound();
                }

                moviePrice = cinemaMovie.Price;
                is3D = cinemaMovie.Is3D;

                List<CinemaPlaceGroup> cinemaPlaceGroups = new List<CinemaPlaceGroup>();
                cinemaPlaceGroups = cinemaPlaceGroupDb.GetList(cinemaId);
                foreach (CinemaPlaceGroup cinemaPlaceGroup in cinemaPlaceGroups)
                {
                    groupPrices[cinemaPlaceGroup.ID] = 0;
                }

                List<CinemaMovieGroupPrice> cinemaMovieGroupPrices = cinemaMovieGroupPriceDb.GetList(cinemaMovie.ID);
                foreach (CinemaMovieGroupPrice cinemaMovieGroupPrice in cinemaMovieGroupPrices)
                {
                    groupPrices[cinemaMovieGroupPrice.CinemaPlaceGroupID] = cinemaMovieGroupPrice.Price;
                }
            }

            ViewBag.GenreID = genreDb.GetSelectList(movie.GenreID);
            ViewBag.cinemaId = cinemaId;
            ViewBag.Image = movie.Image;
            ViewBag.price = moviePrice;
            ViewBag.cinemaPlaceGroups = cinemaPlaceGroups;
            ViewBag.groupPrices = groupPrices;
            ViewBag.movieID = movie.ID;
            ViewBag.is3D = is3D;

            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,GenreID,Name,Duration,Description,Country,Year,Director,Cast,Budget,YoutubeUrl,AgeLimit")] Movie movie,
            HttpPostedFileBase image, Dictionary<int, int> GroupPrices, int price = 0, bool Is3D = false)
        {
            Movie existedMovie = db.Movies.Find(movie.ID);
            if (existedMovie == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    string imageFileName = Core.UploadImage(image, Server.MapPath("~/Images"), movie.ID.ToString());
                    if (imageFileName != "")
                    {
                        if (existedMovie.Image != "")
                        {
                            Core.RemoveImage(Server.MapPath("~/Images"), existedMovie.Image);
                        }
                        movie.Image = imageFileName;
                    }
                }
                else
                {
                    movie.Image = existedMovie.Image;
                }

                //existedMovie = null;
                db.Entry(existedMovie).State = EntityState.Detached;
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();

                CinemaMovie cinemaMovie = cinemaMovieDb.Get(Core.GetCinemaId(), movie.ID);

                if (cinemaMovie != null)
                {
                    cinemaMovie.Price = price;
                    cinemaMovie.Is3D = Is3D != null;
                    cinemaMovieDb.SaveChanges();

                    UpdateCinemaMovieGroupPrices(cinemaMovie.ID, GroupPrices);
                }

                return RedirectToRoute("Administrator", new { Action = "Index", Controller = "Movies" });
            }
            else
            {
                AddModelStateErrors(ModelState.Values);
            }

            PopulateCreateMovieData(movie.ID, movie.GenreID);
            return View("Edit", movie);
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

            if (Core.GetCinemaId() == 0)
            {
                Core.RemoveImage(Server.MapPath("~/Images"), movie.Image);
                db.Movies.Remove(movie);
                db.SaveChanges();
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

        public void UpdateCinemaMovieGroupPrices(int cinemaMovieId, Dictionary<int, int> groupPrices)
        {
            foreach (var groupPrice in groupPrices as Dictionary<int, int>)
            {
                if (groupPrice.Key == 0 || groupPrice.Value == 0)
                {
                    continue;
                }
                CinemaMovieGroupPrice cinemaMovieGroupPrice = cinemaMovieGroupPriceDb.Get(cinemaMovieId, groupPrice.Key);
                if (cinemaMovieGroupPrice != null)
                {
                    cinemaMovieGroupPrice.Price = groupPrice.Value;
                }
                else
                {
                    cinemaMovieGroupPrice = new CinemaMovieGroupPrice()
                    {
                        CinemaMovieID = cinemaMovieId,
                        CinemaPlaceGroupID = groupPrice.Key,
                        Price = groupPrice.Value,
                    };
                    cinemaMovieGroupPriceDb.CinemaMovieGroupPrices.Add(cinemaMovieGroupPrice);
                }
            }

            cinemaMovieGroupPriceDb.SaveChanges();
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
