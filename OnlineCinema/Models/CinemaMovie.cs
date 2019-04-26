using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineCinema.Models
{
    public class CinemaMovie
    {
        // cinema movie ID
        public int ID { get; set; }
        // cinema ID
        public int CinemaID { get; set; }
        // movie ID
        public int MovieID { get; set; }
        // image
        public string Image { get; set; }
        // movie price
        public int Price { get; set; }
    }

    public class CinemaMovieSelect
    {
        // movie ID
        public int ID { get; set; }
        // cinema ID
        public int CinemaID { get; set; }
        // movie name
        public string Name { get; set; }
    }

    public class CinemaMovieContext : DbContext
    {
        public CinemaMovieContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<CinemaMovie> CinemaMovies { get; set; }
        public CinemaMovie CinemaMovie { get; set; }

        public IEnumerable<CinemaMovieSelect> GetList(int cinemaId, string term = "")
        {
            var cinemaMovies = from cm in CinemaMovies
                   join m in Movies on cm.MovieID equals m.ID
                   select new CinemaMovieSelect
                   {
                       ID = m.ID,
                       CinemaID = cm.CinemaID,
                       Name = m.Name,
                   };

            if (cinemaId > 0)
            {
                cinemaMovies = cinemaMovies.Where(c => c.CinemaID == cinemaId);
            }
            if (term != "")
            {
                cinemaMovies = cinemaMovies.Where(s => s.Name.ToString().Contains(term));
            }

            return cinemaMovies.ToList();
        }

        public CinemaMovie Get(int cinemaId, int movieId)
        {
            CinemaMovie cinemaMovie = CinemaMovies.Where(m => m.CinemaID == cinemaId)
                    .Where(m => m.MovieID == movieId)
                    .FirstOrDefault();

            return cinemaMovie;
        }
    }
}