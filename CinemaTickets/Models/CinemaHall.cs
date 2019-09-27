using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CinemaTickets.Models
{
    public class CinemaHall
    {
        // cinema hall ID
        public int ID { get; set; }
        // cinema ID
        public int CinemaID { get; set; }
        // cinema name
        public string Name { get; set; }
    }

    public class CinemaHallContext : DbContext
    {
        public CinemaHallContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<CinemaHall> CinemaHalls { get; set; }

        public IEnumerable<CinemaHall> GetList(int cinemaId)
        {
            var cinemaHalls = from ch in CinemaHalls
                              select ch;

            cinemaHalls = cinemaHalls.Where(c => c.CinemaID == cinemaId);

            return cinemaHalls;
        }
    }
}