using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyShop
{
    class DashViewModel : INotifyPropertyChanged
    {
        public string ordersInWeek { get; set; } = "12";
        public string ordersInMonth { get; set; } = "12";

        public string totalProduct { get; set; } = "12";
        public List<Product> SelectedProducts { get; set; } = new List<Product>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public static DashViewModel load(List<Order> orders, List<Product> products)
        {
            DashViewModel _vm = new DashViewModel();

            _vm.ordersInWeek = countOrderInWeek(orders) + "";
            _vm.ordersInMonth = countOrderInMonth(orders) + "";
            _vm.totalProduct = countProduct(products) + "";
            return _vm;

        }
        private static bool DatesAreInTheSameWeek(DateTime date1)
        {
            DateTime today = DateTime.Now;
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
            var d2 = today.Date.AddDays(-1 * (int)cal.GetDayOfWeek(today));

            return d1 == d2;
        }
        private static bool DatesAreInTheSameMonth(DateTime date1)
        {
            DateTime today = DateTime.Now;
           
            return date1.Month == today.Month;
        }

        private static int countOrderInWeek(List<Order> orders)
        {
            int count = 0;
            foreach (var i in orders)
            {
                if (DatesAreInTheSameWeek(i.OrderDate))
                {
                    count++;
                }

            }
            return count;

        }
        private static int countOrderInMonth(List<Order> orders)
        {
            int count = 0;
            foreach (var i in orders)
            {
                if (DatesAreInTheSameMonth(i.OrderDate))
                {
                    count++;
                }

            }
            return count;

        }
        private static int countProduct(List<Product> products)
        {
            int count = 0;
            foreach (var i in products)
            {

                count += i.Amount;


            }
            return count;

        }
    }
}
