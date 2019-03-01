using BiliMusic.Controls;
using BiliMusic.Helpers;
using BiliMusic.Models;
using BiliMusic.Models.Main;
using BiliMusic.Modules;
using BiliMusic.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace BiliMusic
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Main main;
        public MusicPlay musicPlay;
        public SongFlyoutMenu songFlyout;
        public MainPage()
        {
            this.InitializeComponent();
            //设置标题栏
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(titleBar);
            var bar = ApplicationView.GetForCurrentView().TitleBar;
            var mainColor = (SolidColorBrush)Application.Current.Resources["COLOR_Main"];
            bar.ButtonBackgroundColor = Colors.Transparent;
            bar.ButtonHoverBackgroundColor = mainColor.Color;
            bar.ButtonForegroundColor = Colors.Gray;
#if DEBUG
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler<object>((sender, e) =>
            {
                var m = Windows.System.Diagnostics.ProcessDiagnosticInfo.GetForCurrentProcess().MemoryUsage.GetReport().WorkingSetSizeInBytes / 1024.0 / 1024.0;
                if (m >= 300)
                {
                    DEBUG_INFO.Text = "内存占用:" + m.ToString("0.00") + "MB";
                    DEBUG_INFO.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    DEBUG_INFO.Text = "";
                    DEBUG_INFO.Foreground = new SolidColorBrush(Colors.Green);
                }

            });
            timer.Start();
#endif
           

            main = new Main();
            main.MenuUpdated += Main_MenuUpdated;
            main.MyCreateUpdated += Main_MyCreateUpdated;    
            NavView.DataContext = main;

            songFlyout = new SongFlyoutMenu(main);

            musicPlay = new MusicPlay();
            player.DataContext = musicPlay;

           
            //(App.Current.Resources["song_menu"] as MenuFlyout).Items
        }
       
        private void Main_MyCreateUpdated(object sender, ObservableCollection<MenuModel> e)
        {
            songFlyout.SetMenu();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainFrame.Navigate(typeof(Views.HomePage), 0);
            //设置登录状态
            var userinfo = SettingHelper.StorageHelper.Read<LoginDataV2Model>(SettingHelper.LoginInfo);
            if (userinfo != null)
            {
                if (userinfo.expires_datetime < DateTime.Now)
                {
                    MessageCenter.SendLogout();
                    await new MessageDialog("登录失效了，请重新登录").ShowAsync();
                    await Utils.ShowLoginDialog();
                }
                else
                {
                    UserHelper.isLogin = true;
                    UserHelper.access_key = userinfo.access_token;
                    UserHelper.mid = userinfo.mid;
                    MessageCenter.SendLogined(userinfo);
                }
            }
            songFlyout.SetMenu();
        }
        string par = "HomePage0";
        private void Main_MenuUpdated(object sender, EventArgs e)
        {
            main.selectItem = main.Menus.FirstOrDefault(x => x.Name == par);
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                BackButton.Visibility = Visibility.Visible;
            }
            else
            {
                BackButton.Visibility = Visibility.Collapsed;
            }
            if (main == null || main.Menus == null)
            {
                return;
            }

            par = e.SourcePageType.ToString().Replace("BiliMusic.Views.", "") + ((e.Parameter != null) ? e.Parameter.ToString() : "");
            if (par == "SettingPage")
            {
                NavView.SelectedItem = NavView.SettingsItem;
                return;
            }
            //NavView.SelectedItem= main.Menus.FirstOrDefault(x => x.Name == par);
            main.selectPar = par;
            //main.selectItem = main.Menus.FirstOrDefault(x => x.Name == par);

        }

        private void NavView_PaneClosed(Microsoft.UI.Xaml.Controls.NavigationView sender, object args)
        {
            titleBarTitle.Visibility = Visibility.Collapsed;
        }

        private void NavView_PaneOpened(Microsoft.UI.Xaml.Controls.NavigationView sender, object args)
        {
            titleBarTitle.Visibility = Visibility.Visible;
        }

        private void NavView_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {

        }

        private async void NavView_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                MainFrame.Navigate(typeof(Views.SettingPage));
                //Utils.ShowMessageToast("你选择了设置");
                return;
            }


            var item = args.InvokedItemContainer.Tag as MenuModel;

            switch (item.openMode)
            {
                case MenuOpenMode.Home:
                    MainFrame.Navigate(typeof(Views.HomePage), item.parameters);
                    break;
                case MenuOpenMode.Search:
                    break;
                case MenuOpenMode.Download:
                    break;
                case MenuOpenMode.LoaclMusic:
                    break;
                case MenuOpenMode.Collection:
                    break;
                case MenuOpenMode.Attention:
                    break;
                case MenuOpenMode.Songlist:
                    MainFrame.Navigate(typeof(Views.SonglistPage), item.parameters);
                    break;
                case MenuOpenMode.Song:
                    break;
                case MenuOpenMode.User:
                    break;
                case MenuOpenMode.Webpage:
                    break;
                case MenuOpenMode.Account:
                    break;
                case MenuOpenMode.Rank:
                    break;
                case MenuOpenMode.Radio:
                    break;
                case MenuOpenMode.Login:
                    await Utils.ShowLoginDialog();
                    break;
                case MenuOpenMode.MyCollect:
                    break;
                case MenuOpenMode.MyAttention:
                    break;
                default:
                    break;
            }

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }

        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            musicPlay.playInfo = await musicPlay.LoadMusicInfo(259494);
            musicPlay.mediaPlaybackList.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(musicPlay.playInfo.play_url)));

            //GC.Collect();

            //CoreApplicationView newView = CoreApplication.CreateNewView();
            //int newViewId = 1;
            //await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            //{
            //    Frame frame = new Frame();
            //    frame.Navigate(typeof(DesktopLyricsPage));
            //    Window.Current.Content = frame;
            //    // You have to activate the window in order to show it later.
            //    Window.Current.Activate();

            //    newViewId = ApplicationView.GetForCurrentView().Id;
            //    await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);
            //});

            //Utils.ShowMessageToast("试听中,开通会员听完整",new List<MyUICommand> {
            //    new MyUICommand("开通会员",(toast,command)=>{
            //        Debug.WriteLine("开通会员");
            //    }),
            //    new MyUICommand("关闭",(toast,command)=>{
            //        (toast as MessageToast).Close();
            //    }),
            //});

            //try
            //{
            //    modulesModel modules=null;
            //    var y = modules.id;
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Log("测试一下", LogType.DEBUG, ex);
            //    LogHelper.Log(ex.Message, LogType.ERROR,ex);
            //    LogHelper.Log(ex.Message, LogType.FATAL, ex);
            //    await Launcher.LaunchFolderAsync(Windows.Storage.ApplicationData.Current.LocalFolder);
            //}

            //bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
            //MediaPlayer _mediaPlayer = new MediaPlayer();
            //_mediaPlayer.AutoPlay = true;
            //_mediaPlayer.AudioCategory = MediaPlayerAudioCategory.Media;
            //_mediaPlayer.CommandManager.IsEnabled = true;

            //var _mediaPlaybackList = new MediaPlaybackList();
            //_mediaPlaybackList.AutoRepeatEnabled = true;


            //_mediaPlaybackList.Items.Add(
            //       new MediaPlaybackItem(Windows.Media.Core.MediaSource.CreateFromUri(new Uri("https://upos-hz-mirrorkodou.acgvideo.com/ugaxcode/m190103ws1t9dd2sorv2yi14b22nv6ui-flac.flac?deadline=1551115476&platform=android&upsig=36b8bd9831a1836b0bc3497d9665cbe4"))));
            ////mediaPlayer.SetUriSource(new Uri("https://upos-hz-mirrorks3u.acgvideo.com/ugaxcode/i180302ws8fxcfx8lnnezazn1t32us83-192k.m4a?deadline=1551115065&platform=android&upsig=7500739fd319a8fc2cc1eb1618fe5357"));
            //_mediaPlayer.Source = _mediaPlaybackList;
        }

        private void SliderPosition_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            //Utils.ShowMessageToast("123");
            sliderPosition.SetBinding(Slider.ValueProperty, new Binding()
            {
                Path = new PropertyPath("position"),
                Mode = BindingMode.OneWay
            });
        }

        private void SliderPosition_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Utils.ShowMessageToast(sliderPosition.Value.ToString());
            //musicPlay.mediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(sliderPosition.Value);
            //sliderPosition.SetBinding(Slider.ValueProperty, new Binding()
            //{
            //    Path = new PropertyPath("position"),
            //    Mode = BindingMode.TwoWay
            //});

        }
        private void SliderPosition_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
           
            musicPlay.mediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(sliderPosition.Value);
            sliderPosition.SetBinding(Slider.ValueProperty, new Binding()
            {
                Path = new PropertyPath("position"),
                Mode = BindingMode.TwoWay
            });
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {

            musicPlay.mediaPlayer.Play();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            if (musicPlay.mediaPlayer.PlaybackSession.CanPause)
            {
                musicPlay.mediaPlayer.Pause();
            }

        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (!musicPlay.mediaPlaybackList.AutoRepeatEnabled && !musicPlay.mediaPlaybackList.ShuffleEnabled && musicPlay.mediaPlaybackList.CurrentItemIndex + 1 >= musicPlay.mediaPlaybackList.Items.Count)
            {
                Utils.ShowMessageToast("后面没有了");
                return;
            }
            musicPlay.mediaPlaybackList.MoveNext();
            
        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (!musicPlay.mediaPlaybackList.AutoRepeatEnabled && !musicPlay.mediaPlaybackList.ShuffleEnabled && Convert.ToInt64(musicPlay.mediaPlaybackList.CurrentItemIndex) - 1 < 0)
            {
                Utils.ShowMessageToast("前面没有了");
                return;
            }
            if (musicPlay.mediaPlaybackList.Items.Count==0)
            {
                Utils.ShowMessageToast("前面没有了");
                return;
            }
            musicPlay.mediaPlaybackList.MovePrevious();
            if (musicPlay.mediaPlayer.PlaybackSession.PlaybackState!= MediaPlaybackState.Playing)
            {
                musicPlay.mediaPlayer.Play();

            }
        }

        private void Playlist_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            musicPlay.mediaPlaybackList.MoveTo(Convert.ToUInt32(musicPlay.playList.IndexOf((sender as Grid).DataContext as PlayModel)));
        }

        private void BtnChangePlayMode_Click(object sender, RoutedEventArgs e)
        {
            musicPlay.ChangeMusicPlayMode();
        }

        private void BtnDeleteOne_Click(object sender, RoutedEventArgs e)
        {
            musicPlay.DeletePlayitem((sender as Button).DataContext as PlayModel);
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            musicPlay.ClearPlaylist();
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
           
            FlyoutBase.ShowAttachedFlyout(sender as Grid);
        }

        private void SliderPosition_ManipulationStarting_1(object sender, ManipulationStartingRoutedEventArgs e)
        {

        }

        private void SliderPosition_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Math.Abs(e.NewValue - e.OldValue)>1)
            {
                musicPlay.mediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(e.NewValue);
            }
        }
    }
}
