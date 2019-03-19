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
using Windows.UI.Xaml.Data;
using Windows.UI;

namespace BiliMusic.Modules
{
    public class Ranks : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<RankModel> _ranks;

        public List<RankModel> ranks
        {
            get { return _ranks; }
            set { _ranks = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ranks")); }
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
        public async void LoadRanks()
        {
            try
            {
                loading = true;
                IHttpResults re = await Api.Ranks().Request();

                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<List<RankModel>>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return;
                }
                ranks = data.data;
            }
            catch (Exception ex)
            {
                Utils.ShowMessageToast("加载榜单失败");
                LogHelper.Log("加载榜单失败", LogType.ERROR, ex);
            }
            finally
            {
                loading = false;
            }
        }
    }
}
