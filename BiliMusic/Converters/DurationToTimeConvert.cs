using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BiliMusic.Converters
{
    public class DurationToTimeConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var ts = TimeSpan.FromSeconds((Int64)value);

            return ts.ToString(@"mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
