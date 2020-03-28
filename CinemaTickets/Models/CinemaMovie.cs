using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CinemaTickets.Models
{
    public class CinemaMovie
    {
        // cinema movie ID
        public int ID { get; set; }
        // cinema ID
        public int CinemaID { get; set; }
        // movie ID
        public int MovieID { get; set; }
        // movie price
        public int Price { get; set; }
        // is 3d
        public bool? Is3D { get; set; }
    }

    public class CinemaMovieSelect
    {
        // movie ID
        public int ID { get; set; }
        // cinema ID
        public int? CinemaID { get; set; }
        // movie name
        public string Name { get; set; }
        // cinema name
        public string CinemaName { get; set; }
        // is 3d
        public bool? Is3D { get; set; }
    }

    public class CinemaMovieContext : DbContext
    {
        public CinemaMovieContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<CinemaMovie> CinemaMovies { get; set; }
        public CinemaMovie CinemaMovie { get; set; }

        public List<CinemaMovieSelect> GetList(int cinemaId, string term = "")
        {
            var cinemaMovies = from m in Movies
                   join cm in CinemaMovies on m.ID equals cm.MovieID into mcm
                   from movies in mcm.DefaultIfEmpty()
                   select new CinemaMovieSelect
                   {
                       ID = m.ID,
                       CinemaID = movies.CinemaID,
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