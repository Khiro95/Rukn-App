﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Rukn.Converters
{
    public class FontNameConverter : IValueConverter
    {
        public FontFamily Convert(string fontName)
        {
            return System.Windows.Application.Current.FindResource(fontName) as FontFamily;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string fontName)
            {
                return Convert(fontName);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
