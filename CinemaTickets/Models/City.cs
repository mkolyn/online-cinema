using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTickets.Models
{
    public class City
    {
        // city ID
        public int ID { get; set; }
        // city name
        public string Name { get; set; }
    }

    public class CityContext : DbContext
    {
        public DbSet<City> Cities { get; set; }

        public CityContext() : base("name=DefaultConnection")
        {
        }

        public IEnumerable<SelectListItem> GetSelectList(int ID = 0)
        {
            return from c in Cities
                   select new SelectListItem
                   {
                       Text = c.Name,
                       Value = c.ID.ToString(),
                       Selected = c.ID == ID
                   };
        }
    }
}