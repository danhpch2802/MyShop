using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop
{
    public class RevenueSeries: INotifyPropertyChanged
    {
        public List<DateTime> Date { get; set; }
        public List<int> Revenue { get; set; }
        public List<int> Profit { get; set; }
        public List<int> Year { get; set; }
        public List<int> Month { get; set; }
        public List<int> Week { get; set; }
        public List<int> Quantity { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
