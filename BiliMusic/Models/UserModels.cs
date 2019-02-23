using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliMusic.Models
{
    public enum LoginStatus
    {
        /// <summary>
        /// 登录成功
        /// </summary>
        Success,
        /// <summary>
        /// 登录失败
        /// </summary>
        Fail,
        /// <summary>
        /// 登录错误
        /// </summary>
        Error,
        /// <summary>
        /// 登录需要验证码
        /// </summary>
        NeedCaptcha,
        /// <summary>
        /// 需要安全认证
        /// </summary>
        NeedValidate
    }
    /// <summary>
    /// V2的登录
    /// </summary>
    public class LoginV2Model
    {
       
        public long ts { get; set; }
        public int code { get; set; }
        public LoginDataV2Model data { get; set; }
        /// <summary>
        /// 当错误代码为2100会返回一个链接验证
        /// </summary>
        public string url { get; set; }
        public string message { get; set; }
    }
    public class LoginDataV2Model
    {
        
        public long mid { get; set; }
       
        public string access_token { get; set; }
       
        public string refresh_token { get; set; }
       
        public int expires_in { get; set; }

        public DateTime expires_datetime { get; set; }
    }

    public class LoginCallbackModel
    {
        public LoginStatus status { get; set; }
        public string message { get; set; }
        public string url { get; set; }
    }


}
