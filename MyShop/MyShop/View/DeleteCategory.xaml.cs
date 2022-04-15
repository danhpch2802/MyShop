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
    /// Interaction logic for DeleteCategory.xaml
    /// </summary>
    public partial class DeleteCategory : Window
    {
        SqlDataAccess dao;
        Business _bus;

        public DeleteCategory()
        {
            InitializeComponent();
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

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            Category selected = (Category)categoriesComboBox.SelectedItem;
            dao.deleteCategoryFromDatabase(selected);
            MessageBox.Show("Category has been deleted");
            DialogResult = true;
        }
    }
}
