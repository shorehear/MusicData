using System;
using System.Globalization;
using System.Windows.Data;

namespace JulyPractice
{
    public class RadioButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string selectedInfoType && parameter is string targetInfoType)
            {
                return selectedInfoType == targetInfoType;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked)
            {
                return parameter as string;
            }

            return null;
        }
    }
}
