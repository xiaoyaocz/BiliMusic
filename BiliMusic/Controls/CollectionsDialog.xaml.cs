using BiliMusic.Helpers;
using BiliMusic.Models;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace BiliMusic.Controls
{
    public sealed partial class CollectionsDialog : ContentDialog
    {
        int songId = 0;
        List<int> songIds = null;
        public CollectionsDialog(int _songId)
        {
            this.InitializeComponent();
            songId = _songId;
            LoadCollections();
        }
        public CollectionsDialog(List<int> _songIds)
        {
            this.InitializeComponent();
            songIds = _songIds;
            LoadCollections();

        }
        private async void LoadCollections()
        {
            try
            {
                var re = await Api.MyCreate().Request();
                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<MyCreateMenuModel>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                }
                list.ItemsSource = data.data.list;
                if (songId != 0)
                {
                    foreach (var item in data.data.list.Where(x => x.songsid_list.Contains(songId)))
                    {
                        list.SelectedItems.Add(item);
                    }
                }

            }
            catch (Exception ex)
            {
                Utils.ShowMessageToast("读取创建的歌单失败");
                LogHelper.Log("读取创建的歌单失败", LogType.ERROR, ex);
            }

        }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (PrimaryButtonText == "收藏")
            {
                Collction();
            }
            else
            {
                args.Cancel = true;
                if (txtTitle.Text.Length == 0)
                {
                    Utils.ShowMessageToast("必须输入标题");
                    return;
                }
                Create();
            }
        }
        private async void Collction()
        {
            try
            {
                List<int> menus = new List<int>();
                foreach (MyCreateMenuItemModel item in list.SelectedItems)
                {
                    menus.Add(item.id);
                }
                IHttpResults re = null;
                if (songId != 0)
                {
                    re = await Api.CollectSong(songId, menus).Request();
                }
                else
                {
                    re = await Api.CollectSong(songIds, menus).Request();
                }
                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<object>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return;
                }
                Utils.ShowMessageToast("操作完成");

            }
            catch (Exception ex)
            {
                Utils.ShowMessageToast("收藏歌曲失败");
                LogHelper.Log("收藏歌曲失败", LogType.ERROR, ex);
            }


        }
        private async void Create()
        {
            try
            {
                var re = await Api.CreateCollection(txtTitle.Text, txtDesc.Text, swOpen.IsOn).Request();
                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<object>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return;
                }
                Utils.ShowMessageToast("创建歌单成功");
                MessageCenter.GetMainInfo().Logined();
                pivot.SelectedIndex = 0;
                LoadCollections();
            }
            catch (Exception ex)
            {

                Utils.ShowMessageToast("创建歌单失败");
                LogHelper.Log("创建歌单失败", LogType.ERROR, ex);
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }


        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pivot.SelectedIndex == 0)
            {
                this.PrimaryButtonText = "收藏";
            }
            else
            {
                this.PrimaryButtonText = "创建";
            }
        }
    }
}
