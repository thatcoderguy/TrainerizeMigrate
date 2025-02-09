using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.API
{ 
    public class DeletePhaseRequest
    {
        public int? planid { get; set; }
        public bool closeGap { get; set; }
    }
}
