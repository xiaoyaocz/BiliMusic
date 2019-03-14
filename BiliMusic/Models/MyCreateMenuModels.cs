using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliMusic.Models
{
    public class MyCreateMenuModel
    {
        public int pageNum { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public int size { get; set; }
        public int pages { get; set; }
        public bool isFirstPage { get; set; }
        public bool isLastPage { get; set; }
        public bool hasPreviousPage { get; set; }
        public bool hasNextPage { get; set; }


        public List<MyCreateMenuItemModel> list { get; set; }
    }
    public class MyCreateMenuItemModel
    {
        public int id { get; set; }
        public int mid { get; set; }
        public string title { get; set; }
        public int collection_id { get; set; }
        public string img_url { get; set; }
        public int menu_id { get; set; }
        public int is_default { get; set; }
        public int is_open { get; set; }
        public string status
        {
            get
            {
                if (is_open==1)
                {
                    return "公开";
                }
                else
                {
                    return "私密";
                }
            }
        }

        public int records_num { get; set; }
        public List<int> songsid_list { get; set; }

    }
}
