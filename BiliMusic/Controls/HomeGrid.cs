using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BiliMusic.Controls
{
    public class HomeGrid:Grid
    {
        public HomeGrid()
        {
            
        }

        public int RowNumber
        {
            get { return (int)GetValue(RowNumberProperty); }
            set {
                SetValue(RowNumberProperty, value);
                this.RowDefinitions.Clear();
                for (int i = 0; i < value; i++)
                {
                    this.RowDefinitions.Add(new RowDefinition() {
                        Height=GridLength.Auto
                    });
                }

            }
        }

        // Using a DependencyProperty as the backing store for RowNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowNumberProperty =
            DependencyProperty.Register("RowNumber", typeof(int), typeof(HomeGrid), new PropertyMetadata(10));


    }
}
