using System;
using System.Globalization;
using System.Windows.Data;

namespace Rukn.Converters
{
    public class StringToResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((targetType == typeof(string) || targetType == typeof(object)) && value?.ToString() is string strKey)
            {
                return Resources.ResourceManager.GetString(strKey, Resources.Culture) ?? strKey;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
