using RestSharp.Authenticators;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrainerizeMigrate.API;
using TrainerizeMigrate.Migrations;
using TrainerizeMigrate.Data;

namespace TrainerizeMigrate.DataManagers
{
    public class BodyWeightManager
    {
        private Config _config { get; set; }
        private ApplicationDbContext _context { get; set; }

        public BodyWeightManager(Config config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        public bool ExtractAndStoreData()
        {
            AuthenticationDetails authDetails = Authenticate.AuthenticateWithTrainerize(_config);
            BodyWeightResponse bodyWeightData = PullBodyWeightData(authDetails);

            StoreBodyWeightData(bodyWeightData);

            return false;
        }

        public bool ImportExtractedData()
        {
            return false;
        }

        private BodyWeightResponse PullBodyWeightData(AuthenticationDetails authDetails)
        {
            BodyWeightRequest jsonBody = new BodyWeightRequest()
            {
                //"2025-02-28"
                startDate = DateTime.Now.AddYears(-10).ToString("yyyy-MM-dd"),
                endDate = DateTime.Now.ToString("yyyy-MM-dd"),
                type = "bodyweight",
                unit = "kg",
                userid = authDetails.userId
            };

            var authenticator = new JwtAuthenticator(authDetails.token);
            var options = new RestClientOptions()
            {
                Authenticator = authenticator,
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            RestClient client = new RestClient(options);
            var request = new RestRequest();
            request.Resource = _config.BodyStatsUrl();
            request.Method = Method.Post;
            request.AddJsonBody(jsonBody, ContentType.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = client.Execute(request);

            BodyWeightResponse response = JsonSerializer.Deserialize<BodyWeightResponse>(queryResult.Content);

            return response;
        }

        private bool StoreBodyWeightData(BodyWeightResponse bodyWeightData)
        {
            List<WeightPoint> weightPoints = new List<WeightPoint>();

            foreach(Point datapoint in bodyWeightData.points)
            {
                weightPoints.Add(new WeightPoint()
                {
                    date = datapoint.date,
                    id = datapoint.id,
                    value = datapoint.value
                });
            }


            _context.Body_Weight.Add(new BodyWeight()
            {
                goal = bodyWeightData.goal,
                points = weightPoints,
                unit = bodyWeightData.unit
            });

            //_context.SaveChanges();

            _context.Body_Weight_Point.AddRange(weightPoints);

            _context.SaveChanges();
            return true;
        }

    }

}
