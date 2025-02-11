using System.ComponentModel.DataAnnotations;

namespace TrainerizeMigrate.Data
{
    public class PlanWorkout
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string instruction { get; set; }
        public string type { get; set; }
        public virtual List<WorkoutExcersize> excersizes { get; set; }
        public int? new_id { get; set; }
    }
}
