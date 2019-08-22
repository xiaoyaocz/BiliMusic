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
    public class SongDetail : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int songId { get; set; }
        public SongDetail(int songid)
        {
            songId = songid;
        }
        private SongsDetailModel _detail;

        public SongsDetailModel detail
        {
            get { return _detail; }
            set { _detail = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("detail")); }
        }
        private bool _loading = false;
        public bool loading
        {
            get { return _loading; }
            set
            {
                _loading = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("loading"));
            }
        }


        public async void LoadMusicDeatil()
        {
            try
            {
                loading = true;
                var re = await Api.SongDetail(songId).Request();
                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<SongsDetailModel>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return;
                }
                detail = data.data;
            }
            catch (Exception ex)
            {

                Utils.ShowMessageToast("读取歌曲信息失败");

                LogHelper.Log("读取歌曲信息失败" + songId, LogType.ERROR, ex);
            }
            finally
            {
                loading = false;
            }

        }

    }
}
