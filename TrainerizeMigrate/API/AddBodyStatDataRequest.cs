namespace TrainerizeMigrate.API
{

    public class BodyMeasures
    {
        public string? bloodPressureDiastolic { get; set; }
        public string? bloodPressureSystolic { get; set; }
        public string? bodyFatPercent { get; set; }
        public string? bodyWeight { get; set; }
        public string? caliperAbdomen { get; set; }
        public string? caliperAxilla { get; set; }
        public string? caliperBF { get; set; }
        public string? caliperChest { get; set; }
        public int? caliperMode { get; set; }
        public string? caliperSubscapular { get; set; }
        public string? caliperSuprailiac { get; set; }
        public string? caliperThigh { get; set; }
        public string? caliperTriceps { get; set; }
        public string? chest { get; set; }
        public string? date { get; set; }
        public string? hips { get; set; }
        public string? leftBicep { get; set; }
        public string? leftCalf { get; set; }
        public string? leftForearm { get; set; }
        public string? leftThigh { get; set; }
        public string? neck { get; set; }
        public string? rightBicep { get; set; }
        public string? rightCalf { get; set; }
        public string? rightForearm { get; set; }
        public string? rightThigh { get; set; }
        public string? shoulders { get; set; }
        public string? waist { get; set; }
    }

    public class AddBodyStatDataRequest
    {
        public BodyMeasures bodyMeasures { get; set; }
        public int? id { get; set; }
        public string date { get; set; }
        public string unitBodystats { get; set; }
        public string unitWeight { get; set; }
        public int userID { get; set; }
    }
}
