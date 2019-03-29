using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineCinema.Models
{
    public class Cinema
    {
        // cinema ID
        public int ID { get; set; }
        // cinema name
        public string Name { get; set; }
    }

    public class CinemaContext : DbContext
    {
        public CinemaContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<Cinema> Cinemas { get; set; }
    }
}