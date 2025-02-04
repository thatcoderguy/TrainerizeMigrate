using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.Data
{
    public class WorkoutExcersize
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid SystemId { get; set; }
        public int id { get; set; }
        public int sets { get; set; }
        public string target { get; set; }
        public int restTime { get; set; }
    }
}
