namespace TrainerizeMigrate.API
{
    public class TrainerizeLoginResponse
    {
        public int code { get; set; }
        public object message { get; set; }
        public int userid { get; set; }
        public int groupID { get; set; }
        public string groupname { get; set; }
        public object groupurl { get; set; }
        public int accounttype { get; set; }
        public Token token { get; set; }
    }

    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public long expires_date { get; set; }
        public string refresh_token { get; set; }
        public int userID { get; set; }
    }



}
