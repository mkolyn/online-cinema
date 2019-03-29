using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCinema.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //DataContext db = new D("Data Source=MKOLYN\MSSQLSERVER3;Initial Catalog=onlinecinema;Integrated Security=False;User ID=mkolyn;Password=111111;Connect Timeout=5;");
            //DataContext db = new D("Data Source=MKOLYN\MSSQLSERVER3;Initial Catalog=onlinecinema;Integrated Security=False;User ID=mkolyn;Password=111111;Connect Timeout=5;");
            /*SqlConnection c = new SqlConnection("Data Source=MKOLYN\\MSSQLSERVER3;Initial Catalog=onlinecinema;Integrated Security=False;User ID=mkolyn;Password=111111;Connect Timeout=5;");
            c.Open();
            SqlCommand sql = c.CreateCommand();
            sql.CommandText = "SELECT * FROM movies";
            int a = sql.ExecuteNonQuery();
            Console.WriteLine(a);*/
            //ConfigurationManager.ConnectionStrings["DefaultConnection"];
            //string connectionString = System.Configuration.Setting;
            //CloudConfigurationManager.GetSetting(CONNECTION_STRING_KEY, false);

            ViewBag.Message = ConfigurationManager.ConnectionStrings;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Cinemas()
        {
            ViewBag.Message = "Your cinemas page.";

            return View();
        }

        public ActionResult Cinema(int ID)
        {
            ViewBag.Message = ID;

            return View();
        }
    }
}