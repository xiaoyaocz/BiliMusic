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
    public class Search : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public SearchModel<SearchSongResultModel> searchSongs { get; set; }
        public SearchModel<SearchMenuResultModel> searchMenus { get; set; }
        public SearchModel<SearchAuthorResultModel> searchAuthors { get; set; }
        
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
        private List<string> _hotWords;
        public List<string> hotWords
        {
            get { return _hotWords; }
            set { _hotWords = value; PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("hotWords")); }
        }

        public ObservableCollection<string> historyWords { get; set; }
        public Search()
        {
            LoadHotWords();
        }
        
        public async void LoadHotWords()
        {
            try
            {
                loading = true;
                IHttpResults re = await Api.SearchHotWords().Request();
                
                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<List<string>>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return;
                }
                hotWords = data.data;
            }
            catch (Exception ex)
            {
                Utils.ShowMessageToast("读取搜索热词失败");
                LogHelper.Log("读取搜索热词失败", LogType.ERROR, ex);
            }
            finally
            {
                loading = false;
            }
        }

    }
}
