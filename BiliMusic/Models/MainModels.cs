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
        Home=0,
        /// <summary>
        /// 搜索
        /// </summary>
        Search = 1,
        /// <summary>
        /// 我的缓存
        /// </summary>
        Download=2,
        /// <summary>
        /// 本地音乐
        /// </summary>
        LoaclMusic=3,
        /// <summary>
        /// 我的收藏
        /// </summary>
        Collection=4,
        /// <summary>
        /// 关注的人
        /// </summary>
        Attention=5,
        /// <summary>
        /// 歌单
        /// </summary>
        Songlist=6,
        /// <summary>
        /// 歌曲
        /// </summary>
        Song=7,
        /// <summary>
        /// 用户
        /// </summary>
        User=8,
        /// <summary>
        /// 网页
        /// </summary>
        Webpage=9,
        /// <summary>
        /// 个人中心
        /// </summary>
        Account=10,
        /// <summary>
        /// 榜单
        /// </summary>
        Rank=11,
        /// <summary>
        /// 收音机
        /// </summary>
        Radio = 12,
        /// <summary>
        /// 登录
        /// </summary>
        Login=13,
        /// <summary>
        /// 我的收藏
        /// </summary>
        MyCollect = 14,
        /// <summary>
        /// 我的关注
        /// </summary>
        MyAttention = 15

    }
    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum MenuType
    {
        Header,
        Menuitem,
        Separator
    }
    public class MenuModel
    {
        /// <summary>
        /// Icon
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 点击打开类型
        /// </summary>
        public MenuOpenMode openMode { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public object parameters { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public MenuType menuType { get; set; } = MenuType.Menuitem;
    }
   
   

}
