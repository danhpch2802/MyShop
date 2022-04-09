using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop
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

        public void Disconnect()
        {
            _connection.Close();
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

            reader.Close();

            return result;
        }

        public void updateProductQuantityRemovedOrder(List<DetailOrder> listDetailOrder, Order updateOrder)
        {
            var OldListOrder = loadDetailOrdersfromID(updateOrder);
            var RemovedProducts = OldListOrder.Except(listDetailOrder) // Items in the first list but not in the second
    .       Concat(listDetailOrder.Except(listDetailOrder))      // Items in the second list but not in the first
            .ToList();

            foreach (var order in RemovedProducts)
            {
                   var sql =
                       "UPDATE HangHoa " +
                       "SET HangHoa_soluong = @_amount where HangHoa_id=@_product_id";
                   var command = new SqlCommand(sql, _connection);
                    int NewAmount = order.Product.Amount+order.Quantity;

                    command.Parameters.Add("@_amount", SqlDbType.Int).Value = NewAmount;
                    command.Parameters.Add("@_product_id", SqlDbType.Int).Value = order.Product.productID;
                    command.ExecuteNonQuery();
                }
               
            
        }

        public RevenueSeries loadRevenueYear()
        {
            var sql = "select Year(DonHang.NgayMua) as 'Year', sum(DonHang.TongGia) as 'Revenue', 0.15 * sum(DonHang.TongGia) as 'Profit'" +
                " from DonHang" +
                " group by Year(DonHang.NgayMua)";
            var command = new SqlCommand(sql, _connection);
            var reader = command.ExecuteReader();

            var result = new RevenueSeries()
            {
                Revenue = new List<int>(),
                Profit = new List<int>(),
                Year = new List<int>(),
            };
            while (reader.Read())
            {
                var year = (int)reader["Year"];
                var revenue = (int)reader["Revenue"];
                var profit = (decimal)reader["Profit"];
                result.Year.Add(year);
                result.Profit.Add((int)profit);
                result.Revenue.Add(revenue);
            }
            reader.Close();
            return result;
        }

        public RevenueSeries loadRevenueMonth(int year)
        {
            var sql = "select Month(DonHang.NgayMua) as 'Month', sum(DonHang.TongGia) as 'Revenue', 0.15 * sum(DonHang.TongGia) as 'Profit'" +
                " from DonHang" +
                " where Year(DonHang.NgayMua) = @_year " +
                " group by Month(DonHang.NgayMua)";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@_year", SqlDbType.Int).Value = year;
            var reader = command.ExecuteReader();

            var result = new RevenueSeries()
            {
                Revenue = new List<int>(),
                Profit = new List<int>(),
                Month = new List<int>(),
            };
            while (reader.Read())
            {
                var month = (int)reader["Month"];
                var revenue = (int)reader["Revenue"];
                var profit = (decimal)reader["Profit"];
                result.Month.Add(month);
                result.Profit.Add((int)profit);
                result.Revenue.Add(revenue);
            }
            reader.Close();
            return result;
        }

        public RevenueSeries loadRevenueWeek(int month, int year)
        {
            var sql = "select DATEDIFF(WEEK, DATEADD(MONTH, DATEDIFF(MONTH, 0, DonHang.NgayMua), 0), DonHang.NgayMua) + 1 as'Week', sum(DonHang.TongGia) as 'Revenue', 0.15 * sum(DonHang.TongGia) as 'Profit' " +
                "from DonHang " +
                "where Year(DonHang.NgayMua) = @_year and MONTH(DonHang.NgayMua)= @_month " +
                "group by DATEDIFF(WEEK, DATEADD(MONTH, DATEDIFF(MONTH, 0, DonHang.NgayMua), 0), DonHang.NgayMua) + 1" ;
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@_year", SqlDbType.Int).Value = year;
            command.Parameters.Add("@_month", SqlDbType.Int).Value = month;
            var reader = command.ExecuteReader();

            var result = new RevenueSeries()
            {
                Revenue = new List<int>(),
                Profit = new List<int>(),
                Week = new List<int>(),
            };
            while (reader.Read())
            {
                var week = (int)reader["Week"];
                var revenue = (int)reader["Revenue"];
                var profit = (decimal)reader["Profit"];
                result.Week.Add(week);
                result.Profit.Add((int)profit);
                result.Revenue.Add(revenue);
            }
            reader.Close();
            return result;
        }

        public RevenueSeries loadRevenueDate(DateTime startDP, DateTime endDP)
        {
            var sql = "select DonHang.NgayMua as 'Date', sum(DonHang.TongGia) as 'Revenue', 0.15 * sum(DonHang.TongGia) as 'Profit'" +
                " from DonHang " +
                " where @_startdate <= DonHang.NgayMua and @_enddate >= DonHang.NgayMua " +
                " group by DonHang.NgayMua";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@_startdate", SqlDbType.DateTime).Value = startDP;
            command.Parameters.Add("@_enddate", SqlDbType.DateTime).Value = endDP;
            var reader = command.ExecuteReader();

            var result = new RevenueSeries()
                {
                Date = new List<DateTime>(),
                Revenue= new List<int>(),
                Profit = new List<int>(),
            };
            while (reader.Read())
            {
                var date =  (DateTime)reader["Date"];
                var revenue = (int)reader["Revenue"];
                var profit = (decimal)reader["Profit"];
                result.Date.Add(date);
                result.Profit.Add((int)profit);
                result.Revenue.Add(revenue);
            }
            reader.Close();
            return result;
        }

        public void updateProductQuantity(List<DetailOrder> listDetailOrder) { 
            foreach (var order in listDetailOrder)
            {
                var sql = "select * from [ChiTietDonHang] where DonHang_id=@_orderid and HangHoa_id=@_product_id";
                var command = new SqlCommand(sql, _connection);
                command.Parameters.Add("@_orderid", SqlDbType.Int).Value = order.OrderID;
                command.Parameters.Add("@_product_id", SqlDbType.Int).Value = order.Product.productID;

                var reader = command.ExecuteReader();
                int quantity=0;
                //Check increase or decrease quantity
                if (reader.Read())
                {
                    quantity = (int)reader["SoLuong"];
                    reader.Close();
                    sql =
                       "UPDATE HangHoa " +
                       "SET HangHoa_soluong = @_amount where HangHoa_id=@_product_id";
                    command = new SqlCommand(sql, _connection);
                    int NewAmount = 0;
                    //Increase
                    if (order.Quantity > quantity)
                    {
                        NewAmount = order.Product.Amount - order.Quantity;

                    }
                    //Decrase
                    else if (order.Quantity < quantity)
                    {
                        NewAmount = order.Product.Amount + (quantity - order.Quantity);
                    }

                    command.Parameters.Add("@_amount", SqlDbType.Int).Value = NewAmount;
                    command.Parameters.Add("@_product_id", SqlDbType.Int).Value = order.Product.productID;
                    command.ExecuteNonQuery();
                }
                else
                {
                    reader.Close();
                    sql =
                       "UPDATE HangHoa " +
                       "SET HangHoa_soluong = @_amount where HangHoa_id=@_product_id";
                    command = new SqlCommand(sql, _connection);
                    var NewAmount = order.Product.Amount - order.Quantity;

                    command.Parameters.Add("@_amount", SqlDbType.Int).Value = NewAmount;
                    command.Parameters.Add("@_product_id", SqlDbType.Int).Value = order.Product.productID;
                    command.ExecuteNonQuery();
                }
            }
        }
        public void updateOrder(Order newOrder)
        {
            var sql = "UPDATE DonHang " +
                               "SET TongGia = @_ordertotal where DonHang_id=@_orderid";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@_ordertotal", SqlDbType.Int).Value = newOrder.OrderTotal;
            command.Parameters.Add("@_orderid", SqlDbType.Int).Value = newOrder.OrderID;
            command.ExecuteNonQuery();
        }
        public void updateDetailOrder(List<DetailOrder> listDetailOrder)
        {
            foreach (DetailOrder item in listDetailOrder)
            {

                var sql = "update ChiTietDonHang set SoLuong=@_quantity,Gia=@_total " +
                    " where DonHang_id=@_orderid and HangHoa_id=@_productid ";
                var command = new SqlCommand(sql, _connection);
                command.Parameters.Add("@_orderid", SqlDbType.Int).Value = item.OrderID;
                command.Parameters.Add("@_productid", SqlDbType.Int).Value = item.Product.productID;
                command.Parameters.Add("@_quantity", SqlDbType.Int).Value = item.Quantity;
                command.Parameters.Add("@_total", SqlDbType.Int).Value = item.Total;
                if (command.ExecuteNonQuery() == 0)
                {
                    sql = "insert into ChiTietDonHang(DonHang_id,HangHoa_id,SoLuong,Gia) Values (@_orderid,@_productid,@_quantity,@_total)";
                    command = new SqlCommand(sql, _connection);
                    command.Parameters.Add("@_orderid", SqlDbType.Int).Value = item.OrderID;
                    command.Parameters.Add("@_productid", SqlDbType.Int).Value = item.Product.productID;
                    command.Parameters.Add("@_quantity", SqlDbType.Int).Value = item.Quantity;
                    command.Parameters.Add("@_total", SqlDbType.Int).Value = item.Total;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void addNewDetailOrders(List<DetailOrder> listDetailOrder)
        {
            foreach(DetailOrder item in listDetailOrder)
            {
                var sql = "insert into ChiTietDonHang(DonHang_id,HangHoa_id,SoLuong,Gia) Values (@_orderid,@_productid,@_quantity,@_total)";
                var command = new SqlCommand(sql, _connection);
                command.Parameters.Add("@_orderid", SqlDbType.Int).Value = item.OrderID;
                command.Parameters.Add("@_productid", SqlDbType.Int).Value = item.Product.productID;
                command.Parameters.Add("@_quantity", SqlDbType.Int).Value = item.Quantity;
                command.Parameters.Add("@_total", SqlDbType.Int).Value = item.Total;
                command.ExecuteNonQuery();
            }
        }

        public void addNewOrder(Order newOrder)
        {
            var sql = "insert into DonHang(NgayMua,TongGia) Values(@_orderdate,@_ordertotal)";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@_orderdate", SqlDbType.DateTime).Value = newOrder.OrderDate;
            command.Parameters.Add("@_ordertotal", SqlDbType.Int).Value = newOrder.OrderTotal;
            command.ExecuteNonQuery();
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

            reader.Close();

            return result;
        }

        public void addDataToDatabase(string img, string name, string pcat, int price, int amount)
        {
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
        }

        public void deleteDataFromDatabase(string img, string name, string pcat, int price, int amount)
        {
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
        }

        public void editDataInDatabase(string img, string name, string pcat, int price, int amount)
        {
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
        }

        public void removeOrder(Order order)
        {
                removeDetailOrder(order);
                var sql = "delete from DonHang where DonHang_id = @_orderid ";
                var command = new SqlCommand(sql, _connection);
                command.Parameters.Add("@_orderid", SqlDbType.Int).Value = order.OrderID;
                command.ExecuteNonQuery();
        }

        public void removeDetailOrder(Order order)
        {
            var sql = "delete from ChiTietDonHang where DonHang_id = @_orderid ";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@_orderid", SqlDbType.Int).Value = order.OrderID;
            command.ExecuteNonQuery();
        }

        public Product getProductFromID(int ProductID)
        {
            var sql = "select * from [HangHoa] where DonHang_id = @_orderid ";
            var command = new SqlCommand(sql, _connection);

            var reader = command.ExecuteReader();
            command.Parameters.Add("@_orderid", SqlDbType.Int).Value = ProductID;

            var result = new Product();

            if(reader.Read())
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

                result = new Product
                {
                    productID = productID,
                    Image = productImg,
                    Name = productName,
                    Category = categories,
                    Price = (int)productPrice,
                    Amount = (int)productAmount,
                };
            }
            reader.Close();

            return result;


        }

        public int getNewestOrderID()
        {
            var sql = "select max(DonHang_id) as 'ID' from DonHang";
            var command = new SqlCommand(sql, _connection);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var result= (int)reader["ID"] + 1;
                reader.Close();
                return result;
            }
            else
            {
                reader.Close();
                return 0;
            }
        }

        public List<DetailOrder> loadDetailOrdersfromID(Order order)
        {

            var sql = "select CTDH.DonHang_id,HH.HangHoa_id,HH.HangHoa_img,HH.HangHoa_ten,HH.HangHoa_hieu,HH.HangHoa_gia" +
                ",HH.HangHoa_soluong," +
                "CTDH.SoLuong,CTDH.Gia " +
                "from MyShop.dbo.ChiTietDonHang CTDH join MyShop.dbo.HangHoa HH on CTDH.HangHoa_id = HH.HangHoa_id" +
                " where CTDH.DonHang_id = @_orderid";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@_orderid", SqlDbType.Int).Value = order.OrderID;

            var reader = command.ExecuteReader();

            List<DetailOrder> result = new List<DetailOrder>();

            while (reader.Read())
            {
                var OrderID = (int)reader["DonHang_id"];
                var ProductId = (int)reader["HangHoa_id"];
                var Quantity = (int)reader["SoLuong"];
                var Total = (int)reader["Gia"];

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

                var product = new Product
                {
                    productID = productID,
                    Image = productImg,
                    Name = productName,
                    Category = categories,
                    Price = (int)productPrice,
                    Amount = (int)productAmount,
                };

                result.Add(new DetailOrder()
                {
                    OrderID = OrderID,
                    Product = product,
                    Quantity = Quantity,
                    Total = Total,

                });
            }
            reader.Close();
            return result;
        }
    }
}

