using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using BiliMusic.Models;
using System.IO;

namespace BiliMusic.Helpers
{
    /// <summary>
    /// 网络请求方法封装
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// 发送一个GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public async static Task<IHttpResults> Get(string url,IDictionary<string,string> headers=null, IDictionary<string, string> cookie = null)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest(Method.GET);
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        request.AddHeader(item.Key, item.Value);
                    }
                }
                if (cookie != null)
                {
                    foreach (var item in cookie)
                    {
                        request.AddCookie(item.Key, item.Value);
                    }
                }
                IRestResponse response =await client.ExecuteTaskAsync(request);
                IHttpResults httpResults = new HttpResults()
                {
                    code = (int)response.StatusCode,
                    status = (response.StatusCode == System.Net.HttpStatusCode.OK),
                    results = response.Content,
                    message = StatusCodeToMessage((int)response.StatusCode)
                };
                return httpResults;

            }
            catch (Exception ex)
            {
                return new HttpResults()
                {
                    code = ex.HResult,
                    status = false,
                    message = StatusCodeToMessage(ex.HResult)
                };
            }
               

            
        }

        /// <summary>
        /// 发送一个GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public async static Task<MemoryStream> GetStream(string url, IDictionary<string, string> headers = null, IDictionary<string, string> cookie = null)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest(Method.GET);
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        request.AddHeader(item.Key, item.Value);
                    }
                }
                if (cookie != null)
                {
                    foreach (var item in cookie)
                    {
                        request.AddCookie(item.Key, item.Value);
                    }
                }
                IRestResponse response = await client.ExecuteTaskAsync(request);

                return new MemoryStream(response.RawBytes);

            }
            catch (Exception ex)
            {
                return null;
            }



        }

        /// <summary>
        /// 发送一个POST请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="headers"></param>
        /// <param name="cookie"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async static Task<IHttpResults> Post(string url, string body,IDictionary<string, string> headers = null, IDictionary<string, string> cookie = null,string contentType= "application/x-www-form-urlencoded")
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        request.AddHeader(item.Key, item.Value);
                    }
                }
                if (cookie != null)
                {
                    foreach (var item in cookie)
                    {
                        request.AddCookie(item.Key, item.Value);
                    }
                }
                request.AddParameter(contentType, body, ParameterType.RequestBody);
                IRestResponse response =await client.ExecuteTaskAsync(request);
                IHttpResults httpResults = new HttpResults()
                {
                    code = (int)response.StatusCode,
                    status = (response.StatusCode == System.Net.HttpStatusCode.OK),
                    results = response.Content,
                    message = StatusCodeToMessage((int)response.StatusCode)
                };
                return httpResults;

            }
            catch (Exception ex)
            {
                return new HttpResults()
                {
                    code = ex.HResult,
                    status = false,
                    message = StatusCodeToMessage(ex.HResult)
                };
            }
        }
        
      


        private static string StatusCodeToMessage(int code)
        {

            switch (code)
            {
                case 0:
                case 200:
                    return "请求成功";
                case 504:
                    return "请求超时了";
                case 301:
                case 302:
                case 303:
                case 305:
                case 306:
                case 400:
                case 401:
                case 402:
                case 403:
                case 404:
                case 500:
                case 501:
                case 502:
                case 503:
                case 505:
                    return "请求失败了，代码:"+code;
                case -2147012867:
                case -2147012889:
                    return "请检查的网络连接";
                default:
                    return "未知错误";
            }
        }
    }

    public interface IHttpResults
    {
        bool status { get; set; }
        int code { get; set; }
        string message { get; set; }
        string results { get; set; }
        JObject GetJObject();
        T GetJson<T>();
    }
    public class HttpResults: IHttpResults
    {
        public int code { get; set; }
        public string message { get; set; }
        public string results { get; set; }
        public bool status { get; set; }
        public T GetJson<T>()
        {
            return JsonConvert.DeserializeObject<T>(results);
        }
        public JObject GetJObject()
        {
            try
            {
                var obj = JObject.Parse(results);
                return obj;
            }
            catch (Exception)
            {
                return null;
            }
           
        }

       
    }
   
}
