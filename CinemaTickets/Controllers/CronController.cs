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
    public class CronController : Controller
    {
        private UserContext userDb = new UserContext();
        private CinemaHallMovieContext cinemaHallMovieDb = new CinemaHallMovieContext();
        private CinemaContext cinemaDb = new CinemaContext();
        private GenreContext genreDb = new GenreContext();
        private OrderContext orderDb = new OrderContext();
        private CinemaHallMoviePlaceContext cinemaHallMoviePlaceDb = new CinemaHallMoviePlaceContext();

        public void RemoveUnpaidPlaces()
        {
            List<OrderItemInfo> orderItems = orderDb.GetUnpaidPlaces();

            foreach (OrderItemInfo orderItem in orderItems)
            {
                CinemaHallMoviePlace cinemaHallMoviePlace = cinemaHallMoviePlaceDb.GetCinemaHallMoviePlace(orderItem.CinemaHallMovieID, orderItem.CinemaHallPlaceID);
                cinemaHallMoviePlace.Status = CinemaHallMoviePlace.STATUS_FAILED;
                cinemaHallMoviePlaceDb.SaveChanges();
            }
        }
    }
}