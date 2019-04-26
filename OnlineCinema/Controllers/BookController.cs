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
        private CinemaMovieContext cinemaMovieDb = new CinemaMovieContext();
        private CinemaHallMovieContext cinemaHallMovieDb = new CinemaHallMovieContext();
        private CinemaHallPlaceContext cinemaHallPlaceDb = new CinemaHallPlaceContext();
        private CinemaHallMoviePlaceContext cinemaHallMoviePlaceDb = new CinemaHallMoviePlaceContext();
        private OrderContext orderDb = new OrderContext();

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

            CinemaHallPlaceData cinemaHallPlaceData = cinemaHallPlaceDb.GetCinemaHallPlacesData(cinemaHall.ID, cinemaHallMovie.ID);

            ViewBag.maxRow = cinemaHallPlaceData.MaxRow;
            ViewBag.maxCell = cinemaHallPlaceData.MaxCell;
            ViewBag.cinemaHallRows = cinemaHallPlaceData.CinemaHallRows;
            ViewBag.cinemaHallMovieId = cinemaHallMovie.ID;
            ViewBag.price = cinemaHallMovieDb.GetPrice(cinemaHallMovie.ID);

            return View();
        }

        [HttpPost]
        public ActionResult Create(int id, int[] cinemaHallPlaces)
        {
            Order order = new Order()
            {
                Date = DateTime.Now,
            };
            orderDb.Orders.Add(order);
            orderDb.SaveChanges();

            int price = cinemaHallMovieDb.GetPrice(id);

            for (var i = 0; i < cinemaHallPlaces.Length; i++)
            {
                OrderItem orderItem = new OrderItem()
                {
                    OrderID = order.ID,
                    CinemaHallMovieID = id,
                    CinemaHallPlaceID = cinemaHallPlaces[i],
                    Price = price,
                };
                orderDb.OrderItems.Add(orderItem);

                CinemaHallMoviePlace cinemaHallMoviePlace = new CinemaHallMoviePlace()
                {
                    CinemaHallMovieID = id,
                    CinemaHallPlaceID = cinemaHallPlaces[i],
                    Status = CinemaHallMoviePlace.STATUS_PROCESSING,
                };
                cinemaHallMoviePlaceDb.CinemaHallMoviePlaces.Add(cinemaHallMoviePlace);
            }
            orderDb.SaveChanges();
            cinemaHallMoviePlaceDb.SaveChanges();

            return Json(new { id = order.ID });
        }

        public ActionResult Confirm(int id)
        {
            ViewBag.Styles.Add("book");

            Order order = orderDb.Orders.Find(id);

            List<OrderItemInfo> orderItems = orderDb.GetOrderItems(id);

            ViewBag.orderItems = orderItems;
            ViewBag.date = order.Date;
            ViewBag.isPaid = order.IsPaid;
            ViewBag.orderId = order.ID;

            int totalPrice = 0;
            foreach (OrderItemInfo orderItem in orderItems)
            {
                totalPrice += orderItem.Price;
            }

            ViewBag.totalPrice = totalPrice;

            return View();
        }

        public ActionResult Success(int id)
        {
            Order order = orderDb.Orders.Find(id);
            order.IsPaid = true;
            orderDb.SaveChanges();

            List<OrderItemInfo> orderItems = orderDb.GetOrderItems(id);
            foreach (OrderItemInfo orderItem in orderItems)
            {
                CinemaHallMoviePlace cinemaHallMoviePlace = cinemaHallMoviePlaceDb.GetCinemaHallMoviePlace(
                    orderItem.CinemaHallMovieID, orderItem.CinemaHallPlaceID);

                cinemaHallMoviePlace.Status = CinemaHallMoviePlace.STATUS_SUCCESSFULL;
                cinemaHallMoviePlaceDb.SaveChanges();
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Fail(int id)
        {
            List<OrderItemInfo> orderItems = orderDb.GetOrderItems(id);
            foreach (OrderItemInfo orderItem in orderItems)
            {
                CinemaHallMoviePlace cinemaHallMoviePlace = cinemaHallMoviePlaceDb.GetCinemaHallMoviePlace(
                    orderItem.CinemaHallMovieID, orderItem.CinemaHallPlaceID);

                cinemaHallMoviePlaceDb.CinemaHallMoviePlaces.Remove(cinemaHallMoviePlace);
            }
            cinemaHallMoviePlaceDb.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}