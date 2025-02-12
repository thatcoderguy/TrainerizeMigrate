using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainerizeMigrate.Data;

namespace TrainerizeMigrate.API
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DailyWorkout
    {
        public int id { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public object startTime { get; set; }
        public object endTime { get; set; }
        public string type { get; set; }
        public object media { get; set; }
        public string style { get; set; }
        public string status { get; set; }
        public string instructions { get; set; }
        public List<TrainingSessionStatsExercise> exercises { get; set; }
        public int rounds { get; set; }
        public int duration { get; set; }
        public object workDuration { get; set; }
        public bool hasOverride { get; set; }
        public TrainingSessionStatsTrackingStats trackingStats { get; set; }
        public object dateCreated { get; set; }
        public string dateUpdated { get; set; }
        public int workoutID { get; set; }
    }

    public class Def
    {
        public int id { get; set; }
        public string type { get; set; }
        public bool effortInterval { get; set; }
        public bool restInterval { get; set; }
        public bool minHeartRate { get; set; }
        public bool maxHeartRate { get; set; }
        public bool avgHeartRate { get; set; }
        public bool zone { get; set; }
    }

    public class TrainingSessionStatsExercise
    {
        public long dailyExerciseID { get; set; }
        public Def def { get; set; }
        public List<Stat> stats { get; set; }
    }

    public class TrainingSessionStatsResponse
    {
        public int code { get; set; }
        public string statusMsg { get; set; }
        public List<DailyWorkout> dailyWorkouts { get; set; }
    }

    public class Stat
    {
        public int id { get; set; }
        public int setID { get; set; }
        public int? reps { get; set; }
        public double? weight { get; set; }
        public double? distance { get; set; }
        public double? time { get; set; }
        public double? calories { get; set; }
        public double? level { get; set; }
        public double? speed { get; set; }
    }

    public class TrainingSessionStatsTrackingStats
    {
        public Def def { get; set; }
        public TrainingSessionTrackingStats stats { get; set; }
    }

    public class TrainingSessionTrackingStats
    {
        public object effortInterval { get; set; }
        public object restInterval { get; set; }
        public object minHeartRate { get; set; }
        public object maxHeartRate { get; set; }
        public object avgHeartRate { get; set; }
        public object zone { get; set; }
        public object activeCalories { get; set; }
    }
}
