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
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        public Customer? _addCustomer { get; set; }
        BindingList<Customer> _customers = new BindingList<Customer>();
        public AddCustomerWindow(BindingList<Customer> customers)
        {
            InitializeComponent();
            _customers = new BindingList<Customer>(customers.ToList());
        }

        private void Cancel_Add_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Submit_Add_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerName_Add.Text == "" || PhoneNumber_Add.Text == "")
            {
                MessageBox.Show("Please don't leave any field as blank");
            }
            else
            {
                try
                {
                    string customerName = CustomerName_Add.Text;
                    string phoneNumber = PhoneNumber_Add.Text;
                    bool phoneNumberExists = _customers.Any(c => c.PhoneNumber == phoneNumber);
                    int checkOnlyDigits = int.Parse(PhoneNumber_Add.Text);

                    if (phoneNumberExists)
                    {
                        // Show an error message if the category name already exists
                        MessageBox.Show("A customer with this phone number already exists.");
                    }
                    else
                    {
                        // Create a new Customer object using the input values
                        _addCustomer = new Customer
                        {
                            Name = customerName,
                            PhoneNumber = phoneNumber
                        };
                        this.DialogResult = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please enter valid phone number");
                }

            }
        }
    }
}
