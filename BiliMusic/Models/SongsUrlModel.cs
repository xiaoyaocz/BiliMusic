using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliMusic.Models
{
    public class SongsUrlModel
    {

        public int sid { get; set; }
        /// <summary>
        /// -1是试听，提示开通会员
        /// </summary>
        public int type { get; set; }
        public string info { get; set; }
        public int timeout { get; set; }
        public int size { get; set; }
        public List<string> cdns { get; set; }
        public List<QualitiesModel> qualities { get; set; }
        public string title { get; set; }
        public string cover { get; set; }
    }
}
