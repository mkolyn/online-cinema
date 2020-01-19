using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
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
        // display number of days after today to select ticket date
        const int NEXT_DAYS = 10;
        // ticket should be booked some time before beginning
        public const double BOOK_BEFORE_MINUTES = 30;

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
            else if (Config.DEFAULT_CITY_ID > 0)
            {
                return Config.DEFAULT_CITY_ID;
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

        public static T FromJson<T>(string data)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(data);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string plainText)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(plainText));
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
            var emailMessage = new MailMessage();
            emailMessage.To.Add(new MailAddress(email));
            emailMessage.From = new MailAddress(Config.SMTP_FROM);
            emailMessage.Subject = subject;
            emailMessage.Body = message;
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

        public static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public static string GetFormatedDate(DateTime date)
        {
            string day = date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString();
            string month = date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString();
            string hour = date.Hour < 10 ? "0" + date.Hour.ToString() : date.Hour.ToString();
            string minute = date.Minute < 10 ? "0" + date.Minute.ToString() : date.Minute.ToString();

            return day + "." + month + "." + date.Year + " " + hour + ":" + minute;
        }

        public static string GetFormatedDay(DateTime date)
        {
            string day = date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString();
            string month = date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString();

            return day + "." + month + "." + date.Year;
        }

        public static string GetFormatedTime(DateTime date)
        {
            string hour = date.Hour < 10 ? "0" + date.Hour.ToString() : date.Hour.ToString();
            string minute = date.Minute < 10 ? "0" + date.Minute.ToString() : date.Minute.ToString();

            return hour + ":" + minute;
        }

        public static Dictionary<int, string> GetMonthList()
        {
            return new Dictionary<int, string>
            {
                { 1, "Січ" },
                { 2, "Лют" },
                { 3, "Бер" },
                { 4, "Кві" },
                { 5, "Тра" },
                { 6, "Чер" },
                { 7, "Лип" },
                { 8, "Сер" },
                { 9, "Вер" },
                { 10, "Жов" },
                { 11, "Лис" },
                { 12, "Гру" },
            };
        }

        public static Dictionary<string, string> GetDayList()
        {
            return new Dictionary<string, string>
            {
                { "Today", "Сьогодні" },
                { "Monday", "Понеділок" },
                { "Tuesday", "Вівторок" },
                { "Wednesday", "Середа" },
                { "Thursday", "Четвер" },
                { "Friday", "П'ятниця" },
                { "Saturday", "Субота" },
                { "Sunday", "Неділя" },
            };
        }
    }
}