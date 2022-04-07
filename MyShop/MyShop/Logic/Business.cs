using MyShop.Services;
using System.Collections.Generic;

namespace MyShop.Logic
{
    internal class Business
    {
        public SqlDataAccess _dao ;

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

        public void removeOrder(Order order)
        {
             _dao.removeOrder(order);
        }

    }
}
