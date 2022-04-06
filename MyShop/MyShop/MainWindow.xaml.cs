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

using MyShop.Logic;
using MyShop.Services;
using MyShop.ViewModel;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        Business _bus = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Load_Order()
        {
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);

            if (dao.CanConnect())
            {
                dao.Connect();
                _bus = new Business(dao);

                List<Order> orders = _bus.GetOrders();
                var orders_vm = OrderViewModel.loadOrders(orders);

                ordersListView.ItemsSource = orders;
            }
            else
            {
                MessageBox.Show("Cannot connect to db");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Load_Order();
            //var password = "";

            //try
            //{
            //    var cypherText = AppConfig.getValue(AppConfig.Password);
            //    var cypherTextInBytes = Convert.FromBase64String(cypherText!);

            //    var entropyText = AppConfig.getValue(AppConfig.Entropy);
            //    var entropyTextInBytes = Convert.FromBase64String(entropyText);

            //    var passwordInBytes = ProtectedData.Unprotect(cypherTextInBytes,
            //        entropyTextInBytes, DataProtectionScope.CurrentUser);
            //    password = Encoding.UTF8.GetString(passwordInBytes);
            //} catch (Exception ex)
            //{
            //  MessageBox.Show(ex.Message);
            //}
            //var screen = new LoginWindow(AppConfig.getValue(AppConfig.Username), password);

            //screen.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //screen.Owner = this;
            //var result = screen.ShowDialog();

            //if (result == true)
            //{
            //    var passwordInBytesSave = Encoding.UTF8.GetBytes(password);

            //    var entropy = new byte[20];
            //    using (var rng = new RNGCryptoServiceProvider())
            //    {
            //        rng.GetBytes(entropy);
            //    }
            //    var entropyBase64 = Convert.ToBase64String(entropy);

            //    var cypherTextSave = ProtectedData.Protect(passwordInBytesSave, entropy,
            //        DataProtectionScope.CurrentUser);
            //    var cypherTextBase64 = Convert.ToBase64String(cypherTextSave);

            //    AppConfig.setValue(AppConfig.Password, cypherTextBase64);
            //    AppConfig.setValue(AppConfig.Entropy, entropyBase64);
            //}
        }


        private void filterClick(object sender, RoutedEventArgs e)
        {
            var screen = new FilterWindow();

            screen.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            screen.Owner = this;
            var result = screen.ShowDialog();
            if (result == true) { }
        }
    }
}
