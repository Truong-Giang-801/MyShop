using System;
using System.Globalization;
using System.Windows.Data;

namespace MyShop
{
    public class NumberSaleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 1)
                return "";

            int numberSale = (int)values[0];

            return $"{numberSale} product(s) on sale";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
