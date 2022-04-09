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
    /// Interaction logic for RevenueChartDateWindow.xaml
    /// </summary>
    public partial class RevenueChartDateWindow : Window
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public RevenueChartDateWindow()
        {
            InitializeComponent();
        }
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (startDP.SelectedDate == null || endDP.SelectedDate == null)
            {
                MessageBox.Show("Please choose both start and end date");
            }

            else
            {
                startDate = (DateTime)startDP.SelectedDate;
                endDate = (DateTime)endDP.SelectedDate;
                DialogResult = true;
            }
        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;

        }
    }
}
