using CinemaTickets.Models;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace CinemaTickets.Controllers
{
    public class LiqpayController : Controller
    {
        private OrderContext orderDb = new OrderContext();
        private LiqpayResultContext liqpayResultDb = new LiqpayResultContext();
        private CinemaHallMoviePlaceContext cinemaHallMoviePlaceDb = new CinemaHallMoviePlaceContext();
        private CinemaHallPlaceContext cinemaHallPlaceDb = new CinemaHallPlaceContext();

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
                SendEmail(orderId);
            }

            if (failed)
            {
                orderDb.SetFailedOrder(orderId);
            }

            return Json(new { success }, JsonRequestBehavior.AllowGet);
        }

        public byte[] GenerateQRCode(int orderId)
        {
            string qrCodeMessage = "";

            List<OrderItemInfo> orderItems = orderDb.GetOrderItems(orderId);
            foreach (OrderItemInfo orderItem in orderItems)
            {
                CinemaHallMoviePlace cinemaHallMoviePlace = cinemaHallMoviePlaceDb.GetCinemaHallMoviePlace(
                    orderItem.CinemaHallMovieID, orderItem.CinemaHallPlaceID);

                CinemaHallPlace cinemaHallPlace = cinemaHallPlaceDb.CinemaHallPlaces.Find(cinemaHallMoviePlace.CinemaHallPlaceID);

                qrCodeMessage += "Ряд: " + cinemaHallPlace.Row + " Місце: " + cinemaHallPlace.Cell + "\n";
            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeMessage, QRCodeGenerator.ECCLevel.Q);
            BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;
        }

        public void SendEmail(int orderId)
        {
            var imageSrc = Convert.ToBase64String(GenerateQRCode(orderId));
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates", "SuccessfulOrder.html");
            StreamReader sr = System.IO.File.OpenText(filePath);
            string emailTemplate = sr.ReadToEnd();
            sr.Close();
            emailTemplate = emailTemplate.Replace("{IMG_BASE64_SRC}", imageSrc);
            Core.SendEmail("mkolyn@gmail.com", "Бронювання квитка", emailTemplate);
        }
    }
}