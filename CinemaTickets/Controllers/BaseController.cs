using CinemaTickets.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace CinemaTickets.Controllers
{
    public class BaseController : CoreController
    {
        private CityContext cityDb = new CityContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object isAdmin = ControllerContext.RouteData.Values["isAdmin"];
            if (isAdmin != null && isAdmin.ToString() == "true")
            {
                Response.Redirect("/");
            }
            SetViewBagData();
        }

        public void SetViewBagData()
        {
            ViewBag.Cities = cityDb.Cities.ToList();
            ViewBag.City = Core.GetCityId() > 0 ? cityDb.Cities.Find(Core.GetCityId()).Name : "";
            ViewBag.Styles = new List<string>() { "style" };
            ViewBag.Scripts = new List<string>() { "core", "script" };
            ViewBag.ScriptTexts = new List<string>();
            ViewBag.messages = new List<string>();
            ViewBag.SiteDisabled = Config.SITE_DISABLED;
            ViewBag.AllowFromIPOnly = Config.ALLOW_FROM_IP_ONLY;

            string IPAddress = "";
            if (ViewBag.AllowFromIPOnly != "")
            {
                IPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                IPAddress = string.IsNullOrEmpty(IPAddress) ? Request.ServerVariables["REMOTE_ADDR"] : IPAddress;
            }

            ViewBag.IPAddress = IPAddress;
        }
    }
}