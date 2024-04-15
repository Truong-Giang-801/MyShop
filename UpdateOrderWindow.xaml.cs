using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Design.Behavior;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for UpdateOrderWindow.xaml
    /// </summary>
    public partial class UpdateOrderWindow : Window
    {
        public Order UpdateOrder { get; set; }
        private int id;
        private DateTime date;

        private BindingList<Customer> _customer = new BindingList<Customer>();
        private BindingList<Product> _product = new BindingList<Product>();
        public UpdateOrderWindow(Order order, BindingList<Customer> customers, ObservableCollection<Product> products)
        {
            InitializeComponent();
            UpdateOrder = (Order)order.Clone();
            for (int i = 0; i < customers.Count; i++)
            {
                _customer.Add((Customer)customers[i].Clone());
            }
            for (int i = 0; i < products.Count; i++)
            {
                _product.Add((Product)products[i].Clone());
            }
            comboBoxCustomer.ItemsSource = _customer;
            comboBoxProduct.ItemsSource = _product;
            Debug.WriteLine(order.Customer.Id);
            DataContext = UpdateOrder;
            // Id not from 1 
            //comboBoxCustomer.SelectedItem = _customer[order.Customer.Id];
            //comboBoxProduct.SelectedItem = _product[order.Product.Id];
            id = order.Id;
            date = order.OrderDate;
        }

        private void Quantity_Update_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Submit_Update_Order_Click(object sender, RoutedEventArgs e)
        {
            if (Quantity_Update.Text == "" || comboBoxProduct.SelectedItem == null || comboBoxProduct.SelectedItem == null)
            {
                MessageBox.Show("Please don't leave any field as blank");
            }
            else
            {
                try
                {
                    Product product = (Product)comboBoxProduct.SelectedItem;
                    int quantity = int.Parse(Quantity_Update.Text);
                    Customer customer = (Customer)comboBoxCustomer.SelectedItem;

                    // Create a new Product object using the input values
                    UpdateOrder = new Order
                    {
                        Customer = customer,
                        Quantity = quantity,
                        Product = product
                        
                    };
                    UpdateOrder.Id = id;
                    UpdateOrder.OrderDate = date;
                    this.DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please enter valid information");
                }

            }
        }

        private void Cancel_Update_Order_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void comboBoxProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboBoxCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
