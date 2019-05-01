using OnlineCinema.Models;
using System;
using System.Data.Entity;
using System.Web.Mvc;

namespace OnlineCinema.Controllers
{
    public class LiqpayController : Controller
    {
        private OrderContext orderDb = new OrderContext();
        private LiqpayResultContext liqpayResultDb = new LiqpayResultContext();

        public ActionResult Result(
            string data,
            string signature,
            int amount,
            string liqpay_order_id,
            string order_id,
            int payment_id,
            string status,
            string description,
            string err_code,
            string err_description)
        {
            bool success = false;
            bool failed = false;
            Int32.TryParse(order_id, out int orderId);

            LiqpayResult liqpayResult = liqpayResultDb.Get(orderId);
            if (liqpayResult != null)
            {
                liqpayResultDb.Entry(liqpayResult).State = EntityState.Modified;
            }
            else
            {
                liqpayResult = new LiqpayResult();
                liqpayResultDb.LiqpayResults.Add(liqpayResult);
            }

            liqpayResult.Amount = amount;
            liqpayResult.LiqpayOrderId = liqpay_order_id;
            liqpayResult.OrderId = order_id;
            liqpayResult.PaymentId = payment_id;
            liqpayResult.Status = status;
            liqpayResult.Description = description;
            liqpayResult.ErrorCode = err_code;
            liqpayResult.ErrorDescription = err_description;
            liqpayResult.Date = DateTime.Now;

            liqpayResultDb.SaveChanges();

            if (signature == Liqpay.GetSignature(data))
            {
                switch (liqpayResult.Status)
                {
                    case LiqpayResult.STATUS_SUCCESSFULL:
                        success = true;
                        break;
                    case LiqpayResult.STATUS_TEST:
                        success = true;
                        break;
                    case LiqpayResult.STATUS_FAILED:
                        failed = true;
                        break;
                    case LiqpayResult.STATUS_WRONG_DATA:
                        failed = true;
                        break;
                }
            }
            else
            {
                success = false;
                failed = true;
            }

            if (success)
            {
                orderDb.SetSuccessfullOrder(orderId);
            }

            if (failed)
            {
                orderDb.SetFailedOrder(orderId);
            }

            return Json(new { success }, JsonRequestBehavior.AllowGet);
        }
    }
}