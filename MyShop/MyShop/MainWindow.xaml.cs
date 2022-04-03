using Fluent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        class AppConfig
        {
            public static string Server = "Server";
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
        class SqlDataAccess
        {
            private SqlConnection _connection;
            public SqlDataAccess(string connectionString)
            {
                _connection = new SqlConnection(connectionString);
            }

            public bool CanConnect()
            {
                bool result = true;

                try
                {
                    _connection.Open();
                    _connection.Close();
                }
                catch (Exception ex)
                {
                    result = false;
                }
                return result;
            }

            public void Connect()
            {
                _connection.Open();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            var cypherText = AppConfig.getValue(AppConfig.Password);
            var cypherTextInBytes = Convert.FromBase64String(cypherText!);

            var entropyText = AppConfig.getValue(AppConfig.Entropy);
            var entropyTextInBytes = Convert.FromBase64String(entropyText);

            var passwordInBytes = ProtectedData.Unprotect(cypherTextInBytes,
                entropyTextInBytes, DataProtectionScope.CurrentUser);

            var password = Encoding.UTF8.GetString(passwordInBytes);


            var screen = new LoginWindow(AppConfig.getValue(AppConfig.Username), password);

            screen.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            screen.Owner = this;
            var result = screen.ShowDialog();

            if (result == true)
            {
                

                var passwordInBytesSave = Encoding.UTF8.GetBytes(password);

                var entropy = new byte[20];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(entropy);
                }
                var entropyBase64 = Convert.ToBase64String(entropy);

                var cypherTextSave = ProtectedData.Protect(passwordInBytesSave, entropy,
                    DataProtectionScope.CurrentUser);
                var cypherTextBase64 = Convert.ToBase64String(cypherTextSave);

                AppConfig.setValue(AppConfig.Password, cypherTextBase64);
                AppConfig.setValue(AppConfig.Entropy, entropyBase64);
            }

            
        }
    }
}
