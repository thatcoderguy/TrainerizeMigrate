using RestSharp.Authenticators;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrainerizeMigrate.API;

namespace TrainerizeMigrate.DataManagers
{
    public class BodyWeightManager
    {
        private Config _config { get; set; }

        public BodyWeightManager(Config config)
        {
            _config = config;
        }

        public bool ExtractData()
        {
            AuthenticationDetails authDetails = Authenticate.AuthenticateWithTrainerize(_config);
            BodyWeightResponse bodyWeightData = PullBodyWeightData(authDetails);

            return false;
        }

        public bool ImportData()
        {
            return false;
        }

        private BodyWeightResponse PullBodyWeightData(AuthenticationDetails authDetails)
        {
            BodyWeightRequest jsonBody = new BodyWeightRequest()
            {
                startDate = DateTime.Now.ToShortDateString(),
                endDate = DateTime.Now.AddYears(-10).ToShortDateString(),
                type = "bodyweight",
                unit = "kg",
                userid = authDetails.userId
            };

            RestClient client = new RestClient();
            var request = new RestRequest();
            var authenticator = new JwtAuthenticator(authDetails.token);
            var options = new RestClientOptions()
            {
                Authenticator = authenticator,
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            request.Resource = _config.LoginUrl();
            request.Method = Method.Post;
            request.AddJsonBody(jsonBody, ContentType.Json);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = client.Execute(request);

            BodyWeightResponse response = JsonSerializer.Deserialize<BodyWeightResponse>(queryResult.Content);

            return response;
        }

        private bool StoreBodyWeightData()
        {
            return false;
        }

    }

}
