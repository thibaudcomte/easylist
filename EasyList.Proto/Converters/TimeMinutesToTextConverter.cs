using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace EasyList.Proto.Converters
{
    class TimeMinutesToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is int))
            {
                return DependencyProperty.UnsetValue;
            }

            int duration = (int)value;
            int hours = duration / 60;
            int minutes = duration % 60;

            return $"{hours}h{minutes}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
