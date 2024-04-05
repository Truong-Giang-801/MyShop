using System;
using System.Globalization;
using System.Windows.Data;

namespace MyShop
{
    public class NumberSaleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int)
            {
                int numberSale = (int)values[0];
                return $"{numberSale} product(s) on sale";
            }
            else if (values[0] is string)
            {
                if (int.TryParse((string)values[0], out int numberSale))
                {
                    return $"{numberSale} product(s) on sale";
                }
                else
                {
                    return "Invalid value for number of sales";
                }
            }
            else
            {
                return "Invalid value for number of sales";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
