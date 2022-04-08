using System;
using System.Collections.Generic;

namespace MyShop
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

        public List<DetailOrder> loadDetailOrdersfromID(Order order)
        {
            List<DetailOrder> result = _dao.loadDetailOrdersfromID(order);
            return result;
        }

        public int getNewestOrderID()
        {
            return _dao.getNewestOrderID();
        }

        public void addNewOrder(Order newOrder)
        {
            _dao.addNewOrder(newOrder);
        }

        public void addNewDetailOrders(List<DetailOrder> listDetailOrder)
        {
            _dao.addNewDetailOrders(listDetailOrder);
        }

        public void updateProductQuantity(List<DetailOrder> listDetailOrder)
        {
            _dao.updateProductQuantity(listDetailOrder);
        }
    }
}
