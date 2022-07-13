using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Protecc.Helpers
{
    public class ColorUIHelper
    {
        public static SolidColorBrush HexToBrush(string hex)
        {
            byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            return new SolidColorBrush(Windows.UI.Color.FromArgb(255, r, g, b));
        }
    }
}
