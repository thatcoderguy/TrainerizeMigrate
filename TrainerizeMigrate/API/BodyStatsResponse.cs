namespace TrainerizeMigrate.API
{
    public class BodyStatMeasures
    {
        public double? bodyWeight { get; set; }
        public int? restingHeartRate { get; set; }
        public double? bodyFatPercent { get; set; }
        public double? bodyMassIndex { get; set; }
        public int? caliperMode { get; set; }
        public double? chest { get; set; }
        public double? shoulders { get; set; }
        public double? rightBicep { get; set; }
        public double? leftBicep { get; set; }
        public double? rightThigh { get; set; }
        public double? leftThigh { get; set; }
        public double? rightCalf { get; set; }
        public double? leftCalf { get; set; }
        public double? waist { get; set; }
    }

    public class BodyStatsResponse
    {
        public int id { get; set; }
        public string date { get; set; }
        public string status { get; set; }
        public BodyStatMeasures bodyMeasures { get; set; }
        public string from { get; set; }
        public int code { get; set; }
        public bool fromProgram { get; set; }
        public int numberOfComments { get; set; }
        public object goal { get; set; }
    }


}
