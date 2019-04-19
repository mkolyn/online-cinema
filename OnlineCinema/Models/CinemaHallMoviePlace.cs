using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineCinema.Models
{
    public class CinemaHallMoviePlace
    {
        // cinema hall movie place ID
        public int ID { get; set; }
        // cinema hall movie ID
        public int CinemaHallMovieID { get; set; }
        // cinema hall place ID
        public int CinemaHallPlaceID { get; set; }
    }

    public class CinemaHallMoviePlaceContext : DbContext
    {
        public CinemaHallMoviePlaceContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<CinemaHallMoviePlace> CinemaHallMoviePlaces { get; set; }
    }
}