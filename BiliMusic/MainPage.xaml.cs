using BiliMusic.Helpers;
using BiliMusic.Models;
using BiliMusic.Models.Main;
using BiliMusic.Modules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
            bar.ButtonForegroundColor = Colors.Black;
           
        }

       

        Main main;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
           
            MainFrame.Navigate(typeof(Views.HomePage));
            main = new Main();
            this.DataContext = main;

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

            //NavView.MenuItemsSource = main.Menus;


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
                Utils.ShowMessageToast("你选择了设置");
                return;
            }
            

            var item= args.InvokedItemContainer.Tag as MenuModel;
            
            switch (item.openMode)
            {
                case MenuOpenMode.Home:
                    MainFrame.Navigate(typeof(Views.HomePage),item.parameters);
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
                    MainFrame.Navigate(typeof(Views.SonglistPage),item.parameters);
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
    }
}
