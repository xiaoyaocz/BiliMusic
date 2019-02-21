using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliMusic.Helpers
{
    public class ApiHelper
    {
        private const string _mobi_app = "android";
        private const string _appkey = "1d8b6e7d45233436";
        private const string _appSecret = "560c52ccd288fed045859ed18bffd973";
        private const string _build = "5341000";
        private const string _platform = "android";

        public static string GetSign(string url)
        {
            string result;
            string str = url.Substring(url.IndexOf("?", 4) + 1);
            List<string> list = str.Split('&').ToList();
            list.Sort();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string str1 in list)
            {
                stringBuilder.Append((stringBuilder.Length > 0 ? "&" : string.Empty));
                stringBuilder.Append(str1);
            }
            stringBuilder.Append(_appSecret);
            result = SystemHelper.ToMD5(stringBuilder.ToString());
            return "&sign="+result;
        }
        public static string GetSign(IDictionary<string,string> pars)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in pars.OrderBy(x=>x.Key))
            {
                sb.Append(item.Key);
                sb.Append("=");
                sb.Append(item.Value);
                sb.Append("&");
            }
            var results = sb.ToString().TrimEnd('&');
            results = results + _appSecret;
            return "&sign="+SystemHelper.ToMD5(results);
        }

        /// <summary>
        /// 一些必要的参数
        /// </summary>
        /// <param name="needAccesskey">是否需要accesskey</param>
        /// <returns></returns>
        public static string MustParameter(bool needAccesskey = false)
        {
            var url = "";
            if (needAccesskey&&UserHelper.isLogin)
            {
                url = $"access_key={UserHelper.access_key}";
            }
            return url+$"appkey={_appkey}&build={_build}&mobi_app={_mobi_app}&platform={_platform}&ts={GetTimestampS()}";
        }

        public static long GetTimestampS()
        {
             return Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0)).TotalSeconds); 
        }
        public static long GetTimestampMS()
        {
            return Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0)).TotalMilliseconds);
        }
    }
    
}
