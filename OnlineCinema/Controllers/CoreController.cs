using OnlineCinema.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCinema.Controllers
{
    public class CoreController : Controller
    {
        private CityContext cityDb = new CityContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SetViewBagData();
        }

        public void SetViewBagData()
        {
            ViewBag.Cities = cityDb.Cities.ToList();
            ViewBag.City = Core.GetCityId() > 0 ? cityDb.Cities.Find(Core.GetCityId()).Name : "";
            ViewBag.Styles = new List<string>() { "style" };
            ViewBag.Scripts = new List<string>() { "script" };
        }
    }
}