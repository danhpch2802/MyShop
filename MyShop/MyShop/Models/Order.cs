using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop
{
    public class Order:ICloneable,INotifyPropertyChanged
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderTotal { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
