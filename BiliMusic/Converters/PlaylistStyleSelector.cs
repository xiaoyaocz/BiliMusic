using BiliMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
namespace BiliMusic.Converters
{
    public class PlaylistStyleSelector : StyleSelector
    {
        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            var models = item as PlayModel;
            if (models.select)
            {
                return (Style)App.Current.Resources["playlistItemSelect"];
            }
            else
            {
                return (Style)App.Current.Resources["playlistItem"];
            }
        }
    }

}
