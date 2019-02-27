using BiliMusic.Helpers;
using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadSetting();
        }
        private void LoadSetting()
        {
            //读取主题
            var theme = SettingHelper.StorageHelper.Read<int>(SettingHelper.Theme, 0);
            foreach (RadioButton item in stTheme.Children)
            {
                if (item.Tag.Equals(theme.ToString()))
                {
                    item.IsChecked = true;
                }
                item.Checked += Theme_Checked;
            }
            //读取默认播放清晰度
            var defaultQualities = SettingHelper.StorageHelper.Read<int>(SettingHelper.DefaultQualities, 2);
            foreach (RadioButton item in stDefaultQualities.Children)
            {
                if (item.Tag.Equals(defaultQualities.ToString()))
                {
                    item.IsChecked = true;
                }
                item.Checked += Radio_Checked; 
            }

            //读取默认播放清晰度
            var defaultDownloadQualities = SettingHelper.StorageHelper.Read<int>(SettingHelper.DefaultDownloadQualities, 2);
            foreach (RadioButton item in stDownloadQualities.Children)
            {
                if (item.Tag.Equals(defaultDownloadQualities.ToString()))
                {
                    item.IsChecked = true;
                }
                item.Checked += Radio_Checked;
            }


            switchOriginal.IsOn= SettingHelper.StorageHelper.Read<bool>(SettingHelper.OriginalImage,false);

            GetCacheSize();
        }

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            var rd = sender as RadioButton;
            SettingHelper.StorageHelper.Save<int>(rd.GroupName,Convert.ToInt32(rd.Tag));
        }

        private async void GetCacheSize()
        {
            try
            {
                var storageFolder = ApplicationData.Current.TemporaryFolder;
                storageFolder = await storageFolder.GetFolderAsync("ImageCache");
                if (storageFolder != null)
                {
                    var fileSizeTasks = (await storageFolder.GetFilesAsync()).Select(async file => (await file.GetBasicPropertiesAsync()).Size);
                    var sizes = await Task.WhenAll(fileSizeTasks);
                    var folderSize = sizes.Sum(l => (long)l);
                    var size = (folderSize / (double)1024 / 1024).ToString("0.00");
                    txt_cache.Text = "清除缓存(" + size + "M)";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("读取缓存大小失败了", LogType.ERROR, ex);
            }
        }
        private async void BtnClearCache_Click(object sender, RoutedEventArgs e)
        {
            btnClearCache.IsEnabled = false;
            txt_cache.Text = "清除中...";
            await ImageCache.Instance.ClearAsync();
            Utils.ShowMessageToast("清理完成");
            btnClearCache.IsEnabled = true;
            txt_cache.Text = "清除缓存";
        }

        private void Theme_Checked(object sender, RoutedEventArgs e)
        {
            var rd = sender as RadioButton;
            var theme = Convert.ToInt32(rd.Tag);
            SettingHelper.StorageHelper.Save<int>(rd.GroupName, theme);
            Frame rootFrame = Window.Current.Content as Frame;
            switch (theme)
            {
                case 1:
                    rootFrame.RequestedTheme = ElementTheme.Light;
                    break;
                case 2:
                    rootFrame.RequestedTheme = ElementTheme.Dark;
                    break;
                default:
                    rootFrame.RequestedTheme = ElementTheme.Default;
                    break;
            }
        }

      
        private void SwitchOriginal_Toggled(object sender, RoutedEventArgs e)
        {
            SettingHelper.LoadOriginalImage = switchOriginal.IsOn;
            SettingHelper.StorageHelper.Save<bool>(SettingHelper.OriginalImage, switchOriginal.IsOn);
        }
    }
}
