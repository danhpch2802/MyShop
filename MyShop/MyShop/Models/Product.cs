using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop
{
    internal class Category
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
    internal class Product
    {
        public int productID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public int Amount { get; set; }
        public Category Category { get; set; }
    }
}
