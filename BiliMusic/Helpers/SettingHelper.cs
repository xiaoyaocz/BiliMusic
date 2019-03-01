using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliMusic.Helpers
{
    public static class SettingHelper
    {
        /// <summary>
        /// 用户登录信息
        /// </summary>
        public const string LoginInfo = "LoginInfo";
       /// <summary>
       /// 音乐播放清晰度清晰度
       /// </summary>
        public const string DefaultQualities = "DefaultQualities";
        /// <summary>
        /// 音乐下载清晰度清晰度
        /// </summary>
        public const string DefaultDownloadQualities = "DefaultDownloadQualities";
        /// <summary>
        /// 视频播放清晰度清晰度
        /// </summary>
        public const string DefaultVideoQualities = "DefaultVideoQualities";
        /// <summary>
        /// 应用主题，0跟随系统，1浅色，2深色
        /// </summary>
        public const string Theme = "Theme";
        /// <summary>
        /// 加载原图
        /// </summary>
        public const string OriginalImage = "OriginalImage";
        /// <summary>
        /// 音量
        /// </summary>
        public const string Volume = "Volume";
        /// <summary>
        /// 播放模式
        /// </summary>
        public const string PlayMode = "PlayMode";


        public static bool LoadOriginalImage { get; set; } = false;
        public static LocalObjectStorageHelper StorageHelper = new LocalObjectStorageHelper();
        
    }
}
