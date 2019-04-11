using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineCinema.Models
{
    public class CinemaHallMovie
    {
        // cinema hall place ID
        public int ID { get; set; }
        // cinema hall ID
        public int CinemaHallID { get; set; }
        // movie ID
        public int MovieID { get; set; }
        // movie date
        public DateTime Date { get; set; }
    }

    public class CinemaHallMovieContext : DbContext
    {
        public CinemaHallMovieContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<CinemaHallMovie> CinemaHallMovies { get; set; }
    }

    public class CinemaHallScheduleMovie
    {
        // movie ID
        public int ID { get; set; }
        // start minute
        public int StartMinute { get; set; }
    }
}