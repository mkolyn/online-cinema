using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CinemaTickets.Models
{
    public class Core
    {
        public const int PAGE_SIZE = 10;
        const int NEXT_DAYS = 10;

        public static int GetCinemaId()
        {
            var session = HttpContext.Current.Session;
            if (session["CinemaID"] != null && session["CinemaID"].ToString() != "")
            {
                Int32.TryParse(session["CinemaID"].ToString(), out int cinemaId);
                return cinemaId;
            }
            return 0;
        }

        public static int GetUserId()
        {
            var session = HttpContext.Current.Session;
            if (session["UserID"] != null && session["UserID"].ToString() != "")
            {
                Int32.TryParse(session["UserID"].ToString(), out int userId);
                return userId;
            }
            return 0;
        }

        public static int GetCityId()
        {
            var session = HttpContext.Current.Session;
            if (session["CityID"] != null && session["CityID"].ToString() != "")
            {
                Int32.TryParse(session["CityID"].ToString(), out int cityId);
                return cityId;
            }
            return 0;
        }

        public static List<DateTime> GetNextDates(DateTime date)
        {
            List<DateTime> dates = new List<DateTime>();
            dates.Add(date);

            for (var i = 0; i < NEXT_DAYS; i++)
            {
                date = date.AddDays(1);
                dates.Add(date);
            }

            return dates;
        }

        public static string UploadImage(HttpPostedFileBase image, string folderPath, string imageNamePrefix = "")
        {
            string imageFileName = "";

            if (image != null && image.ContentLength > 0)
            {
                imageFileName = Path.GetFileName(image.FileName) + DateTime.Now.ToFileTime();
                if (imageNamePrefix != "")
                {
                    imageFileName += imageNamePrefix;
                }
                imageFileName = Crypto.SHA256(imageFileName);
                imageFileName += Path.GetExtension(image.FileName);

                image.SaveAs(Path.Combine(folderPath, imageFileName));
            }

            return imageFileName;
        }

        public static void RemoveImage(string folderPath, string imageName)
        {
            if (imageName != null && imageName != "")
            {
                string imagePath = Path.Combine(folderPath, imageName);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }
        }

        public static string ToJson(object data)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(data);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string GetHtmlString(string viewName, ViewDataDictionary viewData, ControllerContext context)
        {
            string html = "";

            ViewEngineResult viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewName);
            var view = viewEngineResult.View;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view, viewData, context.Controller.TempData, sw);
                view.Render(ctx, sw);
                html = sw.ToString();
            }

            return html;
        }

        public static void SendEmail(string email, string subject, string message)
        {
            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var emailMessage = new MailMessage();
            emailMessage.To.Add(new MailAddress(email));
            emailMessage.From = new MailAddress(Config.SMTP_FROM);
            emailMessage.Subject = subject;
            emailMessage.Body = string.Format(body, "CinemaTickets", "cinematickets.com.ua", message);
            emailMessage.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = Config.SMTP_USER,
                    Password = Config.SMTP_PASSWORD
                };

                smtp.Credentials = credential;
                smtp.Host = Config.SMTP_HOST;
                //smtp.Port = Config.SMTP_PORT;
                //smtp.EnableSsl = Config.SMTP_SSL;
                smtp.Timeout = 10000;
                smtp.Send(emailMessage);
            }
        }
    }
}