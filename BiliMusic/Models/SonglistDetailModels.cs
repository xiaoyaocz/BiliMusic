using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BiliMusic.Models
{
    public class SonglistDetailModel
    {
        public menusResponesModel menusRespones { get; set; }
        private List<songsListModel> _songsList;
        public List<songsListModel> songsList {
            get { return _songsList; }
            set
            {
                int i = 1;
                value.ForEach(x => { x.index = i.ToString("00");i++; });
                _songsList = value;
            }
        }
        public List<menusTagModel> menusTags { get; set; }
       
    }
    public class menusResponesModel:System.ComponentModel.INotifyPropertyChanged
    {
        public int menuId { get; set; }
        public string title { get; set; }
        public string coverUrl { get; set; }
        public string intro { get; set; } = "";
        public int type { get; set; }
        public long? playNum { get; set; } = -1;
        public long? collectNum { get; set; } = 0;
        public long? commentNum { get; set; } = 0;
        public int? songNum { get; set; } = -1;
        public string toptitle { get; set; }
        public string face { get; set; }
        public string uname { get; set; }
        public long? uid { get; set; } = 0;
        public string mbnames { get; set; } = "";

        private int? _collected;

        public int? collected
        {
            get { return _collected; }
            set { _collected = value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("collected"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("showCollect"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("showCancelCollect"));
            }
        }




        public bool showPlayNum
        {
            get
            {
                return songNum != null && songNum != -1;
            }
        }

        public bool showIntro
        {
            get
            {
                return intro != "";
            }
        }
        public string bg
        {
            get
            {
                return coverUrl+"@100w.jpg";
            }
        }
        public bool showCreate
        {
            get { return type == 1; }
        }
        public bool showName
        {
            get { return mbnames!=null&&mbnames != ""; }
        }
        public bool showVip
        {
            get { return type == 5; }
        }

        public bool showCollect
        {
            get
            {
                return playNum!=null&&collected == 0;
            }
        }
        public bool showCancelCollect
        {
            get
            {
                return playNum != null && collected == 1;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class songsListModel
    {
        public string index { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string intro { get; set; }
        public string cover_url { get; set; }
        public string author { get; set; }
        public long duration { get; set; }

        /// <summary>
        /// 是否收费
        /// </summary>
        public int is_pay { get; set; }

        public bool isPay
        {
            get { return is_pay == 1; }
        }
    }
    public class menusTagModel
    {
        public int cateId { get; set; }
        public int itemId { get; set; }
        public string itemVal { get; set; }
        public int attr { get; set; }
    }

}
