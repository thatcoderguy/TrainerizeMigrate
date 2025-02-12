using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.Data
{
    public class TrainingSessionStat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid id {  get; set; }
        public long dailyExerciseID { get; set; }
        public int setNumber { get; set; }
        public int reps { get; set; }
        public double weight { get; set; }
        public double? distance { get; set; }
        public double? time { get; set; }
        public double? calories { get; set; }
        public double? level { get; set; }
        public double? speed { get; set; }
    }
}
