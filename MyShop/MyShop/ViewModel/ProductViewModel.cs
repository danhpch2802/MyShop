using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ViewModel
{
    internal class ProductViewModel : INotifyPropertyChanged
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Product> SelectedProducts { get; set; } = new List<Product>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public static ProductViewModel loadProducts(List<Product> products)
        {
            ProductViewModel _vm = new ProductViewModel();

            _vm.Products = products;

            return _vm;

        }
    }
}
