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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New || detail == null)
            {
                menu_id = (int)e.Parameter;
                detail = new MenuDetail(menu_id);
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
                title = item.title,
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
    }
}
