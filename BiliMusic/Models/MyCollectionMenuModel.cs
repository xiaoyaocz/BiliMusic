using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliMusic.Models
{
    public class MyCollectionMenuModel
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
        public List<MyCollectionMenuItemModel> list { get; set; }
    }
    public class MyCollectionMenuItemModel
    {
     
        public int mid { get; set; }
        public string title { get; set; }
        public string intro { get; set; }
        public string coverUrl { get; set; }
        public int menuId { get; set; }
        public string uname { get; set; }
        public string face { get; set; }

    }
}