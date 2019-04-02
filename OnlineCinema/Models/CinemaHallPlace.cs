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

        public bool GetIsJoined()
        {
            return IsJoined;
        }

        public void SetIsJoined(bool IsJoined)
        {
            this.IsJoined = IsJoined;
        }
    }

    public class CinemaHallPlaceContext : DbContext
    {
        public CinemaHallPlaceContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<CinemaHall> CinemaHalls { get; set; }

        public System.Data.Entity.DbSet<OnlineCinema.Models.CinemaHallPlace> CinemaHallPlaces { get; set; }
    }
}