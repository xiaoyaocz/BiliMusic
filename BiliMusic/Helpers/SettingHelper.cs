using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliMusic.Helpers
{
    public class SettingHelper
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

        public const string OriginalImage = "OriginalImage";


        public static bool LoadOriginalImage { get; set; } = false;
        public static LocalObjectStorageHelper StorageHelper = new LocalObjectStorageHelper();
        
    }
}
