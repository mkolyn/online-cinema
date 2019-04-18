using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCinema.Models
{
    public class Core
    {
        const int nextDays = 10;

        public static int GetCinemaId()
        {
            var session = HttpContext.Current.Session;
            if (session["CinemaID"] != null && session["CinemaID"].ToString() != "")
            {
                Int32.TryParse(session["CinemaID"].ToString(), out int cinemaId);
                return cinemaId;
            }
            return 0;
        }

        public static int GetUserId()
        {
            var session = HttpContext.Current.Session;
            if (session["UserID"] != null && session["UserID"].ToString() != "")
            {
                Int32.TryParse(session["UserID"].ToString(), out int userId);
                return userId;
            }
            return 0;
        }

        public static int GetCityId()
        {
            var session = HttpContext.Current.Session;
            if (session["CityID"] != null && session["CityID"].ToString() != "")
            {
                Int32.TryParse(session["CityID"].ToString(), out int cityId);
                return cityId;
            }
            return 0;
        }

        public static List<DateTime> GetNextDates(DateTime date)
        {
            List<DateTime> dates = new List<DateTime>();
            dates.Add(date);

            for (var i = 0; i < nextDays; i++)
            {
                date = date.AddDays(1);
                dates.Add(date);
            }

            return dates;
        }
    }
}