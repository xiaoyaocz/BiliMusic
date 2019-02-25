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
    public class HomeItemNarrowStyleSelector : StyleSelector
    {
        protected override Style SelectStyleCore(object item, DependencyObject container)
        {

            var models = item as modulesModel;
            Style st = new Style();
            st.TargetType = typeof(ContentPresenter);
            st.Setters.Add(new Setter()
            {
                Property = Grid.ColumnProperty,
                Value = 0
            });
            st.Setters.Add(new Setter()
            {
                Property = Grid.RowProperty,
                Value = models.narrow_row
            });
            st.Setters.Add(new Setter()
            {
                Property = Grid.ColumnSpanProperty,
                Value = 2
            });
            return st;

        }
    }
}
