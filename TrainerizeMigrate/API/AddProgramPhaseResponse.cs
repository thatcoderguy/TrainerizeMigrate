namespace TrainerizeMigrate.API
{
    public class AddProgramPhaseResponse
    {
        public string code { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string? instruction { get; set; }
        public string startDate { get; set; }
        public int? duration { get; set; }
        public string durationType { get; set; }
        public string endDate { get; set; }
        public int planType { get; set; }
        public int? version { get; set; }
        public int numberOfWorkouts { get; set; }
    }


}
