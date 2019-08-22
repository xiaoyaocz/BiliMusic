using BiliMusic.Controls;
using BiliMusic.Helpers;
using BiliMusic.Models;
using BiliMusic.Views;
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
                        (item as MenuFlyoutItem).Click -= MenuPlay_Click;
                        (item as MenuFlyoutItem).Click += MenuPlay_Click;
                        break;
                    case "menuAdd":
                        (item as MenuFlyoutItem).Click -= MenuAdd_Click;
                        (item as MenuFlyoutItem).Click += MenuAdd_Click;
                        break;
                    case "menuDown":
                        (item as MenuFlyoutItem).Click -= MenuDownload_Click;
                        (item as MenuFlyoutItem).Click += MenuDownload_Click;
                        break;
                    case "menuShare":
                        (item as MenuFlyoutItem).Click -= MenuShare_Click;
                        (item as MenuFlyoutItem).Click += MenuShare_Click;
                        break;
                    case "menuCollect":
                        (item as MenuFlyoutItem).Click -= MenuCollect_Click; 
                        (item as MenuFlyoutItem).Click += MenuCollect_Click; 
                        break;
                    case "menuInfo":
                        (item as MenuFlyoutItem).Click -= MenuInfo_Click;
                        (item as MenuFlyoutItem).Click += MenuInfo_Click;
                        break;
                    default:
                        break;
                }
            }
        }

        private void MenuAdd_Click(object sender, RoutedEventArgs e)
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
        }

        private async void MenuCollect_Click(object sender, RoutedEventArgs e)
        {
            var songId = 0;
            var context = (sender as MenuFlyoutItem).DataContext;
            if (context is songsListModel)
            {
                songId = (context as songsListModel).id;
            }
            else
            {
                songId = (context as PlayModel).songid;
            }
            if (UserHelper.isLogin || await Utils.ShowLoginDialog())
            {
                try
                {
                    CollectionsDialog cd = new CollectionsDialog(songId);
                    await cd.ShowAsync();
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
            var songId = 0;
            var context = (sender as MenuFlyoutItem).DataContext;
            if (context is songsListModel)
            {
                songId = (context as songsListModel).id;
            }
            else
            {
                songId = (context as PlayModel).songid;
            }
            MessageCenter.SendMainFrameNavigate(typeof(SongDetailsPage),songId);
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
                player.AddToPlay(new PlayModel()
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
