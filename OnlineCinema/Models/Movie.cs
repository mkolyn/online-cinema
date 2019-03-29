using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineCinema.Models
{
    public class Movie
    {
        // movie ID
        public int ID { get; set; }
        // genre ID
        public int GenreID { get; set; }
        // movie name
        public string Name { get; set; }
        // movie duration
        public int Duration { get; set; }
        // movie description
        public string Description { get; set; }
    }

    public class MovieContext : DbContext
    {
        public MovieContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<Movie> Movies { get; set; }
    }
}