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

        public ActionResult Result(string data, string signature,
            [Bind(Include = "amount,liqpay_order_id,order_id,payment_id,status,description,err_code,err_description")] LiqpayResult liqpayResult)
        {
            bool success = false;
            bool failed = false;
            Int32.TryParse(liqpayResult.OrderId, out int orderId);

            int liqpayResultId = liqpayResultDb.GetIdByOrderId(orderId);
            if (liqpayResultId > 0)
            {
                liqpayResult.ID = liqpayResultId;
                liqpayResultDb.Entry(liqpayResult).State = EntityState.Modified;
                liqpayResultDb.SaveChanges();
            }
            else
            {
                liqpayResultDb.LiqpayResults.Add(liqpayResult);
                liqpayResultDb.SaveChanges();
            }

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

            return Json(new { success });
        }
    }
}