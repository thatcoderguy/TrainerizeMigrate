namespace TrainerizeMigrate.API
{
    public class PlanRequest
    {
        public string name { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int? duration { get; set; }
        public string durationType { get; set; }
    }

    public class AddProgramPhaseRequest
    {
        public int userid { get; set; }
        public PlanRequest plan { get; set; }
    }
    }
