using BiliMusic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BiliMusic.Converters
{
    public class ImageCompressionConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (SettingHelper.LoadOriginalImage)
            {
                return value;
            }
            //使用webp图片能更小，可惜UWP还不支持
            return value.ToString()+"@"+ parameter.ToString()+".jpg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
