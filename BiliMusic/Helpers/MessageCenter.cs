using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiliMusic.Models;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BiliMusic.Helpers
{
    public static class MessageCenter
    {
        public delegate void MainFrameNavigateHandler(Type page,object par);
        public static event EventHandler<object> Logined;
        public static event EventHandler Logouted;
        public static event MainFrameNavigateHandler MainFrameNavigatedTo;
        /// <summary>
        /// 发送登录完成事件
        /// </summary>
        public static void SendLogined(LoginDataV2Model data)
        {
            Logined?.Invoke(null, data);
        }
        /// <summary>
        /// 发送注销事件
        /// </summary>
        public static void SendLogout()
        {
            SettingHelper.StorageHelper.Save(SettingHelper.LoginInfo, "");
            UserHelper.isLogin = false;
            UserHelper.access_key = "";
            UserHelper.mid = 0;
            Logouted?.Invoke(null, null);
        }

        public static BiliMusic.Modules.MusicPlay GetMusicPlay()
        {
            return ((Window.Current.Content as Frame).Content as MainPage).musicPlay;
        }
        public static BiliMusic.Modules.Main GetMainInfo()
        {
            return ((Window.Current.Content as Frame).Content as MainPage).main;
        }

        public static void SendMainFrameNavigate(Type page, object par)
        {
            MainFrameNavigatedTo?.Invoke(page, par);
        }


    }
 
}
