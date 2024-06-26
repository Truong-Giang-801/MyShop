﻿using DocumentFormat.OpenXml.Drawing.Charts;
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
        private List<Coupon> _coupon = new List<Coupon>();
        public UpdateOrderWindow(Order order, BindingList<Customer> customers, ObservableCollection<Product> products, BindingList<Coupon> coupons)
        {
            InitializeComponent();
            UpdateOrder = (Order)order.Clone();
            var customerIndex = -1;
            var productIndex = -1;
            for (int i = 0; i < customers.Count; i++)
            {
                _customer.Add((Customer)customers[i].Clone());
                if (UpdateOrder.Customer != null && customers[i].Id == UpdateOrder.Customer.Id)
                {
                    customerIndex = i;
                }
            }
            for (int i = 0; i < products.Count; i++)
            {
                _product.Add((Product)products[i].Clone());
                if (products[i].Id == UpdateOrder.Product.Id)
                {
                    productIndex = i;
                    _product[i].Quantity = _product[i].Quantity + UpdateOrder.Quantity;
                }
            }
            for (int i = 0; i < coupons.Count; i++)
            {
                _coupon.Add((Coupon)coupons[i].Clone());
            }
            comboBoxCustomer.ItemsSource = _customer;
            comboBoxProduct.ItemsSource = _product;
            if (customerIndex != -1)
            {
                comboBoxCustomer.SelectedIndex = customerIndex;
            }
            if (productIndex != -1)
            {
                comboBoxProduct.SelectedIndex = productIndex;
            }
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
            if (Quantity_Update.Text == "" || comboBoxProduct.SelectedItem == null || comboBoxProduct.SelectedItem == null || Datepicker.SelectedDate == null)
            {
                MessageBox.Show("Please don't leave required field as blank");
            }
            else
            {
                try
                {
                    Product product = (Product)comboBoxProduct.SelectedItem;
                    int quantity = int.Parse(Quantity_Update.Text);
                    Customer customer = (Customer)comboBoxCustomer.SelectedItem;
                    date = Datepicker.SelectedDate.Value;
                    string couponcode = Coupon_Update.Text;
                    Coupon coupon = _coupon.Find(c => c.Code == couponcode);

                    // Check if couponcode is not empty before attempting to find the coupon
                    if (!string.IsNullOrEmpty(couponcode))
                    {
                        if (coupon == null)
                        {
                            MessageBox.Show("Invalid coupon code");
                            return; // Exit the method if the coupon is invalid
                        }
                        else
                        {
                            if (coupon.ExpiryDate < date)
                            {
                                MessageBox.Show("Expired coupon");
                                return; // Exit the method if the coupon is expired
                            }
                        }
                    }

                    if (quantity < 1)
                    {
                        MessageBox.Show("Please enter a valid quantity");
                    }
                    else
                    {

                        if (quantity <= product.Quantity)
                        {
                            // Create a new Product object using the input values
                            UpdateOrder = new Order
                            {
                                Customer = customer,
                                Quantity = quantity,
                                Product = product,
                                Coupon = coupon
                            };
                            UpdateOrder.Id = id;
                            UpdateOrder.OrderDate = Datepicker.SelectedDate.Value;
                            this.DialogResult = true;
                        }
                        else
                        {
                            MessageBox.Show("This product don't have that much quantity");
                        }
                    }
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
            Product product = (Product)comboBoxProduct.SelectedItem;
            Quantity_Update_Label.Content = $"Quantity (Max: {product.Quantity})";
        }

        private void comboBoxCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
