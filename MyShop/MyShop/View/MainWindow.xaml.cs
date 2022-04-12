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

using Microsoft.Win32;
using Aspose.Cells;
using System.ComponentModel;
using LiveCharts;
using LiveCharts.Wpf;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        Business _bus = null;
        
        SqlDataAccess dao;
        DashViewModel dashViewModel;
        public MainWindow()
        {
            InitializeComponent();
        }

        // Order tab start here
        OrderViewModel orders_vm;
        List<Product> products;
        BindingList<Product> top;
        int _totalOrder = 0;
        int _currentOrderPage = 0;
        int _totalOrderPages = 0;
        int _rowsOrderPerPage = 10;
        List<Order> orders;
        private void Load_Order()
        {
            string? connectionString = AppConfig.ConnectionString();
            dao = new SqlDataAccess(connectionString!);

            if (dao.CanConnect())
            {
                dao.Connect();
                _bus = new Business(dao);

                orders = _bus.GetOrders();
                orders_vm = OrderViewModel.loadOrders(orders);

                orders_vm.FilterOrders = orders_vm.Orders
                    .Skip((_currentOrderPage - 1) * _rowsOrderPerPage)
                    .Take(_rowsOrderPerPage)
                    .ToList();
                _currentOrderPage = 1;

                _totalOrder = orders_vm.Orders.Count;
                _totalOrderPages = orders_vm.Orders.Count / _rowsOrderPerPage +
                    (orders_vm.Orders.Count % _rowsOrderPerPage == 0 ? 0 : 1);
                currentOrderPagingTextBlock.Text = $"{_currentOrderPage}/{_totalOrderPages}";

                orderDateGrid.ItemsSource = orders_vm.FilterOrders;
            }
            else
            {
                MessageBox.Show("Cannot connect to db");
            }
        }
        private void filterClick(object sender, RoutedEventArgs e)
        {
            var filterWindow = new FilterWindow();

            filterWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            filterWindow.Owner = this;
            var result = filterWindow.ShowDialog();
            if (result == true)
            {
                var startDP = filterWindow.startDate;
                var endDP = filterWindow.endDate;
                orders_vm.FilterOrders = orders_vm.Orders.FindAll(x => startDP <= x.OrderDate && x.OrderDate <= endDP);
                orders_vm.FilterOrders = orders_vm.FilterOrders
                                .Skip((_currentOrderPage - 1) * _rowsOrderPerPage)
                                .Take(_rowsOrderPerPage)
                                .ToList();
                _currentOrderPage = 1;

                _totalOrder = orders_vm.FilterOrders.Count;
                _totalOrderPages = orders_vm.FilterOrders.Count / _rowsOrderPerPage +
                    (orders_vm.Orders.Count % _rowsOrderPerPage == 0 ? 0 : 1);
                currentOrderPagingTextBlock.Text = $"{_currentOrderPage}/{_totalOrderPages}";
                orderDateGrid.ItemsSource = orders_vm.FilterOrders;
            }
        }

        private void addNewOrderClick(object sender, RoutedEventArgs e)
        {
            var id = _bus.getNewestOrderID();
            var GetProduct = _bus.GetProducts();
            var product_vm = ProductViewModel.loadProducts(GetProduct);
            var AddOrderWindow = new AddOrderWindow(product_vm, id);

            AddOrderWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            AddOrderWindow.Owner = this;
            this.Hide();
            var result = AddOrderWindow.ShowDialog();

            if (result == true)
            {
                this.Show();
                if (AddOrderWindow.CreateDetailOrder.Count == 0)
                {
                    MessageBox.Show("Empty product! Abort create new order!");
                }
                else
                {
                    var NewOrder = AddOrderWindow.CreateOrder;
                    var ListDetailOrder = AddOrderWindow.CreateDetailOrder;

                    _bus.addNewOrder(NewOrder);
                    _bus.updateProductQuantity(ListDetailOrder);
                    _bus.addNewDetailOrders(ListDetailOrder);

                    List<Order> orders = _bus.GetOrders();
                    orders_vm = OrderViewModel.loadOrders(orders);

                    
                    //DashBoard
                    List<Product> products = _bus.GetProducts();
                    topProductsList.ItemsSource = _bus.GetTopProducts();
                    dashViewModel = DashViewModel.load(orders, products);
                    this.DataContext = dashViewModel;
                    //
                    orders_vm.FilterOrders = orders_vm.Orders


                .Skip((_currentOrderPage - 1) * _rowsOrderPerPage)
                .Take(_rowsOrderPerPage)
                .ToList();
            _currentOrderPage = 1;

            _totalOrder = orders_vm.Orders.Count;
            _totalOrderPages = orders_vm.Orders.Count / _rowsOrderPerPage +
                (orders_vm.Orders.Count % _rowsOrderPerPage == 0 ? 0 : 1);
            currentOrderPagingTextBlock.Text = $"{_currentOrderPage}/{_totalOrderPages}";
            orderDateGrid.ItemsSource = orders_vm.FilterOrders;
                }
            }
            else
            {
                this.Show();
            }
        }

        private void refreshClick(object sender, RoutedEventArgs e)
        {
            List<Order> orders = _bus.GetOrders();
            orders_vm = OrderViewModel.loadOrders(orders);

            orders_vm.FilterOrders = orders_vm.Orders
                .Skip((_currentOrderPage - 1) * _rowsOrderPerPage)
                .Take(_rowsOrderPerPage)
                .ToList();
            _currentOrderPage = 1;

            _totalOrder = orders_vm.Orders.Count;
            _totalOrderPages = orders_vm.Orders.Count / _rowsOrderPerPage +
                (orders_vm.Orders.Count % _rowsOrderPerPage == 0 ? 0 : 1);
            currentOrderPagingTextBlock.Text = $"{_currentOrderPage}/{_totalOrderPages}";
            orderDateGrid.ItemsSource = orders_vm.FilterOrders;
        }

        private void deleteOrderClick(object sender, RoutedEventArgs e)
        {
            var order = (Order)orderDateGrid.SelectedItem;
            orders_vm.Orders.Remove(order);
            orderDateGrid.Items.Refresh();
            _bus.removeOrder(order);

            orders_vm.FilterOrders = orders_vm.Orders
                .Skip((_currentOrderPage - 1) * _rowsOrderPerPage)
                .Take(_rowsOrderPerPage)
                .ToList();
            _currentOrderPage = 1;

            _totalOrder = orders_vm.Orders.Count;
            _totalOrderPages = orders_vm.Orders.Count / _rowsOrderPerPage +
                (orders_vm.Orders.Count % _rowsOrderPerPage == 0 ? 0 : 1);
            currentOrderPagingTextBlock.Text = $"{_currentOrderPage}/{_totalOrderPages}";
            orderDateGrid.ItemsSource = orders_vm.FilterOrders;

        }

        private void editOrderClick(object sender, RoutedEventArgs e)
        {
            var Order = (Order)orderDateGrid.SelectedItem;
            var GetProduct = _bus.GetProducts();
            var product_vm = ProductViewModel.loadProducts(GetProduct);
            var DetailOrder = _bus.loadDetailOrdersfromID(Order);
            var EditOrderWindow = new EditOrderWindow(product_vm, Order, DetailOrder);

            EditOrderWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            EditOrderWindow.Owner = this;
            this.Hide();
            var result = EditOrderWindow.ShowDialog();

            if (result == true)
            {
                this.Show();
                var UpdateOrder = EditOrderWindow.EditOrder;
                var ListDetailOrder = EditOrderWindow.EditDetailOrder;

                _bus.updateOrder(UpdateOrder);
                _bus.updateProductQuantityRemovedOrder(ListDetailOrder, UpdateOrder);
                _bus.updateProductQuantity(ListDetailOrder);
                _bus.updateDetailOrder(ListDetailOrder);

                List<Order> orders = _bus.GetOrders();
                orders_vm = OrderViewModel.loadOrders(orders);

                orders_vm.FilterOrders = orders_vm.Orders
                                .Skip((_currentOrderPage - 1) * _rowsOrderPerPage)
                                .Take(_rowsOrderPerPage)
                                .ToList();
                _currentOrderPage = 1;

                _totalOrder = orders_vm.Orders.Count;
                _totalOrderPages = orders_vm.Orders.Count / _rowsOrderPerPage +
                    (orders_vm.Orders.Count % _rowsOrderPerPage == 0 ? 0 : 1);
                currentOrderPagingTextBlock.Text = $"{_currentOrderPage}/{_totalOrderPages}";
                orderDateGrid.ItemsSource = orders_vm.FilterOrders;
            }
            else
            {
                this.Show();
            }
        }

        private void viewDetailOrderClick(object sender, MouseButtonEventArgs e)
        {
            var order = (Order)orderDateGrid.SelectedItem;
            List<DetailOrder> detailOrders = _bus.loadDetailOrdersfromID(order);

            var detailOrderWindow = new DetailOrderWindow(order, detailOrders);

            detailOrderWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            detailOrderWindow.Owner = this;
            this.Hide(); ;
            var result = detailOrderWindow.ShowDialog();
            if (result == true)
            {
                this.Show();
            }
        }
        public void prevOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentOrderPage > 1)
                _currentOrderPage--;
            orders_vm.FilterOrders = orders_vm.Orders
                 .Skip((_currentOrderPage - 1) * _rowsOrderPerPage)
                 .Take(_rowsOrderPerPage)
                 .ToList();
            // ép cập nhật giao diện
            currentOrderPagingTextBlock.Text = $"{_currentOrderPage}/{_totalOrderPages}";
            orderDateGrid.ItemsSource = orders_vm.FilterOrders;
        }
        public void nextOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentOrderPage < _totalOrderPages)
                _currentOrderPage++;
            orders_vm.FilterOrders = orders_vm.Orders
                 .Skip((_currentOrderPage-1) * _rowsOrderPerPage)
                 .Take(_rowsOrderPerPage)
                 .ToList();
            // ép cập nhật giao diện
            currentOrderPagingTextBlock.Text = $"{_currentOrderPage}/{_totalOrderPages}";
            orderDateGrid.ItemsSource = orders_vm.FilterOrders;
        }
        //Order tab End here
        //Revenue tab Start here
        //Revenue Chart
        RevenueSeries rs = null;
        
       public void DateRevenueChart(object sender, RoutedEventArgs e)
        {
            var RevenueChartDateWindow = new RevenueChartDateWindow();

            RevenueChartDateWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            RevenueChartDateWindow.Owner = this;
            var result = RevenueChartDateWindow.ShowDialog();
            if (result == true)
            {
                var startDP = RevenueChartDateWindow.startDate;
                var endDP = RevenueChartDateWindow.endDate;
                rs = _bus.loadRevenueDate(startDP, endDP);

                var series1 = new ColumnSeries
                {
                    Title = "Revenue",
                    Values = new LiveCharts.ChartValues<int>(rs.Revenue)
                };

                var series2 = new LineSeries
                {
                    Title = "Profit",
                    Values = new LiveCharts.ChartValues<int>(rs.Profit)
                };

                RevenueChart.Series.Clear();
                RevenueChart.Series.Add(series1);
                RevenueChart.Series.Add(series2);
                var formatDate = new List<string>();
                foreach (var date in rs.Date)
                {
                    var onlyDate = date.Date;
                    formatDate.Add(onlyDate.ToString("d"));
                }
                axisLabel.Labels = formatDate;
            }
        }
        public void WeekRevenueChart(object sender, RoutedEventArgs e)
        {
            var RevenueChartWeekWindow = new WeekRevenueWindow();

            RevenueChartWeekWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            RevenueChartWeekWindow.Owner = this;
            var result = RevenueChartWeekWindow.ShowDialog();
            if (result == true)
            {
                var Month = RevenueChartWeekWindow.Month;
                var Year = RevenueChartWeekWindow.Year;
                rs = _bus.loadRevenueWeek(Month, Year);

                var series1 = new ColumnSeries
                {
                    Title = "Revenue",
                    Values = new LiveCharts.ChartValues<int>(rs.Revenue)
                };

                var series2 = new LineSeries
                {
                    Title = "Profit",
                    Values = new LiveCharts.ChartValues<int>(rs.Profit)
                };

                RevenueChart.Series.Clear();
                RevenueChart.Series.Add(series1);
                RevenueChart.Series.Add(series2);
                var formatDate = new List<string>();
                foreach (var date in rs.Week)
                {
                    formatDate.Add(date.ToString());
                }
                axisLabel.Labels = formatDate;
            }
        }

        public void MonthRevenueChart(object sender, RoutedEventArgs e)
        {
            var RevenueChartMonthWindow = new MonthRevenueWindow();

            RevenueChartMonthWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            RevenueChartMonthWindow.Owner = this;
            var result = RevenueChartMonthWindow.ShowDialog();
            if (result == true)
            {
                var Year = RevenueChartMonthWindow.Year;
                rs = _bus.loadRevenueMonth( Year);

                var series1 = new ColumnSeries
                {
                    Title = "Revenue",
                    Values = new LiveCharts.ChartValues<int>(rs.Revenue)
                };

                var series2 = new LineSeries
                {
                    Title = "Profit",
                    Values = new LiveCharts.ChartValues<int>(rs.Profit)
                };

                RevenueChart.Series.Clear();
                RevenueChart.Series.Add(series1);
                RevenueChart.Series.Add(series2);
                var formatDate = new List<string>();
                foreach (var date in rs.Month)
                {
                    formatDate.Add(date.ToString());
                }
                axisLabel.Labels = formatDate;
            }
        }
        public void YearRevenueChart(object sender,RoutedEventArgs e)
        {
            rs = _bus.loadRevenueYear();

            var series1 = new ColumnSeries
            {
                Title = "Revenue",
                Values = new LiveCharts.ChartValues<int>(rs.Revenue)
            };

            var series2 = new LineSeries
            {
                Title = "Profit",
                Values = new LiveCharts.ChartValues<int>(rs.Profit)
            };

            RevenueChart.Series.Clear();
            RevenueChart.Series.Add(series1);
            RevenueChart.Series.Add(series2);
            var formatDate = new List<string>();
            foreach (var date in rs.Year)
            {
                formatDate.Add(date.ToString());
            }
            axisLabel.Labels = formatDate;
        }
        //Product Chart
        public void DateProductChart(object sender, RoutedEventArgs e)
        {
            var GetProduct = _bus.GetProducts();
            var product_vm = ProductViewModel.loadProducts(GetProduct);
            var ProductChartDateWindow = new ProductChartDateWindow(product_vm);
            ProductChartDateWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ProductChartDateWindow.Owner = this;
            var result = ProductChartDateWindow.ShowDialog();
            if (result == true)
            {
                var startDP = ProductChartDateWindow.startDate;
                var endDP = ProductChartDateWindow.endDate;
                var product = ProductChartDateWindow.selectProduct;
                rs = _bus.loadProductDate(startDP, endDP, product);

                var series1 = new LineSeries
                {
                    Title = "Sold",
                    Values = new LiveCharts.ChartValues<int>(rs.Quantity)
                };
                
                RevenueChart.Series.Clear();
                RevenueChart.Series.Add(series1);
                var formatDate = new List<string>();
                foreach (var date in rs.Date)
                {
                    var onlyDate = date.Date;
                    formatDate.Add(onlyDate.ToString("d"));
                }
                axisLabel.Labels = formatDate;
            }
        }
        public void WeekProductChart(object sender, RoutedEventArgs e)
        {
            var GetProduct = _bus.GetProducts();
            var product_vm = ProductViewModel.loadProducts(GetProduct);
            var ProductChartWeekWindow = new ProductChartWeekWindow(product_vm);
            ProductChartWeekWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ProductChartWeekWindow.Owner = this;
            var result = ProductChartWeekWindow.ShowDialog();
            if (result == true)
            {
                var Month = ProductChartWeekWindow.Month;
                var Year = ProductChartWeekWindow.Year;
                var product = ProductChartWeekWindow.selectProduct;
                rs = _bus.loadProductWeek(Month, Year, product);

                var series1 = new LineSeries
                {
                    Title = "Sold",
                    Values = new LiveCharts.ChartValues<int>(rs.Quantity)
                };

                RevenueChart.Series.Clear();
                RevenueChart.Series.Add(series1);
                var formatDate = new List<string>();
                foreach (var date in rs.Week)
                {
                    formatDate.Add(date.ToString());
                }
                axisLabel.Labels = formatDate;
            }
        }
        public void MonthProductChart(object sender,RoutedEventArgs e)
        {
            var GetProduct = _bus.GetProducts();
            var product_vm = ProductViewModel.loadProducts(GetProduct);
            var ProductChartMonthWindow = new ProductChartMonthWindow(product_vm);
            ProductChartMonthWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ProductChartMonthWindow.Owner = this;
            var result = ProductChartMonthWindow.ShowDialog();
            if (result == true)
            {
                var Year = ProductChartMonthWindow.Year;
                var product = ProductChartMonthWindow.selectProduct;
                rs = _bus.loadProductMonth(Year, product);

                var series1 = new LineSeries
                {
                    Title = "Sold",
                    Values = new LiveCharts.ChartValues<int>(rs.Quantity)
                };

                RevenueChart.Series.Clear();
                RevenueChart.Series.Add(series1);
                var formatDate = new List<string>();
                foreach (var date in rs.Month)
                {
                    formatDate.Add(date.ToString());
                }
                axisLabel.Labels = formatDate;
            }
        }
        public void YearProductChart(object sender, RoutedEventArgs e)
        {
            var GetProduct = _bus.GetProducts();
            var product_vm = ProductViewModel.loadProducts(GetProduct);
            var ProductChartYearWindow = new ProductChartYearWindow(product_vm);
            ProductChartYearWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ProductChartYearWindow.Owner = this;
            var result = ProductChartYearWindow.ShowDialog();
            if (result == true)
            {
                var product = ProductChartYearWindow.selectProduct;
                rs = _bus.loadProductYear(product);

                var series1 = new LineSeries
                {
                    Title = "Sold",
                    Values = new LiveCharts.ChartValues<int>(rs.Quantity)
                };

                RevenueChart.Series.Clear();
                RevenueChart.Series.Add(series1);
                var formatDate = new List<string>();
                foreach (var date in rs.Year)
                {
                    formatDate.Add(date.ToString());
                }
                axisLabel.Labels = formatDate;
            }
        }
        //Revenue tab End here

        //Product
        private void Load_Product() //dao da duoc khai bao o tren
        {
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                _bus = new Business(dao);
                products = _bus.GetProducts();
                top = _bus.GetTopProducts();
                _vm = ProductViewModel.loadProducts(products);
                productsListView.ItemsSource = _vm.Products; // Chua phan trang
            }
        }
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
            var dao = new SqlDataAccess(connectionString!); // dao gọi rồi
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

        //Window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Load_Order();
            Load_Product();
            dashViewModel = DashViewModel.load(orders, products);
            topProductsList.ItemsSource = top;
            this.DataContext = dashViewModel;
            RevenueChart.Series = new SeriesCollection();
            
            var password = "";
            var username = "";
            int lastScreen = 0;
            try
            {
                var cypherText = AppConfig.getValue(AppConfig.Password);
                var cypherTextInBytes = Convert.FromBase64String(cypherText!);

                var entropyText = AppConfig.getValue(AppConfig.Entropy);
                var entropyTextInBytes = Convert.FromBase64String(entropyText);

                var passwordInBytes = ProtectedData.Unprotect(cypherTextInBytes,
                    entropyTextInBytes, DataProtectionScope.CurrentUser);

                password = Encoding.UTF8.GetString(passwordInBytes);
                username = AppConfig.getValue(AppConfig.Username);
                lastScreen = Int32.Parse(AppConfig.getValue(AppConfig.LastScreen));
                

            }
            catch (Exception ex)
            {
            }

            var screen = new LoginWindow(username, password);
            this.Hide();
            screen.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            screen.Owner = this;
            var result = screen.ShowDialog();

            ribbon.SelectedTabIndex = lastScreen;

            if (result == true)
            {
                this.Show();

                var passwordInBytesSave = Encoding.UTF8.GetBytes(screen.pass);

                var entropy = new byte[20];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(entropy);
                }
                var entropyBase64 = Convert.ToBase64String(entropy);

                var cypherTextSave = ProtectedData.Protect(passwordInBytesSave, entropy,
                    DataProtectionScope.CurrentUser);
                var cypherTextBase64 = Convert.ToBase64String(cypherTextSave);
                AppConfig.setValue(AppConfig.Username, screen.name);
                AppConfig.setValue(AppConfig.Password, cypherTextBase64);
                AppConfig.setValue(AppConfig.Entropy, entropyBase64);
                
            }
        }

        
        

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            AppConfig.setValue(AppConfig.LastScreen, ribbon.SelectedTabIndex+"");
            dao.Disconnect();
        }

        private void ribbon_SelectedTabChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ribbon.SelectedTabIndex == 0)
            {
               
            }
        }
    }
}
