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
       /// 播放清晰度清晰度
       /// </summary>
        public const string DefaultQualities = "DefaultQualities";

        public static LocalObjectStorageHelper StorageHelper = new LocalObjectStorageHelper();
        
    }
}
