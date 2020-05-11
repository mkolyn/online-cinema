namespace CinemaTickets.Models
{
    public class Config
    {
        public const bool SITE_DISABLED = false;
        public const bool DEBUG = true;
        public const string ALLOW_FROM_IP_ONLY = "";

        public const int CONFIRM_PAYMENT_MINUTES_TIMEOUT = 15;
        public const int QR_CODE_SIZE = 10;
        public const int DEFAULT_CITY_ID = 1;

        public const string SMTP_HOST = "scp.realhost.pro";
        public const string SMTP_USER = "info@cinematickets.com.ua";
        public const string SMTP_PASSWORD = "ePsOnStC87";
        public const string SMTP_FROM = "info@cinematickets.com.ua";
        public const int SMTP_PORT = 465;
        public const bool SMTP_SSL = true;

        public const string migrationDbDataSource = "DESKTOP-NVEBCLL";
        public const string migrationDbName = "CinemaTickets";
        public const string migrationDbUserId = "mkolyn_admin";
        public const string migrationDbUserPassword = "223973";
        public const string migrationDbConnectTimeout = "10";
    }
}