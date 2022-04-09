using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop { 
    internal class AppConfig
    {
        public static string Server = "Server";
        public static string LastScreen = "Screen";
        public static string Instance = "Instance";
        public static string Database = "Database";
        public static string Username = "Username";
        public static string Password = "Password";
        public static string Entropy = "Entropy";
        public static string? getValue(string key)
        {
            string? value = ConfigurationManager.AppSettings[key];
            return value;
        }
        public static void setValue(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings[key].Value = value;
            configFile.Save(ConfigurationSaveMode.Minimal);
        }
        public static string? ConnectionString()
        {
            string? result = "";

            var builder = new SqlConnectionStringBuilder();
            string? server = AppConfig.getValue(AppConfig.Server);
            string? lastscreen = AppConfig.getValue(AppConfig.LastScreen);
            string? instance = AppConfig.getValue(AppConfig.Instance);
            string? database = AppConfig.getValue(AppConfig.Database);
            string? username = AppConfig.getValue(AppConfig.Username);
            string? password = AppConfig.getValue(AppConfig.Password);


            builder.DataSource = $"{server}\\{instance}";
            builder.InitialCatalog = database;
            builder.IntegratedSecurity = true;
            builder.ConnectTimeout = 3; // s

            result = builder.ToString();
            return result;
        }
    }
}
