using OnlineCinema.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;

namespace OnlineCinema.Controllers
{
    public class AdministratorController : Controller
    {
        private UserContext userDb = new UserContext();

        public ActionResult Index()
        {
            ViewBag.UserId = Core.GetUserId();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                user.Password = Crypto.SHA256(user.Password);
                var currentUser = this.userDb.Users.Where(a => a.Login.Equals(user.Login))
                    .Where(a => a.Password.Equals(user.Password))
                    .FirstOrDefault();

                if (currentUser != null)
                {
                    Session["UserID"] = currentUser.ID.ToString();
                    Session["CinemaID"] = currentUser.CinemaID.ToString();
                    Session["UserFirstName"] = currentUser.FirstName.ToString();
                    Session["UserLastName"] = currentUser.LastName.ToString();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            if (Session["UserID"] != null)
            {
                Session["UserID"] = null;
                Session["CinemaID"] = null;
                Session["UserFirstName"] = null;
                Session["UserLastName"] = null;
            }

            return RedirectToAction("Index");
        }
    }
}