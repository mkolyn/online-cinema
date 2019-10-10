using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CinemaTickets.Models;
using PagedList;

namespace CinemaTickets.Controllers
{
    public class CinemasController : AdminController
    {
        private CinemaContext db = new CinemaContext();
        private CityContext cityDb = new CityContext();
        private CinemaPlaceGroupContext cinemaPlaceGroupDb = new CinemaPlaceGroupContext();

        // GET: Cinemas
        public ActionResult Index(int? page, string searchString = "", int cityId = 0)
        {
            List<Cinema> cinemas = db.GetList(searchString, cityId);

            int pageNumber = page ?? 1;
            ViewBag.CityID = cityDb.GetSelectList();

            return View(cinemas.ToList().ToPagedList(pageNumber, Core.PAGE_SIZE));
        }

        // GET: Cinemas/Create
        public ActionResult Create()
        {
            ViewBag.CityID = cityDb.GetSelectList();
            return View();
        }

        // POST: Cinemas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CityID,Name")] Cinema cinema)
        {
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cinema cinema = db.Cinemas.Find(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = cityDb.GetSelectList(cinema.CityID);

            return View(cinema);
        }

        // POST: Cinemas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CityID,Name")] Cinema cinema)
        {
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
        public ActionResult Halls(int id, string searchString = "")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Session["CinemaID"] != null && Session["CinemaID"].ToString() != "" && Session["CinemaID"].ToString() != id.ToString())
            {
                return RedirectToAction("Index");
            }

            Cinema cinema = db.Cinemas.Find(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }

            CinemaHallContext cinemaHallDb = new CinemaHallContext();
            var cinemaHalls = cinemaHallDb.GetList(id, searchString);

            ViewBag.CinemaID = id;
            ViewBag.searchString = searchString;

            return View(cinemaHalls);
        }

        // GET: Cinemas/Movies/5
        public ActionResult Movies(int? id)
        {
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

        public ActionResult PlaceGroups(int id)
        {
            if (Session["CinemaID"] != null && Session["CinemaID"].ToString() != "" && Session["CinemaID"].ToString() != id.ToString())
            {
                return RedirectToAction("Index");
            }

            Cinema cinema = db.Cinemas.Find(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }

            List<CinemaPlaceGroup> cinemaPlaceGroups = cinemaPlaceGroupDb.GetList(Core.GetCinemaId());

            ViewBag.CinemaID = id;

            return View(cinemaPlaceGroups);
        }
    }
}
