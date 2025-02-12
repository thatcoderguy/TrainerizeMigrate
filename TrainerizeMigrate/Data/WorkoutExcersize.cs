using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrainerizeMigrate.Data
{
    public class WorkoutExcersize
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid SystemId { get; set; }
        public int? id { get; set; }
        public int? sets { get; set; }
        public string? target { get; set; }
        public int? restTime { get; set; }
        public int? superSetID { get; set; }
        public int? intervalTime { get; set; }
        public int? targetDetailType { get; set; }
        public double? targetDetailTime { get; set; }
        public string? targetDetailText { get; set; }
        public int order {  get; set; }
    }
}
