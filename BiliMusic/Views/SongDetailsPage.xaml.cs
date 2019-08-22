using BiliMusic.Controls;
using BiliMusic.Helpers;
using BiliMusic.Models;
using BiliMusic.Modules;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BiliMusic.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SongDetailsPage : Page
    {
        public SongDetailsPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }
        SongDetail songDetail;
        int songId = 0;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New|| songDetail==null)
            {
                songId = Convert.ToInt32(e.Parameter);
                songDetail = new SongDetail(songId);
                this.DataContext = songDetail;
                songDetail.LoadMusicDeatil();
                //id.Text = e.Parameter.ToString();
            }
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.SourcePageType == typeof(SongDetailsPage))
            {
                this.NavigationCacheMode = NavigationCacheMode.Disabled;
            }
            base.OnNavigatingFrom(e);
        }

        private async void TxtIntro_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ContentDialog contentDialog = new ContentDialog() {
                Content=new TextBlock() {
                    Text=songDetail.detail.intro,
                    IsTextSelectionEnabled=true,
                    TextWrapping= TextWrapping.Wrap
                },
                IsPrimaryButtonEnabled=true,
                PrimaryButtonText="关闭"
            };
            await contentDialog.ShowAsync();
        }

        private async void BtnCollect_Click(object sender, RoutedEventArgs e)
        {
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

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            var player = MessageCenter.GetMusicPlay();
            player.AddToPlay(new PlayModel()
            {
                author = songDetail.detail.author,
                title = songDetail.detail.title,
                pic = songDetail.detail.cover_url,
                songid = songDetail.detail.id
            });
        }

        private void BtnShare_Click(object sender, RoutedEventArgs e)
        {
            SystemHelper.SetClipboard("https://www.bilibili.com/audio/au" + songId);
            Utils.ShowMessageToast("已经将地址复制到剪切板");
        }

        private void ListUps_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as SongsMemberModel;
        }

        private void ListVideos_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as SongsVideosModel;
        }

        private void ListHitSongs_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as SongsAudiosModel;
            MessageCenter.SendMainFrameNavigate(typeof(SongDetailsPage),item.id);
        }

        private void ListMenus_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as menusResponesModel;
            MessageCenter.SendMainFrameNavigate(typeof(SonglistPage), item.menuId);
        }
    }
}
