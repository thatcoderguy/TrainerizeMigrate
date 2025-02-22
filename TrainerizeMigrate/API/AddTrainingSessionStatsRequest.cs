using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.API
{
    public class AddTrainingSessionStatsCreatedBy
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class AddTrainingSessionStatsDailyWorkout
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public string? date { get; set; }
        public object startTime { get; set; }
        public object endTime { get; set; }
        public string type { get; set; }
        public string style { get; set; }
        public string status { get; set; }
        public string instructions { get; set; }
        public List<AddTrainingSessionStatsExercise> exercises { get; set; }
        public int rounds { get; set; }
        public int? workoutID { get; set; }
        public int userID { get; set; }
    }

    public class AddTrainingSessionDef
    {
        public int? id { get; set; }
        public int sets { get; set; }
        public int? superSetID { get; set; }
        public string? supersetType { get; set; }
        public int? intervalTime { get; set; }
        public int? restTime { get; set; }
        public string? target {  get; set; }
        public string? recordType { get; set; }
    }

    public class AddTrainingSessionTrackingDef
    {
        public bool effortInterval { get; set; }
        public bool restInterval { get; set; }
        public bool minHeartRate { get; set; }
        public bool maxHeartRate { get; set; }
        public bool avgHeartRate { get; set; }
        public bool zone { get; set; }
    }

    public class AddTrainingSessionStatsExercise
    {
        public long? dailyExerciseID { get; set; }
        public AddTrainingSessionDef def { get; set; }
        public List<AddTrainingSessionStat> stats { get; set; }
    }

    public class AddTrainingSessionStatsRequest
    {
        public List<AddTrainingSessionStatsDailyWorkout> dailyWorkouts { get; set; }
        public string unitDistance { get; set; }
        public string unitWeight { get; set; }
    }

    public class AddTrainingSessionStat
    {
        public int id { get; set; }
        public int? setID { get; set; }
        public int? reps { get; set; }
        public double? weight { get; set; }
        public object distance { get; set; }
        public object time { get; set; }
        public object calories { get; set; }
        public object level { get; set; }
        public object speed { get; set; }
        public object avgSpeed { get; set; }
        public object avgPace { get; set; }
        public Units units { get; set; }
    }

    public class AddTrainingSessionStatsStats
    {
        public object effortInterval { get; set; }
        public object restInterval { get; set; }
        public object minHeartRate { get; set; }
        public object maxHeartRate { get; set; }
        public object avgHeartRate { get; set; }
        public object zone { get; set; }
        public object calories { get; set; }
        public object activeCalories { get; set; }
    }

    public class AddTrainingSessionTrackingStats
    {
        public AddTrainingSessionTrackingDef def { get; set; }
        public AddTrainingSessionStatsStats stats { get; set; }
    }

    public class Units
    {
        public string bodystats { get; set; }
        public string distance { get; set; }
        public string weight { get; set; }
    }




}
