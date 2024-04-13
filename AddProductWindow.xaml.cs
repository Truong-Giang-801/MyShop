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
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public Product? _addProduct { get; set; }
        BindingList<Category> _categories = new BindingList<Category>();
        public AddProductWindow(BindingList<Category> categories)
        {
            InitializeComponent();
            _categories = new BindingList<Category>(categories.ToList());
        }

        private void Submit_Add_Click(object sender, RoutedEventArgs e)
        {
            string productName = ProductName_Add.Text;
            int price = int.Parse(Price_Add.Text);
            int quantity = int.Parse(Quantity_Add.Text);
            Category selectedCategory = (Category)comboBox.SelectedItem;

            // Create a new Product object using the input values
            _addProduct = new Product
            {
                ProductName = productName,
                Price = price,
                Quantity = quantity,
                Category = selectedCategory
            };
            this.DialogResult = true;

        }

        private void Cancel_Add_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Price_Add_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ProductName_Add_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Quantity_Add_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboBox.ItemsSource= _categories;
        }
    }
}
