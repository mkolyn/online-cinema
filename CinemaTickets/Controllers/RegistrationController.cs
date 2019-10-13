using System.Web.Mvc;
using CinemaTickets.Models;
using System.Web.Helpers;

namespace CinemaTickets.Controllers
{
    public class RegistrationController : BaseController
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
                if (db.GetByEmail(user.Email) == null)
                {
                    user.Password = Crypto.SHA256(user.Password);
                    db.Users.Add(user);
                    db.SaveChanges();
                    string url = GetUrl("Registration/ConfirmEmail?email=" + user.Email + "&hash=" + Crypto.SHA256(user.Email));
                    string message = "<a href='" + url + "'>Підтвердити пошту</a>";
                    Core.SendEmail(user.Email, "Registration", message);
                    AddMessage("Для підтвердження реєстрації перейдіть по посиланню, що було відправлено на пошту");
                    return RedirectToRoute("Default", new { controller = "Registration", action = "Index" });
                }
                else
                {
                    AddMessage("Користувач вже зареєстрований");
                }
            }

            return View(user);
        }

        public ActionResult ConfirmEmail(string email, string hash)
        {
            User user = db.GetByEmail(email);
            if (user != null)
            {
                if (hash == Crypto.SHA256(user.Email))
                {
                    user.IsEmailConfirmed = true;
                    db.SaveChanges();
                    AddMessage("Пошта була успішно підтверджена");
                }
                else
                {
                    AddMessage("Сталася помилка під час підтвердження пошти");
                }
            }
            else
            {
                AddMessage("Сталася помилка під час підтвердження пошти");
            }

            return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
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
