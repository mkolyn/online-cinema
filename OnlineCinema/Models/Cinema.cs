using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCinema.Models
{
    public class Cinema
    {
        // cinema ID
        public int ID { get; set; }
        // city ID
        public int CityID { get; set; }
        // cinema name
        public string Name { get; set; }
    }

    public class CinemaContext : DbContext
    {
        public CinemaContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<Cinema> Cinemas { get; set; }

        public IEnumerable<SelectListItem> GetSelectList(int ID = 0, int cityID = 0)
        {
            return from d in Cinemas
                   where d.CityID == cityID || cityID == 0
                   select new SelectListItem
                   {
                       Text = d.Name,
                       Value = d.ID.ToString(),
                       Selected = d.ID == ID,
                   };
        }

        public List<Cinema> GetList(string searchString, int cityId)
        {
            var cinemas = from c in Cinemas
                          select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                cinemas = cinemas.Where(s => s.Name.Contains(searchString));
            }

            if (cityId > 0)
            {
                cinemas = cinemas.Where(s => s.CityID == cityId);
            }

            return cinemas.ToList();
        }
    }
}