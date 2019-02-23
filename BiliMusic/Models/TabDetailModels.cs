using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace BiliMusic.Models
{
    public class TabDetailDataModel
    {
        public List<bannersModel> banners { get; set; }
        public List<modulesModel> modules { get; set; }
    }
    public class bannersModel
    {
        public int bannerId { get; set; }
        public string bannerTitle { get; set; }
        public string bannerImgUrl { get; set; }
        public int bannerType { get; set; }
        public string schema { get; set; }
    }
    public class modulesModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public int type { get; set; }
        public int dataSize { get; set; }
        public int layout { get; set; }
        public string target { get; set; }
        public int shark { get; set; }
        public bool hasMore
        {
            get
            {
                return target != "";
            }
        }
        public bool hasRefresh
        {
            get
            {
                return shark == 1;
            }
        }
        public ObservableCollection<songsModel> songs { get; set; }
        public ObservableCollection<menusModel> menus { get; set; }
        public ObservableCollection<videosModel> videos { get; set; }
    }
    public class videosModel
    {
        public long aid { get; set; }
        public string title { get; set; }
        public string pic { get; set; }
        public long duration { get; set; }
        public long view { get; set; }
        public long reply { get; set; }
    }
    public class songsModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string cover_url { get; set; }
        public string author { get; set; }
        public int song_id { get; set; }

    }
    public class menusModel
    {
        public int menuId { get; set; }
        public string title { get; set; }
        public string coverUrl { get; set; }
        public string mbnames { get; set; }
        public int type { get; set; }//5 付费音乐，2 歌单，4 猫耳，6 合辑
        public long playNum { get; set; }
        public bool showName
        {
            get { return mbnames != ""; }

        }
        public bool isVip
        {
            get { return type == 5; }
        }
    }
}
