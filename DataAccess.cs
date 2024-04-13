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
                var sqlcmd = "select * from Product";
                var commandd = new SqlCommand(sqlcmd, connection);
                var reader = commandd.ExecuteReader();


                while (reader.Read())
                {
                    int id = (int)reader["Id"];
                    string name = (string)reader["Name"];
                    int price = Convert.ToInt32(reader["Price"]);
                    int category = (int)reader["Category"];
                    int quantity = (int)reader["Quantity"];
                    Product products = new Product() { Id = id, ProductName = name, Price = price, Quantity = quantity, Category = category };
                    list.Add(products);
                }
                reader.Close();
                connection.Close();
            }
            return list;
        }
    }
}
