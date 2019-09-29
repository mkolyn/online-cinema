using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;

namespace CinemaTickets.Models
{
    public class Liqpay
    {
        public const int API_VERSION = 3;
        public const string API_PUBLIC_KEY = "sandbox_i70172923559";
        //public const string API_PUBLIC_KEY = "i97066083689";
        public const string API_PRIVATE_KEY = "sandbox_MeyyfO2pqDBDMfbHHKJ6cYj2fCU0dXG7UOD3Cniv";
        //public const string API_PRIVATE_KEY = "t6rz6E8naH9arGVCCgOekPsA8f8ANHLlOvIYNrOV";
        public const string API_ACTION = "pay";
        public const string API_CURRENCY = "UAH";

        // liqpay verion
        public int version { get; set; }
        // liqpay public key
        public string public_key { get; set; }
        // liqpay private key
        public string private_key { get; set; }
        // liqpay action
        public string action { get; set; }
        // liqpay payment amount
        public int amount { get; set; }
        // liqpay payment currency
        public string currency { get; set; }
        // liqpay payment description
        public string description { get; set; }
        // liqpay payment website order id
        public string order_id { get; set; }
        // liqpay test payment
        public int sandbox { get; set; }
        // liqpay server url
        public string server_url { get; set; }

        public Liqpay(int amount, string orderId, string description, string serverUrl, int sandbox = 0)
        {
            version = API_VERSION;
            public_key = API_PUBLIC_KEY;
            private_key = API_PRIVATE_KEY;
            action = API_ACTION;
            currency = API_CURRENCY;
            this.amount = amount;
            order_id = orderId;
            this.description = description;
            server_url = serverUrl;

            if (sandbox == 1)
            {
                this.sandbox = sandbox;
            }
        }

        public static string GetSignature(string liqpayData)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] encodedData = sha.ComputeHash(Encoding.UTF8.GetBytes(API_PRIVATE_KEY + liqpayData + API_PRIVATE_KEY));
            return Convert.ToBase64String(encodedData);
        }

        public object GetData()
        {
            return new
            {
                public_key,
                version,
                action,
                amount,
                currency,
                description,
                order_id,
                server_url
            };
        }
    }
}