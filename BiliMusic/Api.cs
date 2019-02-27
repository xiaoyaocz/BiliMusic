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
        /// SSO
        /// </summary>
        /// <param name="access_key"></param>
        /// <returns></returns>
        public static ApiModel SSO(string access_key)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://passport.bilibili.com/api/login/sso",
                parameter = ApiHelper.MustParameter(false)+ $"&gourl=https%3A%2F%2Faccount.bilibili.com%2Faccount%2Fhome&access_key={access_key}",
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }
        /// <summary>
        /// 读取验证码
        /// </summary>
        /// <returns></returns>
        public static ApiModel Captcha()
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = "https://passport.bilibili.com/captcha",
                headers = Utils.GetDefaultHeaders(),
                parameter=$"ts={Utils.GetTimestampS()}"
            };
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
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/firstpage/{id}",
                parameter = ApiHelper.MustParameter(true),
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }

        /// <summary>
        /// 刷新首页中的模块
        /// </summary>
        /// <param name="id">Module ID</param>
        /// <returns></returns>
        public static ApiModel RefreshModule(int moduleId,int time=1)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/firstpage/shark/{moduleId}",
                parameter = ApiHelper.MustParameter()+"&time="+time,
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }

        /// <summary>
        /// 读取我创建的歌单
        /// </summary>
        /// <returns></returns>
        public static ApiModel MyCreate(int page=1,int pagesize=100)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = "https://api.bilibili.com/audio/music-service-c/collections",
                parameter = ApiHelper.MustParameter(true)+$"&page_index={page}&page_size={pagesize}&mid={UserHelper.mid}",
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }
        /// <summary>
        /// 读取我收藏的歌单
        /// </summary>
        /// <returns></returns>
        public static ApiModel MyCollection(int page=1, int pagesize=100)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/users/{UserHelper.mid}/menus",
                parameter = ApiHelper.MustParameter(true) + $"&page_index={page}&page_size={pagesize}&type=1",
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }


        /// <summary>
        /// 歌单详细
        /// </summary>
        /// <returns></returns>
        public static ApiModel SonglistDetail(int menuid)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/menus/{menuid}",
                parameter = ApiHelper.MustParameter(true),
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }
        /// <summary>
        /// 歌单TAG
        /// </summary>
        /// <returns></returns>
        public static ApiModel SonglistTag(int menuid)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/tags/menus/{menuid}",
                parameter = ApiHelper.MustParameter(),
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }

        /// <summary>
        /// 歌曲详细信息
        /// </summary>
        /// <param name="songid">歌曲ID</param>
        /// <returns></returns>
        public static ApiModel SongDetail(int songid)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/songs/playing",
                parameter = ApiHelper.MustParameter(true)+$"&song_id={songid}"+(UserHelper.isLogin?$"&mid={UserHelper.mid}":""),
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }

        /// <summary>
        /// 歌曲播放地址
        /// </summary>
        /// <param name="songid">歌曲ID</param>
        /// <returns></returns>
        public static ApiModel SongUrl(int songid,int quality)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/url",
                parameter = ApiHelper.MustParameter(true) + $"&songid={songid}&quality={quality}&privilege=2&mid={UserHelper.mid}",
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
