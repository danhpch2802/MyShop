using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        public Order CreateOrder { get; set; }
        public List<DetailOrder> CreateDetailOrder { get; set; }
        public DetailOrderViewModel DetailOrder_vm { get; set; }
        ProductViewModel products_vm { get; set; }

        int _totalItems = 0;
        int _currentPage = 0;
        int _totalPages = 0;
        int _rowsPerPage = 10;

        public AddOrderWindow(ProductViewModel products_vm, int id)
        {
            InitializeComponent();
            DetailOrder_vm=new DetailOrderViewModel();
            CreateOrder = new Order();
            CreateOrder.OrderID = id;
            this.products_vm= (ProductViewModel?)products_vm.Clone();

            this.DataContext = CreateOrder; ;
            detailOrderListView.ItemsSource = DetailOrder_vm.Orders;
        }
        public void LoadProduct()
        {
            products_vm.SelectedProducts = products_vm.Products
                .Skip((_currentPage - 1) * _rowsPerPage)
                .Take(_rowsPerPage)
                .ToList();
            _currentPage = 1; // Quay lại trang đầu tiên
                              // Tính toán lại thông số phân trang
            _totalItems = products_vm.Products.Count;
            _totalPages = _totalItems / _rowsPerPage +
                (products_vm.Products.Count % _rowsPerPage == 0 ? 0 : 1);
            currentPagingTextBlock.Text = $"{_currentPage}/{_totalPages}";
            // ép cập nhật giao diện
            productsListView.ItemsSource = products_vm.Products;
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateOrder.OrderDate = DateTime.Now;
            LoadProduct();
        }

        private void addProductToOrder(object sender, MouseButtonEventArgs e)
        {
            var product = (Product)productsListView.SelectedItem;
            var existed = DetailOrder_vm.Orders.Find(x => x.Product.productID == product.productID);
            if (product.Amount > 0 || existed.Quantity==product.Amount)
            {
                if (existed==null)
                {
                    DetailOrder_vm.Orders.Add(new DetailOrder
                    {
                        OrderID = CreateOrder.OrderID,
                        Product = product,
                        Quantity = 1,
                        Total = product.Price,
                    });

                }
                else
                {
                    existed.Quantity++;
                    existed.Total=existed.Quantity*existed.Product.Price;
                }
                updateTotal();
                detailOrderListView.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Product out of stock!");
            }

        }
        private void updateTotal()
        {
            CreateOrder.OrderTotal = 0;
            foreach (var DetailOrder in DetailOrder_vm.Orders)
            {
                CreateOrder.OrderTotal += DetailOrder.Total;
            }

        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var product = (Product)productsListView.SelectedItem;
            var existed = DetailOrder_vm.Orders.Find(x => x.Product.productID == product.productID);
            existed.Total = product.Price * existed.Quantity;
            detailOrderListView.Items.Refresh();
            updateTotal();
        }


        private void RemoveProductFromOrder(object sender, RoutedEventArgs e)
        {
            var detail = (DetailOrder)detailOrderListView.SelectedItem;
            DetailOrder_vm.Orders.Remove(detail);
            detailOrderListView.Items.Refresh();
            updateTotal();

        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateDetailOrder = DetailOrder_vm.Orders;
            DialogResult = true;
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;

        }
    }
}
