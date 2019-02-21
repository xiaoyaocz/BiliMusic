using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BiliMusic.Models.Main;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.ComponentModel;
using BiliMusic.Helpers;
using BiliMusic.Models;

namespace BiliMusic.Modules
{
    public class Main : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Main()
        {
            _TopMenus = new ObservableCollection<MenuModel>() {
                new MenuModel(){
                    icon=(string)Application.Current.Resources["ICON_Search"],
                    title="搜索",
                    menuType= MenuType.Menuitem,
                    openMode= MenuOpenMode.Search
                },
                new MenuModel(){
                    icon=(string)Application.Current.Resources["ICON_Local"],
                    title="本地音乐",
                    menuType= MenuType.Menuitem,
                    openMode= MenuOpenMode.LoaclMusic
                }
            };
            _HomeMenus = new ObservableCollection<MenuModel>() {
                new MenuModel(){
                    icon=(string)Application.Current.Resources["ICON_Home"],
                    title="综合",
                    menuType= MenuType.Menuitem,
                    openMode= MenuOpenMode.Home,
                    parameters=0
                },
                new MenuModel(){
                    icon=(string)Application.Current.Resources["ICON_Radio"],
                    title="收音叽",
                    menuType= MenuType.Menuitem,
                    openMode= MenuOpenMode.Radio
                },
                new MenuModel(){
                    icon=(string)Application.Current.Resources["ICON_Rank"],
                    title="榜单",
                    menuType= MenuType.Menuitem,
                    openMode= MenuOpenMode.Rank
                },
            };
            _MyMenus = new ObservableCollection<MenuModel>() {
                new MenuModel(){
                    icon=(string)Application.Current.Resources["ICON_PermIdentity"],
                    title="登录",
                    menuType= MenuType.Menuitem,
                    openMode= MenuOpenMode.Login
                }
            };
            CreateMenus();
           
        }
        /// <summary>
        /// 用于榜单NavView的集合
        /// </summary>
        public ObservableCollection<NavigationViewItemBase> Menus { get; set; }

        private ObservableCollection<MenuModel> _Menus { get; set; }


        /// <summary>
        /// 顶部两个菜单，固定搜索、本地音乐
        /// </summary>
        private ObservableCollection<MenuModel> _TopMenus { get; set; }
        /// <summary>
        /// 发现音乐菜单集，固定综合、收音叽、榜单，通过GetHomeMenus读取部分菜单
        /// </summary>
        private ObservableCollection<MenuModel> _HomeMenus { get; set; }
        /// <summary>
        /// 个人中心菜单集
        /// </summary>
        private ObservableCollection<MenuModel> _MyMenus { get; set; }
        /// <summary>
        /// 创建的歌单集合
        /// </summary>
        private ObservableCollection<MenuModel> _MySonglistMenus { get; set; }
        /// <summary>
        /// 收藏的歌单集合
        /// </summary>
        private ObservableCollection<MenuModel> _MyLikeSonglistMenus { get; set; }
        /// <summary>
        /// 登录完成,设置菜单
        /// </summary>
        public void Logged()
        {
            _MyMenus = new ObservableCollection<MenuModel>() {
                new MenuModel(){
                    icon=(string)Application.Current.Resources["ICON_PermIdentity"],
                    title="个人中心",
                    menuType= MenuType.Menuitem,
                    openMode= MenuOpenMode.Login
                },
                 new MenuModel(){
                    icon=(string)Application.Current.Resources["ICON_Star_Border"],
                    title="我的收藏",
                    menuType= MenuType.Menuitem,
                    openMode= MenuOpenMode.MyCollect
                },
                  new MenuModel(){
                    icon=(string)Application.Current.Resources["ICON_Like_Border"],
                    title="我的关注",
                    menuType= MenuType.Menuitem,
                    openMode= MenuOpenMode.MyAttention
                }
            };
            //TODO 更新我的歌单 
            CreateMenus();
        }
        /// <summary>
        /// 注销登录，清除菜单
        /// </summary>
        public void Logout()
        {
            _MyMenus = new ObservableCollection<MenuModel>() {
                new MenuModel(){
                    icon=(string)Application.Current.Resources["ICON_PermIdentity"],
                    title="登录",
                    menuType= MenuType.Menuitem,
                    openMode= MenuOpenMode.Login
                }
            };
            _MySonglistMenus = null;
            _MyLikeSonglistMenus = null;
            CreateMenus();
        }

        /// <summary>
        /// 读取首页Tab菜单
        /// </summary>
        private async Task GetHomeMenus()
        {
            try
            {
                var tab =await Api.Tab().Request();
                if (!tab.status)
                {
                    SystemHelper.SendToast(tab.message);
                    return;
                }
                var data = tab.GetJson<ApiParseModel<List<HomeTabModel>>>();
                if (data.code != 0)
                {
                    SystemHelper.SendToast(data.msg + data.message);
                    return;
                }
                _HomeMenus = new ObservableCollection<MenuModel>();
                _HomeMenus.Add(new MenuModel()
                {
                    icon = (string)Application.Current.Resources["ICON_Home"],
                    title = "综合",
                    menuType = MenuType.Menuitem,
                    openMode = MenuOpenMode.Home,
                    parameters = 0
                });
                foreach (var item in data.data)
                {
                    if (item.title != "综合")
                    {
                        _HomeMenus.Add(new MenuModel()
                        {
                            icon = (string)Application.Current.Resources["ICON_Music2"],
                            menuType = MenuType.Menuitem,
                            title = item.title,
                            openMode = MenuOpenMode.Home,
                            parameters = item.id
                        });
                    }
                }
                _HomeMenus.Add(new MenuModel()
                {
                    icon = (string)Application.Current.Resources["ICON_Radio"],
                    title = "收音叽",
                    menuType = MenuType.Menuitem,
                    openMode = MenuOpenMode.Radio
                });
                _HomeMenus.Add(new MenuModel()
                {
                    icon = (string)Application.Current.Resources["ICON_Rank"],
                    title = "榜单",
                    menuType = MenuType.Menuitem,
                    openMode = MenuOpenMode.Rank
                });
                CreateMenus();
            }
            catch (Exception ex)
            {
                SystemHelper.SendToast(ex.Message);
            }
           
        }
        /// <summary>
        /// 创建完整的菜单
        /// </summary>
        private void CreateMenus()
        {
            _Menus = new ObservableCollection<MenuModel>();
            foreach (var item in _TopMenus)
            {
                _Menus.Add(item);
            }
            _Menus.Add(new MenuModel()
            {
                title = "发现音乐",
                menuType = MenuType.Header
            });
            foreach (var item in _HomeMenus)
            {
                _Menus.Add(item);
            }
            _Menus.Add(new MenuModel()
            {
                title = "我的",
                menuType = MenuType.Header
            });

            foreach (var item in _MyMenus)
            {
                _Menus.Add(item);
            }
            if (_MySonglistMenus != null && _MySonglistMenus.Count != 0)
            {
                _Menus.Add(new MenuModel()
                {
                    title = "创建的歌单",
                    menuType = MenuType.Header
                });
                foreach (var item in _MySonglistMenus)
                {
                    _Menus.Add(item);
                }
            }
            if (_MyLikeSonglistMenus != null && _MyLikeSonglistMenus.Count != 0)
            {
                _Menus.Add(new MenuModel()
                {
                    title = "收藏的歌单",
                    menuType = MenuType.Header
                });
                foreach (var item in _MyLikeSonglistMenus)
                {
                    _Menus.Add(item);
                }
            }

            MenuitemToViewItem();
        }
        /// <summary>
        /// 将MenuModel转为MenuViewItem
        /// </summary>
        private void MenuitemToViewItem()
        {
            Menus = new ObservableCollection<NavigationViewItemBase>();
            foreach (var item in _Menus)
            {
                switch (item.menuType)
                {
                    case MenuType.Header:
                        {
                            Menus.Add(new NavigationViewItemHeader()
                            {
                                Content = item.title,
                                Tag = item,
                                Foreground = (Windows.UI.Xaml.Media.SolidColorBrush)Application.Current.Resources["COLOR_Foreground"]
                            });
                        }
                        break;
                    case MenuType.Menuitem:
                        {
                            Menus.Add(new NavigationViewItem()
                            {
                                Content = item.title,
                                Icon = new Windows.UI.Xaml.Controls.FontIcon()
                                {
                                    FontFamily = (Windows.UI.Xaml.Media.FontFamily)Application.Current.Resources["FONTS_MaterialIcons"],
                                    Glyph = item.icon
                                },
                                Tag = item
                            });
                        }
                        break;
                    case MenuType.Separator:
                        {
                            Menus.Add(new NavigationViewItemSeparator());
                        }
                        break;
                    default:
                        break;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Menus"));
        }

    }
}
