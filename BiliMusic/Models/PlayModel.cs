using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliMusic.Models
{
    public class PlayModel 
    {
        public SongsDetailModel songinfo { get; set; }
        public int songid { get; set; }
        public string title { get; set; }
        public string pic { get; set; }
        public string author { get; set; }
        public string lyric_url { get; set; }
        public Uri play_url { get; set; }
        public List<string> backup_url { get; set; }
        public bool is_preview { get; set; }
        public bool is_collect { get; set; }

        public List<QualitiesModel> qualities { get; set; }

        public string tag
        {
            get
            {
                if (is_preview)
                {
                    return "[试听]";
                }
                else
                {
                    return "";
                }
            }
        }

    }
}
