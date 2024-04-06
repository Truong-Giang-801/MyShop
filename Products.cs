using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyShop
{
    public class Products : INotifyPropertyChanged
    {
        public int NumberSale { get; set; }
        public string ProductName { get; set; }
        public int Category { get; set; }
        public int Price { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
