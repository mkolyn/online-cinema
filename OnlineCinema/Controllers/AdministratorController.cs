using OnlineCinema.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCinema.Controllers
{
    public class AdministratorController : Controller
    {
        private UserContext userDb = new UserContext();

        public ActionResult Index()
        {
            ViewBag.UserId = Session["UserID"] != null ? Session["UserID"].ToString() : "";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                var currentUser = this.userDb.Users.Where(a => a.Login.Equals(user.Login) && a.Password.Equals(user.Password)).FirstOrDefault();
                if (currentUser != null)
                {
                    Session["UserID"] = currentUser.ID.ToString();
                    Session["UserFirstName"] = currentUser.FirstName.ToString();
                    Session["UserLastName"] = currentUser.LastName.ToString();
                }
            }

            return RedirectToAction("Index");
        }
    }
}