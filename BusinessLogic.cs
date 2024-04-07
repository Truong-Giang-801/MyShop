using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Product> GetAllProducts()
        {
            return productsRepository.ReadDataFromDatabase();
        }

    }
}
