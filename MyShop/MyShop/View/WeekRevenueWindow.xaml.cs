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
    /// Interaction logic for WeekRevenueWindow.xaml
    /// </summary>
    public partial class WeekRevenueWindow : Window
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public WeekRevenueWindow()
        {
            InitializeComponent();
        }
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (_YearIntegerUpDown.Value == null || _MonthIntergerUpDown.Value == null)
            {
                MessageBox.Show("Please choose month and year");
            }

            else
            {
                Year = (int)_YearIntegerUpDown.Value;
                Month = (int)_MonthIntergerUpDown.Value;
                DialogResult = true;
            }
        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;

        }
    }
}
