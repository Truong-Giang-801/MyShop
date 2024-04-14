using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public Customer? _updateCustomer { get; set; }
        BindingList<Customer> _customers = new BindingList<Customer>();
        public UpdateCustomerWindow(Customer _cm, BindingList<Customer> customers)
        {
            InitializeComponent();
            _updateCustomer = (Customer)_cm.Clone();
            DataContext = _updateCustomer;
            _customers = new BindingList<Customer>(customers.ToList());
        }

        private void Submit_Update_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerName_Update.Text == "" || PhoneNumber_Update.Text == "")
            {
                MessageBox.Show("Please don't leave any field as blank");
            }
            else
            {
                try
                {
                    string customerName = CustomerName_Update.Text;
                    string phoneNumber = PhoneNumber_Update.Text;
                    bool phoneNumberExists = _customers.Any(c => c.PhoneNumber == phoneNumber);
                    int checkOnlyDigits = int.Parse(PhoneNumber_Update.Text);

                    if (phoneNumberExists)
                    {
                        // Show an error message if the category name already exists
                        MessageBox.Show("A customer with this phone number already exists.");
                    }
                    else
                    {
                        // Create a new Customer object using the input values
                        _updateCustomer.Name = customerName;
                        _updateCustomer.PhoneNumber = phoneNumber;
                        this.DialogResult = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please enter valid phone number");
                }

            }
        }

        private void Cancel_Update_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
