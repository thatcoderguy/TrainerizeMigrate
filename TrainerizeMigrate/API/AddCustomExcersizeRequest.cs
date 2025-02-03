using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.API
{
    public class ExcersizeMedia
    {
        public string type { get; set; }
        public string token { get; set; }
        public object status { get; set; }
    }

    public class CustomExcersizeRequestTag
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class AddCustomExcersizeRequest
    {
        public string name { get; set; }
        public string alternateName { get; set; }
        public string description { get; set; }
        public string recordType { get; set; }
        public string type { get; set; }
        public object lastPerformed { get; set; }
        public string tag { get; set; }
        public List<CustomExcersizeRequestTag> tags { get; set; }
        public int superSetID { get; set; }
        public ExcersizeMedia media { get; set; }
        public string videoType { get; set; }
        public string videoUrl { get; set; }
        //public string videoURL { get; set; }
    }


}
