namespace TrainerizeMigrate.API
{
    public class TrainerizeLoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
        public bool rememberMe { get; set; }
        public string groupUrl { get; set; }
    }
}
