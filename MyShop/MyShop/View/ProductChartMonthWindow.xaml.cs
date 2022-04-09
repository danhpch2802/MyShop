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
    /// Interaction logic for ProductChartWeekWindow.xaml
    /// </summary>
    public partial class ProductChartMonthWindow : Window
    {
        public int Year { get; set; }
        public Product selectProduct { get; set; }
        public ProductViewModel CloneProducts_vm;
        public ProductChartMonthWindow(ProductViewModel products_vm)
        {
            InitializeComponent();
            CloneProducts_vm = (ProductViewModel)products_vm.Clone();
        }
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProduct();
        }
        //Product Fragment
        public void LoadProduct()
        {
            CloneProducts_vm.SelectedProducts = CloneProducts_vm.Products;
            // ép cập nhật giao diện
            productsListView.ItemsSource = CloneProducts_vm.SelectedProducts;
        }
        public void EnterClicked(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (searchBox.Text != "")
                {
                    var selectedSearch = searchComboBox.SelectedIndex;
                    if (selectedSearch == 0)
                    {
                        try
                        {
                            var id = int.Parse(searchBox.Text);
                            productSearchByID(id);
                        }
                        catch (Exception)
                        {
                            System.Windows.MessageBox.Show("ID must be a number");
                        };
                    }
                    else
                    {
                        productSearchByName(searchBox.Text);
                    }
                }
            }
        }
        public void productSearchByID(int id)
        {
            var product = CloneProducts_vm.Products.Find(x => x.productID == id);
            CloneProducts_vm.SelectedProducts = new List<Product>();
            CloneProducts_vm.SelectedProducts.Add(product);

            productsListView.ItemsSource = CloneProducts_vm.SelectedProducts;
            productsListView.Items.Refresh();
        }
        public void productSearchByName(string name)
        {
            var product = CloneProducts_vm.Products.FindAll(x => x.Name.ToLower().Contains(name.ToLower()));
            CloneProducts_vm.SelectedProducts = product;
            // ép cập nhật giao diện
            productsListView.ItemsSource = CloneProducts_vm.SelectedProducts;
            productsListView.Items.Refresh();
        }
        public void reloadProduct(object sender, RoutedEventArgs e)
        {
            CloneProducts_vm.SelectedProducts = CloneProducts_vm.Products;
            // ép cập nhật giao diện
            productsListView.Items.Refresh();
            productsListView.ItemsSource = CloneProducts_vm.SelectedProducts;
        }
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (_YearIntegerUpDown.Value == null || productsListView.SelectedItem == null)
            {
                MessageBox.Show("Please choose information!");
            }

            else
            {
                Year = (int)_YearIntegerUpDown.Value;
                selectProduct = (Product)productsListView.SelectedItem;
                DialogResult = true;
            }
        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;

        }
    }
}
