using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCinema.Models
{
    public class LiqpayResult
    {
        public const string STATUS_SUCCESSFULL = "success";
        public const string STATUS_TEST = "sandbox";
        public const string STATUS_FAILED = "failure";
        public const string STATUS_WRONG_DATA = "error";

        // liqpay result ID
        public int ID { get; set; }
        // payment amount
        public int Amount { get; set; }
        // liqpay order id
        public string LiqpayOrderId { get; set; }
        // website order id
        public string OrderId { get; set; }
        // liqpay payment id
        public int PaymentId { get; set; }
        // liqpay status
        public string Status { get; set; }
        // order description
        public string Description { get; set; }
        // liqpay error code
        public string ErrorCode { get; set; }
        // liqpay error description
        public string ErrorDescription { get; set; }
        // liqpay status date
        public DateTime Date { get; set; }
    }

    public class LiqpayResultContext : DbContext
    {
        public DbSet<LiqpayResult> LiqpayResults { get; set; }

        public LiqpayResultContext() : base("name=DefaultConnection")
        {
        }

        public LiqpayResult Get(int orderId)
        {
            var liqpayQuery = from lr in LiqpayResults
                              where lr.OrderId == orderId.ToString()
                              select lr;

            return liqpayQuery.FirstOrDefault();
        }
    }
}