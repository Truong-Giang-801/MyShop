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
            Sale.Background = Brushes.Transparent;
            Sale.Foreground = Brushes.White;
            Setting.Background = Brushes.Transparent;
            Setting.Foreground = Brushes.White;
            Products.Background = Brushes.Transparent;
            Products.Foreground = Brushes.White;
        }
        private void Exit_button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Log_Out_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            Log_Out.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Log_Out.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB7657"));

        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            Products.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Products.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB7657"));
        }

        private void Payment_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            Payment.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Payment.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB7657"));

        }

        private void Support_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            Support.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Support.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB7657"));

        }

        private void InfoCard_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void InfoCard_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void Sale_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            Sale.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Sale.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB7657"));

        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            Setting.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Setting.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB7657"));

        }

        private void DashBoard_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            DashBoard.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F7F6F4"));
            DashBoard.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB7657"));

        }

        public Products _product;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _product = new Products()
            {
                NumberSale = 123
            };
            this.DataContext = _product;

        }
    }
}