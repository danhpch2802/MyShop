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

        public void updateOrder(Order newOrder)
        {
            _dao.updateOrder(newOrder);
        }

        public void updateDetailOrder(List<DetailOrder> listDetailOrder)
        {
            _dao.updateDetailOrder(listDetailOrder);
        }

        public void updateProductQuantityRemovedOrder(List<DetailOrder> listDetailOrder, Order updateOrder)
        {
            _dao.updateProductQuantityRemovedOrder(listDetailOrder, updateOrder);
        }

        public RevenueSeries loadRevenueDate(DateTime startDP, DateTime endDP)
        {
            var result = _dao.loadRevenueDate(startDP, endDP);
            return result;
        }

        public RevenueSeries loadRevenueWeek(int month, int year)
        {
            var result = _dao.loadRevenueWeek(month, year);
            return result;
        }

        public RevenueSeries loadRevenueMonth(int year)
        {
            var result = _dao.loadRevenueMonth( year);
            return result;
        }

        public RevenueSeries loadRevenueYear()
        {
            var result = _dao.loadRevenueYear();
            return result;
        }

        public RevenueSeries loadProductDate(DateTime startDP, DateTime endDP, Product product)
        {
            var result = _dao.loadProductDate(startDP, endDP, product);
            return result;
        }

        public RevenueSeries loadProductWeek(int month, int year, Product product)
        {
            var result = _dao.loadProductWeek(month, year, product);
            return result;
        }

        public RevenueSeries loadProductMonth(int year, Product product)
        {
            var result = _dao.loadProductMonth( year, product);
            return result;
        }

        public RevenueSeries loadProductYear(Product product)
        {
            var result = _dao.loadProductYear( product);
            return result;
        }
    }
}
