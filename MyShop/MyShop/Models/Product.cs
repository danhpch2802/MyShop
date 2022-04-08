using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop
{
    public class Category: INotifyPropertyChanged
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
    public class Product: INotifyPropertyChanged,ICloneable
    {
        public int productID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public int Amount { get; set; }
        public Category Category { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
