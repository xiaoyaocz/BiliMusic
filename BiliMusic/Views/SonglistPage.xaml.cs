using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BiliMusic.Modules;
using BiliMusic.Helpers;
using BiliMusic.Models;
using Windows.UI.Popups;
using BiliMusic.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BiliMusic.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SonglistPage : Page
    {
        public SonglistPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }
        MenuDetail detail;
        int menu_id = 0;
        int recommed_id = 0;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New || detail == null)
            {
                if (e.Parameter is int)
                {
                    menu_id = (int)e.Parameter;
                    detail = new MenuDetail(menu_id);
                }
                else
                {
                    var data = e.Parameter as object[];
                    recommed_id = (int)data[0];
                    detail = new MenuDetail(0, recommed_id);
                }
                this.DataContext = detail;
                detail.LoadData();
            }
            //title.Text = e.Parameter.ToString();
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.SourcePageType == typeof(SonglistPage))
            {
                this.NavigationCacheMode = NavigationCacheMode.Disabled;
            }
            base.OnNavigatingFrom(e);
        }
        private void ListSongsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (e.ClickedItem as songsListModel);
            var player = MessageCenter.GetMusicPlay();
            player.AddPlay(new PlayModel()
            {
                author = item.author,
                title = item.title + " - " + item.author,
                pic = item.cover_url,
                songid = item.id,
                play_url = new Uri("music://" + item.id)
            });
            //var data = await player.LoadMusicInfo((e.ClickedItem as songsListModel).id);
            //if (data != null)
            //{
            //    player.AddPlay(data);
            //}
        }

        private void Grid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var item = (sender as Grid).DataContext as songsListModel;
            var player = MessageCenter.GetMusicPlay();
            player.AddToPlay(new PlayModel()
            {
                author = item.author,
                title = item.title,
                pic = item.cover_url,
                songid = item.id
            });
        }

        private void BtnPlayAll_Click(object sender, RoutedEventArgs e)
        {
            var player = MessageCenter.GetMusicPlay();
            var data = (sender as Button).DataContext as SonglistDetailModel;
            List<PlayModel> ls = new List<PlayModel>();
            foreach (var item in data.songsList)
            {
                ls.Add(new PlayModel()
                {
                    author = item.author,
                    title = item.title,
                    pic = item.cover_url,
                    songid = item.id
                });

            }
            player.ReplacePlayList(ls);
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {

            FlyoutBase.ShowAttachedFlyout(sender as Grid);
        }


        private async void BtnCollect_Click(object sender, RoutedEventArgs e)
        {
            if (UserHelper.isLogin || await Utils.ShowLoginDialog())
            {
                if (detail.Datas.menusRespones.collected == 0)
                {
                    detail.CollectMenu();
                }
                else
                {
                    detail.CancelCollectMenu();
                }
            }
            else
            {
                Utils.ShowMessageToast("请先登录");
            }


        }

        private async void BtnLike_Click(object sender, RoutedEventArgs e)
        {
            if (UserHelper.isLogin || await Utils.ShowLoginDialog())
            {
                var data = (sender as Button).DataContext as songsListModel;
                CollectionsDialog cd = new CollectionsDialog(data.id);
                await cd.ShowAsync();
            }
            else
            {
                Utils.ShowMessageToast("请先登录");
            }
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as songsListModel;
            var player = MessageCenter.GetMusicPlay();
            player.AddToPlay(new PlayModel()
            {
                author = item.author,
                title = item.title,
                pic = item.cover_url,
                songid = item.id
            });
        }

        private void BtnAddToPlay_Click(object sender, RoutedEventArgs e)
        {
            var player = MessageCenter.GetMusicPlay();
            var data = (sender as Button).DataContext as SonglistDetailModel;
            List<PlayModel> ls = new List<PlayModel>();
            foreach (var item in data.songsList)
            {
                player.AddPlay(new PlayModel()
                {
                    author = item.author,
                    title = item.title,
                    pic = item.cover_url,
                    songid = item.id
                });
            }
        }

        private void BtnCancelSelect_Click(object sender, RoutedEventArgs e)
        {
            selectCommands.Visibility = Visibility.Collapsed;
            notSelectCommands.Visibility = Visibility.Visible;
            listSongsList.SelectionMode = ListViewSelectionMode.None;


        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            selectCommands.Visibility = Visibility.Visible;
            notSelectCommands.Visibility = Visibility.Collapsed;
            listSongsList.SelectionMode = ListViewSelectionMode.Multiple;
        }

        private void BtnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (listSongsList.SelectedItems.Count == 0)
            {
                listSongsList.SelectAll();
            }
            else
            {
                listSongsList.SelectedItems.Clear();
            }
        }

        private void BtnSelectAddToPlay_Click(object sender, RoutedEventArgs e)
        {
            if (listSongsList.SelectedItems.Count==0)
            {
                Utils.ShowMessageToast("没有选择任何项");
                return;
            }
            var player = MessageCenter.GetMusicPlay();
            foreach (songsListModel item in listSongsList.SelectedItems)
            {
                player.AddPlay(new PlayModel()
                {
                    author = item.author,
                    title = item.title,
                    pic = item.cover_url,
                    songid = item.id
                });
            }

        }

        private void BtnSelectDownload_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void BtnSelectLike_Click(object sender, RoutedEventArgs e)
        {
            if (listSongsList.SelectedItems.Count == 0)
            {
                Utils.ShowMessageToast("没有选择任何项");
                return;
            }
            if (UserHelper.isLogin || await Utils.ShowLoginDialog())
            {
                List<int> songids = new List<int>();
                foreach (songsListModel item in listSongsList.SelectedItems)
                {
                    songids.Add(item.id);
                }
                CollectionsDialog cd = new CollectionsDialog(songids);
                await cd.ShowAsync();
            }
            else
            {
                Utils.ShowMessageToast("请先登录");
            }

        }

        private void BtnShare_Click(object sender, RoutedEventArgs e)
        {
            SystemHelper.SetClipboard("https://m.bilibili.com/audio/am" + menu_id);
            Utils.ShowMessageToast("已经将地址复制到剪切板");
        }
    }
}
