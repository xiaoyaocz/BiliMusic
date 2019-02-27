using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliMusic.Models
{
    public class QualitiesModel
    {
        public int type { get; set; }
        public string desc { get; set; }
        public long size { get; set; }
        public string bps { get; set; }
        public string tag { get; set; }
        public int require { get; set; }
        public string requiredesc { get; set; }
    }
}
