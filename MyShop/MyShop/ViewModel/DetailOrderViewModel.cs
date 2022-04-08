using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop
{
    public class DetailOrderViewModel : INotifyPropertyChanged,ICloneable
    {
        public List<DetailOrder> Orders { get; set; } = new List<DetailOrder>();
        public List<DetailOrder> ViewOrders { get; set; } = new List<DetailOrder>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
