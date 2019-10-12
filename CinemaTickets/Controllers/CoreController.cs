using CinemaTickets.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace CinemaTickets.Controllers
{
    public class CoreController : Controller
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
            ViewBag.Scripts = new List<string>() { "script" };
            ViewBag.messages = new List<string>();
            ViewBag.SiteDisabled = Config.SITE_DISABLED;
        }

        public string GetUrl(string url)
        {
            return Request.Url.Scheme + "://" + Request.Url.Host + (Request.Url.Port > 0 ? ":" + Request.Url.Port : "") + "/" + url;
        }

        public void AddDebugInfo(string info)
        {
            using (StreamWriter sw = new StreamWriter(Server.MapPath("~/Errors/debug.txt"), true))
            {
                sw.WriteLine(info);
            }
        }

        public void AddModelStateErrors(ICollection<ModelState> modelStates)
        {
            foreach (ModelState modelState in modelStates)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    ViewBag.messages.Add(error.ErrorMessage + " " + error.Exception);
                }
            }
        }

        public void AddMessage(string message)
        {
            if (Session["Messages"] == null)
            {
                Session["Messages"] = new List<string>();
            }
            List<string> messages = Session["Messages"] as List<string>;
            messages.Add(message);
            Session["Messages"] = messages;
        }
    }
}