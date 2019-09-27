using System.Web.Mvc;
using CinemaTickets.Models;
using System.Web.Helpers;

namespace CinemaTickets.Controllers
{
    public class RegistrationController : CoreController
    {
        private UserContext db = new UserContext();

        // GET: Registration
        public ActionResult Index()
        {
            return View();
        }

        // POST: Registration/Index
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "ID,FirstName,LastName,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Password = Crypto.SHA256(user.Password);
                db.Users.Add(user);
                db.SaveChanges();
                string url = Url.Action("ConfirmEmail", "Registration", new { hash = Crypto.SHA256(user.Email) });
                string message = "<a href='" + url + "'>Підтвердити пошту</a>";
                Core.SendEmail(user.Email, "Registration", message);
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult ConfirmEmail(string hash)
        {
            User user = db.GetByHash(hash);
            user.IsEmailConfirmed = true;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
