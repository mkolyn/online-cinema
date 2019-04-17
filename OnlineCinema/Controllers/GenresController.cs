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
    public class GenresController : Controller
    {
        private GenreContext db = new GenreContext();

        public void LoginIfNotAuthorized()
        {
            if (Session["UserID"] == null || Session["UserID"].ToString() == "")
            {
                Response.Redirect("administrator");
            }
        }

        // GET: Genres
        public ActionResult Index()
        {
            LoginIfNotAuthorized();
            return View(db.Genres.ToList());
        }

        // GET: Genres/Details/5
        public ActionResult Details(int? id)
        {
            LoginIfNotAuthorized();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }

            return View(genre);
        }

        // GET: Genres/Create
        public ActionResult Create()
        {
            LoginIfNotAuthorized();
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Genre genre)
        {
            LoginIfNotAuthorized();

            if (ModelState.IsValid)
            {
                db.Genres.Add(genre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(genre);
        }

        // GET: Genres/Edit/5
        public ActionResult Edit(int? id)
        {
            LoginIfNotAuthorized();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }

            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Genre genre)
        {
            LoginIfNotAuthorized();

            if (ModelState.IsValid)
            {
                db.Entry(genre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(genre);
        }

        // GET: Genres/Delete/5
        public ActionResult Delete(int? id)
        {
            LoginIfNotAuthorized();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoginIfNotAuthorized();
            Genre genre = db.Genres.Find(id);
            db.Genres.Remove(genre);
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
