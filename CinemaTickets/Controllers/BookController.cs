using CinemaTickets.Models;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;

namespace CinemaTickets.Controllers
{
    public class BookController : BaseController
    {
        private UserContext userDb = new UserContext();
        private GenreContext genreDb = new GenreContext();
        private CityContext cityDb = new CityContext();
        private CinemaMovieContext cinemaMovieDb = new CinemaMovieContext();
        private CinemaHallMovieContext cinemaHallMovieDb = new CinemaHallMovieContext();
        private CinemaHallPlaceContext cinemaHallPlaceDb = new CinemaHallPlaceContext();
        private CinemaHallMoviePlaceContext cinemaHallMoviePlaceDb = new CinemaHallMoviePlaceContext();
        private OrderContext orderDb = new OrderContext();
        private CinemaMovieGroupPriceContext cinemaMovieGroupPriceDb = new CinemaMovieGroupPriceContext();
        private CinemaPlaceGroupContext cinemaPlaceGroupDb = new CinemaPlaceGroupContext();

        public ActionResult Index(int id)
        {
            //ViewBag.Styles.Add("places");
            ViewBag.Scripts.Add("places");
            ViewBag.Styles.Add("book");

            CinemaHallMovie cinemaHallMovie = cinemaHallMovieDb.CinemaHallMovies.Find(id);
            if (cinemaHallMovie == null)
            {
                return HttpNotFound();
            }

            CinemaHall cinemaHall = cinemaHallMovieDb.CinemaHalls.Find(cinemaHallMovie.CinemaHallID);
            Cinema cinema = cinemaHallMovieDb.Cinemas.Find(cinemaHall.CinemaID);
            Movie movie = cinemaHallMovieDb.Movies.Find(cinemaHallMovie.MovieID);
            Genre genre = genreDb.Genres.Find(movie.GenreID);
            CinemaMovie cinemaMovie = cinemaMovieDb.Get(cinema.ID, movie.ID);
            City city = cityDb.Cities.Find(Core.GetCityId());

            DateTime date = cinemaHallMovie.Date;

            ViewBag.cityName = city.Name;
            ViewBag.cinemaName = cinema.Name;
            ViewBag.cinemaHallName = cinemaHall.Name;
            ViewBag.movieName = movie.Name;
            ViewBag.genreName = genre.Name;
            ViewBag.dayName = Core.GetShortDayList()[date.DayOfWeek.ToString()];
            ViewBag.formattedDate = Core.GetFormatedDate(new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0));
            ViewBag.formattedTime = Core.GetFormatedTime(new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0));
            ViewBag.Duration = movie.Duration;
            ViewBag.is3D = cinemaMovie.Is3D;

            CinemaHallPlaceData cinemaHallPlaceData = cinemaHallPlaceDb.GetCinemaHallPlacesData(cinemaHall.ID, cinemaHallMovie.ID);

            ViewBag.maxRow = cinemaHallPlaceData.MaxRow;
            ViewBag.maxCell = cinemaHallPlaceData.MaxCell;
            ViewBag.cinemaHallRows = cinemaHallPlaceData.CinemaHallRows;
            ViewBag.cinemaHallMovieId = cinemaHallMovie.ID;
            ViewBag.price = cinemaHallMovieDb.GetPrice(cinemaHallMovie.ID);
            ViewBag.isPlacesFromScreen = cinemaHall.IsPlacesFromScreen;

            return View();
        }

        [HttpPost]
        public ActionResult UpdatePlaces(int id, int[] cinemaHallPlaces)
        {
            CinemaHallMovie cinemaHallMovie = cinemaHallMovieDb.CinemaHallMovies.Find(id);
            CinemaHall cinemaHall = cinemaHallMovieDb.CinemaHalls.Find(cinemaHallMovie.CinemaHallID);
            CinemaHallPlaceData cinemaHallPlaceData = cinemaHallPlaceDb.GetCinemaHallPlacesData(cinemaHall.ID, cinemaHallMovie.ID, cinemaHallPlaces);

            ViewData["isPlacesFromScreen"] = cinemaHall.IsPlacesFromScreen;
            ViewData["maxRow"] = cinemaHallPlaceData.MaxRow;
            ViewData["maxCell"] = cinemaHallPlaceData.MaxCell;
            ViewData["cinemaHallRows"] = cinemaHallPlaceData.CinemaHallRows;

            return PartialView("../CinemaHallPlaces/Places");
        }

        [HttpPost]
        public ActionResult Create(int id, int[] cinemaHallPlaces)
        {
            bool success = true;
            bool canCreateOrder = true;
            string message = "";
            int orderId = 0;

            Order order = new Order()
            {
                Date = DateTime.Now,
            };
            orderDb.Orders.Add(order);
            orderDb.SaveChanges();
            orderId = order.ID;

            int price = cinemaHallMovieDb.GetPrice(id);
            CinemaHallMovie cinemaHallMovie = cinemaHallMovieDb.CinemaHallMovies.Find(id);
            CinemaHall cinemaHall = cinemaHallMovieDb.CinemaHalls.Find(cinemaHallMovie.CinemaHallID);
            CinemaMovie cinemaMovie = cinemaMovieDb.Get(cinemaHall.CinemaID, cinemaHallMovie.MovieID);

            for (var i = 0; i < cinemaHallPlaces.Length; i++)
            {
                // check if places were booked before
                int cinemaHallPlaceId = cinemaHallPlaces[i];
                CinemaHallMoviePlace cinemaHallMoviePlace = cinemaHallMoviePlaceDb.GetCinemaHallMoviePlace(id, cinemaHallPlaceId);
                if (cinemaHallMoviePlace == null)
                {
                    int cinemaPlaceGroupID = cinemaPlaceGroupDb.GetCinemaPlaceGroupId(cinemaHallPlaceId);
                    CinemaMovieGroupPrice cinemaMovieGroupPrice = cinemaMovieGroupPriceDb.Get(cinemaMovie.ID, cinemaPlaceGroupID);
                    int orderItemPrice = cinemaMovieGroupPrice != null ? cinemaMovieGroupPrice.Price : price;

                    OrderItem orderItem = new OrderItem()
                    {
                        OrderID = orderId,
                        CinemaHallMovieID = id,
                        CinemaHallPlaceID = cinemaHallPlaceId,
                        Price = orderItemPrice,
                    };
                    orderDb.OrderItems.Add(orderItem);

                    cinemaHallMoviePlace = new CinemaHallMoviePlace()
                    {
                        CinemaHallMovieID = id,
                        CinemaHallPlaceID = cinemaHallPlaceId,
                        Status = CinemaHallMoviePlace.STATUS_PROCESSING,
                    };
                    cinemaHallMoviePlaceDb.CinemaHallMoviePlaces.Add(cinemaHallMoviePlace);
                }
                else
                {
                    canCreateOrder = false;
                }

                if (!canCreateOrder)
                {
                    break;
                }
            }

            if (canCreateOrder)
            {
                cinemaHallMoviePlaceDb.SaveChanges();
            }
            else
            {
                orderDb.OrderItems = null;
                orderDb.Orders.Remove(order);
                orderId = 0;
                success = false;
                message = Messages.ORDER_HAS_BEEN_BOOKED;
            }
            orderDb.SaveChanges();

            return Json(new { success = success, message = message, id = orderId });
        }

        public ActionResult Confirm(int id)
        {
            ViewBag.Styles.Add("book");
            ViewBag.Scripts.Add("book");

            Order order = orderDb.Orders.Find(id);
            string expiredDate = order.Date.AddMinutes(Config.CONFIRM_PAYMENT_MINUTES_TIMEOUT).ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");

            List<OrderItemInfo> orderItems = orderDb.GetOrderItems(id);
            OrderItemInfo orderInfo = orderItems.First();
            CinemaMovie cinemaMovie = cinemaMovieDb.Get(orderInfo.CinemaID, orderInfo.MovieID);
            Cinema cinema = cinemaHallMovieDb.Cinemas.Find(orderInfo.CinemaID);

            ViewBag.orderItems = orderItems;
            ViewBag.formattedDate = Core.GetFormatedDate(order.Date);
            ViewBag.formattedDay = Core.GetFormatedDay(order.Date);
            ViewBag.formattedTime = Core.GetFormatedTime(order.Date);
            ViewBag.isPaid = order.IsPaid;
            ViewBag.orderId = order.ID;

            ViewBag.movieName = orderInfo.MovieName;
            ViewBag.cinemaName = orderInfo.CinemaName;
            ViewBag.dayName = Core.GetDayList()[order.Date.DayOfWeek.ToString()];
            ViewBag.movieImage = orderInfo.MovieImage;
            ViewBag.movieAgeLimit = orderInfo.MovieAgeLimit;
            ViewBag.is3D = cinemaMovie.Is3D;

            DateTime confirmToDate = order.Date.AddMinutes(Config.CONFIRM_PAYMENT_MINUTES_TIMEOUT);
            ViewBag.ScriptTexts.Add("var YEAR = " + confirmToDate.Year + ";");
            ViewBag.ScriptTexts.Add("var MONTH = " + confirmToDate.Month + ";");
            ViewBag.ScriptTexts.Add("var DAY = " + confirmToDate.Day + ";");
            ViewBag.ScriptTexts.Add("var HOUR = " + confirmToDate.Hour + ";");
            ViewBag.ScriptTexts.Add("var MINUTE = " + confirmToDate.Minute + ";");
            ViewBag.ScriptTexts.Add("var SECOND = " + confirmToDate.Second + ";");

            ViewBag.canConfirmBooking = confirmToDate.CompareTo(DateTime.Now) > 0;

            int totalPrice = 0;
            foreach (OrderItemInfo orderItem in orderItems)
            {
                totalPrice += orderItem.Price;
            }
            string description = "Оплата квитка (квитків) на фільм." + "\n";
            description += orderDb.GetOrderItemDetailsGrouped(orderItems);

            ApiData apiData = new ApiData
            {
                publicKey = cinema.LiqpayPublicKey,
                privateKey = cinema.LiqpayPrivateKey,
                isTestPayment = cinema.IsTestPayment,
            };
            Liqpay liqpay = new Liqpay(totalPrice, id.ToString(), description, GetUrl("Liqpay/Result"), GetUrl("Home/Thankyou"), expiredDate, apiData);
            string liqpayData = Core.Base64Encode(Core.ToJson(liqpay.GetData()));
            string liqpaySignature = Liqpay.GetSignature(cinema.LiqpayPrivateKey + liqpayData + cinema.LiqpayPrivateKey);

            ViewBag.totalPrice = totalPrice;
            ViewBag.liqpayData = liqpayData;
            ViewBag.liqpaySignature = liqpaySignature;

            if (Config.DEBUG)
            {
                AddDebugInfo("liqpay json data: " + Core.ToJson(liqpay.GetData()) + ", liqpay data: " + liqpayData + ", liqpay signature: " + liqpaySignature);
            }

            return View();
        }

        public ActionResult RemovePlace(int id)
        {
            OrderItem orderItem = orderDb.OrderItems.Find(id);
            int orderId = orderItem.OrderID;
            orderDb.OrderItems.Remove(orderItem);
            orderDb.SaveChanges();

            CinemaHallMoviePlace cinemaHallMoviePlace = cinemaHallMoviePlaceDb.GetCinemaHallMoviePlace(orderItem.CinemaHallMovieID, orderItem.CinemaHallPlaceID);
            cinemaHallMoviePlaceDb.CinemaHallMoviePlaces.Remove(cinemaHallMoviePlace);
            cinemaHallMoviePlaceDb.SaveChanges();

            if (orderDb.OrderItems.Count() == 0)
            {
                orderDb.Orders.Remove(orderDb.Orders.Find(orderId));
            }

            return RedirectToRoute("Default", new { Controller = "Book", Action = "Confirm", Id = orderItem.OrderID });
        }

        public ActionResult CheckOrder(int id, int[] orderItemIds)
        {
            string message = "";
            bool success = true;

            // if order item was removed
            orderItemIds = orderItemIds.Distinct().ToArray();
            if (orderDb.GetOrderItemsByIds(orderItemIds).Count != orderItemIds.Length)
            {
                success = false;
                message = Messages.ORDER_HAS_BEEN_CHANGED;
            }
            else
            {
                Order order = orderDb.Orders.Find(id);
                if (order.IsProcessing)
                {
                    success = false;
                    message = Messages.ORDER_IS_BEING_PROCESSED;
                }
            }

            return Json(new { success = success, message = message });
        }

        public ActionResult Success(int id)
        {
            orderDb.SetSuccessfullOrder(id);

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Fail(int id)
        {
            orderDb.SetFailedOrder(id);

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetQRCode(int id)
        {
            string qrCodeMessage = orderDb.GetOrderItemDetails(id);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeMessage, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(Config.QR_CODE_SIZE);
            return File(Core.BitmapToBytes(qrCodeImage), "image/jpeg");
        }

        [HttpPost]
        public ActionResult UpdateOrder(int id, string email)
        {
            Order order = orderDb.Orders.Find(id);
            order.Email = email;
            order.IsProcessing = true;
            orderDb.SaveChanges();

            return Json("ok");
        }
    }
}