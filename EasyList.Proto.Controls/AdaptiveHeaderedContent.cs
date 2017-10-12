using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace EasyList.Proto.Controls
{
    public sealed class AdaptiveHeaderedContent : ContentControl
    {
        public AdaptiveHeaderedContent()
        {
            this.DefaultStyleKey = typeof(AdaptiveHeaderedContent);
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(AdaptiveHeaderedContent), new PropertyMetadata(""));


    }
}
