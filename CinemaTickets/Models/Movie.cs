using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CinemaTickets.Models
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
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        // image
        public string Image { get; set; }
        // youtube url
        public string YoutubeUrl { get; set; }
        // country
        public string Country { get; set; }
        // year
        public int? Year { get; set; }
        // director
        public string Director { get; set; }
        // cast
        public string Cast { get; set; }
        // budget
        public int? Budget { get; set; }
    }

    public class MovieContext : DbContext
    {
        public MovieContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<Movie> Movies { get; set; }
    }

    public class MovieAutocomplete
    {
        public string label { get; set; }
        public string value { get; set; }
        public int id { get; set; }
    }
}