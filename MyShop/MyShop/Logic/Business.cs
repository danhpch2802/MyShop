using MyShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Logic
{
    internal class Business
    {
        SqlDataAccess _dao;

        public Business(SqlDataAccess dao)
        {
            _dao = dao;
        }
        public List<Order> GetOrders()
        {
            List<Order> result = _dao.GetOrders();

            return result;
        }
        public List<Product> GetProducts()
        {
            List<Product> result = _dao.GetProducts();

            return result;
        }
    }
}
