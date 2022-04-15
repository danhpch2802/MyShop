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
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategory : Window
    {
        SqlDataAccess dao;
        Business _bus = null;

        public AddCategory()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            string catName = categoryName.Text;
            var categories = new Category()
            {
                Name = catName
            };
            dao.GetCategories().Add(categories);
            dao.addCategoryToDatabase(categories);
            MessageBox.Show("Category has been added!");
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
