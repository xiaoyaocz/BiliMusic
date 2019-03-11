using BiliMusic.Helpers;
using BiliMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace BiliMusic.Modules
{
    public class SongFlyoutMenu
    {
        public MenuFlyout menuFlyout { get; set; }
        private Main main;
        public SongFlyoutMenu(Main m)
        {
            main = m;
            menuFlyout = (App.Current.Resources["song_menu"] as MenuFlyout);
            main.MyCreateUpdated += Main_MyCreateUpdated;
            SetMenu();
        }

        private void Main_MyCreateUpdated(object sender, System.Collections.ObjectModel.ObservableCollection<Models.Main.MenuModel> e)
        {
            SetMenu();
        }

        public void SetMenu()
        {
            foreach (var item in menuFlyout.Items)
            {
                switch (item.Name)
                {
                    case "menuPlay":
                        (item as MenuFlyoutItem).Click += MenuPlay_Click;
                        break;
                    case "menuDown":
                        (item as MenuFlyoutItem).Click += MenuDownload_Click;
                        break;
                    case "menuShare":
                        (item as MenuFlyoutItem).Click += MenuShare_Click;
                        break;
                    case "menuCollect":
                        if (!UserHelper.isLogin)
                        {
                            item.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            item.Visibility = Visibility.Visible;
                            var collects = item as MenuFlyoutSubItem;
                            collects.Items.Clear();
                            if (main._MySonglistMenus != null)
                            {
                                foreach (var menuitem in main._MySonglistMenus)
                                {
                                    var menu = new MenuFlyoutItem()
                                    {
                                        Text = menuitem.title,
                                        Tag = menuitem.parameters2
                                    };
                                    menu.Click += MenuCollect_Click;
                                    collects.Items.Add(menu);
                                }
                            }
                            collects.Items.Add(new MenuFlyoutSeparator());
                            var menuCreate = new MenuFlyoutItem()
                            {
                                //Icon = new SymbolIcon(Symbol.Add),
                                Text = "创建歌单"
                            };
                            menuCreate.Click += MenuCreate_Click;
                            collects.Items.Add(menuCreate);
                        }

                        break;
                    case "menuInfo":
                        (item as MenuFlyoutItem).Click += MenuInfo_Click;
                        break;
                    default:
                        break;
                }
            }
        }

        private void MenuCreate_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private async void MenuCollect_Click(object sender, RoutedEventArgs e)
        {
            var songId = 0;
            var context= (sender as MenuFlyoutItem).DataContext;
            if (context is songsListModel)
            {
                songId = (context as songsListModel).id;
            }
            else
            {
                songId = (context as PlayModel).songid;
            }
            if (UserHelper.isLogin||await Utils.ShowLoginDialog())
            {
                try
                {
                    IHttpResults re = await Api.CollectSong(songId, (int)(sender as MenuFlyoutItem).Tag).Request();
                    if (!re.status)
                    {
                        Utils.ShowMessageToast(re.message);
                        return;
                    }
                    var data = re.GetJson<ApiParseModel<object>>();
                    if (data.code != 0)
                    {
                        Utils.ShowMessageToast(data.msg + data.message);
                        return;
                    }
                    Utils.ShowMessageToast("收藏成功");
                }
                catch (Exception ex)
                {

                    Utils.ShowMessageToast("收藏歌曲失败");
                    LogHelper.Log("收藏歌曲失败", LogType.ERROR, ex);
                }
            }
            else
            {
                Utils.ShowMessageToast("请先登录");
            }
          

        }

        private void MenuInfo_Click(object sender, RoutedEventArgs e)
        {
           
        }

      


        private void MenuShare_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            var data = (sender as MenuFlyoutItem).DataContext;
            if (data is songsListModel)
            {
                id = (data as songsListModel).id;
            }
            else if (data is PlayModel)
            {
                id = (data as PlayModel).songid;
            }
            SystemHelper.SetClipboard("https://www.bilibili.com/audio/au" + id);
            Utils.ShowMessageToast("已经将地址复制到剪切板");
        }

        private void MenuDownload_Click(object sender, RoutedEventArgs e)
        {
            Utils.ShowMessageToast("下载功能开发中....");
        }

        private void MenuPlay_Click(object sender, RoutedEventArgs e)
        {
            var player = MessageCenter.GetMusicPlay();
            var data = (sender as MenuFlyoutItem).DataContext;
            if (data is songsListModel)
            {
                var item = data as songsListModel;
                player.AddPlay(new PlayModel()
                {
                    author = item.author,
                    title = item.title,
                    pic = item.cover_url,
                    songid = item.id
                });

            }
            else if (data is PlayModel)
            {
                var play = data as PlayModel;
                var index = player.mediaPlaybackList.Items.IndexOf(player.mediaPlaybackList.Items.FirstOrDefault(x => x.Source.CustomProperties["id"].Equals(play.songid)));
                MessageCenter.GetMusicPlay().mediaPlaybackList.MoveTo(Convert.ToUInt32(index));
            }
        }
    }
}
