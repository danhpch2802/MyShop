using System.Collections.Generic;
using System.Windows;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for DetailOrderWindow.xaml
    /// </summary>
    public partial class DetailOrderWindow : Window
    {
        public Order DetailOrder;
        public DetailOrderWindow(Order order, List<DetailOrder> detailOrder)
        {
            InitializeComponent();
            DetailOrder = (Order)order.Clone();
            this.DataContext = DetailOrder;
            detailOrderListView.ItemsSource = detailOrder;
        }

        public void exitBtnEvent(object sender,RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
