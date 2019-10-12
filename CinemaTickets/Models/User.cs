using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace CinemaTickets.Models
{
    public class User
    {
        // user ID
        public int ID { get; set; }
        // cinema ID
        public int? CinemaID { get; set; }
        // first name
        public string FirstName { get; set; }
        // last name
        public string LastName { get; set; }
        // login
        public string Login { get; set; }
        // email
        public string Email { get; set; }
        // is email confirmed
        public bool IsEmailConfirmed { get; set; }
        // password
        public string Password { get; set; }
    }

    public class UserContext : DbContext
    {
        public UserContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }

        public User GetByEmail(string email)
        {
            return Users.Where(s => s.Email == email).FirstOrDefault();
        }
    }
}