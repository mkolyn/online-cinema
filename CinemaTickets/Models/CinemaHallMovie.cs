using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CinemaTickets.Models
{
    public class CinemaHallMovie
    {
        // cinema hall place ID
        public int ID { get; set; }
        // cinema hall ID
        public int CinemaHallID { get; set; }
        // movie ID
        public int MovieID { get; set; }
        // movie date
        public DateTime Date { get; set; }
    }

    public class CinemaHallScheduleMovie
    {
        // cinema hall movie ID
        public int CinemaHallMovieID { get; set; }
        // cinema movie ID
        public int CinemaHallID { get; set; }
        // cinema ID
        public int CinemaID { get; set; }
        // cinema name
        public string CinemaName { get; set; }
        // city id
        public int CityID { get; set; }
        // genre id
        public int GenreID { get; set; }
        // genre id
        public string GenreName { get; set; }
        // movie ID
        public int MovieID { get; set; }
        // movie name
        public string MovieName { get; set; }
        // movie image
        public string Image { get; set; }
        // start minute
        public int StartMinute { get; set; }
        // duration
        public int Duration { get; set; }
        // movie date
        public DateTime Date { get; set; }
        // is removed
        public bool IsRemoved { get; set; }
        // price
        public int Price { get; set; }
        // movie formatted date
        public string FormattedDate { get; set; }
        // movie formatted time
        public string FormattedTime { get; set; }
        // is 3D
        public bool? Is3D { get; set; }
    }

    public class CinemaHallMovieContext : DbContext
    {
        public CinemaHallMovieContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<CinemaHallMovie> CinemaHallMovies { get; set; }
        public DbSet<CinemaMovie> CinemaMovies { get; set; }
        public DbSet<CinemaHall> CinemaHalls { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        private List<int> GetMovieIds(int year, int month, int day, int cinemaId, int genreId, string searchString)
        {
            List<int> movieIds = new List<int>();

            var movies = from chm in CinemaHallMovies
                         join cm in CinemaMovies on chm.MovieID equals cm.MovieID
                         join ch in CinemaHalls on chm.CinemaHallID equals ch.ID
                         join c in Cinemas on ch.CinemaID equals c.ID
                         join m in Movies on chm.MovieID equals m.ID
                         join g in Genres on m.GenreID equals g.ID
                         where cm.CinemaID == ch.CinemaID
                         select new CinemaHallScheduleMovie
                         {
                             Date = chm.Date,
                             MovieID = m.ID,
                             MovieName = m.Name,
                             GenreID = m.GenreID,
                             CityID = c.CityID,
                             CinemaID = c.ID,
                         };

            int cityId = Core.GetCityId();

            movies = movies.Where(m => m.CityID == cityId)
                .Where(m => m.Date.Year == year)
                .Where(m => m.Date.Month == month)
                .Where(m => m.Date.Day == day);

            if (cinemaId > 0)
            {
                movies = movies.Where(m => m.CinemaID == cinemaId);
            }

            if (genreId > 0)
            {
                movies = movies.Where(m => m.GenreID == genreId);
            }

            if (searchString != "")
            {
                movies = movies.Where(m => m.MovieName.ToString().Contains(searchString));
            }

            foreach (CinemaHallScheduleMovie movie in movies.ToList())
            {
                if (!movieIds.Contains(movie.MovieID))
                {
                    movieIds.Add(movie.MovieID);
                }
            }

            return movieIds;
        }

        private List<int> GetComingSoonMovieIds()
        {
            List<int> movieIds = new List<int>();
            DateTime date = DateTime.Now;
            DateTime nextWeek = date.AddDays(Core.NEXT_DAYS);
            DateTime inTwoWeeks = nextWeek.AddDays(Core.NEXT_DAYS);

            var movies = from chm in CinemaHallMovies
                         join cm in CinemaMovies on chm.MovieID equals cm.MovieID
                         join ch in CinemaHalls on chm.CinemaHallID equals ch.ID
                         join c in Cinemas on ch.CinemaID equals c.ID
                         join m in Movies on chm.MovieID equals m.ID
                         where cm.CinemaID == ch.CinemaID
                         select new CinemaHallScheduleMovie
                         {
                             Date = chm.Date,
                             MovieID = m.ID,
                             CityID = c.CityID,
                         };

            int cityId = Core.GetCityId();

            movies = movies.Where(m => m.CityID == cityId)
                .Where(m => m.Date > nextWeek)
                .Where(m => m.Date < inTwoWeeks);

            foreach (CinemaHallScheduleMovie movie in movies.ToList())
            {
                if (!movieIds.Contains(movie.MovieID))
                {
                    movieIds.Add(movie.MovieID);
                }
            }

            return movieIds;
        }

        public List<Movie> GetList(int year, int month, int day, int cinemaId = 0, int genreId = 0, string searchString = "")
        {
            List<int> movieIds = GetMovieIds(year, month, day, cinemaId, genreId, searchString);

            var movies = from m in Movies
                         select m;

            movies = movies.Where(m => movieIds.Contains(m.ID));

            return movies.ToList();
        }

        public List<Movie> GetComingSoonList()
        {
            List<int> movieIds = GetComingSoonMovieIds();

            var movies = from m in Movies
                         select m;

            movies = movies.Where(m => movieIds.Contains(m.ID));

            return movies.ToList();
        }

        public List<CinemaHallScheduleMovie> GetListByCinemaHallId(int cinemaHallId, int year, int month, int day)
        {
            var cinemaHallMovies = from chm in CinemaHallMovies
                                   join ch in CinemaHalls on chm.CinemaHallID equals ch.ID
                                   join cm in CinemaMovies on ch.CinemaID equals cm.CinemaID
                                   join m in Movies on chm.MovieID equals m.ID
                                   where cm.MovieID == m.ID
                                   orderby chm.Date
                                   select new CinemaHallScheduleMovie
                                   {
                                       CinemaHallMovieID = chm.ID,
                                       CinemaHallID = chm.CinemaHallID,
                                       MovieID = m.ID,
                                       MovieName = m.Name,
                                       Duration = m.Duration,
                                       Date = chm.Date,
                                       Price = cm.Price,
                                   };

            cinemaHallMovies = cinemaHallMovies.Where(s => s.CinemaHallID == cinemaHallId)
                .Where(s => s.Date.Year == year)
                .Where(s => s.Date.Month == month)
                .Where(s => s.Date.Day == day);

            return cinemaHallMovies.ToList();
        }

        public void SaveMovie(int cinemaHallMovieID, int cinemaHallID, int movieID, bool isRemoved, DateTime date)
        {
            if (cinemaHallMovieID > 0)
            {
                CinemaHallMovie cinemaHallMovie = CinemaHallMovies.Find(cinemaHallMovieID);
                if (isRemoved)
                {
                    CinemaHallMovies.Remove(cinemaHallMovie);
                }
                else
                {
                    cinemaHallMovie.Date = date;
                }
                SaveChanges();
            }
            else
            {
                var cinemaHallMovies = from chm in CinemaHallMovies
                                       select chm;

                cinemaHallMovies = cinemaHallMovies.Where(s => s.CinemaHallID == cinemaHallID)
                    .Where(s => s.MovieID == movieID)
                    .Where(s => s.Date == date);

                if (cinemaHallMovies.FirstOrDefault() == null)
                {
                    CinemaHallMovie cinemaHallMovie = new CinemaHallMovie
                    {
                        CinemaHallID = cinemaHallID,
                        MovieID = movieID,
                        Date = date,
                    };

                    CinemaHallMovies.Add(cinemaHallMovie);
                    SaveChanges();
                }
            }
        }

        public int GetPrice(int cinemaHallMovieId)
        {
            var cinemaMovie = from chm in CinemaHallMovies
                              join ch in CinemaHalls on chm.CinemaHallID equals ch.ID
                              join c in Cinemas on ch.CinemaID equals c.ID
                              join cm in CinemaMovies on c.ID equals cm.CinemaID
                              where chm.ID == cinemaHallMovieId && cm.MovieID == chm.MovieID
                              select cm;

            return cinemaMovie.First().Price;
        }

        public CinemaHallScheduleMovie GetLastByCinemaHallId(int cinemaHallId, int year, int month, int day)
        {
            var cinemaHallMovies = from chm in CinemaHallMovies
                                   join ch in CinemaHalls on chm.CinemaHallID equals ch.ID
                                   join cm in CinemaMovies on ch.CinemaID equals cm.CinemaID
                                   join m in Movies on chm.MovieID equals m.ID
                                   where cm.MovieID == m.ID
                                   orderby chm.Date descending
                                   select new CinemaHallScheduleMovie
                                   {
                                       CinemaHallMovieID = chm.ID,
                                       CinemaHallID = chm.CinemaHallID,
                                       MovieID = m.ID,
                                       MovieName = m.Name,
                                       Duration = m.Duration,
                                       Date = chm.Date,
                                       Price = cm.Price,
                                   };

            cinemaHallMovies = cinemaHallMovies.Where(s => s.CinemaHallID == cinemaHallId)
                .Where(s => s.Date.Year == year)
                .Where(s => s.Date.Month == month)
                .Where(s => s.Date.Day == day);

            return cinemaHallMovies.FirstOrDefault();
        }

        public string GetCinemaHallScheduleHoursHtml(int cinemaHallId, DateTime date, ControllerContext controllerContext)
        {
            int prevMovieEndHour = 0;
            int prevMovieEndMinute = 0;

            DateTime prevDate = date.AddDays(-1).Date;

            CinemaHallScheduleMovie cinemaHallLastMovie = GetLastByCinemaHallId(cinemaHallId, prevDate.Year, prevDate.Month, prevDate.Day);
            if (cinemaHallLastMovie != null)
            {
                DateTime lastMovieEndDate = cinemaHallLastMovie.Date.AddMinutes(cinemaHallLastMovie.Duration);
                if (lastMovieEndDate.Year == date.Year && lastMovieEndDate.Month == date.Month && lastMovieEndDate.Day == date.Day)
                {
                    prevMovieEndHour = lastMovieEndDate.Hour;
                    prevMovieEndMinute = lastMovieEndDate.Minute;
                }
            }

            ViewDataDictionary viewData = new ViewDataDictionary()
            {
                { "prevMovieEndHour", prevMovieEndHour },
                { "prevMovieEndMinute", prevMovieEndMinute },
            };

            return Core.GetHtmlString("Hours", viewData, controllerContext);
        }

        public List<CinemaHallScheduleMovie> GetMovieScheduleList(int movieId, DateTime startDate, DateTime endDate)
        {
            var cinemaHallScheduleMovie = from chm in CinemaHallMovies
                                          join ch in CinemaHalls on chm.CinemaHallID equals ch.ID
                                          join cm in CinemaMovies on ch.CinemaID equals cm.CinemaID
                                          join c in Cinemas on ch.CinemaID equals c.ID
                                          join m in Movies on chm.MovieID equals m.ID
                                          where chm.MovieID == movieId && cm.MovieID == movieId
                                          select new CinemaHallScheduleMovie
                                          {
                                              CinemaHallMovieID = chm.ID,
                                              MovieID = m.ID,
                                              CinemaName = c.Name,
                                              Date = chm.Date,
                                              Is3D = cm.Is3D,
                                          };

            cinemaHallScheduleMovie = cinemaHallScheduleMovie.Where(chsm => chsm.Date >= startDate).Where(chsm => chsm.Date <= endDate);

            return cinemaHallScheduleMovie.ToList();
        }

        public Dictionary<CinemaMovieSelect, Dictionary<string, Dictionary<string, int>>> GetMovieSchedule(int movieId, DateTime startDate, DateTime endDate)
        {
            // e.g. [Ефект][14.03.2020][19:20] = [1, 2, 3]
            Dictionary<CinemaMovieSelect, Dictionary<string, Dictionary<string, int>>> movieScheduleList;
            movieScheduleList = new Dictionary<CinemaMovieSelect, Dictionary<string, Dictionary<string, int>>>();
            List<CinemaHallScheduleMovie> movieScheduleDataList = GetMovieScheduleList(movieId, startDate, endDate);
            Dictionary<int, CinemaMovieSelect> cinemaMovies = new Dictionary<int, CinemaMovieSelect>();

            foreach (CinemaHallScheduleMovie movieScheduleData in movieScheduleDataList)
            {
                string time = Core.GetFormatedTime(movieScheduleData.Date);
                string cinemaName = movieScheduleData.CinemaName;
                string day = Core.GetFormatedDay(movieScheduleData.Date);
                int cinemaHallID = movieScheduleData.CinemaHallID;
                CinemaMovieSelect cinemaMovie;

                if (!cinemaMovies.ContainsKey(cinemaHallID))
                {
                    cinemaMovie = new CinemaMovieSelect()
                    {
                        CinemaName = cinemaName,
                        Is3D = movieScheduleData.Is3D,
                    };
                    cinemaMovies.Add(cinemaHallID, cinemaMovie);
                }
                else
                {
                    cinemaMovie = cinemaMovies[cinemaHallID];
                }


                if (!movieScheduleList.ContainsKey(cinemaMovie))
                {
                    movieScheduleList.Add(cinemaMovie, new Dictionary<string, Dictionary<string, int>>());
                }
                if (!movieScheduleList[cinemaMovie].ContainsKey(day))
                {
                    movieScheduleList[cinemaMovie].Add(day, new Dictionary<string, int>());
                }

                movieScheduleList[cinemaMovie][day].Add(time, movieScheduleData.CinemaHallMovieID);
            }

            return movieScheduleList;
        }

        public DateTime GetMovieMaxDate(int movieId)
        {
            DateTime date = DateTime.Now;
            List<CinemaHallScheduleMovie> cinemaHallScheduleMovie = GetMovieScheduleList(movieId, date.AddMinutes(Core.BOOK_BEFORE_MINUTES), date.AddDays(Core.NEXT_DAYS));
            return cinemaHallScheduleMovie.Count > 0 ? cinemaHallScheduleMovie.Max(m => m.Date) : DateTime.Now;
        }
    }
}