using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OnlineCinema.Models
{
    public class CinemaMovieGroupPrice
    {
        // cinema movie group price ID
        public int ID { get; set; }
        // cinema movie ID
        public int CinemaMovieID { get; set; }
        // cinema place gpoup ID
        public int CinemaPlaceGroupID { get; set; }
        // movie group price
        public int Price { get; set; }
    }

    public class CinemaMovieGroupPriceContext : DbContext
    {
        public CinemaMovieGroupPriceContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<CinemaMovieGroupPrice> CinemaMovieGroupPrices { get; set; }

        public List<CinemaMovieGroupPrice> GetList(int cinemaMovieId)
        {
            var cinemaMovieGroupPrices = from cmgp in CinemaMovieGroupPrices
                                         select cmgp;

            cinemaMovieGroupPrices = cinemaMovieGroupPrices.Where(s => s.CinemaMovieID == cinemaMovieId);

            return cinemaMovieGroupPrices.ToList();
        }

        public CinemaMovieGroupPrice Get(int cinemaMovieId, int cinemaPlaceGroupID)
        {
            var cinemaMovieGroupPrice = from cmgp in CinemaMovieGroupPrices
                                         select cmgp;

            cinemaMovieGroupPrice = cinemaMovieGroupPrice.Where(s => s.CinemaMovieID == cinemaMovieId)
                .Where(s => s.CinemaPlaceGroupID == cinemaPlaceGroupID);

            return cinemaMovieGroupPrice.FirstOrDefault();
        }
    }
}