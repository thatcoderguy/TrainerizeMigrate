namespace TrainerizeMigrate.API
{
    public class Plan
    {
        public int id { get; set; }
        public string name { get; set; }
        public string instruction { get; set; }
        public string startDate { get; set; }
        public int? duration { get; set; }
        public string durationType { get; set; }
        public string endDate { get; set; }
        public string planType { get; set; }
        public string modified { get; set; }
    }

    public class ProgramPhasesResponse
    {
        public List<Plan> plans { get; set; }
    }
}
