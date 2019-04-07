using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineCinema.Models
{
    public class User
    {
        // cinema ID
        public int ID { get; set; }
        // first name
        public string FirstName { get; set; }
        // last name
        public string LastName { get; set; }
        // login
        public string Login { get; set; }
        // password
        public string Password { get; set; }
    }

    public class UserContext : DbContext
    {
        public UserContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
    }
}