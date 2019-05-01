using OnlineCinema.Models;
using System;
using System.Web.Mvc;

namespace OnlineCinema.Controllers
{
    public class LiqpayController : Controller
    {
        private OrderContext orderDb = new OrderContext();

        public ActionResult Result(string data, string signature, int amount, string order_id, string status)
        {
            bool success = true;
            Int32.TryParse(order_id, out int orderId);

            if (signature == Liqpay.GetSignature(data))
            {
                orderDb.SetSuccessfullOrder(orderId);
            }
            else
            {
                orderDb.SetFailedOrder(orderId);
                success = false;
            }

            return Json(new { success });
        }
    }
}