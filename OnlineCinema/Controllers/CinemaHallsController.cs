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
    public class CinemaHallsController : Controller
    {
        private CinemaHallContext db = new CinemaHallContext();

        public void LoginIfNotAuthorized()
        {
            if (Session["UserID"] == null || Session["UserID"].ToString() == "")
            {
                Response.Redirect("Home");
            }
        }

        // GET: CinemaHalls
        public ActionResult Index()
        {
            LoginIfNotAuthorized();
            return View(db.CinemaHalls.ToList());
        }

        // GET: CinemaHalls/Details/5
        public ActionResult Details(int? id)
        {
            LoginIfNotAuthorized();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CinemaHall cinemaHall = db.CinemaHalls.Find(id);
            if (cinemaHall == null)
            {
                return HttpNotFound();
            }
            return View(cinemaHall);
        }

        // GET: CinemaHalls/Create
        public ActionResult Create(int? id)
        {
            LoginIfNotAuthorized();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CinemaContext cinemaDb = new CinemaContext();
            Cinema cinema = cinemaDb.Cinemas.Find(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            ViewBag.CinemaId = id;
            return View();
        }

        // POST: CinemaHalls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CinemaID,Name")] CinemaHall cinemaHall)
        {
            LoginIfNotAuthorized();
            if (ModelState.IsValid)
            {
                db.CinemaHalls.Add(cinemaHall);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cinemaHall);
        }

        // GET: CinemaHalls/Edit/5
        public ActionResult Edit(int? id)
        {
            LoginIfNotAuthorized();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CinemaHall cinemaHall = db.CinemaHalls.Find(id);
            if (cinemaHall == null)
            {
                return HttpNotFound();
            }
            return View(cinemaHall);
        }

        // POST: CinemaHalls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CinemaID,Name")] CinemaHall cinemaHall)
        {
            LoginIfNotAuthorized();
            if (ModelState.IsValid)
            {
                db.Entry(cinemaHall).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cinemaHall);
        }

        // GET: CinemaHalls/Delete/5
        public ActionResult Delete(int? id)
        {
            LoginIfNotAuthorized();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CinemaHall cinemaHall = db.CinemaHalls.Find(id);
            if (cinemaHall == null)
            {
                return HttpNotFound();
            }
            return View(cinemaHall);
        }

        // POST: CinemaHalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoginIfNotAuthorized();
            CinemaHall cinemaHall = db.CinemaHalls.Find(id);
            db.CinemaHalls.Remove(cinemaHall);
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
    }
}
