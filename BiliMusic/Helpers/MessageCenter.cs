using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiliMusic.Models;
using System.Threading.Tasks;

namespace BiliMusic.Helpers
{
    public static class MessageCenter
    {
        public static event EventHandler<object> Logined;
        public static event EventHandler Logouted;
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
            Logouted?.Invoke(null,null);
        }

    }
}
