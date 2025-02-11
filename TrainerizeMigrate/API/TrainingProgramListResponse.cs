namespace TrainerizeMigrate.API
{
    public class TrainingProgramListResponse
    {
        public List<Program> programs { get; set; }
    }

    public class Program
    {
        public int? userProgramID { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string durationType { get; set; }
        public string subscribeType { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public bool isEmpty { get; set; }
        public string accessLevel { get; set; }
        public string? userGroup { get; set; }
    }
}
