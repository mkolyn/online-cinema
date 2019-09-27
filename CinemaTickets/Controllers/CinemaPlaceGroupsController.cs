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
    public class CinemaPlaceGroupsController : AdminController
    {
        private CinemaPlaceGroupContext db = new CinemaPlaceGroupContext();

        // GET: CinemaPlaceGroups/Create
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

        // POST: CinemaPlaceGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CinemaID,Name")] CinemaPlaceGroup cinemaPlaceGroup)
        {
            if (ModelState.IsValid)
            {
                CheckCinemaRights(cinemaPlaceGroup.CinemaID);
                db.CinemaPlaceGroups.Add(cinemaPlaceGroup);
                db.SaveChanges();
                return RedirectToAction("PlaceGroups", "Cinemas", new { id = cinemaPlaceGroup.CinemaID });
            }

            return View(cinemaPlaceGroup);
        }

        // GET: CinemaPlaceGroups/Edit/5
        public ActionResult Edit(int id)
        {
            CinemaPlaceGroup cinemaPlaceGroup = db.CinemaPlaceGroups.Find(id);
            if (cinemaPlaceGroup == null)
            {
                return HttpNotFound();
            }
            CheckCinemaRights(cinemaPlaceGroup.CinemaID);

            return View(cinemaPlaceGroup);
        }

        // POST: CinemaPlaceGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CinemaID,Name")] CinemaPlaceGroup cinemaPlaceGroup)
        {
            if (ModelState.IsValid)
            {
                CheckCinemaRights(cinemaPlaceGroup.CinemaID);
                db.Entry(cinemaPlaceGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PlaceGroups", "Cinemas", new { id = cinemaPlaceGroup.CinemaID });
            }

            return View(cinemaPlaceGroup);
        }

        // GET: CinemaPlaceGroups/Delete/5
        public ActionResult Delete(int id)
        {
            CinemaPlaceGroup cinemaPlaceGroup = db.CinemaPlaceGroups.Find(id);
            if (cinemaPlaceGroup == null)
            {
                return HttpNotFound();
            }
            CheckCinemaRights(cinemaPlaceGroup.CinemaID);

            return View(cinemaPlaceGroup);
        }

        // POST: CinemaPlaceGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CinemaPlaceGroup cinemaPlaceGroup = db.CinemaPlaceGroups.Find(id);
            CheckCinemaRights(cinemaPlaceGroup.CinemaID);
            db.CinemaPlaceGroups.Remove(cinemaPlaceGroup);
            db.SaveChanges();
            return RedirectToAction("PlaceGroups", "Cinemas", new { id = cinemaPlaceGroup.CinemaID });
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
