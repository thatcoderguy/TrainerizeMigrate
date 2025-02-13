using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.API
{
    public class TrainingSessionStatsRequest
    {
        public List<int?>? ids { get; set; }
        public int userID { get; set; }
        public string unitDistance { get; set; }
        public string unitWeight { get; set; }
    }
}
