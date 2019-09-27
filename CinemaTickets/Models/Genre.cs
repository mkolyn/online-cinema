using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTickets.Models
{
    public class Genre
    {
        // movie ID
        public int ID { get; set; }
        // movie name
        public string Name { get; set; }
    }

    public class GenreContext : DbContext
    {
        public DbSet<Genre> Genres { get; set; }

        public GenreContext() : base("name=DefaultConnection")
        {
        }

        public IEnumerable<SelectListItem> GetSelectList(int ID = 0)
        {
            return from d in Genres
                   select new SelectListItem
                   {
                       Text = d.Name,
                       Value = d.ID.ToString(),
                       Selected = d.ID == ID
                   };
        }
    }
}