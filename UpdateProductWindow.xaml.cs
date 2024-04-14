using DocumentFormat.OpenXml.Office2010.Excel;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class UpdateProductWindow : Window
    {
        public Product _updateProduct { get; set; }
        BindingList<Category> _categories = new BindingList<Category>();
        public UpdateProductWindow(Product _pd, BindingList<Category> categories)
        {
            
            InitializeComponent();
            _updateProduct = (Product)_pd.Clone();
            _categories = new BindingList<Category>(categories.ToList());
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

        private void Submit_Update_Click(object sender, RoutedEventArgs e)
        {
            if (ProductName_Update.Text == "" || Price_Update.Text == "" || Quantity_Update.Text == "" || comboBox.SelectedItem == null)
            {
                MessageBox.Show("Please don't leave any field as blank");
            }
            else
            {
                try
                {
                    string productName = ProductName_Update.Text;
                    int price = int.Parse(Price_Update.Text);
                    int quantity = int.Parse(Quantity_Update.Text);
                    Category selectedCategory = (Category)comboBox.SelectedItem;
                    // Create a new Product object using the input values
                    _updateProduct.ProductName = productName;
                    _updateProduct.Quantity = quantity;
                    _updateProduct.Price = price;
                    _updateProduct.Category = selectedCategory;
                    this.DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please enter valid information");
                }

            }
            

        }

        private void Cancel_Update_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _updateProduct;
            comboBox.ItemsSource = _categories;
            // Find the category in _categories that matches _updateProduct.Category.Id
            var selectedCategory = _categories.FirstOrDefault(c => c.Id == _updateProduct.Category.Id);

            // Set the SelectedItem of the ComboBox to the found category
            comboBox.SelectedItem = selectedCategory;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ProductName_Update_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Price_Update_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Quantity_Update_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
