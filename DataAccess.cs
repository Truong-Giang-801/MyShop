using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                int id = 0;
                string name = "All";
                Category catAll = new Category() { Id = id, CategoryName = name };
                list.Add(catAll);

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
                    Debug.WriteLine(product.Category.CategoryName);
                    list.Add(product);
                }
                reader.Close();
                connection.Close();
            }
            return list;
        }
    }
}
