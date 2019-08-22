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
using BiliMusic.Models;
using BiliMusic.Helpers;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BiliMusic.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }
        Search search;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                txtSearch.Text = "";
                searchInfo.Visibility = Visibility.Visible;
                pivotResults.Visibility = Visibility.Collapsed;
                search = new Search();
                DataContext = search;
            }
        }
        private void TxtSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (txtSearch.Text.Length == 0)
            {
                searchInfo.Visibility = Visibility.Visible;
                pivotResults.Visibility = Visibility.Collapsed;
                search.LoadHistory();
                return;
            }
            searchInfo.Visibility = Visibility.Collapsed;
            pivotResults.Visibility = Visibility.Visible;
            search.DoSearch(txtSearch.Text);
        }

        private void BtnWord_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = (sender as HyperlinkButton).DataContext.ToString();
            TxtSearch_QuerySubmitted(null, null);
        }

        private void BtnClearHistory_Click(object sender, RoutedEventArgs e)
        {
            search.ClearHistory();
        }

        private void SvSongs_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (svSongs.VerticalOffset >= svSongs.ScrollableHeight - 100)
            {
                if (!search.loading)
                {
                    search.SearchSongs();
                }
            }
        }

        private void SvMenus_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (svMenus.VerticalOffset >= svMenus.ScrollableHeight - 100)
            {
                if (!search.loading)
                {
                    search.SearchMenus();
                }
            }
        }
        private void SvMusician_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (svMusician.VerticalOffset >= svMusician.ScrollableHeight - 100)
            {
                if (!search.loading)
                {
                    search.SearchMusician();
                }
            }
        }
        private void ListMenus_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as SearchMenuResultModel;
            MessageCenter.SendMainFrameNavigate(typeof(SonglistPage), item.id);
        }

        private void ListSongs_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as SearchSongResultModel;
            MessageCenter.SendMainFrameNavigate(typeof(SongDetailsPage), item.id);
        }

       

        private void ListMusicians_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
