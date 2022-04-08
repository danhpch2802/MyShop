using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop
{
    public class DetailOrder
    {
        public int OrderID { get; set; }
        public int ProductId { get; set; }
        public string ProductImg { get; set; }
        public string ProductCat { get; set; }
        public string ProductName { get; set; }
        public int Quantity  {get; set; }
        public int Total { get; set; }

    }
}
