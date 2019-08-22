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
using Windows.UI.Xaml.Data;
using Windows.UI;

namespace BiliMusic.Modules
{
    public class Main : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler MenuUpdated;
        public event EventHandler<ObservableCollection<MenuModel>> MyCreateUpdated;

        /// <summary>
        /// 用于榜单NavView的集合
        /// </summary>
        public ObservableCollection<NavigationViewItemBase> Menus { get; set; }
        //public string selectPar { get; set; } = "HomePage0";

        private string _selectPar= "HomePage0";
        public string selectPar
        {
            get { return _selectPar; }
            set
            {
                _selectPar = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("selectPar"));
                selectItem = Menus.FirstOrDefault(x => x.Name == selectPar);
            }
        }

        private NavigationViewItemBase _selectItem;
        public NavigationViewItemBase selectItem
        {
            get { return _selectItem; }
            set
            {
                _selectItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("selectItem"));
            }
        }



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
        public ObservableCollection<MenuModel> _MySonglistMenus { get; set; }
        /// <summary>
        /// 收藏的歌单集合
        /// </summary>
        private ObservableCollection<MenuModel> _MyLikeSonglistMenus { get; set; }
        /// <summary>
        /// 收藏的专辑集合
        /// </summary>
        private ObservableCollection<MenuModel> _MyAlbumMenus { get; set; }

        public Main()
        {
            //注册登录完成事件
            MessageCenter.Logined += MessageCenter_Logined;
            MessageCenter.Logouted += MessageCenter_Logouted;
            _TopMenus = new ObservableCollection<MenuModel>() {
                new MenuModel(){
                    icon=(string)Application.Current.Resources["ICON_Search"],
                    title="搜索",
                    menuType= MenuType.Menuitem,
                    openMode= MenuOpenMode.Search,
                    name="SearchPage"
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
                    name="HomePage0",
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
                    name="RankPage",
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

        private void MessageCenter_Logouted(object sender, EventArgs e)
        {
            Logout();
        }

        private void MessageCenter_Logined(object sender, object e)
        {
            Logined();
        }


        /// <summary>
        /// 登录完成,设置菜单
        /// </summary>
        public async void Logined()
        {
            _MyMenus = new ObservableCollection<MenuModel>() {
                new MenuModel(){
                    icon=(string)Application.Current.Resources["ICON_PermIdentity"],
                    title="个人中心",
                    menuType= MenuType.Menuitem,
                    openMode= MenuOpenMode.Account
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
            await GetMyCreate();
            await GetMyCollection();
            await GetMyAlbum();
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
            _MyAlbumMenus = null;
            CreateMenus();
        }

        /// <summary>
        /// 读取我收藏的歌单
        /// </summary>
        /// <returns></returns>
        private async Task GetMyCollection()
        {
            try
            {
                var re = await Api.MyCollection().Request();
                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<MyCollectionMenuModel>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return;
                }
                _MyLikeSonglistMenus = new ObservableCollection<MenuModel>();
                foreach (var item in data.data.list)
                {
                    _MyLikeSonglistMenus.Add(new MenuModel()
                    {
                        name = "SonglistPage" + item.menuId,
                        icon = (string)Application.Current.Resources["ICON_SongList"],
                        title = item.title,
                        menuType = MenuType.Menuitem,
                        openMode = MenuOpenMode.Songlist,
                        parameters = item.menuId
                    });
                }

            }
            catch (Exception ex)
            {
                Utils.ShowMessageToast("读取收藏的歌单失败");
                LogHelper.Log("读取收藏的歌单失败", LogType.ERROR, ex);
                
            }
        }

        /// <summary>
        /// 读取我创建的歌单
        /// </summary>
        /// <returns></returns>
        private async Task GetMyCreate()
        {
            try
            {
                var re = await Api.MyCreate().Request();
                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<MyCreateMenuModel>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return;
                }
                _MySonglistMenus = new ObservableCollection<MenuModel>();
                foreach (var item in data.data.list)
                {
                    _MySonglistMenus.Add(new MenuModel()
                    {
                        name = "SonglistPage" + item.menu_id,
                        icon = (string)Application.Current.Resources["ICON_SongList"],
                        title = item.title,
                        menuType = MenuType.Menuitem,
                        openMode = MenuOpenMode.Songlist,
                        parameters = item.menu_id,
                        parameters2=item.id
                    });
                }
                MyCreateUpdated?.Invoke(this, _MySonglistMenus);
            }
            catch (Exception ex)
            {
                Utils.ShowMessageToast("读取创建的歌单失败");
                LogHelper.Log("读取创建的歌单失败", LogType.ERROR, ex);
            }
        }

        /// <summary>
        /// 读取我收藏的专辑
        /// </summary>
        /// <returns></returns>
        private async Task GetMyAlbum()
        {
            try
            {
                var re = await Api.MyAlbum().Request();
                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<MyCollectionMenuModel>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return;
                }
                _MyAlbumMenus = new ObservableCollection<MenuModel>();
                foreach (var item in data.data.list)
                {
                    _MyAlbumMenus.Add(new MenuModel()
                    {
                        name = "SonglistPage" + item.menuId,
                        icon = (string)Application.Current.Resources["ICON_SongList"],
                        title = item.title,
                        menuType = MenuType.Menuitem,
                        openMode = MenuOpenMode.Songlist,
                        parameters = item.menuId
                    });
                }

            }
            catch (Exception ex)
            {
                Utils.ShowMessageToast("读取收藏的专辑失败");
                LogHelper.Log("读取收藏的专辑失败", LogType.ERROR, ex);

            }
        }


        /// <summary>
        /// 读取首页Tab菜单
        /// </summary>
        private async Task GetHomeMenus()
        {
            try
            {
                var tab = await Api.Tab().Request();
                if (!tab.status)
                {
                    Utils.ShowMessageToast(tab.message);
                    return;
                }
                var data = tab.GetJson<ApiParseModel<List<HomeTabModel>>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return;
                }
                _HomeMenus = new ObservableCollection<MenuModel>();
                _HomeMenus.Add(new MenuModel()
                {
                    name = "HomePage0",
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
                            name = "HomePage" + item.id,
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
                    name = "RankPage",
                    icon = (string)Application.Current.Resources["ICON_Rank"],
                    title = "榜单",
                    menuType = MenuType.Menuitem,
                    openMode = MenuOpenMode.Rank
                });
                CreateMenus();
            }
            catch (Exception ex)
            {

                Utils.ShowMessageToast("读取菜单信息失败");
                LogHelper.Log("读取菜单信息失败", LogType.ERROR, ex);
            }

        }
        /// <summary>
        /// 创建完整的菜单
        /// </summary>
        private async void CreateMenus()
        {
            if (_HomeMenus.Count <= 3)
            {
                await GetHomeMenus();
            }
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
            if (_MyAlbumMenus != null && _MyAlbumMenus.Count != 0)
            {
                _Menus.Add(new MenuModel()
                {
                    title = "收藏的专辑",
                    menuType = MenuType.Header
                });
                foreach (var item in _MyAlbumMenus)
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
            
            var m = new ObservableCollection<NavigationViewItemBase>();
            foreach (var item in _Menus)
            {
                switch (item.menuType)
                {
                    case MenuType.Header:
                        {
                            var header = new NavigationViewItemHeader()
                            {
                                Content = item.title,
                                Tag = item,
                                Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Gray)
                            };
                            m.Add(header);
                        }
                        break;
                    case MenuType.Menuitem:
                        {
                            m.Add(new NavigationViewItem()
                            {
                                Name = item.name,
                                Content = new Windows.UI.Xaml.Controls.TextBlock()
                                {
                                    Text = item.title,
                                    TextTrimming = TextTrimming.WordEllipsis,
                                    Margin = new Thickness(0, 0, 8, 0)
                                },
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
                            m.Add(new NavigationViewItemSeparator());
                        }
                        break;
                    default:
                        break;
                }
            }
            Menus = m;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Menus"));
            
            MenuUpdated?.Invoke(this, null);
           
            
        }

    }
}
