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
    }
}