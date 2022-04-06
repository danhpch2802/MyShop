using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Services
{
    internal class SqlDataAccess
    {
        private SqlConnection _connection;
        public SqlDataAccess(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public bool CanConnect()
        {
            bool result = true;

            try
            {
                _connection.Open();
                _connection.Close();
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public void Connect()
        {
            _connection.Open();
        }

        public List<Order> GetOrders()
        {
            var sql = "select * from [DonHang]";
            var command = new SqlCommand(sql, _connection);

            var reader = command.ExecuteReader();

            List<Order> result = new List<Order>();

                while (reader.Read()) { 
                var orderID = (int)reader["DonHang_id"];
                var orderDate = (DateTime)reader["NgayMua"];
                var orderTotal = (int)reader["TongGia"];
                result.Add(new Order()
                {
                    OrderID = orderID,
                    OrderDate = orderDate,
                    OrderTotal = orderTotal,
                })
                    ;
                }
            

            return result;
        }
    }
}

