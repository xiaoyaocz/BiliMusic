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
        private async void Banner_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var data = (sender as ImageEx).DataContext as bannersModel;

            if (data.schema.Contains("bilibili://"))
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri(data.schema));
            }
            else
            {
                this.Frame.Navigate(typeof(WebPage), data.schema);
            }

           

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


        private void LsRecommend_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as songsModel;
            var player = MessageCenter.GetMusicPlay();
            player.AddPlay(new PlayModel()
            {
                author = item.author,
                title = item.title,
                pic = item.cover_url,
                songid = item.id
            });
        }

        private void BtnShowMoreRecommend_Click(object sender, RoutedEventArgs e)
        {
            var data = (sender as Button).DataContext as modulesModel;
            this.Frame.Navigate(typeof(SonglistPage),new object[] { data.id,"Recommend" });
        }

        private async void BtnPlayAll_Click(object sender, RoutedEventArgs e)
        {
            tabDetail.loading = true;

            try
            {
                var player = MessageCenter.GetMusicPlay();
                var module = (sender as Button).DataContext as modulesModel;

                var re = await Api.RecommendDetail(module.id).Request();
                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<SonglistDetailModel>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return;
                }
                List<PlayModel> ls = new List<PlayModel>();
                foreach (var item in data.data.songsList)
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
            catch (Exception ex)
            {
                Utils.ShowMessageToast("播放全部推荐歌曲失败");
                LogHelper.Log("首页播放全部推荐歌曲失败", LogType.ERROR, ex);
            }
            finally
            {
                tabDetail.loading = false;
            }
          
           
        }
    }
}
