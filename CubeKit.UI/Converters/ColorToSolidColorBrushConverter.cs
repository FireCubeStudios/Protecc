using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace CubeKit.UI.Converters
{
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => new SolidColorBrush((Color)value);

        public object ConvertBack(object value, Type targetType, object parameter, string language) => ((SolidColorBrush)value).Color;
    }
}
