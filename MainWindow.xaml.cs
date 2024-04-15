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
using System.Globalization;
using Microsoft.Identity.Client;
using LiveCharts;

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
            this.Closing += MainWindow_Closing;

            if (string.IsNullOrEmpty(Properties.Settings.Default.ItemsPerProductPage))
            {
                Properties.Settings.Default.ItemsPerProductPage = "7";
                Properties.Settings.Default.Save();
            }

            if (string.IsNullOrEmpty(Properties.Settings.Default.ItemsPerOrderPage))
            {
                Properties.Settings.Default.ItemsPerOrderPage = "9";
                Properties.Settings.Default.Save();
            }
            itemsPerPage = int.Parse(Properties.Settings.Default.ItemsPerProductPage);
            itemsPerOrderPage = int.Parse(Properties.Settings.Default.ItemsPerOrderPage);

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
                this.DataContext = new MyViewModel { VersionText = "Trial Version", SubVersionText = $"You got {daysLeft} day(s) left" };
            }
            else
            {
                this.DataContext = new MyViewModel { VersionText = "Full Version", SubVersionText = $"Unlimited access" };
            }
        }
        private int currentPage;
        private int currentOrderPage;
        private int itemsPerOrderPage;
        private int itemsPerPage;

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
        private void PaginationOrderListBox()
        {
            BindingList<Order> listBoxOrder = _orders;
            while (listBoxOrder.Count <= currentOrderPage * itemsPerOrderPage)
            {
                currentOrderPage--;
            }
            OrderPage.Text = $"{currentOrderPage + 1}/{(listBoxOrder.Count - 1) / itemsPerOrderPage + 1}";
            int startIndex = currentOrderPage * itemsPerOrderPage;
            int endIndexOrder = Math.Min(startIndex + itemsPerOrderPage, listBoxOrder.Count);

            var pageOrders = _orders.Skip(startIndex).Take(endIndexOrder - startIndex);
            ListBoxOrder.ItemsSource = pageOrders;

        }

        private void NextPageButtonProduct_Click(object sender, RoutedEventArgs e)
        {
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
        private void NextPageButtonOrder_Click(object sender, RoutedEventArgs e)
        {
            if (currentOrderPage < (_orders.Count - 1) / itemsPerOrderPage)
            {
                currentOrderPage++;

                PaginationOrderListBox();
            }
        }

        private void PreviousPageButtonOrder_Click(object sender, RoutedEventArgs e)
        {
            currentOrderPage = Math.Max(0, currentOrderPage - 1);
            PaginationOrderListBox();
        }


        private void ApplySelectionAndFilter()
        {
            int selectedPriceIndex = comboBox1.SelectedIndex;
            var selectedCategory = comboBox.SelectedItem as Category;
            int selectedCategoryIndex = comboBox.SelectedIndex;
            LoadData();
            Update_DashBoard();
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
            PaginationOrderListBox();
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
            ListBoxCustomers.ItemsSource = _customers;

            OrdersService orderService = new OrdersService();
            _orders = orderService.GetAllOrders();
            ListBoxOrder.ItemsSource = _orders;

            CouponService couponService = new CouponService();
            _coupons = couponService.GetAllCoupons();
            ListBoxCoupon.ItemsSource = _coupons;

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
            Order.Foreground = Brushes.White;
            Coupon.Background = Brushes.Transparent;
            Coupon.Foreground = Brushes.White;
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
            CouponScreen.Visibility = Visibility.Hidden;
        }
        private void Exit_button_Click(object sender, RoutedEventArgs e)
        {
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
            MessageBoxResult result = MessageBox.Show(
            "Are you sure you want to logout?",
            "Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (Properties.Settings.Default.ToggleCheckpoint == true)
                {
                    if (DashboardScreen.Visibility == Visibility.Visible)
                    {
                        Properties.Settings.Default.Checkpoint = "Dashboard";
                    }

                    if (CategoryScreen.Visibility == Visibility.Visible)
                    {
                        Properties.Settings.Default.Checkpoint = "Category";
                    }

                    if (CustomerScreen.Visibility == Visibility.Visible)
                    {
                        Properties.Settings.Default.Checkpoint = "Customer";
                    }

                    if (OrderScreen.Visibility == Visibility.Visible)
                    {
                        Properties.Settings.Default.Checkpoint = "Order";
                    }

                    if (SettingScreen.Visibility == Visibility.Visible)
                    {
                        Properties.Settings.Default.Checkpoint = "Setting";
                    }

                    if (ProductScreen.Visibility == Visibility.Visible)
                    {
                        Properties.Settings.Default.Checkpoint = "Product";
                    }

                    // Save the settings after updating the Checkpoint
                    Properties.Settings.Default.Save();

                }
                else
                {
                    if (DashboardScreen.Visibility == Visibility.Visible)
                    {
                        Properties.Settings.Default.Checkpoint = "Dashboard";
                    }
                    Properties.Settings.Default.Save();
                }
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

            NumberOfOrdersTextBox.Text = itemsPerOrderPage.ToString();
            NumberOfProductsTextBox.Text = itemsPerPage.ToString();
            CheckBox.IsChecked = Properties.Settings.Default.ToggleCheckpoint;

            Setting.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Setting.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));
        }
        private void DashBoard_Click(object sender, RoutedEventArgs e)
        {
            setVisibleOff();
            setButtonDashBoard();
            Update_DashBoard();
            DashboardScreen.Visibility = Visibility.Visible;
            DashBoard.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            DashBoard.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));
        }
        private void Order_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            currentOrderPage = 0;
            PaginationOrderListBox();
            OrderScreen.Visibility = Visibility.Visible;
            Order.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Order.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));
        }
        private void Coupon_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            CouponScreen.Visibility = Visibility.Visible;
            Coupon.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Coupon.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));
        }

        BindingList<Category> _categories = new BindingList<Category>();
        ObservableCollection<Product> _products = new ObservableCollection<Product>();
        BindingList<Customer> _customers = new BindingList<Customer>();
        BindingList<Order> _orders = new BindingList<Order>();
        BindingList<Order> _orders1 = new BindingList<Order>();
        BindingList<Coupon> _coupons = new BindingList<Coupon>();

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

                // Loop until a valid file is selected or the user cancels
                while (true)
                {
                    if (openFileDialog.ShowDialog() == true)
                    {
                        string filename = openFileDialog.FileName;
                        // Assuming you have a method to validate the file, e.g., check file size or type
                        DataImportService dataImportService = new DataImportService();
                        dataImportService.ImportDataFromExcel(filename);
                        currentPage = 0;
                        break; // Exit the loop if a valid file is selected
                    }
                }
                LoadData();
            }
            setVisibleOff();
            if (Properties.Settings.Default.Checkpoint == "Dashboard")
            {
                DashboardScreen.Visibility = Visibility.Visible;
                DashBoard_Click(sender, e);
            }

            if (Properties.Settings.Default.Checkpoint == "Category")
            {
                CategoryScreen.Visibility = Visibility.Visible;
                Category_Click(sender, e);
            }

            if (Properties.Settings.Default.Checkpoint == "Customer")
            {
                CustomerScreen.Visibility = Visibility.Visible;
                Customer_Click(sender, e);
            }

            if (Properties.Settings.Default.Checkpoint == "Order")
            {
                OrderScreen.Visibility = Visibility.Visible;
                Order_Click(sender, e);
            }

            if (Properties.Settings.Default.Checkpoint == "Setting")
            {
                SettingScreen.Visibility = Visibility.Visible;
                Setting_Click(sender, e);
            }

            if (Properties.Settings.Default.Checkpoint == "Product")
            {
                ProductScreen.Visibility = Visibility.Visible;
                Products_Click(sender, e);
            }

            if (Properties.Settings.Default.Checkpoint == "Coupon")
            {
                CouponScreen.Visibility = Visibility.Visible;
                Coupon_Click(sender, e);
            }
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
            MessageBoxResult result = MessageBox.Show(
            "Import new data will clear all old data in database. Are you sure you want to import new data? ",
            "Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
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
            var screen = new AddOrderWindow(_customers, _products, _coupons);
            // Show the window modally and wait for the user to close it
            bool? result = screen.ShowDialog();

            // Check if the user clicked the submit button (usually represented by a positive result)
            if (result == true)
            {
                var newOrder = screen.AddOrder;
                OrdersService ordersService = new OrdersService();
                ordersService.InsertOrder(newOrder);


                // Show a message to the user
                MessageBox.Show("Order added successfully!");

                ApplySelectionAndFilter();
            }
        }

        private void Update_Order_Click(object sender, RoutedEventArgs e)
        {
            // Get the button that raised the event
            Button button = (Button)sender;
            // Use VisualTreeHelper to find the ListBoxItem
            ListBoxItem listBoxItem = FindParent<ListBoxItem>((DependencyObject)button);
            var order = listBoxItem.DataContext as Order;

            var screen = new UpdateOrderWindow(order, _customers, _products, _coupons);
            // Show the window modally and wait for the user to close it
            bool? result = screen.ShowDialog();

            // Check if the user clicked the submit button (usually represented by a positive result)
            if (result == true)
            {
                order = screen.UpdateOrder;
                OrdersService orderService = new OrdersService();


                orderService.UpdateOrder(order);

                // Show a message to the user
                MessageBox.Show("Order updated successfully!");

                ApplySelectionAndFilter();
            }

        }

        private void Delete_Order_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
           "Are you sure you want delete this order?",
           "Confirmation",
           MessageBoxButton.YesNo,
           MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Get the button that raised the event
                Button button = (Button)sender;
                // Use VisualTreeHelper to find the ListBoxItem
                ListBoxItem listBoxItem = FindParent<ListBoxItem>((DependencyObject)button);
                var order = listBoxItem.DataContext as Order;

                if (order != null)
                {
                    var id = order.Id;

                    // Use the DeleteProduct method from the business logic layer
                    OrdersService orderService = new OrdersService();
                    orderService.DeleteOrder(id);

                    // Show a message to the user

                    MessageBox.Show($"Delete order successfully!");
                    ApplySelectionAndFilter();
                }
            }
        }

        private void Detail_Order_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            // Use VisualTreeHelper to find the ListBoxItem
            ListBoxItem listBoxItem = FindParent<ListBoxItem>((DependencyObject)button);
            var order = listBoxItem.DataContext as Order;

            var screen = new DetailOrderWindow(order);
            // Show the window modally and wait for the user to close it
            bool? result = screen.ShowDialog();
            if (result == true)
            {
            }
        }



        private void Add_Coupon_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddCouponWindow(_coupons);
            bool? result = screen.ShowDialog();

            if (result == true)
            {
                var newCoupon = screen._addCoupon;
                CouponService couponsService = new CouponService();
                couponsService.InsertCoupon(newCoupon);

                MessageBox.Show("Coupon added successfully!");
                ApplySelectionAndFilter();
            }
        }

        private void Delete_Coupon_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
       "Are you sure you want to delete this coupon?",
       "Confirmation",
       MessageBoxButton.YesNo,
       MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Button button = (Button)sender;
                ListBoxItem listBoxItem = FindParent<ListBoxItem>((DependencyObject)button);
                var coupon = listBoxItem.DataContext as Coupon;

                if (coupon != null)
                {
                    var id = coupon.Id;
                    CouponService couponService = new CouponService();
                    couponService.DeleteCoupon(id);

                    MessageBox.Show("Coupon deleted successfully!");
                    ApplySelectionAndFilter();
                }
            }
        }

        private void Update_Coupon_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ListBoxItem listBoxItem = FindParent<ListBoxItem>((DependencyObject)button);
            var coupon = listBoxItem.DataContext as Coupon;

            var screen = new UpdateCouponWindow(coupon, _coupons);
            bool? result = screen.ShowDialog();

            if (result == true)
            {
                coupon = screen._updateCoupon;
                CouponService couponService = new CouponService();
                couponService.UpdateCoupon(coupon);

                MessageBox.Show("Coupon updated successfully!");
                ApplySelectionAndFilter();
            }
        }


        private int Price_Order_Click_Count = 0;

        private void Product_Price_Order_Click(object sender, RoutedEventArgs e)
        {
            Price_Order_Click_Count++; // Increment the click count

            // Sort in ascending order if click count is odd, otherwise sort in descending order
            if (Price_Order_Click_Count % 2 == 1) // Odd click count
            {
                _products1.Sort((p1, p2) => p1.Price.CompareTo(p2.Price));
                Product_Price_Order.Content = "🔼";


            }
            else // Even click count
            {
                _products1.Sort((p1, p2) => p2.Price.CompareTo(p1.Price));
                Product_Price_Order.Content = "🔽";


            }
            PaginationProductListBox();
        }
        private int Quantity_Order_Click_Count = 0;

        private void Product_Quantity_Order_Click(object sender, RoutedEventArgs e)
        {
            Quantity_Order_Click_Count++; // Increment the click count

            // Sort in ascending order if click count is odd, otherwise sort in descending order
            if (Quantity_Order_Click_Count % 2 == 1) // Odd click count
            {
                _products1.Sort((p1, p2) => p1.Quantity.CompareTo(p2.Quantity));
                Product_Quantity_Order.Content = "🔼";
            }
            else // Even click count
            {
                _products1.Sort((p1, p2) => p2.Quantity.CompareTo(p1.Quantity));
                Product_Quantity_Order.Content = "🔽";

            }
            PaginationProductListBox();
        }

        private int Order_Quantity_Order_Click_Count = 0;
        private void Order_Quantity_Order_Click(object sender, RoutedEventArgs e)
        {
            List<Order> orders = new List<Order>();
            orders = _orders.ToList<Order>();

            Order_Quantity_Order_Click_Count++;
            if (Order_Quantity_Order_Click_Count % 2 == 1) // Odd click count
            {
                orders.Sort((p1, p2) => p1.Quantity.CompareTo(p2.Quantity));
                Order_Quantity_Order.Content = "🔼";
            }
            else // Even click count
            {
                orders.Sort((p1, p2) => p2.Quantity.CompareTo(p1.Quantity));
                Order_Quantity_Order.Content = "🔽";
            }
            _orders.Clear();
            foreach (var order in orders)
            {
                _orders.Add(order);
            }
            PaginationOrderListBox();
        }
        private int Order_Date_Order_Click_Count = 0;

        private void Order_Date_Order_Click(object sender, RoutedEventArgs e)
        {

            List<Order> orders = new List<Order>();
            orders = _orders.ToList<Order>();

            Order_Date_Order_Click_Count++;
            if (Order_Date_Order_Click_Count % 2 == 1) // Odd click count
            {
                orders.Sort((p1, p2) => p1.OrderDate.CompareTo(p2.OrderDate));
                Order_Date_Order.Content = "🔼";
            }
            else // Even click count
            {
                orders.Sort((p1, p2) => p2.OrderDate.CompareTo(p1.OrderDate));
                Order_Date_Order.Content = "🔽";
            }
            _orders.Clear();
            foreach (var order in orders)
            {
                _orders.Add(order);
            }
            PaginationOrderListBox();
        }

        private void SearchButton_Date_Click(object sender, RoutedEventArgs e)
        {
            DateTime startDate = StartDatePicker.SelectedDate.Value;
            DateTime endDate = EndDatePicker.SelectedDate.Value;
            _orders1.Clear();
            foreach (var order in _orders)
            {
                _orders1.Add(order);
            }
            Debug.WriteLine(_orders1.Count);
            if (startDate != null && endDate != null)
            {
                var filteredOrders = _orders.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate).ToList();
                _orders.Clear();
                foreach (var order in filteredOrders)
                {
                    _orders.Add(order);
                }
                PaginationOrderListBox();
                _orders.Clear();

                foreach (var order in _orders1)
                {
                    _orders.Add(order);
                }
            }
            // Assuming _orders is your BindingList<Order>

        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int newItemsPerProductPage = int.Parse(NumberOfProductsTextBox.Text);
                int newItemsPerOrderPage = int.Parse(NumberOfOrdersTextBox.Text);
                if (newItemsPerProductPage > 7 || newItemsPerProductPage < 1 || newItemsPerOrderPage > 9 || newItemsPerOrderPage < 1)
                {
                    MessageBox.Show("Please enter value in valid range");
                }
                else
                {
                    Properties.Settings.Default.ItemsPerProductPage = NumberOfProductsTextBox.Text;
                    Properties.Settings.Default.ItemsPerOrderPage = NumberOfOrdersTextBox.Text;
                    Properties.Settings.Default.ToggleCheckpoint = CheckBox.IsChecked.HasValue && CheckBox.IsChecked.Value;
                    Properties.Settings.Default.Save();

                }
                itemsPerPage = newItemsPerProductPage;
                itemsPerOrderPage = newItemsPerOrderPage;
                MessageBox.Show("Succesfully save your settings");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please only enter number in these field");
            }
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Properties.Settings.Default.ToggleCheckpoint == true)
            {
                if (DashboardScreen.Visibility == Visibility.Visible)
                {
                    Properties.Settings.Default.Checkpoint = "Dashboard";
                }

                if (CategoryScreen.Visibility == Visibility.Visible)
                {
                    Properties.Settings.Default.Checkpoint = "Category";
                }

                if (CustomerScreen.Visibility == Visibility.Visible)
                {
                    Properties.Settings.Default.Checkpoint = "Customer";
                }

                if (OrderScreen.Visibility == Visibility.Visible)
                {
                    Properties.Settings.Default.Checkpoint = "Order";
                }

                if (SettingScreen.Visibility == Visibility.Visible)
                {
                    Properties.Settings.Default.Checkpoint = "Setting";
                }

                if (ProductScreen.Visibility == Visibility.Visible)
                {
                    Properties.Settings.Default.Checkpoint = "Product";
                }

                if (CouponScreen.Visibility == Visibility.Visible)
                {
                    Properties.Settings.Default.Checkpoint = "Coupon";
                }
                // Save the settings after updating the Checkpoint
                Properties.Settings.Default.Save();

            }
            else
            {
                if (DashboardScreen.Visibility == Visibility.Visible)
                {
                    Properties.Settings.Default.Checkpoint = "Dashboard";
                }
                Properties.Settings.Default.Save();
            }
        }

        private void Sales_Loaded(object sender, RoutedEventArgs e)
        {
      
        }


        private void Update_DashBoard()
        {
            decimal purchase = 0;
            decimal profit = 0;
            int numberSale = 0;
            var ordersGroupedByDate = _orders.GroupBy(order => order.OrderDate.Date)
                                              .OrderBy(group => group.Key);
            if (_orders.Count != 0)
            {
                // Calculate income and profit for each day
                List<float> incomePerDay = new List<float>();
                List<float> profitPerDay = new List<float>();

                foreach (var group in ordersGroupedByDate)
                {
                    float dailyIncome = 0;
                    float dailyProfit = 0;

                    foreach (var order in group)
                    {
                        dailyIncome += order.Product.Price * order.Quantity / 1000000;
                        dailyProfit += order.Product.Price * order.Quantity / 1000000 * 15 / 100;
                    }

                    incomePerDay.Add(dailyIncome);
                    profitPerDay.Add(dailyProfit);
                }

                // Set the X-axis labels
                var xAxisLabels = ordersGroupedByDate.Select(group => group.Key.ToShortDateString()).OrderBy(date => DateTime.Parse(date)).ToList();
                chartMain.AxisX[0].Labels = xAxisLabels; // Assuming you have only one X-axis

                // Set the Y-axis range based on the maximum values in incomePerDay and profitPerDay
                double maxYValue = Math.Max(incomePerDay.Max(), profitPerDay.Max()) * 1.5; // Add some margin
                chartMain.AxisY[0].MaxValue = maxYValue; // Assuming you have only one Y-axis

                // Set the values to the chart
                Income_Line.Values = new ChartValues<float>(incomePerDay);
                Profit_Line.Values = new ChartValues<float>(profitPerDay);

                
            }
            BindingList<Product> _5products = new BindingList<Product>();

            var top5ProductsWithQuantity = _orders
                     .GroupBy(order => order.Product)
                     .Select(group => new { Product = group.Key, TotalQuantity = group.Sum(order => order.Quantity) })
                     .OrderByDescending(group => group.TotalQuantity)
                     .Take(5)
                     .ToList();
            // Convert the list of products to a BindingList
            LisboxTop5Product.ItemsSource = top5ProductsWithQuantity;

            var top5CustomersWithTotalSpent = _orders
                .GroupBy(order => order.Customer) // Group orders by customer
                .Select(group => new
                {
                    Customer = group.Key,
                    TotalSpent = group.Sum(order => order.Product.Price * order.Quantity) // Calculate total price for each customer
                })
                .OrderByDescending(group => group.TotalSpent) // Order by total price in descending order
                .Take(5) // Take the top 5
                .Select(group => new
                {
                    Customer = group.Customer,
                    TotalSpent = string.Format(new CultureInfo("vi-VN"), "{0:C}", group.TotalSpent) // Format total spent as currency in Vietnamese
                })
                .ToList();


            LisboxTop5Spent.ItemsSource = top5CustomersWithTotalSpent;
            // Calculate total number of products sold
            numberSale = _products.Sum(product => product.Quantity);
            In_stock.SubTitle = $"{numberSale} products are being sold";

            // Calculate total purchase
            purchase = _orders.Sum(order => (order.Quantity * order.Product.Price) * ((100 - order.Coupon?.DiscountPercentage) ?? 100) / 100);
            string purchaseInVietnamese = purchase.ToString("C", CultureInfo.CreateSpecificCulture("vi-VN"));
            Purchase.SubTitle = $"Total income {purchaseInVietnamese}";

            // Calculate total profit
            profit = purchase * 15 / 100;
            string profitInVietnamese = profit.ToString("C", CultureInfo.CreateSpecificCulture("vi-VN"));
            Profit.SubTitle = $"Total profit {profitInVietnamese} (15% income)";

        }
    }

}
