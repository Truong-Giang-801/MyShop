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
        private int _numberSale;
        private string? _productName;
        private int _category;
        private int _price;

        public int NumberSale
        {
            get { return _numberSale; }
            set
            {
                _numberSale = value;
                OnPropertyChanged(nameof(NumberSale));
            }
        }

        public string? ProductName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                OnPropertyChanged(nameof(ProductName));
            }
        }

        public int Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        public int Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
