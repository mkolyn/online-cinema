using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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

        public System.Data.Entity.DbSet<OnlineCinema.Models.CinemaHallPlace> CinemaHallPlaces { get; set; }

        public CinemaHallPlaceData GetCinemaHallPlacesData(int id)
        {
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
            int cinemaHallJoinedPlacesGroupNumber = 0;

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