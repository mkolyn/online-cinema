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
    public class Messages
    {
        public const string ORDER_HAS_BEEN_CHANGED = "Були зроблені зміни у бронюванні";
        public const string ORDER_IS_BEING_PROCESSED = "Ви вже перейшли на сторінку оплати";
    }
}