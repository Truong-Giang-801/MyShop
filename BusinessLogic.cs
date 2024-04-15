using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyShop
{
    public class DataImportService
    {
        private CategoryRepository categoryRepository;
        private ProductsRepository productsRepository;
        private CustomerRepository customerRepository;
        private OrdersRepository orderRepository;

        public DataImportService()
        {
            categoryRepository = new CategoryRepository();
            productsRepository = new ProductsRepository();
            customerRepository = new CustomerRepository();
            orderRepository = new OrdersRepository();
        }

        public void ImportDataFromExcel(string filename)
        {

            // Drop old tables and create new ones
            orderRepository.CreateTables();
            productsRepository.CreateTables();
            categoryRepository.CreateTables();
            customerRepository.CreateTables();
            productsRepository.AddForeignKey();
            orderRepository.AddForeignKey();

            // Read Excel file
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filename, false))
            {
                var workbookPart = document.WorkbookPart;
                var sheets = workbookPart.Workbook.Descendants<Sheet>().ToList();

                // Process Category sheet
                var categorySheet = sheets.FirstOrDefault(s => s.Name == "Category");
                if (categorySheet != null)
                {
                    var worksheetPart = (WorksheetPart)workbookPart.GetPartById(categorySheet.Id);
                    var cells = worksheetPart.Worksheet.Descendants<Cell>().ToList();
                    int row = 2;
                    Cell nameCell = cells.FirstOrDefault(c => c.CellReference == $"B{row}");

                    while (nameCell != null)
                    {
                        string stringId = nameCell.InnerText;
                        var stringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault()!;
                        string name = stringTable.SharedStringTable.ElementAt(int.Parse(stringId)).InnerText;

                        categoryRepository.InsertCategory(name);

                        row++;
                        nameCell = cells.FirstOrDefault(c => c.CellReference == $"B{row}");
                    }
                }

                // Process Product sheet
                var productSheet = sheets.FirstOrDefault(s => s.Name == "Product");
                if (productSheet != null)
                {
                    var worksheetPart = (WorksheetPart)workbookPart.GetPartById(productSheet.Id);
                    var cells = worksheetPart.Worksheet.Descendants<Cell>().ToList();
                    int row = 2;
                    Cell nameCell = cells.FirstOrDefault(c => c.CellReference == $"B{row}");
                    Cell priceCell = cells.FirstOrDefault(c => c.CellReference == $"C{row}");
                    Cell categoryCell = cells.FirstOrDefault(c => c.CellReference == $"D{row}");
                    Cell quantityCell = cells.FirstOrDefault(c => c.CellReference == $"E{row}");

                    while (nameCell != null)
                    {
                        var stringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault()!;

                        string nameStringId = nameCell.InnerText;
                        string name = stringTable.SharedStringTable.ElementAt(int.Parse(nameStringId)).InnerText;

                        string priceStringId = priceCell.InnerText;
                        decimal price = decimal.Parse(priceStringId);

                        string categoryStringId = categoryCell.InnerText;
                        int category = int.Parse(categoryStringId);

                        string quantityStringId = quantityCell.InnerText;
                        int quantity = int.Parse(quantityStringId);

                        productsRepository.InsertProduct(name, price, category, quantity);

                        row++;
                        nameCell = cells.FirstOrDefault(c => c.CellReference == $"B{row}");
                        priceCell = cells.FirstOrDefault(c => c.CellReference == $"C{row}");
                        categoryCell = cells.FirstOrDefault(c => c.CellReference == $"D{row}");
                        quantityCell = cells.FirstOrDefault(c => c.CellReference == $"E{row}");
                    }
                }
            }
        }

    }
    public class CategoryService
    {
        private CategoryRepository categoryRepository = new CategoryRepository();

        public BindingList<Category> GetAllCategories()
        {
            return categoryRepository.ReadDataFromDatabase();
        }

        public BindingList<Category> AddAllObjectToCategories(BindingList<Category> _categories)
        {
            // Clone the categories list and add a new category as the first item
            BindingList<Category> clonedCategories = new BindingList<Category>(_categories.Select(c => new Category { Id = c.Id, CategoryName = c.CategoryName }).ToList());
            Category allCategory = new Category { Id = 0, CategoryName = "All" };
            clonedCategories.Insert(0, allCategory);

            return clonedCategories;
        }
        public void InsertCategory(Category category)
        {
            categoryRepository.InsertCategory(category.CategoryName);
        }
        public void DeleteCategory(int categoryId)
        {
            // Perform any necessary checks or operations here
            categoryRepository.DeleteCategory(categoryId);
        }
        public void UpdateCategory(Category category)
        {
            // Perform any necessary checks or operations here
            categoryRepository.UpdateCategory(category.Id, category.CategoryName);
        }
        // Các phương thức khác liên quan đến Category...
    }

    public class ProductsService
    {
        private ProductsRepository productsRepository = new ProductsRepository();

        public ObservableCollection<Product> GetAllProducts()
        {
            return productsRepository.ReadDataFromDatabase();
        }
        public void DeleteProduct(int productId)
        {
            // Perform any necessary checks or operations here
            productsRepository.DeleteProduct(productId);
        }
        public void InsertProduct(Product product)
        {
            productsRepository.InsertProduct(product.ProductName, product.Price, product.Category.Id, product.Quantity);
        }
        public void UpdateProduct(Product product)
        {
            productsRepository.UpdateProduct(product.Id, product.ProductName, product.Price, product.Category.Id, product.Quantity);
        }
    }
    public class CustomerService
    {
        private CustomerRepository customerRepository = new CustomerRepository();

        public BindingList<Customer> GetAllCustomers()
        {
            return customerRepository.ReadDataFromDatabase();
        }

        public void InsertCustomer(Customer customer)
        {
            // Perform any necessary checks or operations here
            // For example, validate the customer data before inserting
            customerRepository.InsertCustomer(customer.Name, customer.PhoneNumber);
        }

        public void DeleteCustomer(int customerId)
        {
            // Perform any necessary checks or operations here
            // For example, check if the customer has any orders before deletion
            customerRepository.DeleteCustomer(customerId);
        }

        public void UpdateCustomer(Customer customer)
        {
            // Perform any necessary checks or operations here
            // For example, validate the customer data before updating
            customerRepository.UpdateCustomer(customer.Id, customer.Name, customer.PhoneNumber);
        }

        // Additional methods related to Customer...
    }
    public class OrdersService
    {
        private OrdersRepository ordersRepository = new OrdersRepository();

        public BindingList<Order> GetAllOrders()
        {
            return ordersRepository.ReadDataFromDatabase();
        }

        public void DeleteOrder(int orderId)
        {
            // Perform any necessary checks or operations here
            ordersRepository.DeleteOrder(orderId);
        }

        public void InsertOrder(Order order)
        {
            // Example of adding business logic before inserting an order
            // You might want to validate the order, check stock availability, etc.
            // For simplicity, we're directly passing the order to the repository
            ordersRepository.InsertOrder(order);
        }

        public void UpdateOrder(Order order)
        {
            // Example of adding business logic before updating an order
            // You might want to validate the order, check stock availability, etc.
            // For simplicity, we're directly passing the order to the repository
            ordersRepository.UpdateOrder(order);
        }
    }
}
