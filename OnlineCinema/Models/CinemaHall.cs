using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineCinema.Models
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
    }
}