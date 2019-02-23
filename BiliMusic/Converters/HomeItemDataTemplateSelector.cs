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
    public class HomeItemDataTemplateSelector : DataTemplateSelector
    {
        public ResourceDictionary resource;
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var models = item as modulesModel;
            switch (models.type)
            {
                case 2:
                    //返回编辑推荐模板
                    return resource["Recommend"] as DataTemplate;
                case 3:
                    //返回歌单模板
                    return resource["SongList"] as DataTemplate;
                case 4:
                    return resource["Videos"] as DataTemplate;
                default:
                    break;
            }


            return base.SelectTemplateCore(item, container);
        }
    }
}
