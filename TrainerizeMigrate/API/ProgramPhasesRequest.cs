using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.API
{
    public class ProgramPhasesRequest
    {
        public int userID { get; set; }
        public int? userProgramID { get; set; }
    }
}
