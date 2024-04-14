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
using System.Linq;
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
            if (string.IsNullOrEmpty(Properties.Settings.Default.FirstOpenDate))
            {
                // If not, save the current date as the first open date
                Properties.Settings.Default.FirstOpenDate = DateTime.Now.ToString("yyyy-MM-dd");
                Properties.Settings.Default.Save();
            }
            else
            {
                string ActivationKey = "VK7JG-NPHTM-C97JM-9MPGT-3V66T";
                // Convert the FirstOpenDate string back to a DateTime object
                DateTime firstOpenDate = DateTime.Parse(Properties.Settings.Default.FirstOpenDate);

                // Calculate the difference between the current date and the FirstOpenDate
                TimeSpan difference = DateTime.Now - firstOpenDate;
                int daysLeft = 15 - (int)difference.TotalDays;

                // Check if the difference is exactly 15 days
                if (string.IsNullOrEmpty(Properties.Settings.Default.ActivationKey) || Properties.Settings.Default.ActivationKey != "VK7JG-NPHTM-C97JM-9MPGT-3V66T")
                {
                    if (difference.TotalDays < 15)
                    {
                        MessageBoxResult result = MessageBox.Show(
                        $"Your trial version got {daysLeft} days left. Do you want to enter your activation key?",
                        "Trial Version",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            var screen = new ActivationKeyInput();
                            // Show the window modally and wait for the user to close it
                            bool? state = screen.ShowDialog();

                            // Check if the user clicked the submit button (usually represented by a positive result)
                            if (state == true)
                            {
                                Properties.Settings.Default.ActivationKey = screen.activationKey;
                                Properties.Settings.Default.Save();
                            }
                        }
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show(
                        $"Your trial version expired. Please enter your activation key",
                        "Trial Version expired",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                        var screen = new ActivationKeyInput();
                        // Show the window modally and wait for the user to close it
                        bool? state = screen.ShowDialog();

                        // Check if the user clicked the submit button (usually represented by a positive result)
                        if (state == true)
                        {
                            Properties.Settings.Default.ActivationKey = screen.activationKey;
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            Application.Current.Shutdown();
                        }
                    }
                }
            }
            UpdateUIBasedOnActivationKey();
        }
        private void UpdateUIBasedOnActivationKey()
        {
            string ActivationKey = "VK7JG-NPHTM-C97JM-9MPGT-3V66T";
            if (string.IsNullOrEmpty(Properties.Settings.Default.ActivationKey) || Properties.Settings.Default.ActivationKey != ActivationKey)
            {
                // Calculate the number of days left for the trial version
                DateTime firstOpenDate = DateTime.Parse(Properties.Settings.Default.FirstOpenDate);
                TimeSpan difference = DateTime.Now - firstOpenDate;
                int daysLeft = 15 - (int)difference.TotalDays;

                // Update the TextBlock to show the number of days left
                this.DataContext = new MyViewModel { VersionText = "Trial Version", SubVersionText = $"You got {daysLeft} day(s) left"};
            }
            else
            {
                this.DataContext = new MyViewModel { VersionText = "Full Version", SubVersionText = $"Unlimited access" };
            }
        }
        private int itemsPerPage = 7;
        private int currentPage = 0;

        private void PaginationProductListBox()
        {
            List<Product> listBoxProduct = _products1;
            while (listBoxProduct.Count <= currentPage * itemsPerPage)
            {
                currentPage--;
            }
            ProductPage.Text = $"{currentPage + 1}/{(listBoxProduct.Count - 1) / itemsPerPage + 1}";
            int startIndex = currentPage * itemsPerPage;
            int endIndexProduct = Math.Min(startIndex + itemsPerPage, listBoxProduct.Count);

            var pageProducts = _products1.Skip(startIndex).Take(endIndexProduct - startIndex);
            ListBoxProducts.ItemsSource = pageProducts;

        }

        private void NextPageButtonProduct_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(_products1.Count);
            if (currentPage < (_products1.Count - 1) / itemsPerPage)
            {
                currentPage++;

                PaginationProductListBox();
            }
        }

        private void PreviousPageButtonProduct_Click(object sender, RoutedEventArgs e)
        {
            currentPage = Math.Max(0, currentPage - 1);
            PaginationProductListBox();
        }

        private void ApplySelectionAndFilter()
        {
            int selectedPriceIndex = comboBox1.SelectedIndex;
            var selectedCategory = comboBox.SelectedItem as Category;
            int selectedCategoryIndex = comboBox.SelectedIndex;
            LoadData();
            comboBox1.SelectedIndex = selectedPriceIndex;
            comboBox.SelectedIndex = selectedCategoryIndex;
            ApplyFilter(selectedCategoryIndex, selectedCategory.CategoryName, selectedPriceIndex);

        }
        private void ApplySearch()
        {
            string searchText = textBoxSearch.Text.ToLower();

            if (comboBox.SelectedIndex == 0 && comboBox1.SelectedIndex == 0)
            {
                _products1 = new List<MyShop.Product>(_products);
            }
            // Filter the products based on the search text
            _products1 = _products1.Where(p => p.ProductName.ToLower().Contains(searchText)).ToList();
            PaginationProductListBox();
        }
        public void LoadData()
        {
            // Using ProductService to fetch all products from the database
            ProductsService productService = new ProductsService();
            _products = productService.GetAllProducts();

            // Using CategoryService to fetch all categories from the database
            CategoryService categoryService = new CategoryService();
            _categories = categoryService.GetAllCategories();
            ListBoxCategories.ItemsSource = _categories;

            CustomerService customerService = new CustomerService();
            _customers = customerService.GetAllCustomers();
            Debug.WriteLine(_customers.Count);
            ListBoxCustomers.ItemsSource = _customers;

            // Clone the BindingList and add an "All" category
            BindingList<Category> clonedCategories = categoryService.AddAllObjectToCategories(_categories);
            var temp = currentPage;
            comboBox.ItemsSource = clonedCategories;
            comboBox.SelectedIndex = 0;
            currentPage = temp;
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
            Order.Background = Brushes.Transparent;
            Order.Foreground= Brushes.White;
            Exit_button.Background = Brushes.Transparent;
            Exit_button.Foreground = Brushes.White;
            
        }
        private void setVisibleOff()
        {
            DashboardScreen.Visibility = Visibility.Hidden;
            CategoryScreen.Visibility = Visibility.Hidden;
            CustomerScreen.Visibility = Visibility.Hidden;
            OrderScreen.Visibility = Visibility.Hidden;
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
            currentPage = 0;
            PaginationProductListBox();
            ProductScreen.Visibility = Visibility.Visible;
            Products.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Products.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));
        }
        private void Category_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            currentPage = 0;
            CategoryScreen.Visibility = Visibility.Visible;
            Category.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Category.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));
        }
        private void Customer_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            currentPage = 0;
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
        BindingList<Customer> _customers = new BindingList<Customer>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                LoadData();
            }
            catch (Exception ex)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls;*.csv";
                if (openFileDialog.ShowDialog() == true)
                {
                    string filename = openFileDialog.FileName;
                    DataImportService dataImportService = new DataImportService();
                    dataImportService.ImportDataFromExcel(filename);
                    currentPage = 0;
                }
                LoadData();
            }
            setVisibleOff();
            DashboardScreen.Visibility = Visibility.Visible;

        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private static List<Product> _products1 = new List<Product>();
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPage = 0;
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
            currentPage = 0;
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
                _products1 = _products.ToList();
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
            ApplySearch();
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentPage = 0;
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
                currentPage = 0;
            }

            int selectedPriceIndex = comboBox1.SelectedIndex;
            // Apply combined filter
            var selectedCategory = comboBox.SelectedItem as Category;
            int selectedCategoryIndex = comboBox.SelectedIndex;
            LoadData();
            comboBox1.SelectedIndex = selectedPriceIndex;
            comboBox.SelectedIndex = selectedCategoryIndex;
            ApplyFilter(selectedCategoryIndex, selectedCategory.CategoryName, selectedPriceIndex);
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            currentPage = 0;
            OrderScreen.Visibility = Visibility.Visible;
            Order.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Order.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));
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
            Debug.WriteLine(currentPage);
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
            Debug.WriteLine(currentPage);
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

            }
        }

        private void PreviousPageButtonCategory_Click(object sender, RoutedEventArgs e)
        {
            currentPage = Math.Max(0, currentPage - 1);
        }

        private void Add_Customer_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddCustomerWindow(_customers);
            // Show the window modally and wait for the user to close it
            bool? result = screen.ShowDialog();

            // Check if the user clicked the submit button (usually represented by a positive result)
            if (result == true)
            {

                // Assuming screen._addProduct contains the new product
                var newCustomer = screen._addCustomer;

                // Use the InsertProduct method from the business logic layer
                CustomerService customerService = new CustomerService();
                customerService.InsertCustomer(newCustomer);

                // Show a message to the user
                MessageBox.Show("Customer added successfully!");

                ApplySelectionAndFilter();
            }
        }

        private void Delete_Customer_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
            "Are you sure you want delete this customer?",
            "Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Get the button that raised the event
                Button button = (Button)sender;
                // Use VisualTreeHelper to find the ListBoxItem
                ListBoxItem listBoxItem = FindParent<ListBoxItem>((DependencyObject)button);
                var customer = listBoxItem.DataContext as Customer;

                if (customer != null)
                {
                    var id = customer.Id;

                    // Use the DeleteProduct method from the business logic layer
                    CustomerService customerService = new CustomerService();
                    customerService.DeleteCustomer(id);

                    // Show a message to the user
                    MessageBox.Show($"Delete customer successfully!");
                    ApplySelectionAndFilter();
                }
            }
        }

        private void Update_Customer_Click(object sender, RoutedEventArgs e)
        {
            // Get the button that raised the event
            Button button = (Button)sender;
            // Use VisualTreeHelper to find the ListBoxItem
            ListBoxItem listBoxItem = FindParent<ListBoxItem>((DependencyObject)button);
            var customer = listBoxItem.DataContext as Customer;
            var screen = new UpdateCustomerWindow(customer, _customers);
            // Show the window modally and wait for the user to close it
            bool? result = screen.ShowDialog();

            // Check if the user clicked the submit button (usually represented by a positive result)
            if (result == true)
            {
                customer = screen._updateCustomer;
                // Use the UpdateProduct method from the business logic layer
                CustomerService customerService = new CustomerService();
                customerService.UpdateCustomer(customer);

                // Show a message to the user
                MessageBox.Show("Customer updated successfully!");

                ApplySelectionAndFilter();
            }
        }

        private void textBoxSearchCustomer_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ListBoxCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Add_Order_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Update_Order_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Order_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Detail_Order_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PreviousPageButtonOrder_Click(object sender, RoutedEventArgs e)
        {
            //currentPage = Math.Max(0, currentPage - 1);
        }

        private void NextPageButtonOrder_Click(object sender, RoutedEventArgs e)
        {
            //if (currentPage < orders.Count / itemsPerPage)
            //{
            //    currentPage++;
            //}
        }

    }
}
