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
    public class CinemaHallPlacesController : RunBeforeController
    {
        private CinemaHallPlaceContext db = new CinemaHallPlaceContext();
        private CinemaHallContext cinemaHallDb = new CinemaHallContext();

        // GET: CinemaHallPlaces/5
        public ActionResult Index(int id)
        {
            CinemaHall cinemaHall = cinemaHallDb.CinemaHalls.Find(id);
            if (cinemaHall == null)
            {
                return HttpNotFound();
            }
            CheckCinemaRights(cinemaHall.CinemaID);

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

            CinemaHallPlace[,] cinemaHallRows = new CinemaHallPlace[maxRow, maxCell];
            bool[,] cinemaHallIsJoinedPlaces = new bool[maxRow, maxCell];
            foreach (CinemaHallPlace cinemaHallPlace in cinemaHallPlaces)
            {
                if (cinemaHallPlace.Rows > 1 || cinemaHallPlace.Cells > 1)
                {
                    for (var i = cinemaHallPlace.Row - 1; i < cinemaHallPlace.Row - 1 + cinemaHallPlace.Rows; i++)
                    {
                        for (var j = cinemaHallPlace.Cell - 1; j < cinemaHallPlace.Cell - 1 + cinemaHallPlace.Cells; j++)
                        {
                            cinemaHallIsJoinedPlaces[i, j] = true;
                        }
                    }
                }
                if (cinemaHallIsJoinedPlaces[cinemaHallPlace.Row - 1, cinemaHallPlace.Cell - 1] == true)
                {
                    cinemaHallPlace.SetIsJoined(true);
                }
                cinemaHallRows[cinemaHallPlace.Row - 1, cinemaHallPlace.Cell - 1] = cinemaHallPlace;
            }

            ViewBag.CinemaHallID = id;
            ViewBag.maxRow = maxRow;
            ViewBag.maxCell = maxCell;
            ViewBag.cinemaHallRows = cinemaHallRows;

            return View();
        }

        // POST: CinemaHallPlaces/Save/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Save(int id, List<CinemaHallPlace> places)
        {
            CinemaHall cinemaHall = cinemaHallDb.CinemaHalls.Find(id);
            if (cinemaHall == null)
            {
                return HttpNotFound();
            }
            CheckCinemaRights(cinemaHall.CinemaID);

            var cinemaHallPlaces = from chp in db.CinemaHallPlaces
                                   select chp;

            cinemaHallPlaces = cinemaHallPlaces.Where(s => s.CinemaHallID == id);

            List<List<int>> existedPlaces = new List<List<int>>();
            foreach (CinemaHallPlace cinemaHallPlace in cinemaHallPlaces)
            {
                List<int> existedPlace = new List<int> { cinemaHallPlace.Row, cinemaHallPlace.Cell };
                existedPlaces.Add(existedPlace);
            }

            foreach (CinemaHallPlace place in places)
            {
                bool isPlaceExisted = false;
                foreach (List<int> existedPlace in existedPlaces)
                {
                    if (existedPlace[0] == place.Row && existedPlace[1] == place.Cell)
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
                        Row = place.Row,
                        Cell = place.Cell,
                        Rows = place.Rows,
                        Cells = place.Cells,
                    };
                    db.CinemaHallPlaces.Add(cinemaHallPlace);
                }
                else
                {
                    cinemaHallPlaces = from chp in db.CinemaHallPlaces
                                       select chp;

                    cinemaHallPlaces = cinemaHallPlaces.Where(s => s.CinemaHallID == id)
                        .Where(s => s.Row == place.Row)
                        .Where(s => s.Cell == place.Cell);

                    IEnumerator<CinemaHallPlace> cinemaHallPlaceEnum = cinemaHallPlaces.GetEnumerator();
                    cinemaHallPlaceEnum.MoveNext();
                    CinemaHallPlace cinemaHallPlace = cinemaHallPlaceEnum.Current;
                    cinemaHallPlace.Rows = place.Rows;
                    cinemaHallPlace.Cells = place.Cells;
                    db.Entry(cinemaHallPlace).State = EntityState.Modified;
                    cinemaHallPlaceEnum.Dispose();
                }
            }

            db.SaveChanges();

            foreach (List<int> existedPlace in existedPlaces)
            {
                bool isPlaceExisted = false;
                foreach (CinemaHallPlace place in places)
                {
                    if (existedPlace[0] == place.Row && existedPlace[1] == place.Cell)
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
