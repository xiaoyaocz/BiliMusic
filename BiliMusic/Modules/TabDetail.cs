using BiliMusic.Models;
using BiliMusic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BiliMusic.Modules
{
    public class TabDetail 
    {
        public int tab_id { get; set; }
        public TabDetailDataModel Datas { get; set; }
        public TabDetail(int id)
        {
            tab_id = id;
        }
    
        public async Task LoadData()
        {
            try
            {
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
                ls.InsertRange(ls.Count, d);
                data.data.modules =ls;
                Datas = data.data;
            }
            catch (Exception ex)
            {
                Utils.ShowMessageToast("读取首页信息失败");
                //TODO 保存错误信息
            }
           
        }

    }
}
