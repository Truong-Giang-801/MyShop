﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for DetailOrderWindow.xaml
    /// </summary>
    public partial class DetailOrderWindow : Window
    {
        public DetailOrderWindow(Order order)
        {
            InitializeComponent();
            this.DataContext = order; 
            decimal price = (order.Quantity * order.Product.Price) * ((100-order.Coupon?.DiscountPercentage) ?? 100) / 100;
            Price.Text = price.ToString();
            
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
