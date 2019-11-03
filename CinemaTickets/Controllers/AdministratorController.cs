using CinemaTickets.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;

namespace CinemaTickets.Controllers
{
    public class AdministratorController : CoreController
    {
        private UserContext userDb = new UserContext();

        public ActionResult Index()
        {
            int userId = Core.GetUserId();

            ViewBag.UserId = userId;
            ViewBag.SiteDisabled = Config.SITE_DISABLED;
            ViewBag.Styles = new List<string>() { "admin" };
            ViewBag.Scripts = new List<string>() { "core" };

            if (userId > 0)
            {
                User user = userDb.Users.Find(userId);
                ViewBag.Login = user.Login;
            }

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
                    Session["UserFirstName"] = currentUser.FirstName != null ? currentUser.FirstName.ToString() : "";
                    Session["UserLastName"] = currentUser.LastName != null ? currentUser.LastName.ToString() : "";
                }
                else
                {
                    AddMessage("Невірний логін або пароль");
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