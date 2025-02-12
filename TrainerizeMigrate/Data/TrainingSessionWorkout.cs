using System.ComponentModel.DataAnnotations;

namespace TrainerizeMigrate.Data
{
    public class TrainingSessionWorkout
    {
        [Key]
        public int dailyWorkoutId { get; set; }
        public int? workoutId { get; set; }
        public string? workout { get; set; }
        public string? date { get; set; }
        public int? rpe { get; set; }
        public int? numOfComments { get; set; }
        public string? notes { get; set; }
        public List<TrainingSessionStat> stats { get; set; }
        public int? newdailyWorkoutId { get; set; }
    }
}
