namespace TrainerizeMigrate.API
{
    public class TrackingDef
    {
        public bool effortInterval { get; set; }
        public bool restInterval { get; set; }
        public bool minHeartRate { get; set; }
        public bool maxHeartRate { get; set; }
        public bool avgHeartRate { get; set; }
        public bool zone { get; set; }
    }

    public class ExcersizeDef
    {
        public int? id { get; set; }
        public int? superSetID { get; set; }
        public int? sets { get; set; }
        public string? target { get; set; }
        public AddWorkoutTargetDetail targetDetail { get; set; }
        public int? intervalTime { get; set; }
        public int? restTime { get; set; }
        public string? supersetType { get; set; }
    }

    public class AddWorkoutxercise
    {
        public ExcersizeDef def { get; set; }
    }

    public class AddWorkoutRequest
    {
        public string type { get; set; }
        public int? userID { get; set; }
        public int? trainingPlanID { get; set; }
        public WorkoutDef workoutDef { get; set; }
    }

    public class AddWorkoutTargetDetail
    {
        public int? type { get; set; }
        public string? text { get; set; }
        public double? time { get; set; }
    }

    public class TrackingStats
    {
        public WrapperDef def { get; set; }
    }
    public class WrapperDef
    {
        public TrackingDef def { get; set; }
    }

    public class WorkoutDef
    {
        public string name { get; set; }
        public string instructions { get; set; }
        public string type { get; set; }
        public List<AddWorkoutxercise> exercises { get; set; }
        public TrackingStats trackingStats { get; set; }
        public List<string> tags { get; set; }
        public int rounds { get; set; }
    }


}
