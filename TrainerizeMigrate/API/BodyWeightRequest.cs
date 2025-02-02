using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.API
{
    public class BodyWeightRequest
    {
        public int userid { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string type { get; set; }
        public string unit { get; set; }
    }


}
