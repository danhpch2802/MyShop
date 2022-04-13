using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MyShop
{
    public class Category : INotifyPropertyChanged
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}