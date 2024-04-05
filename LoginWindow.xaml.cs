using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;


namespace MyShop
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            CheckSavedLoginInfo();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Kiểm tra thông tin đăng nhập (đơn giản chỉ là so sánh với giá trị mặc định)
            if (username == "admin" && password == "password")
            {
                // Mã hóa password
                string encryptedPassword = EncryptPassword(password);

                // Lưu thông tin đăng nhập ở local
                SaveLoginInfo(username, encryptedPassword);

                // Điều hướng đến màn hình chính
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Username or password is incorrect.");
            }
        }

        private string EncryptPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void SaveLoginInfo(string username, string encryptedPassword)
        {
            Properties.Settings.Default.Username = username;
            Properties.Settings.Default.EncryptedPassword = encryptedPassword;
            Properties.Settings.Default.Save();
        }

        private void CheckSavedLoginInfo()
        {
            // Kiểm tra xem có thông tin đăng nhập đã lưu không
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Username) && !string.IsNullOrEmpty(Properties.Settings.Default.EncryptedPassword))
            {
                // Mã hóa mật khẩu đúng
                string correctPassword = "password";
                string encryptedCorrectPassword = EncryptPassword(correctPassword);

                // So sánh thông tin đăng nhập đã lưu với thông tin đăng nhập đúng
                if (Properties.Settings.Default.Username == "admin" && Properties.Settings.Default.EncryptedPassword == encryptedCorrectPassword)
                {
                    // Sử dụng thông tin đăng nhập đã lưu để đăng nhập tự động
                    MainWindow mainWindow = new MainWindow();
                    this.Close();
                    mainWindow.Show();
                }
            }
        }

    }
}
