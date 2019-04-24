using OnlineCinema.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCinema.Controllers
{
    public class BookController : CoreController
    {
        private UserContext userDb = new UserContext();
        private CinemaHallMovieContext cinemaHallMovieDb = new CinemaHallMovieContext();
        private CinemaHallPlaceContext cinemaHallPlaceDb = new CinemaHallPlaceContext();

        public ActionResult Index(int id)
        {
            ViewBag.Styles.Add("places");
            ViewBag.Scripts.Add("places");

            CinemaHallMovie cinemaHallMovie = cinemaHallMovieDb.CinemaHallMovies.Find(id);
            if (cinemaHallMovie == null)
            {
                return HttpNotFound();
            }

            CinemaHall cinemaHall = cinemaHallMovieDb.CinemaHalls.Find(cinemaHallMovie.CinemaHallID);
            Cinema cinema = cinemaHallMovieDb.Cinemas.Find(cinemaHall.CinemaID);
            Movie movie = cinemaHallMovieDb.Movies.Find(cinemaHallMovie.MovieID);

            DateTime date = cinemaHallMovie.Date;

            ViewBag.cinemaName = cinema.Name;
            ViewBag.cinemaHallName = cinemaHall.Name;
            ViewBag.movieName = movie.Name;
            ViewBag.date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);

            CinemaHallPlaceData cinemaHallPlaceData = cinemaHallPlaceDb.GetCinemaHallPlacesData(cinemaHall.ID);

            ViewBag.maxRow = cinemaHallPlaceData.MaxRow;
            ViewBag.maxCell = cinemaHallPlaceData.MaxCell;
            ViewBag.cinemaHallRows = cinemaHallPlaceData.CinemaHallRows;

            return View();
        }
    }
}