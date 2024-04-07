using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop
{
    public class CategoryService
    {
        private CategoryRepository categoryRepository = new CategoryRepository();

        public BindingList<Category> GetAllCategories()
        {
            return categoryRepository.ReadDataFromDatabase();
        }

        // Các phương thức khác liên quan đến Category...
    }

    public class ProductsService
    {
        private ProductsRepository productsRepository = new ProductsRepository();

        public BindingList<Products> GetAllProducts()
        {
            return productsRepository.ReadDataFromDatabase();
        }

        // Các phương thức khác liên quan đến Category...
    }
}
