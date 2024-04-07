using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop
{
    public partial class Category : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public string? CategoryName { get; set; }

        public virtual ICollection<Products> Products { get; set; } = new List<Products>();

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
