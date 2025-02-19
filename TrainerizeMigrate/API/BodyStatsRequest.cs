using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.API
{
    public class BodyStatsRequest
    {
        public string date { get; set; }
        public int? id { get; set; }
        public string unitBodystats { get; set; }
        public string unitWeight { get; set; }
        public int userID { get; set; }
    }
}
