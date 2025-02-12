namespace TrainerizeMigrate.API
{
    public class WorkoutTrainingSessionListReponse
    {
        public List<WorkoutList>? workouts { get; set; }
        public int? total { get; set; }
    }

    public class WorkoutList
    {
        public int? dailyWorkoutId { get; set; }
        public int? workoutId { get; set; }
        public string? workout { get; set; }
        public string? date { get; set; }
        public int? rpe { get; set; }
        public int? numOfComments { get; set; }
        public string? notes { get; set; }
    }
}
