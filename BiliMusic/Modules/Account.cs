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
using BiliMusic.Helpers;

namespace BiliMusic.Modules
{
    public class Account
    {

        private async Task<string> EncryptedPassword(string passWord)
        {
            string base64String;
            try
            {
                HttpBaseProtocolFilter httpBaseProtocolFilter = new HttpBaseProtocolFilter();
                httpBaseProtocolFilter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Expired);
                httpBaseProtocolFilter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Untrusted);
                var jObjects = (await Api.GetKey().Request()).GetJObject();
                string str = jObjects["data"]["hash"].ToString();
                string str1 = jObjects["data"]["key"].ToString();
                string str2 = string.Concat(str, passWord);
                string str3 = Regex.Match(str1, "BEGIN PUBLIC KEY-----(?<key>[\\s\\S]+)-----END PUBLIC KEY").Groups["key"].Value.Trim();
                byte[] numArray = Convert.FromBase64String(str3);
                AsymmetricKeyAlgorithmProvider asymmetricKeyAlgorithmProvider = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithmNames.RsaPkcs1);
                CryptographicKey cryptographicKey = asymmetricKeyAlgorithmProvider.ImportPublicKey(WindowsRuntimeBufferExtensions.AsBuffer(numArray), 0);
                IBuffer buffer = CryptographicEngine.Encrypt(cryptographicKey, WindowsRuntimeBufferExtensions.AsBuffer(Encoding.UTF8.GetBytes(str2)), null);
                base64String = Convert.ToBase64String(WindowsRuntimeBufferExtensions.ToArray(buffer));
            }
            catch (Exception)
            {
                base64String = passWord;
            }
            return base64String;
        }

        /// <summary>
        /// 登录V2
        /// </summary>
        /// <returns></returns>
        public async Task<LoginCallbackModel> LoginV2(string username, string password, string captcha = "")
        {
            try
            {

                var results = await Api.LoginV2(username,await EncryptedPassword(password), captcha).Request();
                var m = results.GetJson<LoginV2Model>();
                if (m.code == 0)
                {
                    m.data.expires_datetime = Utils.TimestampToDatetime(m.ts).AddSeconds(m.data.expires_in);
                    //设置登录状态
                    UserHelper.isLogin = true;
                    UserHelper.access_key = m.data.access_token;
                    UserHelper.mid = m.data.mid;
                    //保持登录信息

                    SettingHelper.StorageHelper.Save(SettingHelper.LoginInfo, m.data);
                    //执行SSO
                    await Api.SSO(m.data.access_token).Request();
                    //发送登录成功事件
                    MessageCenter.SendLogined(m.data);

                    return new LoginCallbackModel()
                    {
                        status = LoginStatus.Success,
                        message = "登录成功"
                    };
                }
                else if (m.code == -2100)
                {
                    return new LoginCallbackModel()
                    {
                        status = LoginStatus.NeedValidate,
                        url = m.url,
                        message = "登录需要验证"
                    };
                }
                else if (m.code == -105)
                {
                    return new LoginCallbackModel()
                    {
                        status = LoginStatus.NeedCaptcha,
                        message = "登录需要验证码"
                    };
                }
                else
                {
                    return new LoginCallbackModel()
                    {
                        status = LoginStatus.Fail,
                        message = m.message
                    };
                }
            }
            catch (Exception ex)
            {
                return new LoginCallbackModel()
                {
                    status = LoginStatus.Error,
                    message = "登录出现小问题,请重试"
                };
            }
        }

        /// <summary>
        /// 安全验证后保存状态
        /// </summary>
        /// <param name="access_key"></param>
        /// <param name="refresh_token"></param>
        /// <param name="expires"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<bool> SaveLogin(string access_key, string refresh_token, int expires, long userid)
        {
            try
            {
                //设置登录状态
                UserHelper.isLogin = true;
                UserHelper.access_key = access_key;
                UserHelper.mid = userid;
                var data = new LoginDataV2Model()
                {
                    access_token= access_key,
                    expires_datetime=DateTime.Now.AddSeconds(expires),
                    expires_in= expires,
                    mid= userid,
                    refresh_token= refresh_token
                };
                //保持登录信息
                SettingHelper.StorageHelper.Save(SettingHelper.LoginInfo, data);
                //执行SSO
                await Api.SSO(access_key).Request();
                //}
                MessageCenter.SendLogined(data);
                return true;

            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
