using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public Product _updateProduct { get; set; }
        public UpdateWindow(Product _pd)
        {
            
            InitializeComponent();
            _updateProduct = (Product)_pd.Clone();
        }

        private void SubmitAdd_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Cancel_Add_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void Price_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Quantity_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _updateProduct;
        }
    }
}
