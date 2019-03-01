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
                        }
                        {
                            var collects = item as MenuFlyoutSubItem;
                            collects.Items.Clear();
                            if (main._MySonglistMenus!=null)
                            {
                                foreach (var menuitem in main._MySonglistMenus)
                                {
                                    var menu = new MenuFlyoutItem()
                                    {
                                        Text = menuitem.title,
                                        Tag = item
                                    };
                                    menu.Click += MenuCollect_Click;
                                    collects.Items.Add(menu);
                                }
                            }
                           
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

        private void MenuCollect_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MenuInfo_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MenuShare_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            var data = (sender as MenuFlyoutItem).DataContext;
            if (data is songsListModel)
            {
                id =( data as songsListModel).id;
            }
            else if (data is PlayModel)
            {
                id = (data as PlayModel).songid;
            }
            SystemHelper.SetClipboard("https://www.bilibili.com/audio/au"+id);
            Utils.ShowMessageToast("已经将地址复制到剪切板");
        }

        private void MenuDownload_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MenuPlay_Click(object sender, RoutedEventArgs e)
        {
            var player= MessageCenter.GetMusicPlay();
            var data = (sender as MenuFlyoutItem).DataContext;
            if (data is songsListModel)
            {
                var item= data as songsListModel;
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
                var index = player.mediaPlaybackList.Items.IndexOf(player.mediaPlaybackList.Items.FirstOrDefault(x=>x.Source.CustomProperties["id"].Equals(play.songid)));
                MessageCenter.GetMusicPlay().mediaPlaybackList.MoveTo(Convert.ToUInt32(index));
            }
        }
    }
}
