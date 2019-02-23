using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.Web.Http.Filters;
using Microsoft.Toolkit.Uwp.Helpers;
using BiliMusic.Models;

namespace BiliMusic.Helpers
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public static class UserHelper
    {
        public static bool isLogin { get; set; } = false;
        public static string access_key { get; set; }
        public static long mid { get; set; }

    }
}
