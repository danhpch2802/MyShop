using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Order> FilterOrders { get; set; } = new List<Order>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public static OrderViewModel loadOrders(List<Order> orders)
        {
            OrderViewModel _vm=new OrderViewModel();

            _vm.Orders = orders;
            _vm.FilterOrders = orders;


        return _vm;

        }
    }
}
