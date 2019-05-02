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
    public class AdminController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoginIfNotAuthorized();
            CheckRights();
            SetViewBagData();
        }

        public void LoginIfNotAuthorized()
        {
            if (Session["UserID"] == null || Session["UserID"].ToString() == "")
            {
                Response.Redirect("/administrator");
            }
        }

        public void CheckRights()
        {
            Dictionary<string, List<string>> cinemaRightsAllowPages = new Dictionary<string, List<string>>();
            cinemaRightsAllowPages.Add("Cinemas", new List<string>() { "Halls", "PlaceGroups" });
            cinemaRightsAllowPages.Add("Genres", new List<string>());
            cinemaRightsAllowPages.Add("Movies", new List<string>() { "Index", "Create", "Edit", "Delete", "DeleteConfirmed", "Find" });
            cinemaRightsAllowPages.Add("Users", new List<string>());

            if (Core.GetCinemaId() > 0)
            {
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();

                if (cinemaRightsAllowPages.ContainsKey(controllerName) && !cinemaRightsAllowPages[controllerName].Contains(actionName))
                {
                    Response.Redirect("/administrator");
                }
            }
        }

        public void CheckCinemaRights(int cinemaID)
        {
            if (Core.GetCinemaId() > 0 && Core.GetCinemaId() != cinemaID)
            {
                Response.Redirect("/administrator");
            }
        }

        public void SetViewBagData()
        {
            ViewBag.Styles = new List<string>() { "admin" };
            ViewBag.Scripts = new List<string>() { "core", "admin" };
        }
    }
}