using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MelissandreServiceLibrary.Converter
{
    public class EnumToListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type enumType = value as Type;
            if (enumType != null && enumType.IsEnum)
            {
                var enumValues = System.Enum.GetValues(enumType);
                var list = new List<object>();
                foreach (var enumValue in enumValues)
                {
                    list.Add(enumValue);
                }
                return list;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
