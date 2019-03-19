using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliMusic.Models
{
    public class RankModel
    {
        public int menuId { get; set; }
        public string title { get; set; }
        public string coverUrl { get; set; }
        public string intro { get; set; }
        public int type { get; set; }
        public List<AudiosModel> audios { get; set; }
    }
    public class AudiosModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
    }


}
