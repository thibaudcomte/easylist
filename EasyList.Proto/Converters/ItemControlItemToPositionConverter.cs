using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace EasyList.Proto.Converters
{
    class ItemControlItemToPositionConverter : IValueConverter
    {
        public ItemsControl ItemsControl { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (ItemsControl == null)
                return DependencyProperty.UnsetValue;

            var container = ItemsControl.ContainerFromItem(value);
            if(container == null)
                return DependencyProperty.UnsetValue;

            return 1 + ItemsControl.IndexFromContainer(container);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
