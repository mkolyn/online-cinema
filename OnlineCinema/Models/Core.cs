using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OnlineCinema.Models
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
    }
}