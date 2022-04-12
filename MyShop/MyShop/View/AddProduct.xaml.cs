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
        SqlDataAccess _dao;
        public AddProduct()
        {
            InitializeComponent();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            string Name = productNameTextBox.Text;
            int Price = int.Parse(productPriceTextBox.Text);
            string Category = productCategoryTextBox.Text;
            int Amount = int.Parse(productAmountTextBox.Text);
            string Image = productImageTextBox.Text;

            _dao.addDataToDatabase(Image, Name, Category, Price, Amount);
        }

        private void browseImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void editCategory(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
