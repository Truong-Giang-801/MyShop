using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyShop.UserControls;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool IsMaximize = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximize)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1280;
                    this.Height = 780;

                    IsMaximize = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximize = true;
                }
            }
        }
        private void setButtonDashBoard()
        {
            DashBoard.Background = Brushes.Transparent;
            DashBoard.Foreground = Brushes.White;
            Log_Out.Background = Brushes.Transparent;
            Log_Out.Foreground = Brushes.White;
            Payment.Background = Brushes.Transparent;
            Payment.Foreground = Brushes.White;
            Support.Background = Brushes.Transparent;
            Support.Foreground = Brushes.White;
            Account.Background = Brushes.Transparent;
            Account.Foreground = Brushes.White;
            Setting.Background = Brushes.Transparent;
            Setting.Foreground = Brushes.White;
            Products.Background = Brushes.Transparent;
            Products.Foreground = Brushes.White;
            Exit_button.Background = Brushes.Transparent;
            Exit_button.Foreground = Brushes.White;
        }
        private void setVisibleOff()
        {
            DashboardScreen.Visibility = Visibility.Hidden;
            PaymentScreen.Visibility = Visibility.Hidden;
            SupportScreen.Visibility = Visibility.Hidden;
            ProfileScreen.Visibility = Visibility.Hidden;
            SettingScreen.Visibility = Visibility.Hidden;
            ProductScreen.Visibility = Visibility.Hidden;
        }
        private void Exit_button_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            Exit_button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Exit_button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB7657"));
            MessageBoxResult result = MessageBox.Show(
            "Are you sure you want to exit?",
            "Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Perform the action after user confirms.
                // For example, delete a record or save changes.
                Application.Current.Shutdown();
            }
        }

        private void Log_Out_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            Log_Out.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Log_Out.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB7657"));
            MessageBoxResult result = MessageBox.Show(
            "Are you sure you want to logout?",
            "Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Perform the action after user confirms.
                // For example, delete a record or save changes.
                Properties.Settings.Default.Username = "";
                Properties.Settings.Default.EncryptedPassword = "";
                Properties.Settings.Default.Save();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            ProductScreen.Visibility = Visibility.Visible;
            Products.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Products.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB7657"));
        }

        private void Payment_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            PaymentScreen.Visibility = Visibility.Visible;
            Payment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Payment.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB7657"));

        }

        private void Support_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            SupportScreen.Visibility = Visibility.Visible;
            Support.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Support.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB7657"));

        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            ProfileScreen.Visibility = Visibility.Visible;
            Account.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Account.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB7657"));

        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            SettingScreen.Visibility = Visibility.Visible;
            Setting.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Setting.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB7657"));

        }

        private void DashBoard_Click(object sender, RoutedEventArgs e)
        {
            setVisibleOff();
            setButtonDashBoard();
            DashboardScreen.Visibility = Visibility.Visible;
            DashBoard.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7F6F4"));
            DashBoard.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB7657"));
        }

        public Products _product;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setVisibleOff();
            DashboardScreen.Visibility= Visibility.Visible;
            _product = new Products()
            {
                NumberSale = 123
            };
            this.DataContext = _product;

        }

    }
}