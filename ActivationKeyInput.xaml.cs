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

namespace MyShop
{
    /// <summary>
    /// Interaction logic for ActivationKeyInput.xaml
    /// </summary>
    public partial class ActivationKeyInput : Window
    {
        public string activationKey { get; set; }
        public ActivationKeyInput()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            activationKey = ActivationKey.Text;
            if (activationKey == "VK7JG-NPHTM-C97JM-9MPGT-3V66T")
            {
                MessageBox.Show(
            "Activation succesfully",
            "Success",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show(
            "Wrong activation key",
            "Failure",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
