using Microsoft.Win32;
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
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        SqlDataAccess dao;
        Business _bus = null;
        public AddProduct()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            string Name = productNameTextBox.Text;
            int Price = int.Parse(productPriceTextBox.Text);
            string Category = productCategoryTextBox.Text;
            int Amount = int.Parse(productAmountTextBox.Text);
            string Image = productImageTextBox.Text;
            var categories = new Category()
            {
                Name = Category
            };
            var p = new Product()
            {
                Name = Name,
                Price = Price,
                Category = categories,
                Image = Image,
                Amount = Amount
            };
            dao.addDataToDatabase(p);
            MessageBox.Show("Product has been added!");
        }

        private void browseImage_Click(object sender, RoutedEventArgs e)
        {
            //Create a new instance of openFileDialog
            OpenFileDialog res = new OpenFileDialog();

            //Filter
            res.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";

            //When the user select the file
            if (res.ShowDialog() == true)
            {
                var filePath = res.FileName;
                productImageTextBox.Text = filePath;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string? connectionString = AppConfig.ConnectionString();
            dao = new SqlDataAccess(connectionString!);

            if (dao.CanConnect())
            {
                dao.Disconnect();
                dao.Connect();
                _bus = new Business(dao);
            }
            else
            {
                MessageBox.Show("Cannot connect to db");
            }
        }
    }
}
