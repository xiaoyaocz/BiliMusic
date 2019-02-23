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
    public class HomeItemStyleSelector : StyleSelector
    {
        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
           
            var models = item as modulesModel;
            if (models.dataSize==3&&models.type==3)
            {
                Style st = new Style();
                st.TargetType = typeof(ContentPresenter);
                Setter setter = new Setter();
                setter.Property = ContentPresenter.MaxWidthProperty;
                setter.Value = 1024/2;
                st.Setters.Add(setter);
                return st;
            }
            else
            {
                Style st = new Style();
                st.TargetType = typeof(ContentPresenter);
                Setter setter = new Setter();
                setter.Property = ContentPresenter.MaxWidthProperty;
                setter.Value = 1024 ;
                st.Setters.Add(setter);
                return st;
                //return base.SelectStyleCore(item, container);
            }
          
        }
    }
}
