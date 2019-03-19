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
using BiliMusic.SqliteModel;

namespace BiliMusic.Modules
{
    public enum SearchMode
    {
        /// <summary>
        /// 单曲
        /// </summary>
        music = 0,
        /// <summary>
        /// 歌单
        /// </summary>
        menus = 1,
        /// <summary>
        /// 音乐人
        /// </summary>
        musician = 2,
        /// <summary>
        /// 音乐视频，计划
        /// </summary>
        video = 3
    }
    public class Search : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private SearchModel<SearchSongResultModel> _searchSongs;
        public SearchModel<SearchSongResultModel> searchSongs
        {
            get { return _searchSongs; }
            set { _searchSongs = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("searchSongs")); }
        }
        private SearchModel<SearchMenuResultModel> _searchMenus;
        public SearchModel<SearchMenuResultModel> searchMenus
        {
            get { return _searchMenus; }
            set { _searchMenus = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("searchMenus")); }
        }
        private SearchModel<SearchAuthorResultModel> _searchAuthors;
        public SearchModel<SearchAuthorResultModel> searchAuthors
        {
            get { return _searchAuthors; }
            set { _searchAuthors = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("searchAuthors")); }
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
        private List<string> _hotWords;
        public List<string> hotWords
        {
            get { return _hotWords; }
            set { _hotWords = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("hotWords")); }
        }
        public string keyword { get; set; }
        public ObservableCollection<string> historyWords { get; set; }
        public Search()
        {
            historyWords = new ObservableCollection<string>();

            LoadHistory();
            LoadHotWords();
        }

        public void DoSearch(string _keyword)
        {
            keyword = _keyword;
            searchSongs = null;
            searchMenus = null;
            searchAuthors = null;
            SaveHistory(keyword);
            SearchSongs();
            SearchMenus();
            SearchMusician();
        }
        public async void SearchSongs()
        {
            var page = 1;
            var maxPage = 1;
            if (searchSongs != null)
            {
                page = searchSongs.page+1;
                maxPage = searchSongs.num_pages;
            }
            if (page> maxPage)
            {
                Utils.ShowMessageToast("已经加载完了...");
                return;
            }
            try
            {
                loading = true;
                IHttpResults re = await Api.Search(SearchMode.music.ToString(),keyword,page).Request();

                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<SearchModel<SearchSongResultModel>>>();
                if (data.code != 0||data.data.code!="0")
                {
                    Utils.ShowMessageToast("搜索单曲失败");
                    return;
                }
                if (searchSongs==null)
                {
                    searchSongs = data.data;
                }
                else
                {
                    searchSongs.page = data.data.page;
                    foreach (var item in data.data.result)
                    {
                        searchSongs.result.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowMessageToast("搜索单曲失败");
                LogHelper.Log($"搜索单曲失败，关键词：{keyword}", LogType.ERROR, ex);
            }
            finally
            {
                loading = false;
            }

        }
        public async void SearchMenus()
        {
            var page = 1;
            var maxPage = 1;
            if (searchMenus != null)
            {
                page = searchMenus.page + 1;
                maxPage = searchMenus.num_pages;
            }
            if (page > maxPage)
            {
                Utils.ShowMessageToast("已经加载完了...");
                return;
            }
            try
            {
                loading = true;
                IHttpResults re = await Api.Search(SearchMode.menus.ToString(), keyword, page).Request();

                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<SearchModel<SearchMenuResultModel>>>();
                if (data.code != 0 || data.data.code != "0")
                {
                    Utils.ShowMessageToast("搜索单曲失败");
                    return;
                }
                if (searchMenus == null)
                {
                    searchMenus = data.data;
                }
                else
                {
                    searchMenus.page = data.data.page;
                    foreach (var item in data.data.result)
                    {
                        searchMenus.result.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowMessageToast("搜索歌单失败");
                LogHelper.Log($"搜索歌单失败，关键词：{keyword}", LogType.ERROR, ex);
            }
            finally
            {
                loading = false;
            }

        }
        public async void SearchMusician()
        {
            var page = 1;
            var maxPage = 1;
            if (searchAuthors != null)
            {
                page = searchAuthors.page + 1;
                maxPage = searchAuthors.num_pages;
            }
            if (page > maxPage)
            {
                Utils.ShowMessageToast("已经加载完了...");
                return;
            }
            try
            {
                loading = true;
                IHttpResults re = await Api.Search(SearchMode.musician.ToString(), keyword, page).Request();

                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<SearchModel<SearchAuthorResultModel>>>();
                if (data.code != 0 || data.data.code != "0")
                {
                    Utils.ShowMessageToast("搜索单曲失败");
                    return;
                }
                if (searchAuthors == null)
                {
                    searchAuthors = data.data;
                }
                else
                {
                    searchAuthors.page = data.data.page;
                    foreach (var item in data.data.result)
                    {
                        searchAuthors.result.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.ShowMessageToast("搜索歌单失败");
                LogHelper.Log($"搜索歌单失败，关键词：{keyword}", LogType.ERROR, ex);
            }
            finally
            {
                loading = false;
            }

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

        public void LoadHistory()
        {
            historyWords.Clear();
            using (BiliMusicContext db = new BiliMusicContext())
            {
                foreach (var item in db.SearchHistorys.OrderByDescending(x => x.datetime))
                {
                    historyWords.Add(item.content);
                }
            }
        }
        private void SaveHistory(string keyword)
        {
            using (BiliMusicContext db = new BiliMusicContext())
            {
                var search = db.SearchHistorys.FirstOrDefault(x => x.content.Equals(keyword));
                if (search != null)
                {
                    search.datetime = DateTime.Now;
                }
                else
                {
                    db.SearchHistorys.Add(new SearchHistory()
                    {
                        content = keyword,
                        datetime = DateTime.Now
                    });
                }
                db.SaveChangesAsync();
            }

        }

        public void ClearHistory()
        {
            historyWords.Clear();
            using (BiliMusicContext db = new BiliMusicContext())
            {
                db.SearchHistorys.RemoveRange(db.SearchHistorys);
                db.SaveChangesAsync();
            }
        }
    }
}
