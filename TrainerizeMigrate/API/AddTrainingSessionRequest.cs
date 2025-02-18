using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.API
{
    public class AddDailyWorkout
    {
        public int userID { get; set; }
        public int? id { get; set; }
        public string date { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int? workoutID { get; set; }
    }

    public class AddTrainingSessionRequest
    {
        public List<AddDailyWorkout> dailyWorkouts { get; set; }
        public string unitDistance { get; set; }
        public string unitWeight { get; set; }
    }
}
