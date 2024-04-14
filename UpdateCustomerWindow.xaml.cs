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
    /// Interaction logic for UpdateCustomerWindow.xaml
    /// </summary>
    public partial class UpdateCustomerWindow : Window
    {
        public Customer _customer { get; private set; }

        public UpdateCustomerWindow(Customer customer)
        {
            InitializeComponent();
            _customer =new Customer
            {
                Name = customer.Name,
                PhoneNumber= customer.PhoneNumber,
                Id = customer.Id,
            };
            DataContext = _customer;
        }
        private void CustomerName_Update_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PhoneNumber_Update_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Submit_Update_Customer_Click(object sender, RoutedEventArgs e)
        {
            string Name = CustomerName_Update.Text;
            string phone = PhoneNumber_Update.Text;

            // Create a new Product object using the input values
            _customer = new Customer()
            {
                Name = Name,
                PhoneNumber = phone,
            };
            this.DialogResult = true;
        }

        private void Cancel_Update_Customer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
