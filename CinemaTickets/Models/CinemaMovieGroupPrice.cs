using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CinemaTickets.Models
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

    public class CinemaHallPlacePrice
    {
        public int ID;
        public int Price;
    }

    public class CinemaMovieGroupPriceContext : DbContext
    {
        public CinemaMovieGroupPriceContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<CinemaMovieGroupPrice> CinemaMovieGroupPrices { get; set; }
        public DbSet<CinemaHall> CinemaHalls { get; set; }
        public DbSet<CinemaHallPlace> CinemaHallPlaces { get; set; }
        public DbSet<CinemaHallPlaceGroup> CinemaHallPlaceGroups { get; set; }

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

        public Dictionary<int, int> GetGroupPlacePrices(int cinemaHallId, int cinemaMovieId)
        {
            var cinemaHallPlacePriceQuery = from ch in CinemaHalls
                                            join chp in CinemaHallPlaces on ch.ID equals chp.CinemaHallID
                                            join chpg in CinemaHallPlaceGroups on chp.ID equals chpg.CinemaHallPlaceID
                                            join cmgp in CinemaMovieGroupPrices on chpg.CinemaPlaceGroupID equals cmgp.CinemaPlaceGroupID
                                            where cmgp.CinemaMovieID == cinemaMovieId && ch.ID == cinemaHallId
                                            select new CinemaHallPlacePrice
                                            {
                                                ID = chp.ID,
                                                Price = cmgp.Price,
                                            };

            List<CinemaHallPlacePrice> cinemaHallPlacePrices = cinemaHallPlacePriceQuery.ToList();
            Dictionary<int, int> prices = new Dictionary<int, int>();
            foreach (CinemaHallPlacePrice cinemaHallPlacePrice in cinemaHallPlacePrices)
            {
                prices.Add(cinemaHallPlacePrice.ID, cinemaHallPlacePrice.Price);
            }

            return prices;
        }
    }
}