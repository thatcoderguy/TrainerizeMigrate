using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.API
{

    public class ExcersizeDetailsResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string alternateName { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string recordType { get; set; }
        public string videoType { get; set; }
        public string videoUrl { get; set; }
        public string videoStatus { get; set; }
        public int numPhotos { get; set; }
        public object lastPerformed { get; set; }
        public string version { get; set; }
    }
}
