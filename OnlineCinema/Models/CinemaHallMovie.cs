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
        public DbSet<CinemaMovie> CinemaMovies { get; set; }
        public DbSet<CinemaHall> CinemaHalls { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }

        public List<CinemaHallScheduleMovie> GetList(int year, int month, int day)
        {
            var movies = from chm in CinemaHallMovies
                         join cm in CinemaMovies on chm.MovieID equals cm.MovieID
                         join ch in CinemaHalls on chm.CinemaHallID equals ch.ID
                         join c in Cinemas on ch.CinemaID equals c.ID
                         join m in Movies on chm.MovieID equals m.ID
                         where cm.CinemaID == ch.CinemaID
                         select new CinemaHallScheduleMovie
                         {
                             Date = chm.Date,
                             MovieName = m.Name,
                             Duration = m.Duration,
                             CityID = c.CityID,
                             Image = cm.Image,
                         };

            int cityId = Core.GetCityId();

            movies = movies.Where(m => m.CityID == cityId)
                .Where(m => m.Date.Year == year)
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
        // city id
        public int CityID { get; set; }
        // movie ID
        public int MovieID { get; set; }
        // movie name
        public string MovieName { get; set; }
        // movie image
        public string Image { get; set; }
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