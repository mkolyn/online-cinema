using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineCinema.Models;

namespace OnlineCinema.Controllers
{
    public class CinemasController : Controller
    {
        private CinemaContext db = new CinemaContext();

        public void LoginIfNotAuthorized()
        {
            if (Session["UserID"] == null || Session["UserID"].ToString() == "")
            {
                Response.Redirect("administrator/Home");
            }
        }

        // GET: Cinemas
        public ActionResult Index(string searchString)
        {
            LoginIfNotAuthorized();
            var cinemas = from c in db.Cinemas
                         select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                cinemas = cinemas.Where(s => s.Name.Contains(searchString));
            }

            return View(cinemas);
        }

        // GET: Cinemas/Details/5
        public ActionResult Details(int? id)
        {
            LoginIfNotAuthorized();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = db.Cinemas.Find(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            return View(cinema);
        }

        // GET: Cinemas/Create
        public ActionResult Create()
        {
            LoginIfNotAuthorized();
            return View();
        }

        // POST: Cinemas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Cinema cinema)
        {
            LoginIfNotAuthorized();
            if (ModelState.IsValid)
            {
                db.Cinemas.Add(cinema);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cinema);
        }

        // GET: Cinemas/Edit/5
        public ActionResult Edit(int? id)
        {
            LoginIfNotAuthorized();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = db.Cinemas.Find(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            return View(cinema);
        }

        // POST: Cinemas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Cinema cinema)
        {
            LoginIfNotAuthorized();
            if (ModelState.IsValid)
            {
                db.Entry(cinema).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cinema);
        }

        // GET: Cinemas/Delete/5
        public ActionResult Delete(int? id)
        {
            LoginIfNotAuthorized();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = db.Cinemas.Find(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            return View(cinema);
        }

        // POST: Cinemas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoginIfNotAuthorized();
            Cinema cinema = db.Cinemas.Find(id);
            db.Cinemas.Remove(cinema);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Cinemas/Halls/5
        public ActionResult Halls(int? id)
        {
            LoginIfNotAuthorized();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = db.Cinemas.Find(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }

            CinemaHallContext cinemaHallDb = new CinemaHallContext();
            var cinemaHalls = from ch in cinemaHallDb.CinemaHalls
                          select ch;

            cinemaHalls = cinemaHalls.Where(s => s.CinemaID == id);

            ViewBag.CinemaID = id;

            return View(cinemaHalls);
        }

        // GET: Cinemas/Movies/5
        public ActionResult Movies(int? id)
        {
            LoginIfNotAuthorized();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = db.Cinemas.Find(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }

            CinemaHallContext cinemaHallDb = new CinemaHallContext();
            var cinemaHalls = from ch in cinemaHallDb.CinemaHalls
                              select ch;

            cinemaHalls = cinemaHalls.Where(s => s.CinemaID == id);

            ViewBag.CinemaID = id;

            return View(cinemaHalls);
        }
    }
}
