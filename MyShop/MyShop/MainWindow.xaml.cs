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
using Microsoft.Win32;
using Aspose.Cells;

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

                orderDateGrid.ItemsSource = orders;
            }
            else
            {
                MessageBox.Show("Cannot connect to db");
            }
        }

        private void Load_Product()
        {
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                _bus = new Business(dao);
                List<Product> products = _bus.GetProducts();
                var products_vm = ProductViewModel.loadProducts(products);
                productsListView.ItemsSource = products;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Load_Order();
            Load_Product();
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

        // Product Tab
        List<Category>? _categories = null; // Không biết fix sao
        ProductViewModel _vm = new ProductViewModel();
        int _totalItems = 0;
        int _currentPage = 0;
        int _totalPages = 0;
        int _rowsPerPage = 10;
        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
                _currentPage++;
            _vm.SelectedProducts = _vm.Products
                    .Skip((_currentPage - 1) * _rowsPerPage)
                    .Take(_rowsPerPage)
                    .ToList();
            // ép cập nhật giao diện
            productsListView.ItemsSource = _vm.SelectedProducts;
            currentPagingTextBlock.Text = $"{_currentPage}/{_totalPages}";
        }

        private void loadExcelFile_Click(object sender, RoutedEventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _categories = new List<Category>();
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            var datascreen = new OpenFileDialog();
            if (datascreen.ShowDialog() == true)
            {
                string filename = datascreen.FileName;
                var workbook = new Workbook(filename);
                var tabs = workbook.Worksheets;
                // In ra các tab để d ebug
                foreach (var tab in tabs)
                {
                    Category cat = new Category()
                    {
                        Name = tab.Name,
                        Products = new List<Product>()
                    };
                    var column = 'D';
                    var row = 4;
                    var cell = tab.Cells[$"{column}{row}"];
                    while (cell.Value != null)
                    {
                        // Get data from excel file
                        string name = cell.StringValue;
                        int price = tab.Cells[$"F{row}"].IntValue;
                        string image = tab.Cells[$"C{row}"].StringValue;
                        int amount = tab.Cells[$"G{row}"].IntValue;
                        string pcategory = tab.Cells[$"E{row}"].StringValue;
                        var p = new Product()
                        {
                            Name = name,
                            Image = image,
                            Price = price,
                            Amount = amount,
                            Category = cat,
                        };
                        // Insert data to SQL Database
                        dao.addDataToDatabase(p.Image, p.Name, pcategory, p.Price, p.Amount);
                        cat.Products.Add(p);
                        row++;
                        cell = tab.Cells[$"{column}{row}"];
                    }
                    _categories.Add(cat); // Model
                }
            }
            categoriesComboBox.ItemsSource = _categories;
            productsListView.ItemsSource = _vm.SelectedProducts;
        }

        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
                _currentPage--;
            _vm.SelectedProducts = _vm.Products
                    .Skip((_currentPage - 1) * _rowsPerPage)
                    .Take(_rowsPerPage)
                    .ToList();
            // ép cập nhật giao diện
            productsListView.ItemsSource = _vm.SelectedProducts;
            currentPagingTextBlock.Text = $"{_currentPage}/{_totalPages}";
        }

        private void categoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Cập nhật lại thông tin phân trang
            int i = categoriesComboBox.SelectedIndex;
            if (i >= 0)
            {
                // Thay đổi view model
                _vm.Products = _categories![i].Products;
                _vm.SelectedProducts = _vm.Products
                    .Skip((_currentPage - 1) * _rowsPerPage)
                    .Take(_rowsPerPage)
                    .ToList();
                _currentPage = 1; // Quay lại trang đầu tiên
                // Tính toán lại thông số phân trang
                _totalItems = _vm.Products.Count;
                _totalPages = _vm.Products.Count / _rowsPerPage +
                    (_vm.Products.Count % _rowsPerPage == 0 ? 0 : 1);
                currentPagingTextBlock.Text = $"{_currentPage}/{_totalPages}";
                // ép cập nhật giao diện
                productsListView.ItemsSource = _vm.SelectedProducts;
            }
        }
        private void addNewProductManager(object sender, RoutedEventArgs e)
        {
            AddProduct addWindow = new AddProduct();
            addWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addWindow.Show();
        }
    }
}
