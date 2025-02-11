namespace TrainerizeMigrate.API
{
    public class Filter
    {
        public string? equipments { get; set; }
        public string? duration { get; set; }
    }

    public class PhaseWorkoutPlansRequest
    {
        public int planID { get; set; }
        public int start { get; set; }
        public int count { get; set; }
        public string? sort { get; set; }
        public string? searchTerm { get; set; }
        public Filter? filter { get; set; }
    }
}
