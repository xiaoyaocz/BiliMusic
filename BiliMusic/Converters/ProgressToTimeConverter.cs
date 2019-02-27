using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BiliMusic.Converters
{
    public class ProgressToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int i = 0;
            int.TryParse(value.ToString(), out i);
            TimeSpan ts = new TimeSpan(0, 0, i);
            return ts.ToString(@"mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
