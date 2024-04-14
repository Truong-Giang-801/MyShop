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
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        public AddCustomerWindow()
        {
            InitializeComponent();
        }
        public Customer Customer { get; private set; }
        private void PhoneNumber_Add_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void CustomerName_Add_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Submit_Add_Customer_Click(object sender, RoutedEventArgs e)
        {
            string Name = CustomerName_Add.Text;
            string phone = PhoneNumber_Add.Text;

            // Create a new Product object using the input values
            Customer = new Customer()
            {
                Name = Name,
                PhoneNumber = phone,
            };
            this.DialogResult = true;
        }

        private void Cancel_Add_Customer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
