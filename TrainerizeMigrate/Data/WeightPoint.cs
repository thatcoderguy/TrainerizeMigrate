using System.ComponentModel.DataAnnotations;

namespace TrainerizeMigrate.Data
{
    public class WeightPoint
    {
        [Key]
        public int id { get; set; }
        public string? date { get; set; }
        public double? value { get; set; }
        public int? newbodystatid { get; set; } = null;
    }
}
