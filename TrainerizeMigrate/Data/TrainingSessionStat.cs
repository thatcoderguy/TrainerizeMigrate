using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainerizeMigrate.Data
{
    public class TrainingSessionStat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid id {  get; set; }
        public long? dailyExerciseID { get; set; }
        public int? excersizeId { get; set; }
        public int? setNumber { get; set; }
        public int? reps { get; set; }
        public double? weight { get; set; }
        public double? distance { get; set; }
        public double? time { get; set; }
        public double? calories { get; set; }
        public double? level { get; set; }
        public double? speed { get; set; }
        public long? newdailyExerciseID { get; set; }
    }
}
