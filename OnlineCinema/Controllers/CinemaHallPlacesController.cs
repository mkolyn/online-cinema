using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using OnlineCinema.Models;

namespace OnlineCinema.Controllers
{
    public class CinemaHallPlacesController : Controller
    {
        private CinemaHallPlaceContext db = new CinemaHallPlaceContext();

        // GET: CinemaHallPlaces/5
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CinemaHallContext cinemaHallDb = new CinemaHallContext();
            CinemaHall cinemaHall = cinemaHallDb.CinemaHalls.Find(id);
            if (cinemaHall == null)
            {
                return HttpNotFound();
            }
            
            var cinemaHallPlaces = from chp in db.CinemaHallPlaces
                              select chp;

            cinemaHallPlaces = cinemaHallPlaces.Where(s => s.CinemaHallID == id);

            int maxRow = 0;
            int maxCell = 0;
            foreach (CinemaHallPlace cinemaHallPlace in cinemaHallPlaces)
            {
                if (cinemaHallPlace.Row > maxRow)
                {
                    maxRow = cinemaHallPlace.Row;
                }
                if (cinemaHallPlace.Cell > maxCell)
                {
                    maxCell = cinemaHallPlace.Cell;
                }
            }

            //List<List<int>> cinemaHallRows = new List<List<int>>(maxRow);
            //HashSet<int> cinemaHallRows = new HashSet<int>();
            int[,] cinemaHallRows = new int[maxRow, maxCell];
            foreach (CinemaHallPlace cinemaHallPlace in cinemaHallPlaces)
            {
                //cinemaHallRows[cinemaHallPlace.Row - 1].Insert(cinemaHallPlace.Cell - 1, cinemaHallPlace.Row + cinemaHallPlace.Cell);
                //cinemaHallRows[cinemaHallPlace.Row - 1];
                cinemaHallRows[cinemaHallPlace.Row - 1, cinemaHallPlace.Cell - 1] = cinemaHallPlace.Row + cinemaHallPlace.Cell;
            }

            ViewBag.CinemaHallID = id;
            ViewBag.maxRow = maxRow;
            ViewBag.maxCell = maxCell;
            ViewBag.cinemaHallRows = cinemaHallRows;

            return View();
        }

        // POST: CinemaHallPlaces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CinemaHallID,Row,Cell,Rows,Cells")] CinemaHallPlace cinemaHallPlace)
        {
            if (ModelState.IsValid)
            {
                db.CinemaHallPlaces.Add(cinemaHallPlace);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cinemaHallPlace);
        }

        // POST: CinemaHallPlaces/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Save(int id, List<List<int>> places)
        {
            var cinemaHallPlaces = from chp in db.CinemaHallPlaces
                                   select chp;

            cinemaHallPlaces = cinemaHallPlaces.Where(s => s.CinemaHallID == id);

            List<List<int>> existedPlaces = new List<List<int>>();
            foreach (CinemaHallPlace cinemaHallPlace in cinemaHallPlaces)
            {
                List<int> existedPlace = new List<int> { cinemaHallPlace.Row, cinemaHallPlace.Cell };
                existedPlaces.Add(existedPlace);
            }

            foreach (List<int> place in places)
            {
                bool isPlaceExisted = false;
                foreach (List<int> existedPlace in existedPlaces)
                {
                    if (existedPlace[0] == place[0] && existedPlace[1] == place[1])
                    {
                        isPlaceExisted = true;
                        break;
                    }
                }

                if (!isPlaceExisted)
                {
                    CinemaHallPlace cinemaHallPlace = new CinemaHallPlace()
                    {
                        CinemaHallID = id,
                        Row = place[0],
                        Cell = place[1],
                        Rows = 1,
                        Cells = 1,
                    };
                    db.CinemaHallPlaces.Add(cinemaHallPlace);
                    db.SaveChanges();
                }
            }

            foreach (List<int> existedPlace in existedPlaces)
            {
                bool isPlaceExisted = false;
                foreach (List<int> place in places)
                {
                    if (existedPlace[0] == place[0] && existedPlace[1] == place[1])
                    {
                        isPlaceExisted = true;
                        break;
                    }
                }

                if (!isPlaceExisted)
                {
                    cinemaHallPlaces = from chp in db.CinemaHallPlaces
                                           select chp;

                    int row = existedPlace[0];
                    int cell = existedPlace[1];

                    cinemaHallPlaces = cinemaHallPlaces.Where(s => s.CinemaHallID == id)
                        .Where(s => s.Row == row)
                        .Where(s => s.Cell == cell);

                    foreach (CinemaHallPlace cinemaHallPlace in cinemaHallPlaces)
                    {
                        db.CinemaHallPlaces.Remove(cinemaHallPlace);
                    }

                    db.SaveChanges();
                }
            }

            return Json(existedPlaces);
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
