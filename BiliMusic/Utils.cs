using BiliMusic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
