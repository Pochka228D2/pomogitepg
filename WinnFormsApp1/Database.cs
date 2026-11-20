using MySqlConnector;
using System;

namespace WinnFormsApp1
{
    internal static class Database
    {
        public static MySqlConnection CreateConnection()
        {
            string connectionString = Environment.GetEnvironmentVariable("CHUDO_OBUV_MYSQL")
                ?? "Server=88.218.168.195;Database=isp-41_yakunina_la_chudo_obuv;User ID=isp-41_yakunina_la;Password=isp-41_yakunina_la;";
            return new MySqlConnection(connectionString);
        }
    }
}
