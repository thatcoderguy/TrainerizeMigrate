using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.Data
{
    public class PlanWorkout
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string instruction { get; set; }
        public virtual List<WorkoutExcersize> excersizes { get; set; }
        public int? new_id { get; set; }
    }
}
