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
using BiliMusic.Models;
using BiliMusic.Modules;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.System;
using BiliMusic.Models;
using Windows.Media.Playback;
using Windows.Media.Core;
using BiliMusic.Helpers;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BiliMusic.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        int tab_id = 0;
        public HomePage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            homeItemDataTemplateSelector.resource = this.Resources;
        }
        TabDetail tabDetail;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New || tabDetail == null)
            {
                tab_id = Convert.ToInt32(e.Parameter);
                tabDetail = new TabDetail(tab_id);
                this.DataContext = tabDetail;
                tabDetail.LoadData();
            }

        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.SourcePageType == typeof(HomePage))
            {
                this.NavigationCacheMode = NavigationCacheMode.Disabled;
            }
            base.OnNavigatingFrom(e);
        }
        private void Banner_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var data = (sender as ImageEx).DataContext as bannersModel;
            this.Frame.Navigate(typeof(WebPage), data.schema);

            //await Launcher.LaunchUriAsync(new Uri(data.schema));
        }

        private void SongMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            var data = e.ClickedItem as menusModel;
            this.Frame.Navigate(typeof(SonglistPage), data.menuId);
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            tabDetail.RefreshModule((sender as Button).DataContext as modulesModel);
        }


        private async void LsRecommend_ItemClick(object sender, ItemClickEventArgs e)
        {
            var player = MessageCenter.GetMusicPlay();
            var data = await player.LoadMusicInfo((e.ClickedItem as songsModel).song_id);
            if (data != null)
            {
                player.AddPlay(data);
            }
        }
    }
}
