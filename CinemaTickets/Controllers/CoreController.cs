using CinemaTickets.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CinemaTickets.Controllers
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
            ViewBag.SiteDisabled = Config.SITE_DISABLED;
        }

        public string GetUrl(string url)
        {
            return Request.Url.Scheme + "://" + Request.Url.Host + "/" + url;
        }
    }
}