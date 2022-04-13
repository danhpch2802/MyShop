using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Security.Cryptography;
using System.Diagnostics;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    /// 
    class TaiKhoan
    {
        public string id { get; set; }
        public string username { get; set; }
        public string password { get; set; }

    }
    
    public partial class LoginWindow : Window
    {

        public string name { get; set; }
        public string pass { get; set; }

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public LoginWindow(string n, string p)
        {
            InitializeComponent();
            name = n;
            pass = p;
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            userNameTextBox.Text = name;
            passWordBox.Password = pass;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var connectionString =
                "Server=.\\sqlexpress;Database=MyShop;Trusted_Connection=True;";

            // Kết nối
            var connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Sau khi kết nối thành công
            var sql = "select * from TaiKhoan";
            var command = new SqlCommand(sql, connection);

            var reader = command.ExecuteReader();

            List<TaiKhoan> taikhaons = new List<TaiKhoan>();

            while (reader.Read())
            {
                var _id = (string)reader["TaiKhoan_id"];
                var _name = (string)reader["TaiKhoan_username"];
                var _password = (string)reader["TaiKhoan_password"];

                TaiKhoan taikhoan = new TaiKhoan()
                {
                    id = _id,
                    username = _name,
                    password = _password
                };
                taikhaons.Add(taikhoan);
            }
            string hashpass = hashPassword(passWordBox.Password);
            Debug.WriteLine(hashpass);

            if (check_account(userNameTextBox.Text, hashpass, taikhaons))
            {
                name = userNameTextBox.Text;
                pass = passWordBox.Password;
                DialogResult = true;
            }
            else
            {
                textBlock.Text = "Password or UserName is wrong!";
                userNameTextBox.Text = "";
                passWordBox.Password = "";
            }
        }


        private bool check_account(string username,string password, List<TaiKhoan> tk)
        {
            for(int i = 0; i < tk.Count; i++)
            {
                if (tk[i].username == username && tk[i].password == password)
                {
                    return true;
                }
            }
            return false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
                
            DialogResult = false;
            
        }

        private bool username_is_not_exit(string username, List<TaiKhoan> tk)
        {
            for (int i = 0; i < tk.Count; i++)
            {
                if (tk[i].username == username || username=="")
                {
                    return false;
                }
            }
            return true;
        }

        private string hashPassword(string pass)
        {
            SHA1CryptoServiceProvider sHA1 = new SHA1CryptoServiceProvider();
            byte[] password_byte = Encoding.ASCII.GetBytes(pass);
            byte[] salt_byte = sHA1.ComputeHash(password_byte);
            return Convert.ToBase64String(salt_byte);
        }

    }
}
