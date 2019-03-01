using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace BiliMusic.Models
{
    public class PlayModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public SongsDetailModel songinfo { get; set; }
        public int songid { get; set; }
        public string title { get; set; }
        public string pic { get; set; }
        public string author { get; set; } = "";
        public string lyric_url { get; set; }
        public Uri play_url { get; set; }
        public List<string> backup_url { get; set; }
        public bool is_preview { get; set; }
        public bool is_collect { get; set; }

        public List<QualitiesModel> qualities { get; set; }

        public bool _select = false;
        public bool select
        {
            get { return _select; }
            set
            {
                _select = value;
                if (PropertyChanged != null)
                {
                    DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("select"));
                    });
                }
            }
        }

        public SolidColorBrush _color=(SolidColorBrush)App.Current.Resources["COLOR_Foreground"];
        public SolidColorBrush color
        {
            get { return _color; }
            set
            {
                _color = value;
                if (PropertyChanged != null)
                {
                    DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("color"));
                    });
                }
            }
        }

        public string title_show
        {
            get {
                if (author=="")
                {
                    return title;
                }
                return title + " - " + author;
            }
        }
        public string tag
        {
            get
            {
                if (is_preview)
                {
                    return "[试听]";
                }
                else
                {
                    return "";
                }
            }
        }

    }
}
