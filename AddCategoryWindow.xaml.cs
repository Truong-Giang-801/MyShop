using DocumentFormat.OpenXml.Bibliography;
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
    /// Interaction logic for AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        public Category? _addCategory { get; set; }
        BindingList<Category> _categories = new BindingList<Category>();
        public AddCategoryWindow(BindingList<Category> categories)
        {
            InitializeComponent();
            _categories = new BindingList<Category>(categories.ToList());
        }

        private void Submit_Add_Click(object sender, RoutedEventArgs e)
        {
            string categoryName = CategoryName_Add.Text;

            bool categoryExists = _categories.Any(c => c.CategoryName == categoryName);
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
                    _addCategory = new Category
                    {
                        CategoryName = categoryName
                    };
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

        private void ProductName_Add_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
