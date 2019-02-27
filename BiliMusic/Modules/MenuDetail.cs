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
        public int menuid { get; set; }
        public MenuDetail(int id)
        {
            menuid = id;
        }

        public async void LoadData()
        {
            try
            {
                loading = true;
                var re = await Api.SonglistDetail(menuid).Request();
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

                data.data.menusTags =await LoadTags();
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

        public async Task<List<menusTagModel>> LoadTags()
        {
            try
            {
                loading = true;
                var re = await Api.SonglistTag(menuid).Request();
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
