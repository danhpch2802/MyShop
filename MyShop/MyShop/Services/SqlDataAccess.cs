using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

            while (reader.Read())
            {
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

        public List<Product> GetProducts()
        {
            var sql = "select * from [HangHoa]";
            var command = new SqlCommand(sql, _connection);

            var reader = command.ExecuteReader();

            List<Product> result = new List<Product>();
            
            while (reader.Read())
            {
                var productID = (int)reader["HangHoa_id"];
                var productImg = (string)reader["HangHoa_img"];
                var productName = (string)reader["HangHoa_ten"];
                var productCat = (string)reader["HangHoa_hieu"];
                var productPrice = (decimal)reader["HangHoa_gia"];
                var productAmount = (int)reader["HangHoa_soluong"];

                var categories = new Category()
                {
                    Name = productCat
                };

                result.Add(new Product()
                {
                    productID = productID,
                    Name = productName,
                    Price = (int)productPrice,
                    Image = productImg,
                    Category = categories,
                    Amount = productAmount
                })
                    ;
            }


            return result;
        }

        public void addDataToDatabase(string img, string name, string pcat, int price, int amount)
        {
            Connect();
            // Send to database
            var sql = "SET IDENTITY_INSERT [dbo].[HangHoa] OFF; " +
                "BEGIN " +
                    "IF NOT EXISTS (SELECT * FROM HangHoa WHERE HangHoa_ten = @_name AND HangHoa_hieu = @_pcat) " +
                    "BEGIN " +
                    "INSERT INTO HangHoa(HangHoa_img, HangHoa_ten, HangHoa_hieu, HangHoa_gia, HangHoa_soluong) VALUES(@_img, @_name, @_pcat, @_price, @_amount) " +
                    "END " +
                "END ";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@_img", SqlDbType.VarChar).Value = img;
            command.Parameters.Add("@_name", SqlDbType.NVarChar).Value = name;
            command.Parameters.Add("@_pcat", SqlDbType.VarChar).Value = pcat;
            command.Parameters.Add("@_price", SqlDbType.Decimal).Value = price;
            command.Parameters.Add("@_amount", SqlDbType.Int).Value = amount;
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public void deleteDataFromDatabase(string img, string name, string pcat, int price, int amount)
        {
            Connect();
            // Delete from database
            var sql = "IF EXISTS (SELECT * FROM HangHoa WHERE HangHoa_ten = @_name) " +
                "BEGIN " +
                "DELETE FROM HangHoa " +
                "WHERE HangHoa_ten = @_name AND HangHoa_hieu = @_pcat " +
                "END";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@_img", SqlDbType.VarChar).Value = img;
            command.Parameters.Add("@_name", SqlDbType.NVarChar).Value = name;
            command.Parameters.Add("@_pcat", SqlDbType.VarChar).Value = pcat;
            command.Parameters.Add("@_price", SqlDbType.Decimal).Value = price;
            command.Parameters.Add("@_amount", SqlDbType.Int).Value = amount;
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public void editDataInDatabase(string img, string name, string pcat, int price, int amount)
        {
            Connect();
            // Edit data in database
            var sql = "IF EXISTS (SELECT * FROM HangHoa WHERE HangHoa_ten = @_name) " +
                "BEGIN " +
                "UPDATE HangHoa " +
                "SET HangHoa_ten = @_name, HangHoa_hieu = @_pcat, HangHoa_img = @_img, HangHoa_gia = @_price, HangHoa_soluong = @_amount ";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@_img", SqlDbType.VarChar).Value = img;
            command.Parameters.Add("@_name", SqlDbType.NVarChar).Value = name;
            command.Parameters.Add("@_pcat", SqlDbType.VarChar).Value = pcat;
            command.Parameters.Add("@_price", SqlDbType.Decimal).Value = price;
            command.Parameters.Add("@_amount", SqlDbType.Int).Value = amount;
            command.ExecuteNonQuery();
            _connection.Close();
        }
    }
}

