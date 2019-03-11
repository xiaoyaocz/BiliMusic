using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BiliMusic.Models.Main;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.ComponentModel;
using BiliMusic.Helpers;
using BiliMusic.Models;

namespace BiliMusic.Modules
{
    public class MenuDetail : INotifyPropertyChanged
    {
       
        private SonglistDetailModel _Datas;

        public event PropertyChangedEventHandler PropertyChanged;

        public SonglistDetailModel Datas
        {
            get { return _Datas; }
            set
            {
                _Datas = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Datas"));
                }
            }
        }
        private bool _loading = true;
        public bool loading
        {
            get { return _loading; }
            set
            {
                _loading = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("loading"));
                }
            }
        }
        /// <summary>
        /// 歌单ID
        /// </summary>
        public int menuId { get; set; }
        /// <summary>
        /// 编辑推荐ID
        /// </summary>
        public int recommedId { get; set; }
        public MenuDetail(int id,int recommed_id = 0)
        {
            menuId = id;
            recommedId = recommed_id;
        }
        /// <summary>
        /// 加载歌单信息
        /// </summary>
        public async void LoadData()
        {
            try
            {
                loading = true;
                IHttpResults re;
                if (recommedId == 0)
                {
                    re = await Api.SonglistDetail(menuId).Request();
                }
                else
                {
                    re = await Api.RecommendDetail(recommedId).Request();
                }
                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<SonglistDetailModel>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return;
                }
                if (recommedId == 0)
                {
                    data.data.menusTags = await LoadTags();
                }
              
                Datas = data.data;
               
            }
            catch (Exception ex)
            {
                
                Utils.ShowMessageToast("读取歌单信息失败");
                LogHelper.Log("读取歌单信息失败", LogType.ERROR, ex);
            }
            finally
            {
                loading = false;
            }

        }

        /// <summary>
        /// 收藏歌单
        /// </summary>
        /// <param name="menuId"></param>
        public async void CollectMenu()
        {
            try
            {
                loading = true;
                IHttpResults re= await Api.MenuCollect(menuId).Request();
                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<object>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return;
                }
                Datas.menusRespones.collected = 1;
                Utils.ShowMessageToast("收藏成功");
                MessageCenter.GetMainInfo().Logined();
            }
            catch (Exception ex)
            {

                Utils.ShowMessageToast("读取歌单信息失败");
                LogHelper.Log("读取歌单信息失败", LogType.ERROR, ex);
            }
            finally
            {
                loading = false;
            }
        }
        
        /// <summary>
        /// 收藏歌单
        /// </summary>
        /// <param name="menuId"></param>
        public async void CancelCollectMenu()
        {
            try
            {
                loading = true;
                IHttpResults re = await Api.CancelMenuCollect(menuId).Request();
                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<object>>();
                
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return;
                }
                Datas.menusRespones.collected = 0;
                Utils.ShowMessageToast("已经取消收藏");
                MessageCenter.GetMainInfo().Logined();
            }
            catch (Exception ex)
            {

                Utils.ShowMessageToast("读取歌单信息失败");
                LogHelper.Log("读取歌单信息失败", LogType.ERROR, ex);
            }
            finally
            {
                loading = false;
            }
        }


        /// <summary>
        /// 加载歌单TAG信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<menusTagModel>> LoadTags()
        {
            try
            {
                if (menuId==0)
                {
                    return null;
                }
                loading = true;

                var re = await Api.SonglistTag(menuId).Request();
                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return null;
                }
                var data = re.GetJson<ApiParseModel<List<menusTagModel>>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return null;
                }


                return data.data;

            }
            catch (Exception ex)
            {
                Utils.ShowMessageToast("读取歌单TAG失败");
                LogHelper.Log("读取歌单TAG失败", LogType.ERROR, ex);
                return null;
            }
           
        }
    }
}
