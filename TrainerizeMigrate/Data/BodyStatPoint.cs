using System.ComponentModel.DataAnnotations;

namespace TrainerizeMigrate.Data
{
    public class BodyMeasurePoint
    {
        [Key]
        public int id { get; set; }
        public string? date { get; set; }
        public int? newbodystatid { get; set; } = null;
        public double bodyWeight { get; set; }
        public int restingHeartRate { get; set; }
        public double bodyFatPercent { get; set; }
        public double bodyMassIndex { get; set; }
        public int caliperMode { get; set; }
        public double chest { get; set; }
        public double shoulders { get; set; }
        public double rightBicep { get; set; }
        public double leftBicep { get; set; }
        public double rightThigh { get; set; }
        public double leftThigh { get; set; }
        public double rightCalf { get; set; }
        public double leftCalf { get; set; }
        public double waist { get; set; }

    }
}
