using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.API
{
    public class AddTrainingSessionResponse
    {
        public int code { get; set; }
        public string statusMsg { get; set; }
        public List<int> dailyWorkoutIDs { get; set; }
        public int milestoneWorkout { get; set; }
    }
}
