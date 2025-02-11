using System.Text.Json.Serialization;

namespace TrainerizeMigrate.API
{
    public class BodyWeightRequest
    {
        public int userid { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        [JsonPropertyName("type")]
        public string type { get; set; }
        public string unit { get; set; }
    }


}
