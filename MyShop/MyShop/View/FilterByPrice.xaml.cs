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
    /// Interaction logic for FilterByPrice.xaml
    /// </summary>
    public partial class FilterByPrice : Window
    {
        public int fromPrice { get; set; }
        public int toPrice { get; set; }
        public FilterByPrice()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (startPrice == null || endPrice == null)
            {
                MessageBox.Show("Please fill in both fields", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else
            {
                fromPrice = int.Parse(startPrice.Text);
                toPrice = int.Parse(endPrice.Text);
                DialogResult = true;
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
