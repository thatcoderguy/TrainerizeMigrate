using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.API
{    
    public class AddBodyStatDataReponse
    {
        public int id { get; set; }
        public object date { get; set; }
        public object status { get; set; }
        public object bodyMeasures { get; set; }
        public object from { get; set; }
        public int code { get; set; }
        public bool fromProgram { get; set; }
        public int numberOfComments { get; set; }
        public object goal { get; set; }
    }
}
