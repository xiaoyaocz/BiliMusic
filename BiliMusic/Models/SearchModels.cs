using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace BiliMusic.Models
{
    public class SearchModel<T>
    {
        public int page { get; set; }
        public int pagesize { get; set; }
        public int num_pages { get; set; }
        public string seid { get; set; }
        public string code { get; set; }
        public string msg { get; set; }
        public ObservableCollection<T> result { get; set; }
    }
    public class SearchSongResultModel
    {
        public int review_count { get; set; }
        public int id { get; set; }
        public int play_count { get; set; }
        public string cover { get; set; }

        public string title { get; set; }

        public string author { get; set; }
    }
    public class SearchMenuResultModel
    {
        public int id { get; set; }
        public int play_count { get; set; }
        public int favor_count { get; set; }
        public int music_count { get; set; }
        public string cover { get; set; }
        private string _title;

        public string title
        {
            get { return _title; }
            set { _title = value.Replace("<em class=\"keyword\">", "").Replace("</em>",""); }
        }

    }

    public class SearchAuthorResultModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int id { get; set; }
        private string _uname;
        public string uname
        {
            get { return _uname; }
            set { _uname = value.Replace("<em class=\"keyword\">", "").Replace("</em>", ""); }
        }
        public string cover { get; set; }
        public int fans_count { get; set; }
        public int play_count { get; set; }

        private int _is_follow;

        public int is_follow
        {
            get { return _is_follow; }
            set {
                _is_follow = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("is_follow"));
            }
        }



    }



}
