using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;

namespace CinemaTickets.Models
{
    public class LiqpayData
    {
        public string public_key;
        public int version;
        public string action;
        public float amount;
        public string currency;
        public string description;
        public string order_id;
        public string server_url;
        public string result_url;
        public string liqpay_order_id;
        public int payment_id;
        public string status;
        public string err_code;
        public string err_description;
        public string expired_date;
    }

    public class ApiData
    {
        public string publicKey;
        public string privateKey;
        public bool isTestPayment;
    }

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
        // liqpay result url
        public string result_url { get; set; }
        // liqpay expired date
        public string expired_date { get; set; }

        public Liqpay(int amount, string orderId, string description, string serverUrl, string resultUrl, string expiredDate, ApiData apiData)
        {
            version = API_VERSION;
            public_key = apiData.publicKey;
            private_key = apiData.privateKey;
            action = API_ACTION;
            currency = API_CURRENCY;
            this.amount = amount;
            order_id = orderId;
            this.description = description;
            server_url = serverUrl;
            result_url = resultUrl;
            expired_date = expiredDate;

            if (apiData.isTestPayment)
            {
                this.sandbox = sandbox;
            }
        }

        public static string GetSignature(string liqpayData)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] encodedData = sha.ComputeHash(Encoding.UTF8.GetBytes(liqpayData));
            return Convert.ToBase64String(encodedData);
        }

        public LiqpayData GetData()
        {
            LiqpayData liqpayData = new LiqpayData
            {
                public_key = public_key,
                action = action,
                version = version,
                amount = amount,
                currency = currency,
                description = description,
                order_id = order_id,
                server_url = server_url,
                result_url = result_url,
                expired_date = expired_date,
            };

            return liqpayData;
        }
    }
}