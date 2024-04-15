﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyShop
{
    public class CategoryRepository
    {
        private string connectionString = Properties.Settings.Default.ConnectionString;

        public BindingList<Category> ReadDataFromDatabase()
        {
            BindingList<Category> list = new BindingList<Category>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sqlcmd = "SELECT * FROM Category";
                var command = new SqlCommand(sqlcmd, connection);
                var reader = command.ExecuteReader();
                int id;
                string name;

                while (reader.Read())
                {
                    id = (int)reader["Id"];
                    name = (string)reader["Name"];

                    Category cat = new Category() { Id = id, CategoryName = name };
                    list.Add(cat);
                } 
                reader.Close();
                connection.Close();
            }
            return list;
        }
        public void InsertCategory(string name)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Category (Name) VALUES (@Name)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteCategory(int categoryId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Step 1: Delete all products associated with the category
                string deleteProductsSql = "DELETE FROM Product WHERE Category = @CategoryId";
                using (SqlCommand deleteProductsCommand = new SqlCommand(deleteProductsSql, connection))
                {
                    deleteProductsCommand.Parameters.AddWithValue("@CategoryId", categoryId);
                    deleteProductsCommand.ExecuteNonQuery();
                }

                connection.Close();

                connection.Open();
                // Step 2: Delete the category itself
                string deleteCategorySql = "DELETE FROM Category WHERE Id = @CategoryId";
                using (SqlCommand deleteCategoryCommand = new SqlCommand(deleteCategorySql, connection))
                {
                    deleteCategoryCommand.Parameters.AddWithValue("@CategoryId", categoryId);
                    deleteCategoryCommand.ExecuteNonQuery();
                }
            }
        }
        public void UpdateCategory(int categoryId, string newName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string updateSql = "UPDATE Category SET Name = @NewName WHERE Id = @CategoryId";
                using (SqlCommand updateCommand = new SqlCommand(updateSql, connection))
                {
                    updateCommand.Parameters.AddWithValue("@NewName", newName);
                    updateCommand.Parameters.AddWithValue("@CategoryId", categoryId);
                    updateCommand.ExecuteNonQuery();
                }
            }
        }
        public void CreateTables()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string createTableSql = @"
                DROP TABLE IF EXISTS Category;
                CREATE TABLE Category (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(255) NOT NULL
                );";
                using (SqlCommand createTableCommand = new SqlCommand(createTableSql, connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }
    }
    public class ProductsRepository
    {
        private string connectionString = Properties.Settings.Default.ConnectionString;

        public ObservableCollection<Product> ReadDataFromDatabase()
        {
            ObservableCollection<Product> list = new ObservableCollection<Product>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sqlcmd = "SELECT * FROM Product";
                var command = new SqlCommand(sqlcmd, connection);
                var reader = command.ExecuteReader();

                // Read categories once and store them in a dictionary for quick lookup
                var categoryRepo = new CategoryRepository();
                var categories = categoryRepo.ReadDataFromDatabase().ToDictionary(c => c.Id, c => c);

                while (reader.Read())
                {
                    int id = (int)reader["Id"];
                    string name = (string)reader["Name"];
                    int price = Convert.ToInt32(reader["Price"]);
                    int quantity = (int)reader["Quantity"];
                    int categoryId = (int)reader["Category"]; // Assuming "Category" column stores the category ID

                    // Find the Category object that matches the category ID
                    Category category = categories.ContainsKey(categoryId) ? categories[categoryId] : null;

                    // Create a new Product object and assign the properties
                    Product product = new Product()
                    {
                        Id = id,
                        ProductName = name,
                        Price = price,
                        Quantity = quantity,
                        Category = category // Assign the Category object
                        
                    };
                    list.Add(product);
                }
                reader.Close();
                connection.Close();
            }
            return list;
        }
        public void InsertProduct(string name, decimal price, int category, int quantity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Product (Name, Price, Category, Quantity) VALUES (@Name, @Price, @Category, @Quantity)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteProduct(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Step 1: Delete all orders associated with the product
                string deleteOrdersSql = "DELETE FROM Orders WHERE ProductId = @ProductId";
                using (SqlCommand deleteOrdersCommand = new SqlCommand(deleteOrdersSql, connection))
                {
                    deleteOrdersCommand.Parameters.AddWithValue("@ProductId", productId);
                    deleteOrdersCommand.ExecuteNonQuery();
                }

                // Step 2: Delete the product itself
                string deleteProductSql = "DELETE FROM Product WHERE Id = @Id";
                using (SqlCommand deleteProductCommand = new SqlCommand(deleteProductSql, connection))
                {
                    deleteProductCommand.Parameters.AddWithValue("@Id", productId);
                    deleteProductCommand.ExecuteNonQuery();
                }
            }
        }
        public void UpdateProduct(int productId, string name, decimal price, int category, int quantity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string updateSql = "UPDATE Product SET Name = @Name, Price = @Price, Category = @Category, Quantity = @Quantity WHERE Id = @Id";
                using (SqlCommand updateCommand = new SqlCommand(updateSql, connection))
                {
                    updateCommand.Parameters.AddWithValue("@Id", productId);
                    updateCommand.Parameters.AddWithValue("@Name", name);
                    updateCommand.Parameters.AddWithValue("@Price", price);
                    updateCommand.Parameters.AddWithValue("@Category", category);
                    updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                    updateCommand.ExecuteNonQuery();
                }
            }
        }
        public void CreateTables()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string createTableSql = @"
                DROP TABLE IF EXISTS Product;

                CREATE TABLE Product (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(255) NOT NULL,
                    Price MONEY NOT NULL,
                    Category INT NOT NULL,
                    Quantity INT NOT NULL
                );";
                using (SqlCommand createTableCommand = new SqlCommand(createTableSql, connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }
        public void AddForeignKey()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string createTableSql = @"
                ALTER TABLE Product
                ADD FOREIGN KEY (Category) REFERENCES Category(Id);
                ";
                using (SqlCommand createTableCommand = new SqlCommand(createTableSql, connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }
    }
    public class CustomerRepository
    {
        private string connectionString = Properties.Settings.Default.ConnectionString;

        public BindingList<Customer> ReadDataFromDatabase()
        {
            BindingList<Customer> list = new BindingList<Customer>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sqlcmd = "SELECT * FROM Customer";
                var command = new SqlCommand(sqlcmd, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = (int)reader["Id"];
                    string name = (string)reader["Name"];
                    string phoneNumber = (string)reader["PhoneNumber"];

                    Customer customer = new Customer() { Id = id, Name = name, PhoneNumber = phoneNumber };
                    list.Add(customer);
                }
                reader.Close();
                connection.Close();
            }
            return list;
        }

        public void InsertCustomer(string name, string phoneNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Customer (Name, PhoneNumber) VALUES (@Name, @PhoneNumber)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateCustomer(int customerId, string newName, string newPhoneNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string updateSql = "UPDATE Customer SET Name = @NewName, PhoneNumber = @NewPhoneNumber WHERE Id = @CustomerId";
                using (SqlCommand updateCommand = new SqlCommand(updateSql, connection))
                {
                    updateCommand.Parameters.AddWithValue("@NewName", newName);
                    updateCommand.Parameters.AddWithValue("@NewPhoneNumber", newPhoneNumber);
                    updateCommand.Parameters.AddWithValue("@CustomerId", customerId);
                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCustomer(int customerId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string deleteSql = "DELETE FROM Customer WHERE Id = @CustomerId";
                using (SqlCommand deleteCommand = new SqlCommand(deleteSql, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@CustomerId", customerId);
                    deleteCommand.ExecuteNonQuery();
                }
            }
        }

        public void CreateTables()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string createTableSql = @"
            DROP TABLE IF EXISTS Customer;
            CREATE TABLE Customer (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                Name NVARCHAR(255) NOT NULL,
                PhoneNumber NVARCHAR(20) NOT NULL
            );";
                using (SqlCommand createTableCommand = new SqlCommand(createTableSql, connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }
    }
    public class OrdersRepository
    {
        private string connectionString = Properties.Settings.Default.ConnectionString;

        public ObservableCollection<Order> ReadDataFromDatabase()
        {
            ObservableCollection<Order> list = new ObservableCollection<Order>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sqlcmd = "SELECT * FROM Orders";
                var command = new SqlCommand(sqlcmd, connection);
                var reader = command.ExecuteReader();

                // Assuming you have repositories for Customer and Product to fetch related entities
                var customerRepo = new CustomerRepository();
                var productRepo = new ProductsRepository();
                var customers = customerRepo.ReadDataFromDatabase().ToDictionary(c => c.Id, c => c);
                var products = productRepo.ReadDataFromDatabase().ToDictionary(p => p.Id, p => p);

                while (reader.Read())
                {
                    int id = (int)reader["Id"];
                    DateTime orderDate = (DateTime)reader["OrderDate"];
                    int customerId = (int)reader["CustomerId"];
                    int productId = (int)reader["ProductId"];
                    int quantity = (int)reader["Quantity"];

                    // Find the Customer and Product objects that match the IDs
                    Customer customer = customers.ContainsKey(customerId) ? customers[customerId] : null;
                    Product product = products.ContainsKey(productId) ? products[productId] : null;

                    // Create a new UpdateOrder object and assign the properties
                    Order order = new Order()
                    {
                        Id = id,
                        OrderDate = orderDate,
                        Customer = customer,
                        Product = product,
                        Quantity = quantity
                    };
                    list.Add(order);
                }
                reader.Close();
                connection.Close();
            }
            return list;
        }

        public void InsertOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Orders (OrderDate, CustomerId, ProductId, Quantity) VALUES (@OrderDate, @CustomerId, @ProductId, @Quantity)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    command.Parameters.AddWithValue("@CustomerId", order.Customer?.Id ?? (object)DBNull.Value); // Handle nullable CustomerId
                    command.Parameters.AddWithValue("@ProductId", order.Product.Id);
                    command.Parameters.AddWithValue("@Quantity", order.Quantity);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOrder(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string deleteSql = "DELETE FROM Orders WHERE Id = @Id";
                using (SqlCommand deleteCommand = new SqlCommand(deleteSql, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@Id", orderId);
                    deleteCommand.ExecuteNonQuery();
                }
            }
        }

        public void UpdateOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string updateSql = "UPDATE Orders SET OrderDate = @OrderDate, CustomerId = @CustomerId, ProductId = @ProductId, Quantity = @Quantity WHERE Id = @Id";
                using (SqlCommand updateCommand = new SqlCommand(updateSql, connection))
                {
                    updateCommand.Parameters.AddWithValue("@Id", order.Id);
                    updateCommand.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    updateCommand.Parameters.AddWithValue("@CustomerId", order.Customer?.Id ?? (object)DBNull.Value); // Handle nullable CustomerId
                    updateCommand.Parameters.AddWithValue("@ProductId", order.Product.Id);
                    updateCommand.Parameters.AddWithValue("@Quantity", order.Quantity);
                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        public void CreateTables()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string createTableSql = @"
            DROP TABLE IF EXISTS Orders;
            CREATE TABLE Orders (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                OrderDate DATETIME NOT NULL,
                CustomerId INT,
                ProductId INT NOT NULL,
                Quantity INT NOT NULL
            );";
                using (SqlCommand createTableCommand = new SqlCommand(createTableSql, connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }
        public void AddForeignKey()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string createTableSql = @"
                ALTER TABLE Orders
                ADD FOREIGN KEY (CustomerId) REFERENCES Customer(Id);
                ALTER TABLE Orders
                ADD FOREIGN KEY (ProductId) REFERENCES Product(Id);
                ";
                using (SqlCommand createTableCommand = new SqlCommand(createTableSql, connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
