using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace EasyList.Proto.Converters
{
    class NegateBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool) && !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
