using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Rukn.Converters
{
    public class FontSizeConverter : IValueConverter
    {
        private static readonly System.Windows.FontSizeConverter fontSizeConverter = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                return value;
            }
            return fontSizeConverter.ConvertTo(value, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return fontSizeConverter.ConvertFrom(value);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
