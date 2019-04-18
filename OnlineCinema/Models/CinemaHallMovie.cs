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
        public DbSet<Movie> Movies { get; set; }

        public List<CinemaHallScheduleMovie> GetList(int year, int month, int day)
        {
            var movies = from chm in CinemaHallMovies
                         join m in Movies on chm.MovieID equals m.ID
                         select new CinemaHallScheduleMovie
                         {
                             Date = chm.Date,
                             MovieName = m.Name,
                         };

            movies = movies.Where(m => m.Date.Year == year)
                .Where(m => m.Date.Month == month)
                .Where(m => m.Date.Day == day);

            return movies.ToList();
        }
    }

    public class CinemaHallScheduleMovie
    {
        // cinema hall movie ID
        public int CinemaHallMovieID { get; set; }
        // cinema movie ID
        public int CinemaHallID { get; set; }
        // movie ID
        public int MovieID { get; set; }
        // movie name
        public string MovieName { get; set; }
        // start minute
        public int StartMinute { get; set; }
        // duration
        public int Duration { get; set; }
        // movie date
        public DateTime Date { get; set; }
        // is removed
        public bool IsRemoved { get; set; }
    }
}