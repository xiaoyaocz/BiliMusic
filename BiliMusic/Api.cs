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
        /// 读取我收藏的专辑
        /// </summary>
        /// <returns></returns>
        public static ApiModel MyAlbum(int page = 1, int pagesize = 100)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/users/{UserHelper.mid}/menus",
                parameter = ApiHelper.MustParameter(true) + $"&page_index={page}&page_size={pagesize}&type=2",
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
        /// 编辑推荐详细
        /// </summary>
        /// <returns></returns>
        public static ApiModel RecommendDetail(int id)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/firstpage/hit-songs/{id}",
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

        /// <summary>
        /// 收藏歌单
        /// </summary>
        /// <param name="menuid">歌单ID</param>
        /// <returns></returns>
        public static ApiModel MenuCollect(int menuid)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/menucollect/add",
                parameter = ApiHelper.MustParameter(true) + $"&menuId={menuid}&mid={UserHelper.mid}",
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }

        /// <summary>
        /// 取消收藏歌单
        /// </summary>
        /// <param name="menuid">歌单ID</param>
        /// <returns></returns>
        public static ApiModel CancelMenuCollect(int menuid)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/menucollect/del",
                parameter = ApiHelper.MustParameter(true) + $"&menuId={menuid}&mid={UserHelper.mid}",
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }

        /// <summary>
        /// 删除歌单
        /// </summary>
        /// <param name="collectionId">歌单ID</param>
        /// <returns></returns>
        public static ApiModel DeleteSongMenu(int collectionId)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/collections/batchDeleteMenus",
                parameter = ApiHelper.MustParameter(true) + $"&collectionIds={collectionId}&mid={UserHelper.mid}",
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }

        /// <summary>
        /// 收藏歌曲
        /// </summary>
        /// <param name="songId">歌曲ID</param>
        /// <param name="menuIds">歌单ID</param>
        /// <returns></returns>
        public static ApiModel CollectSong(int songId,IList<int> menuIds)
        {
            StringBuilder menuId = new StringBuilder();
            foreach (var item in menuIds)
            {
                menuId.Append( item.ToString() + ",");
            }
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.POST,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/collections/songs/{songId}",
                body = ApiHelper.MustParameter(true) +$"&collection_id_list={Uri.EscapeDataString(menuId.ToString().TrimEnd(','))}&mid={UserHelper.mid}&song_id={songId}"
            };
            api.body += ApiHelper.GetSign(api.body);
            return api;
        }
        /// <summary>
        /// 批量收藏歌曲
        /// </summary>
        /// <param name="songIds">歌曲ID</param>
        /// <param name="menuIds">歌单ID</param>
        /// <returns></returns>
        public static ApiModel CollectSong(IList<int> songIds, IList<int> menuIds)
        {
            StringBuilder menuId = new StringBuilder();
            foreach (var item in menuIds)
            {
                menuId.Append(item.ToString() + ",");
            }
            StringBuilder songId = new StringBuilder();
            foreach (var item in songIds)
            {
                songId.Append(item.ToString() + ",");
            }
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.POST,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/collections/songs/batch",
                body = ApiHelper.MustParameter(true) + $"&collection_ids={Uri.EscapeDataString(menuId.ToString().TrimEnd(','))}&mid={UserHelper.mid}&song_ids={Uri.EscapeDataString(songId.ToString().TrimEnd(','))}"
            };
            api.body += ApiHelper.GetSign(api.body);
            return api;
        }

        /// <summary>
        /// 创建歌曲
        /// </summary>
        /// <param name="songId">歌曲ID</param>
        /// <param name="menuId">歌单ID</param>
        /// <returns></returns>
        public static ApiModel CreateCollection(string title,string desc,bool isOpen)
        {
            
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.POST,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/collections",
                body = ApiHelper.MustParameter(true) + $"&title={Uri.EscapeDataString(title)}&desc={Uri.EscapeDataString(desc)}&is_open={ (isOpen?"1":"0") }&mid={UserHelper.mid}"
            };
            api.body += ApiHelper.GetSign(api.body);
            return api;
        }


        /// <summary>
        /// 搜索
        /// 该API使用需要注意，data.code是否为0
        /// </summary>
        /// <param name="mode">搜索类型，可选music，menus，musician</param>
        /// <param name="keyword">关键字</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public static ApiModel Search(string mode,string keyword,int page,int pageSize=20)
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/s",
                parameter = ApiHelper.MustParameter(false) + $"&keyword={Uri.EscapeDataString(keyword)}&page={page}&pagesize={pageSize}&search_type={mode}",
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }
        /// <summary>
        /// 搜索热词
        /// </summary>
        /// <returns></returns>
        public static ApiModel SearchHotWords()
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/s/hit-words",
                parameter = ApiHelper.MustParameter(false),
                headers = Utils.GetDefaultHeaders()
            };
            api.parameter += ApiHelper.GetSign(api.parameter);
            return api;
        }

        /// <summary>
        /// 榜单
        /// </summary>
        /// <returns></returns>
        public static ApiModel Ranks()
        {
            ApiModel api = new ApiModel()
            {
                method = RestSharp.Method.GET,
                baseUrl = $"https://api.bilibili.com/audio/music-service-c/menus/rank",
                parameter = ApiHelper.MustParameter(false),
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
