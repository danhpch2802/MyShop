using System.Collections.Generic;
using System.Windows;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for DetailOrderWindow.xaml
    /// </summary>
    public partial class DetailOrderWindow : Window
    {
        public DetailOrderViewModel DetailViewModel;

        public DetailOrderWindow(Order order, List<DetailOrder> detailOrder)
        {
            InitializeComponent();
            DetailViewModel = new DetailOrderViewModel();
            DetailViewModel.Orders = detailOrder;
            this.DataContext = order;
            detailOrderListView.ItemsSource = DetailViewModel.Orders;
        }

        public void exitBtnEvent(object sender,RoutedEventArgs e)
        {
            DialogResult = true;
        }

    }
}
