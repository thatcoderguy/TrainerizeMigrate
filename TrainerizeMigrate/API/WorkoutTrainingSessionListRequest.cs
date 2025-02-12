namespace TrainerizeMigrate.API
{
    public class WorkoutTrainingSessionListRequest
    {
        public int? count { get; set; }
        public int? start { get; set; }
        public string? startDate { get; set; }
        public string? endDate { get; set; }
        public int userID { get; set; }
    }
}
