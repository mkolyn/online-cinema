using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CinemaTickets.Models
{
    public class CinemaHallMoviePlace
    {
        public const int STATUS_PROCESSING = 0;
        public const int STATUS_SUCCESSFULL = 1;
        public const int STATUS_FAILED = -1;
        // cinema hall movie place ID
        public int ID { get; set; }
        // cinema hall movie ID
        public int CinemaHallMovieID { get; set; }
        // cinema hall place ID
        public int CinemaHallPlaceID { get; set; }
        // cinema hall movie place status
        public int Status { get; set; }
    }

    public class CinemaHallMoviePlaceInfo
    {
        // cinema hall movie row
        public int Row;
        // cinema hall movie cell
        public int Cell;
        // cinema hall movie status
        public int Status;
    }

    public class CinemaHallMoviePlaceContext : DbContext
    {
        public CinemaHallMoviePlaceContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<CinemaHallMoviePlace> CinemaHallMoviePlaces { get; set; }
        public DbSet<CinemaHallPlace> CinemaHallPlaces { get; set; }

        public CinemaHallMoviePlace GetCinemaHallMoviePlace(int cinemaHallMovieID, int cinemaHallPlaceID)
        {
            return CinemaHallMoviePlaces.Where(s => s.CinemaHallMovieID == cinemaHallMovieID)
                .Where(s => s.CinemaHallPlaceID == cinemaHallPlaceID)
                .FirstOrDefault();
        }

        public List<CinemaHallMoviePlaceInfo> GetCinemaHallMoviePlaces(int cinemaHallMovieID)
        {
            var cinemaHallMoviePlaces = from chmp in CinemaHallMoviePlaces
                                        join chp in CinemaHallPlaces on chmp.CinemaHallPlaceID equals chp.ID
                                        where chmp.CinemaHallMovieID == cinemaHallMovieID
                                        select new CinemaHallMoviePlaceInfo
                                        {
                                            Row = chp.Row,
                                            Cell = chp.Cell,
                                            Status = chmp.Status,
                                        };

            return cinemaHallMoviePlaces.ToList();
        }
    }
}