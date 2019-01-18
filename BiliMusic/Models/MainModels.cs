using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BiliMusic.Models.Main
{
    public enum MenuOpenMode
    {
        /// <summary>
        /// /首页
        /// </summary>
        Home,
        /// <summary>
        /// 搜索
        /// </summary>
        Search,
        /// <summary>
        /// 我的缓存
        /// </summary>
        Download,
        /// <summary>
        /// 本地音乐
        /// </summary>
        LoaclMusic,
        /// <summary>
        /// 我的收藏
        /// </summary>
        Collection,
        /// <summary>
        /// 关注的人
        /// </summary>
        Attention,
        /// <summary>
        /// 歌单
        /// </summary>
        Songlist,
        /// <summary>
        /// 歌曲
        /// </summary>
        Song,
        /// <summary>
        /// 用户
        /// </summary>
        User,
        /// <summary>
        /// 网页
        /// </summary>
        Webpage,
        /// <summary>
        /// 个人中心
        /// </summary>
        Account

    }
    public class MenuModel
    {
        public string icon { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool selected { get; set; }
        /// <summary>
        /// 是否有红点
        /// </summary>
        public bool hasDot { get; set; }
        
        public MenuOpenMode openMode { get; set; }
        public object parameters { get; set; }
    }
    public class MenuCategoryModel
    {
        public string title { get; set; }
        public bool hasTitle { get; set; }
        public bool showAll { get; set; }
        public ObservableCollection<MenuModel> items { get; set; }
    }

   

}
