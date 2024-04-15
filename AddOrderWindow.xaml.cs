using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        public Order AddOrder { get; set; }
        private BindingList<Customer> _customer = new BindingList<Customer>();
        private BindingList<Product> _product =  new BindingList<Product>();
        public AddOrderWindow(BindingList<Customer> customers,ObservableCollection<Product> products)
        {
            InitializeComponent();
            for (int i = 0; i < customers.Count; i++)
            {
                _customer.Add((Customer)customers[i].Clone());
            }
            for (int i = 0;i < products.Count; i++) {
                _product.Add((Product)products[i].Clone());
            }
            comboBoxCustomer.ItemsSource = _customer;
            comboBoxProduct.ItemsSource = _product;
        }

        private void comboBoxCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboBoxProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Submit_Add_Order_Click(object sender, RoutedEventArgs e)
        {
            if (Quantity_Add.Text == "" ||comboBoxProduct.SelectedItem == null ||comboBoxProduct.SelectedItem==null)
            {
                MessageBox.Show("Please don't leave any field as blank");
            }
            else
            {
                try
                {
                    Product product = (Product)comboBoxProduct.SelectedItem;
                    int quantity = int.Parse(Quantity_Add.Text);
                    Customer customer = (Customer)comboBoxCustomer.SelectedItem;

                    // Create a new Product object using the input values
                    AddOrder = new Order
                    {
                        OrderDate = DateTime.Now,
                        Customer = customer,
                        Quantity = quantity,
                        Product = product
                    };
                    this.DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please enter valid information");
                }

            }
        }

        private void Cancel_Add_Order_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Quantity_Add_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
