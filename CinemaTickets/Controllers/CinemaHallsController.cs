using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CinemaTickets.Models;

namespace CinemaTickets.Controllers
{
    public class CinemaHallsController : AdminController
    {
        private CinemaHallContext db = new CinemaHallContext();

        // GET: CinemaHalls/Create
        public ActionResult Create(int id)
        {
            CheckCinemaRights(id);

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
            if (ModelState.IsValid)
            {
                CheckCinemaRights(cinemaHall.CinemaID);
                db.CinemaHalls.Add(cinemaHall);
                db.SaveChanges();
                return RedirectToAction("Halls", "Cinemas", new { id = cinemaHall.CinemaID });
            }

            return View(cinemaHall);
        }

        // GET: CinemaHalls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CinemaHall cinemaHall = db.CinemaHalls.Find(id);
            if (cinemaHall == null)
            {
                return HttpNotFound();
            }
            CheckCinemaRights(cinemaHall.CinemaID);

            return View(cinemaHall);
        }

        // POST: CinemaHalls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CinemaID,Name")] CinemaHall cinemaHall)
        {
            if (ModelState.IsValid)
            {
                CheckCinemaRights(cinemaHall.CinemaID);
                db.Entry(cinemaHall).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Halls", "Cinemas", new { id = cinemaHall.CinemaID });
            }

            return View(cinemaHall);
        }

        // GET: CinemaHalls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CinemaHall cinemaHall = db.CinemaHalls.Find(id);
            if (cinemaHall == null)
            {
                return HttpNotFound();
            }
            CheckCinemaRights(cinemaHall.CinemaID);

            return View(cinemaHall);
        }

        // POST: CinemaHalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CinemaHall cinemaHall = db.CinemaHalls.Find(id);
            CheckCinemaRights(cinemaHall.CinemaID);
            db.CinemaHalls.Remove(cinemaHall);
            db.SaveChanges();
            return RedirectToAction("Halls", "Cinemas", new { id = cinemaHall.CinemaID });
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
