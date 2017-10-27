using EasyList.Proto.Core.Retailers;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace EasyList.Proto.Converters
{
    class RetailerToLogoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            IRetailer retailer = (IRetailer)value;
            switch (retailer.Name)
            {
                case "Intermarche":
                    return "ms-appx:///Images/intermarche.png";

                case "Carrefour":
                    return "ms-appx:///Images/carrefour.png";

                default:
                    break;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
