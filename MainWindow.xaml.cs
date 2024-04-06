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
using DevExpress.Xpo.DB.Helpers;
namespace MyShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class ProductsViewModel
        {
            public ObservableCollection<Products> ProductsList { get; set; }

            public ProductsViewModel()
            {
                ProductsList = new ObservableCollection<Products>();
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ProductsViewModel();

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
                    this.WindowState = WindowState.Maximized;

                    IsMaximize = true;
                }
            }
        }
        private void setButtonDashBoard()
        {
            DashBoard.Background = Brushes.Transparent;
            DashBoard.Foreground = Brushes.White;
            Log_Out.Background = Brushes.Transparent;
            Log_Out.Foreground = Brushes.White;
            Payment.Background = Brushes.Transparent;
            Payment.Foreground = Brushes.White;
            Support.Background = Brushes.Transparent;
            Support.Foreground = Brushes.White;
            Account.Background = Brushes.Transparent;
            Account.Foreground = Brushes.White;
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
            PaymentScreen.Visibility = Visibility.Hidden;
            SupportScreen.Visibility = Visibility.Hidden;
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

        private void Payment_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            PaymentScreen.Visibility = Visibility.Visible;
            Payment.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Payment.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));

        }

        private void Support_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            SupportScreen.Visibility = Visibility.Visible;
            Support.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Support.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));

        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            setButtonDashBoard();
            setVisibleOff();
            ProfileScreen.Visibility = Visibility.Visible;
            Account.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#F7F6F4"));
            Account.Foreground = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FB7657"));

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

        public Products _product;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xlsx;*.xls;*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileName;

                //using (var document = SpreadsheetDocument.Open(filename, false))
                //{
                //    var wbPart = document.WorkbookPart!;
                //    var sheets = wbPart.Workbook.Descendants<Sheet>()!;
                //    var sheet = sheets.FirstOrDefault(s => s.Name == "Product");
                //    var wsPart = (WorksheetPart)(wbPart.GetPartById(sheet.Id!));
                //    var cells = wsPart.Worksheet.Descendants<Cell>();
                //    int row = 2;

                //    while (true)
                //    {
                //        _product = new Products();
                //        // Read from column B
                //        Cell nameCell = cells.FirstOrDefault(c => c?.CellReference == $"B{row}")!;
                //        if (nameCell == null) break; // Exit loop if no more data in column B
                //        string stringId = nameCell.InnerText;
                //        var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault()!;
                //        string name = stringTable.SharedStringTable.ElementAt(int.Parse(stringId)).InnerText;
                //        _product.ProductName = name;
                //        Debug.WriteLine($"Name: {name}");

                //        // Read from column C
                //        Cell anotherCell = cells.FirstOrDefault(c => c?.CellReference == $"C{row}")!;
                //        if (anotherCell == null) break; // Exit loop if no more data in column C
                //        int anotherValue = int.Parse(anotherCell.CellValue.Text); // Directly parse the cell value as an integer
                //        Debug.WriteLine($"Another Value: {anotherValue}");
                //        _product.Price = anotherValue;
                //        // Read from column D
                //        Cell anotherCell1 = cells.FirstOrDefault(c => c?.CellReference == $"D{row}")!;
                //        if (anotherCell1 == null) break; // Exit loop if no more data in column D
                //        int anotherValue1 = int.Parse(anotherCell1.CellValue.Text); // Directly parse the cell value as an integer
                //        Debug.WriteLine($"Another Value: {anotherValue1}");
                //        _product.Category = anotherValue1;

                //        // Add the product to the ProductsList in the ViewModel
                //        ((ProductsViewModel)DataContext).ProductsList.Add(_product);

                //        row++;
                //    }
                //}
                string connectionString = Properties.Settings.Default.ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Connect to Database
                    connection.Open();

                    // Create table
                    string createTableSql = $@"
                        DROP TABLE IF EXISTS Product;
                        DROP TABLE IF EXISTS Category;
                        CREATE TABLE Category (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Name NVARCHAR(255) NOT NULL
                        );
                        DROP TABLE IF EXISTS Product;
                        CREATE TABLE Product (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Name NVARCHAR(255) NOT NULL,
                            Price MONEY NOT NULL,
                            Category INT NOT NULL,
                            Quantity INT NOT NULL,
                            FOREIGN KEY (Category) REFERENCES Category(Id)
                        );";
                    using (SqlCommand createTableCommand = new SqlCommand(createTableSql, connection))
                    {
                        createTableCommand.ExecuteNonQuery();
                    }

                    // Get sheets from Excel file
                    var document = SpreadsheetDocument.Open(filename, false);
                    var wbPart = document.WorkbookPart!;
                    var sheets = wbPart.Workbook.Descendants<Sheet>()!;

                    // Deal with Category sheet
                    var sheet = sheets.FirstOrDefault(s => s.Name == "Category");
                    var wsPart = (WorksheetPart)wbPart.GetPartById(sheet.Id!);
                    var cells = wsPart.Worksheet.Descendants<Cell>();
                    int row = 2;
                    Cell nameCell = cells.FirstOrDefault(c => c?.CellReference == $"B{row}")!;

                    // Insert data from the current sheet into the database
                    while (nameCell != null)
                    {
                        string stringId = nameCell.InnerText;
                        var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault()!;
                        string name = stringTable.SharedStringTable.ElementAt(int.Parse(stringId)).InnerText;

                        string sql = $"INSERT INTO Category (Name) VALUES (@Name)";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Name", name);
                            command.ExecuteNonQuery();
                        }

                        row++;
                        nameCell = cells.FirstOrDefault(c => c?.CellReference == $"B{row}")!;
                    }

                    //Deal with Product sheet
                    sheet = sheets.FirstOrDefault(s => s.Name == "Product");
                    wsPart = (WorksheetPart)wbPart.GetPartById(sheet.Id!);
                    cells = wsPart.Worksheet.Descendants<Cell>();
                    row = 2;
                    nameCell = cells.FirstOrDefault(c => c?.CellReference == $"B{row}")!;
                    Cell priceCell = cells.FirstOrDefault(c => c?.CellReference == $"C{row}")!;
                    Cell categoryCell = cells.FirstOrDefault(c => c?.CellReference == $"D{row}")!;
                    Cell quantityCell = cells.FirstOrDefault(c => c?.CellReference == $"E{row}")!;

                    // Insert data from the current sheet into the database
                    while (nameCell != null)
                    {
                        var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault()!;

                        string nameStringId = nameCell.InnerText;
                        string name = stringTable.SharedStringTable.ElementAt(int.Parse(nameStringId)).InnerText;

                        string priceStringId = priceCell.InnerText;
                        decimal price = decimal.Parse(priceStringId);

                        string categoryStringId = categoryCell.InnerText;
                        int category = int.Parse(categoryStringId);

                        string quantityStringId = quantityCell.InnerText;
                        int quantity = int.Parse(quantityStringId);

                        string sql = "INSERT INTO Product (Name, Price, Category, Quantity) VALUES (@Name, @Price, @Category, @Quantity)";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Name", name);
                            command.Parameters.AddWithValue("@Price", price);
                            command.Parameters.AddWithValue("@Category", category);
                            command.Parameters.AddWithValue("@Quantity", quantity);
                            command.ExecuteNonQuery();
                        }

                        row++;
                        nameCell = cells.FirstOrDefault(c => c?.CellReference == $"B{row}")!;
                        priceCell = cells.FirstOrDefault(c => c?.CellReference == $"C{row}")!;
                        categoryCell = cells.FirstOrDefault(c => c?.CellReference == $"D{row}")!;
                        quantityCell = cells.FirstOrDefault(c => c?.CellReference == $"E{row}")!;
                    }
                }
            }
        
            setVisibleOff();
            DashboardScreen.Visibility = Visibility.Visible;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedIndex == -1)
            {
                placeholderText.Visibility = Visibility.Visible;
            }
            else
            {
                placeholderText.Visibility = Visibility.Collapsed;
            }

        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                placeholderText1.Visibility = Visibility.Visible;
            }
            else
            {
                placeholderText1.Visibility = Visibility.Collapsed;
            }
        }
    }
}