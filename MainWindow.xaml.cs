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

        private void Exit_button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Log_Out_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Payment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Support_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InfoCard_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void InfoCard_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void Sale_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}