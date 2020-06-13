using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CinemaTickets.Models
{
    public class Messages
    {
        public const string ORDER_HAS_BEEN_CHANGED = "Були зроблені зміни у бронюванні";
        public const string ORDER_IS_BEING_PROCESSED = "Ви вже перейшли на сторінку оплати";
        public const string ORDER_HAS_BEEN_BOOKED = "Місця вже були заброньовані";
        public const string CINEMA_PLACE_GROUP_HAS_MOVIES = "Існують фільми, які використовують дану групу";
        public const string CINEMA_HALL_HAS_MOVIES = "Існують фільми, які були додані в даний зал";
        public const string CINEMA_HALL_MOVIE_HAS_MOVIES = "Даний фільм був доданий в кінотеатр(и)";
    }
}