using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BiliMusic.Converters
{
    public class NumberToStringConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var number = System.Convert.ToDouble(value);
            if (number >= 10000)
            {
                return ((double)number / 10000).ToString("0.0") + "万";
            }
            return number.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
