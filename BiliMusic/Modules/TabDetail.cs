using BiliMusic.Models;
using BiliMusic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace BiliMusic.Modules
{
    public class TabDetail : INotifyPropertyChanged
    {
       
        public int tab_id { get; set; }
        private TabDetailDataModel _Datas;
        public TabDetailDataModel Datas {
            get { return _Datas; }
            set
            {
                _Datas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Datas"));
            }
        }

        private ObservableCollection<bannersModel> _banners;
        public ObservableCollection<bannersModel> banners
        {
            get { return _banners; }
            set { _banners = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("banners")); }
        }

        private ObservableCollection<modulesModel> _modules;
        public ObservableCollection<modulesModel> modules
        {
            get { return _modules; }
            set { _modules = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("modules")); }
        }

        //public ObservableCollection<modulesModel> modules { get; set; }


        private bool _loading=true;
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
       
        public TabDetail(int id)
        {
            tab_id = id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async void LoadData()
        {
            try
            {
                loading = true;
                   var re = await Api.TabDetail(tab_id).Request();
                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<TabDetailDataModel>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return;
                }
                var ls = data.data.modules.Where(x => x.type == 2 || x.type == 3 || x.type == 4).ToList();
                var d = ls.Where(x => x.type == 3 && x.dataSize == 3).ToList();
                ls.RemoveAll(x => x.type == 3 && x.dataSize == 3);

                //布局用得是Grid，设置一下行列
                int row = 0;
                //数据超过了4列默认占一行
                foreach (var item in ls)
                {
                    item.row = row;
                    item.column = 0;
                    item.narrow_row = row;
                    row++;
                }
                //数据只有3列默认两组占一行
                var i = 0;
                var c_row = row;
                foreach (var item in d)
                {
                    i++;
                    item.row = c_row;
                    item.narrow_row = row;
                    item.column = i - 1;
                    if (i%2==0)
                    {
                        c_row++;
                        i = 0;
                    }
                    row++;
                }
                ls.InsertRange(ls.Count, d);
                ObservableCollection<modulesModel> _modules = new ObservableCollection<modulesModel>();
                foreach (var item in ls)
                {
                    _modules.Add(item);
                }
                //data.data.modules =ls;
                modules = _modules;
                banners = data.data.banners;
                //Datas = data.data;
            }
            catch (Exception ex)
            {
                Utils.ShowMessageToast("读取首页信息失败");
                LogHelper.Log("读取首页信息失败TABID："+ tab_id, LogType.ERROR, ex);
            }
            finally
            {
                loading = false;
            }
        }


        public async void RefreshModule(modulesModel item)
        {
            try
            {
                loading = true;
                var re = await Api.RefreshModule(item.id,item.time).Request();
                if (!re.status)
                {
                    Utils.ShowMessageToast(re.message);
                    return;
                }
                var data = re.GetJson<ApiParseModel<modulesModel>>();
                if (data.code != 0)
                {
                    Utils.ShowMessageToast(data.msg + data.message);
                    return;
                }
                data.data.time = item.time + 1;
                data.data.column = item.column;
                data.data.row = item.row;
                data.data.narrow_row = item.narrow_row;
                modules[modules.IndexOf(item)] = data.data;
            }
            catch (Exception ex)
            {
                Utils.ShowMessageToast("刷新信息失败");
                LogHelper.Log("刷新信息失败", LogType.ERROR, ex);
            }
            finally
            {
                loading = false;
            }
        }

    }
}
