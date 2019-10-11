using CinemaTickets.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaTickets.Controllers
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
            cinemaRightsAllowPages.Add("Movies", new List<string>() { "Index", "Create", "CreateCinemaMovie", "Edit", "Delete", "DeleteConfirmed", "Find", "Details" });
            cinemaRightsAllowPages.Add("Users", new List<string>());

            if (Core.GetCinemaId() > 0)
            {
                string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                string actionName = ControllerContext.RouteData.Values["action"].ToString();

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
            ViewBag.messages = new List<string>();
            ViewBag.SiteDisabled = Config.SITE_DISABLED;
        }

        public void AddMessage(string message)
        {
            ViewBag.messages.Add(message);
        }

        public void AddModelStateErrors(ICollection<ModelState> modelStates)
        {
            foreach (ModelState modelState in modelStates)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    ViewBag.messages.Add(error.ErrorMessage + " " + error.Exception);
                    //ViewBag.messages.Add(error.ErrorMessage.ToString());
                }
            }
        }
    }
}