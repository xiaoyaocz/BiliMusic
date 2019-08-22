using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliMusic.Models
{
    public class SongsDetailModel
    {
        public string index { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string intro { get; set; }
        public string cover_url { get; set; }
        public string author { get; set; }
        public long? duration { get; set; } = 0;
        public long? up_mid { get; set; }
        public string up_name { get; set; }
        public string up_img { get; set; }
        public int up_cert_type { get; set; }
        public string up_cert_info { get; set; }
        public string lyric_url { get; set; }

        public long? fans { get; set; } = 0;
        public int isFromVideo { get; set; }
        public string avid { get; set; }
        public long? snum { get; set; } = 0;
        public long? reply_count { get; set; } = 0;
        public long? play_count { get; set; } = 0;
        public long? collect_count { get; set; } = 0;
        public long? coin_num { get; set; } = 0;
        /// <summary>
        /// 硬币最多可投数
        /// </summary>
        public int coinceiling { get; set; }

        public int is_collect { get; set; }
        public bool _is_collect { get { return is_collect == 1; } }
        public int up_is_follow { get; set; }
        public bool _up_is_follow { get { return up_is_follow == 1; } }
        public bool _showfollow { get { return up_is_follow == 0; } }


        /// <summary>
        /// 是否可以缓存
        /// </summary>
        public bool is_cacheable { get; set; }
        /// <summary>
        /// 是否收费
        /// </summary>
        public int is_pay { get; set; }

        public bool isPay
        {
            get { return up_name == "付费音乐"; }
        }

        public bool showName
        {
            get { return up_name != "付费音乐"; }
        }

        public List<QualitiesModel> qualities { get; set; }
        /// <summary>
        /// 歌单推荐
        /// </summary>
        public List<menusResponesModel> menusRespones { get; set; }
        public List<SongsVideosModel> videos { get; set; }
        public List<SongsAudiosModel> up_hit_audios { get; set; }
        public List<SongsMemberModel> memberList { get; set; }
        public bool showMenusRespones
        {
            get
            {
                return menusRespones != null && menusRespones.Count != 0;
            }
        }
        public bool showVideos
        {
            get
            {
                return videos != null && videos.Count != 0;
            }
        }
        public bool showUp_hit_audios
        {
            get
            {
                return up_hit_audios != null && up_hit_audios.Count != 0;
            }
        }
        public bool showMemberList
        {
            get
            {
                return memberList != null && memberList.Count != 0;
            }
        }
    }
    public class SongsMemberModel
    {
        public string face { get; set; }
        public string name { get; set; }
        public long mid { get; set; }
        public List<string> m_type_info { get; set; }
        public string info
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in m_type_info)
                {
                    sb.Append(item);
                    sb.Append("、");
                }
                return sb.ToString().TrimEnd('、');
            }
        }
    }
    public class SongsVideosModel
    {
        public int aid { get; set; }
        public string title { get; set; }
        public string ptitle { get; set; }
        public string pic { get; set; }
        public long view { get; set; }
        public long reply { get; set; }
    }
    public class SongsAudiosModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string cover { get; set; }
        public string uname { get; set; }
    }
    public class SongsVideoModel
    {
        public long aid { get; set; }
        public string title { get; set; }
        public string ptitle { get; set; }
        public string pic { get; set; }
        public long duration { get; set; }
        public long view { get; set; }
        public long reply { get; set; }
        
    }
}
