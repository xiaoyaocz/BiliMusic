using BiliMusic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiliMusic.Controls;
using Windows.UI.Popups;

namespace BiliMusic
{
    public static class Utils
    {
        /// <summary>
        /// 发送请求，扩展方法
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        public async static Task<IHttpResults> Request(this ApiModel api)
        {
            if (api.method == RestSharp.Method.GET)
            {
                return await HttpHelper.Get(api.url, api.headers, api.cookies);
            }
            else
            {
                return await HttpHelper.Post(api.url, api.body, api.headers, api.cookies);
            }
        }

        /// <summary>
        /// 默认一些请求头
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, string> GetDefaultHeaders()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("user-agent", "Mozilla/5.0 BiliDroid/5.34.1 (bbcallen@gmail.com)");
            headers.Add("Referer", "https://www.bilibili.com/");
            return headers;
        }
        /// <summary>
        /// 将时间戳转为时间
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static DateTime TimestampToDatetime(long ts)
        {
            DateTime dtStart = new DateTime(1970, 1, 1,8,0,0);
            long lTime = long.Parse(ts + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        /// <summary>
        /// 生成时间戳/秒
        /// </summary>
        /// <returns></returns>
        public static long GetTimestampS()
        {
            return Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0)).TotalSeconds);
        }
        /// <summary>
        /// 生成时间戳/豪秒
        /// </summary>
        /// <returns></returns>
        public static long GetTimestampMS()
        {
            return Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0)).TotalMilliseconds);
        }

        public static void ShowMessageToast(string message)
        {
            MessageToast ms = new MessageToast(message, TimeSpan.FromSeconds(2));
            ms.Show();
        }
        public static void ShowMessageToast(string message,List<MyUICommand> commands, int seconds = 15)
        {
            MessageToast ms = new MessageToast(message, TimeSpan.FromSeconds(seconds), commands);
            ms.Show();
        }

        public async static Task<bool> ShowLoginDialog()
        {
            LoginDialog login = new LoginDialog();
            await login.ShowAsync();
            if (UserHelper.isLogin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
