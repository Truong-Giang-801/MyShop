using System;
using System.Collections.Generic;
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
    /// Interaction logic for UpdateCategoryWindow.xaml
    /// </summary>
    public partial class UpdateCategoryWindow : Window
    {
        public Category? _updateCategory { get; set; }
        BindingList<Category> _categories = new BindingList<Category>();
        public UpdateCategoryWindow(Category _cg, BindingList<Category> categories)
        {
            InitializeComponent();
            _updateCategory = (Category)_cg.Clone();
            DataContext = _updateCategory;
            _categories = new BindingList<Category>(categories.ToList());
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void ProductName_Update_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Submit_Add_Click(object sender, RoutedEventArgs e)
        {
            string categoryName = CategoryName_Update.Text;

            // Check if the category name already exists in the categories list
            bool categoryExists = _categories.Any(c => c.CategoryName == categoryName && c.Id != _updateCategory.Id);
            if (categoryName != "")
            {
                if (categoryExists)
                {
                    // Show an error message if the category name already exists
                    MessageBox.Show("A category with this name already exists. Please choose a different name.");
                }
                else
                {
                    // If the category name does not exist, update the category
                    _updateCategory.CategoryName = categoryName;
                    this.DialogResult = true;
                }
            }
            else
            {
                MessageBox.Show("Category name can't be blank");
            }
            
        }

        private void Cancel_Add_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
