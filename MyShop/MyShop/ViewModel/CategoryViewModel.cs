using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        public List<Category> Categories { get; set; } = new List<Category>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public static CategoryViewModel loadCategories(List<Category> categories)
        {
            CategoryViewModel _vm = new CategoryViewModel();

            _vm.Categories = categories;

            return _vm;

        }
    }
}
