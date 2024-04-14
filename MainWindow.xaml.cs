using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyShop.UserControls;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Microsoft.Win32;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Data;
using DocumentFormat.OpenXml.Office2010.Excel;
namespace MyShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private int itemsPerPage = 5;
        private int currentPage = 0;

        private void UpdateListBox()
        {
            int startIndex = currentPage * itemsPerPage;
            int endIndex = Math.Min(startIndex + itemsPerPage, _products.Count);

            var pageProducts = _products1.Skip(startIndex).Take(endIndex - startIndex);
            var pageCategories = _categories.Skip(startIndex).Take(endIndex - startIndex);
            ListBoxCategories.ItemsSource = pageCategories;
            ListBoxProducts.ItemsSource = pageProducts;
        }

        private void NextPageButtonProduct_Click(object sender, RoutedEventArgs e)
        {

            if (currentPage < _products1.Count/itemsPerPage)
            {
                currentPage++;

                UpdateListBox();
            }
        }

        private void PreviousPageButtonProduct_Click(object sender, RoutedEventArgs e)
        {
            currentPage = Math.Max(0, currentPage - 1);
            UpdateListBox();
        }

        private void ApplySelectionAndFilter()
        {
            int selectedPriceIndex = comboBox1.SelectedIndex;
            var selectedCategory = comboBox.SelectedItem as Category;
            int selectedCategoryIndex = comboBox.SelectedIndex;
            LoadProductsAndCategories();
            comboBox1.SelectedIndex = selectedPriceIndex;
            comboBox.SelectedIndex = selectedCategoryIndex;
            ApplyFilter(selectedCategoryIndex, selectedCategory.CategoryName, selectedPriceIndex);
            UpdateListBox();

        }
        private void ApplySearch()
        {
            string searchText = textBoxSearch.Text.ToLower();

            if (comboBox.SelectedIndex == 0 && comboBox1.SelectedIndex == 0)
            {
                _products1 = new List<MyShop.Product>(_products);
            }
            // Filter the products based on the search text
            var filteredProducts = _products1.Where(p => p.ProductName.ToLower().Contains(searchText)).ToList();
            ObservableCollection<Product> productsBindingList = new ObservableCollection<Product>(filteredProducts);

            // Update the ListBox with the filtered products
            ListBoxProducts.ItemsSource = productsBindingList;
        }
        public void LoadProductsAndCategories()
        {
            // Using ProductService to fetch all products from the database
            ProductsService productService = new ProductsService();
            _products = productService.GetAllProducts();
            ListBoxProducts.ItemsSource = _products;

            // Using CategoryService to fetch all categories from the database
            CategoryService categoryService = new CategoryService();
            _categories = categoryService.GetAllCategories();
            ListBoxCategories.ItemsSource = _categories;

            // Clone the BindingList and add an "All" category
            BindingList<Category> clonedCategories = categoryService.AddAllObjectToCategories(_categories);
            comboBox.ItemsSource = clonedCategories;
            comboBox.SelectedIndex = 0;
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private bool IsMaximize = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximize)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1280;
                    this.Height = 780;
                    IsMaximize = false;
                }
                else
                {
                }
            }
        }
        private void setButtonDashBoard()
        {
            DashBoard.Background = Brushes.Transparent;
            DashBoard.Foreground = Brushes.White;
            Log_Out.Background = Brushes.Transparent;
            Log_Out.Foreground = Brushes.White;
            Category.Background = Brushes.Transparent;
            Category.Foreground = Brushes.White;
            Customer.Background = Brushes.Transparent;
            Customer.Foreground = Brushes.White;
            Import.Background = Brushes.Transparent;
            Import.Foreground = Brushes.White;
            Setting.Background = Brushes.Transparent;
            Setting.Foreground = Brushes.White;
            Products.Background = Brushes.Transparent;
            Products.Foreground = Brushes.White;
            Exit_button.Background = Brushes.Transparent;
            Exit_button.Foreground = Brushes.White;
        }
        private void setVisibleOff()
        {
            DashboardScreen.Visibility = Visibility.Hidden;
            CategoryScreen.Visibility = Visibility.Hidden;
            CustomerScreen.Visibility = Visibility.Hidden;
            ProfileScreen.Visibility = Visibility.Hidden;
            SettingScreen.Visibility = Visibility.Hidden;
            ProductScreen.Visibility = Visibility.Hidden;
        }
        private void Exit_button_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            Exit_button.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Exit_button.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));
            MessageBoxResult result = MessageBox.Show(
            "Are you sure you want to exit?",
            "Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Perform the action after user confirms.
                // For example, delete a record or save changes.
                Application.Current.Shutdown();
            }
        }
        private void Log_Out_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            Log_Out.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Log_Out.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));
            MessageBoxResult result = MessageBox.Show(
            "Are you sure you want to logout?",
            "Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Perform the action after user confirms.
                // For example, delete a record or save changes.
                Properties.Settings.Default.Username = "";
                Properties.Settings.Default.EncryptedPassword = "";
                Properties.Settings.Default.Save();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }
        private void Products_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            ProductScreen.Visibility = Visibility.Visible;
            Products.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Products.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));
        }
        private void Category_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            CategoryScreen.Visibility = Visibility.Visible;
            Category.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Category.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));
        }
        private void Customer_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            CustomerScreen.Visibility = Visibility.Visible;
            Customer.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Customer.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));
        }
        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            SettingScreen.Visibility = Visibility.Visible;
            Setting.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Setting.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));
        }
        private void DashBoard_Click(object sender, RoutedEventArgs e)
        {
            setVisibleOff();
            setButtonDashBoard();
            DashboardScreen.Visibility = Visibility.Visible;
            DashBoard.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            DashBoard.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));
        }
        BindingList<Category> _categories = new BindingList<Category>();
        ObservableCollection<Product> _products = new ObservableCollection<Product>();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProductsAndCategories();
            setVisibleOff();
            DashboardScreen.Visibility = Visibility.Visible;
            UpdateListBox();

        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        List<Product> _products1 = new List<Product>();
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Assuming comboBox is for category selection
            if (comboBox.SelectedIndex == -1)
            {
                placeholderText.Visibility = Visibility.Visible;
            }
            else
            {
                var selectedCategory = comboBox.SelectedItem as Category;

                if (selectedCategory == null)
                {
                    Debug.WriteLine("Selected item is not a Category object.");
                }
                else
                {
                    // Update the category selection state
                    string selectedCategoryName = selectedCategory.CategoryName;
                    int selectedCategoryIndex = comboBox.SelectedIndex;
                    // Apply combined filter
                    ApplyFilter(selectedCategoryIndex, selectedCategoryName, comboBox1.SelectedIndex);
                }
            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Assuming comboBox1 is for price selection
            if (comboBox1.SelectedIndex == -1)
            {
                placeholderText1.Visibility = Visibility.Visible;
            }
            else
            {
                // Update the price selection state
                int selectedPriceIndex = comboBox1.SelectedIndex;
                // Apply combined filter
                var selectedCategory = comboBox.SelectedItem as Category;
                int selectedCategoryIndex = comboBox.SelectedIndex;
                if (selectedCategory != null)
                {
                    ApplyFilter(selectedCategoryIndex, selectedCategory.CategoryName, selectedPriceIndex);
                }
                else
                {
                    // Handle the case where the selected item is not a Category object or is null
                }
            }
        }

        private void ApplyFilter(int selectedCategoryIndex, string selectedCategoryName, int selectedPriceIndex)
        {
            _products1.Clear();
            if (selectedCategoryIndex == 0 && selectedPriceIndex == 0)
            {
                ListBoxProducts.ItemsSource = _products;
                ApplySearch();
                return;
            }

            // Determine the price threshold based on the selected price index
            int priceThreshold = 0;
            if (selectedPriceIndex >= 1 && selectedPriceIndex <= 4)
            {
                priceThreshold = 5000000 * selectedPriceIndex;
            }


            // Filter products based on category and price
            if (selectedCategoryIndex == 0)
            {
                _products1 = _products.Where(p => p.Price <= priceThreshold).ToList();
            }
            else
            {
                var selectedCategory = _categories.FirstOrDefault(c => c.CategoryName == selectedCategoryName);
                if (selectedPriceIndex == 0 && selectedCategory != null)
                {
                    _products1 = _products.Where(p => p.Category.Id == selectedCategory.Id).ToList();
                }
                else if (selectedCategory != null && selectedPriceIndex > -1)
                {
                    _products1 = _products.Where(p => p.Category.Id == selectedCategory.Id && p.Price <= priceThreshold).ToList();
                }
            }

            ObservableCollection<Product> productsBindingList = new ObservableCollection<Product>(_products1);
            ListBoxProducts.ItemsSource = productsBindingList;
            ApplySearch();
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplySearch();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xlsx;*.xls;*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileName;
                DataImportService dataImportService = new DataImportService();
                dataImportService.ImportDataFromExcel(filename);
            }

            int selectedPriceIndex = comboBox1.SelectedIndex;
            // Apply combined filter
            var selectedCategory = comboBox.SelectedItem as Category;
            int selectedCategoryIndex = comboBox.SelectedIndex;
            LoadProductsAndCategories();
            comboBox1.SelectedIndex = selectedPriceIndex;
            comboBox.SelectedIndex= selectedCategoryIndex;
            ApplyFilter(selectedCategoryIndex, selectedCategory.CategoryName, selectedPriceIndex);
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            
        }  

        private void Coupon_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Add_Product_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddProductWindow(_categories);
            // Show the window modally and wait for the user to close it
            bool? result = screen.ShowDialog();

            // Check if the user clicked the submit button (usually represented by a positive result)
            if (result == true)
            {

                // Assuming screen._addProduct contains the new product
                var newProduct = screen._addProduct;

                // Use the InsertProduct method from the business logic layer
                ProductsService productsService = new ProductsService();
                productsService.InsertProduct(newProduct);

                // Show a message to the user
                MessageBox.Show("Product added successfully!");


                ApplySelectionAndFilter();
            }

        }

        private void Delete_Product_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
            "Are you sure you want delete this product?",
            "Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Get the button that raised the event
                Button button = (Button)sender;
                // Use VisualTreeHelper to find the ListBoxItem
                ListBoxItem listBoxItem = FindParent<ListBoxItem>((DependencyObject)button);
                var product = listBoxItem.DataContext as Product;

                if (product != null)
                {
                    var id = product.Id;

                    // Use the DeleteProduct method from the business logic layer
                    ProductsService productsService = new ProductsService();
                    productsService.DeleteProduct(id);

                    // Show a message to the user
                    MessageBox.Show($"Delete product successfully!");

                    ApplySelectionAndFilter();
                }
            }
        }

        private void Update_Product_Click(object sender, RoutedEventArgs e)
        {
            // Get the button that raised the event
            Button button = (Button)sender;
            // Use VisualTreeHelper to find the ListBoxItem
            ListBoxItem listBoxItem = FindParent<ListBoxItem>((DependencyObject)button);
            var product = listBoxItem.DataContext as Product;

            var screen = new UpdateProductWindow(product, _categories);
            // Show the window modally and wait for the user to close it
            bool? result = screen.ShowDialog();

            // Check if the user clicked the submit button (usually represented by a positive result)
            if (result == true)
            {
                product = screen._updateProduct;
                // Use the UpdateProduct method from the business logic layer
                ProductsService productsService = new ProductsService();
                productsService.UpdateProduct(product);

                // Show a message to the user
                MessageBox.Show("Product updated successfully!");


                ApplySelectionAndFilter();
            }
        }

        private void Delete_Category_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
            "Are you sure you want delete this category?",
            "Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                result = MessageBox.Show(
            "WARNING: Delete a category will also delete all products in that category. Are you sure you want to proceed?",
            "Warning",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Get the button that raised the event
                    Button button = (Button)sender;
                    // Use VisualTreeHelper to find the ListBoxItem
                    ListBoxItem listBoxItem = FindParent<ListBoxItem>((DependencyObject)button);
                    var category = listBoxItem.DataContext as Category;

                    if (category != null)
                    {
                        var id = category.Id;

                        // Use the DeleteProduct method from the business logic layer
                        CategoryService categoryService = new CategoryService();
                        categoryService.DeleteCategory(id);

                        // Show a message to the user
                        MessageBox.Show($"Delete category successfully!");
                        ApplySelectionAndFilter();
                    }
                }
            }
        }

        private void Add_Category_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddCategoryWindow(_categories);
            // Show the window modally and wait for the user to close it
            bool? result = screen.ShowDialog();

            // Check if the user clicked the submit button (usually represented by a positive result)
            if (result == true)
            {

                // Assuming screen._addProduct contains the new product
                var newCategory = screen._addCategory;

                // Use the InsertProduct method from the business logic layer
                CategoryService categoryService = new CategoryService();
                categoryService.InsertCategory(newCategory);
                MessageBox.Show("Category added successfully!");

                ApplySelectionAndFilter();
            }
        }

        private void Update_Category_Click(object sender, RoutedEventArgs e)
        {
            // Get the button that raised the event
            Button button = (Button)sender;
            // Use VisualTreeHelper to find the ListBoxItem
            ListBoxItem listBoxItem = FindParent<ListBoxItem>((DependencyObject)button);
            var category = listBoxItem.DataContext as Category;
            var screen = new UpdateCategoryWindow(category, _categories);
            // Show the window modally and wait for the user to close it
            bool? result = screen.ShowDialog();

            // Check if the user clicked the submit button (usually represented by a positive result)
            if (result == true)
            {
                category = screen._updateCategory;
                // Use the UpdateProduct method from the business logic layer
                CategoryService categoryService = new CategoryService();
                categoryService.UpdateCategory(category);

                // Show a message to the user
                MessageBox.Show("Category updated successfully!");

                ApplySelectionAndFilter();
            }
        }
        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            // Get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            // We've reached the end of the tree
            if (parentObject == null) return null;

            // Check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                // Use recursion to proceed with next level
                return FindParent<T>(parentObject);
            }
        }

        private void ListBoxProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void NextPageButtonCategory_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < _categories.Count / itemsPerPage)
            {
                currentPage++;

                UpdateListBox();
            }
        }

        private void PreviousPageButtonCategory_Click(object sender, RoutedEventArgs e)
        {
            currentPage = Math.Max(0, currentPage - 1);
            UpdateListBox();
        }

    }
}
