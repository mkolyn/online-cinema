using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OnlineCinema.Models
{
    public class CinemaHallPlace
    {
        // cinema hall place ID
        public int ID { get; set; }
        // cinema hall ID
        public int CinemaHallID { get; set; }
        // cinema row
        public int Row { get; set; }
        // cinema cell
        public int Cell { get; set; }
        // cinema row rows
        public int Rows { get; set; }
        // cinema cell cells
        public int Cells { get; set; }
        // is joined place
        private bool IsJoined { get; set; }
        // joined gpoup name
        private string JoinedGroupName { get; set; }
        // is booked
        private bool IsBooked { get; set; }
        // group price
        private int Price { get; set; }

        public bool GetIsJoined()
        {
            return IsJoined;
        }

        public void SetIsJoined(bool IsJoined)
        {
            this.IsJoined = IsJoined;
        }

        public string GetJoinedGroupName()
        {
            return JoinedGroupName;
        }

        public void SetJoinedGroupName(string JoinedGroupName)
        {
            this.JoinedGroupName = JoinedGroupName;
        }

        public bool GetIsBooked()
        {
            return IsBooked;
        }

        public void SetIsBooked(bool IsBooked)
        {
            this.IsBooked = IsBooked;
        }

        public int GetPrice()
        {
            return Price;
        }

        public void SetPrice(int Price)
        {
            this.Price = Price;
        }
    }

    public class CinemaHallPlaceData
    {
        public int MaxRow { get; set; }
        public int MaxCell { get; set; }
        public CinemaHallPlace[,] CinemaHallRows { get; set; }
    }

    public class CinemaHallPlaceContext : DbContext
    {
        public CinemaHallPlaceContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<CinemaHall> CinemaHalls { get; set; }
        private CinemaHallMoviePlaceContext cinemaHallMoviePlaceDb = new CinemaHallMoviePlaceContext();
        private CinemaMovieGroupPriceContext cinemaMovieGroupPriceDb = new CinemaMovieGroupPriceContext();
        private CinemaHallMovieContext cinemaHallMovieDb = new CinemaHallMovieContext();
        private CinemaMovieContext cinemaMovieDb = new CinemaMovieContext();

        public DbSet<CinemaHallPlace> CinemaHallPlaces { get; set; }

        public CinemaHallPlaceData GetCinemaHallPlacesData(int id, int cinemaHallMovieId = 0)
        {
            CinemaHallMovie cinemaHallMovie = cinemaHallMovieDb.CinemaHallMovies.Find(cinemaHallMovieId);
            CinemaHall cinemaHall = CinemaHalls.Find(cinemaHallMovie.CinemaHallID);
            CinemaMovie cinemaMovie = cinemaMovieDb.Get(cinemaHall.CinemaID, cinemaHallMovie.MovieID);

            var cinemaHallPlaces = from chp in CinemaHallPlaces
                                   select chp;

            cinemaHallPlaces = cinemaHallPlaces.Where(s => s.CinemaHallID == id);

            int maxRow = 0;
            int maxCell = 0;
            foreach (CinemaHallPlace cinemaHallPlace in cinemaHallPlaces)
            {
                if (cinemaHallPlace.Row > maxRow)
                {
                    maxRow = cinemaHallPlace.Row;
                }
                if (cinemaHallPlace.Cell > maxCell)
                {
                    maxCell = cinemaHallPlace.Cell;
                }
            }

            CinemaHallPlace[,] cinemaHallRows = new CinemaHallPlace[maxRow, maxCell];
            bool[,] cinemaHallIsJoinedPlaces = new bool[maxRow, maxCell];
            string[,] cinemaHallJoinedPlacesGroupName = new string[maxRow, maxCell];
            bool[,] cinemaHallIsBookedPlaces = new bool[maxRow, maxCell];
            int cinemaHallJoinedPlacesGroupNumber = 0;
            Dictionary<int, int> prices = new Dictionary<int, int>();

            if (cinemaHallMovieId > 0)
            {
                List<CinemaHallMoviePlaceInfo> cinemaHallMoviePlaces = cinemaHallMoviePlaceDb.GetCinemaHallMoviePlaces(cinemaHallMovieId);
                foreach (CinemaHallMoviePlaceInfo cinemaHallMoviePlace in cinemaHallMoviePlaces)
                {
                    cinemaHallIsBookedPlaces[cinemaHallMoviePlace.Row - 1, cinemaHallMoviePlace.Cell - 1] = true;
                }

                prices = cinemaMovieGroupPriceDb.GetGroupPlacePrices(id, cinemaMovie.ID);
            }

            foreach (CinemaHallPlace cinemaHallPlace in cinemaHallPlaces)
            {
                if (cinemaHallPlace.Rows > 1 || cinemaHallPlace.Cells > 1)
                {
                    cinemaHallJoinedPlacesGroupNumber++;
                    for (var i = cinemaHallPlace.Row - 1; i < cinemaHallPlace.Row - 1 + cinemaHallPlace.Rows; i++)
                    {
                        for (var j = cinemaHallPlace.Cell - 1; j < cinemaHallPlace.Cell - 1 + cinemaHallPlace.Cells; j++)
                        {
                            cinemaHallIsJoinedPlaces[i, j] = true;
                            cinemaHallJoinedPlacesGroupName[i, j] = "group" + cinemaHallJoinedPlacesGroupNumber;
                        }
                    }
                }

                if (cinemaHallIsJoinedPlaces[cinemaHallPlace.Row - 1, cinemaHallPlace.Cell - 1] == true)
                {
                    cinemaHallPlace.SetIsJoined(true);
                    cinemaHallPlace.SetJoinedGroupName(cinemaHallJoinedPlacesGroupName[cinemaHallPlace.Row - 1, cinemaHallPlace.Cell - 1]);
                }

                if (cinemaHallIsBookedPlaces[cinemaHallPlace.Row - 1, cinemaHallPlace.Cell - 1] == true)
                {
                    cinemaHallPlace.SetIsBooked(true);
                }

                if (prices.ContainsKey(cinemaHallPlace.ID) && prices[cinemaHallPlace.ID] > 0)
                {
                    cinemaHallPlace.SetPrice(prices[cinemaHallPlace.ID]);
                }

                cinemaHallRows[cinemaHallPlace.Row - 1, cinemaHallPlace.Cell - 1] = cinemaHallPlace;
            }

            return new CinemaHallPlaceData
            {
                MaxRow = maxRow,
                MaxCell = maxCell,
                CinemaHallRows = cinemaHallRows,
            };
        }
    }
}