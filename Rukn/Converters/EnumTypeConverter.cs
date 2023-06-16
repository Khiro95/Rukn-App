using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rukn.Converters
{
    public class EnumTypeConverter : EnumConverter
    {
        public EnumTypeConverter(Type type) : base(type) { }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value?.ToString() is string strKey)
                {
                    return Resources.ResourceManager.GetString(strKey, Resources.Culture) ?? strKey;
                }

                return null;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
