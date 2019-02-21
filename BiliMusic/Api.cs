using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiliMusic.Helpers;

namespace BiliMusic
{

    public static class Api
    {
        /// <summary>
        /// 读取登录密码加密信息
        /// </summary>
        /// <returns></returns>
        public static ApiModel GetKey()
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.POST,
                baseUrl = "https://passport.bilibili.com/api/oauth2/getKey",
                body = ApiHelper.MustParameter()
            };
            api.body += ApiHelper.GetSign(api.body);
            return api;
        }
        /// <summary>
        /// 登录API V2
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="captcha">验证码</param>
        /// <returns></returns>
        public static ApiModel LoginV2(string username, string password, string captcha = "")
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.POST,
                baseUrl = "https://passport.bilibili.com/api/oauth2/login",
                body = $"username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}{((captcha == "") ? "" : "&captcha=" + captcha)}&" + ApiHelper.MustParameter()
            };
            api.body += ApiHelper.GetSign(api.body);
            return api;
        }
        /// <summary>
        /// 登录API V3
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="gee_type"></param>
        /// <returns></returns>
        public static ApiModel LoginV3(string username, string password, int gee_type = 10)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.POST,
                baseUrl = "https://passport.bilibili.com/api/v3/oauth2/login",
                body = $"username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}&gee_type={gee_type}&" + ApiHelper.MustParameter()
            };
            api.body += ApiHelper.GetSign(api.body);
            return api;
        }
        /// <summary>
        /// 读取个人信息
        /// </summary>
        /// <returns></returns>
        public static ApiModel GetMyInfo(string mid)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/users/{mid}",
                parameter = ApiHelper.MustParameter(true),
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }
        /// <summary>
        /// 读取首页Tab
        /// </summary>
        /// <returns></returns>
        public static ApiModel Tab()
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = "https://api.bilibili.com/audio/music-service-c/firstpage/tab",
                parameter = ApiHelper.MustParameter(),
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }
        /// <summary>
        /// 读取Tab详情
        /// </summary>
        /// <param name="id">Tab ID</param>
        /// <returns></returns>
        public static ApiModel TabDetail(int id = 0)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = "https://api.bilibili.com/audio/music-service-c/firstpage/{id}",
                parameter = ApiHelper.MustParameter(true),
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }



    }

    public class ApiModel
    {
        /// <summary>
        /// 请求方法
        /// </summary>
        public RestSharp.Method method { get; set; }
        /// <summary>
        /// API地址
        /// </summary>
        public string baseUrl { get; set; }
        /// <summary>
        /// Url参数
        /// </summary>
        public string parameter { get; set; }
        /// <summary>
        /// 发送内容体，用于POST方法
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 请求头
        /// </summary>
        public IDictionary<string, string> headers { get; set; }
        /// <summary>
        /// 请求cookie
        /// </summary>
        public IDictionary<string, string> cookies { get; set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string url
        {
            get
            {
                return baseUrl + "?" + parameter;
            }
        }
    }
}
