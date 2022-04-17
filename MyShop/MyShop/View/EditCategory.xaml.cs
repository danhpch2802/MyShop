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
    /// Interaction logic for EditCategory.xaml
    /// </summary>
    public partial class EditCategory : Window
    {
        SqlDataAccess dao;
        Business _bus = null;
        CategoryViewModel category_vm;
        public EditCategory()
        {
            InitializeComponent();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            Category oldCategory = (Category)categoriesComboBox.SelectedItem;
            var newName = newCategoryName.Text;
            dao.editCategoryInDatabase(oldCategory, newName);
            MessageBox.Show("Category has been updated");

            DialogResult = true;
        }
        List<Category> categories;
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
            }
            else
            {
                MessageBox.Show("Cannot connect to db");
            }
        }
    }
}
