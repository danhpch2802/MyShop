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
    /// Interaction logic for EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        SqlDataAccess dao;
        Business _bus = null;

        public Product ProductToEdit { get; set; }

        public EditProductWindow(Product p)
        {
            InitializeComponent();
            ProductToEdit = p;
            this.DataContext = ProductToEdit;
        }

        List<Category> categories;

        private void editButton_Click(object sender, RoutedEventArgs e) 
        {
            MessageBox.Show("Update Successful!", "Update Confirm", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
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
                categories = _bus.GetCategories();
                categoriesComboBox.ItemsSource = categories;

                int countNotFound = 0;

                for (int i = 0; i < categories.Count; i++)
                {
                    if (ProductToEdit.Category.Name == categories[i].Name)
                    {
                        categoriesComboBox.SelectedIndex = i;
                    }
                    else 
                    {
                        countNotFound++;
                    }
                }
                if (countNotFound == categories.Count)
                {
                    MessageBox.Show("Category of this product has been changed or removed.\nPlease choose a different category for this product.", "Category Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            else
            {
                MessageBox.Show("Cannot connect to db");
            }
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
    }
}
